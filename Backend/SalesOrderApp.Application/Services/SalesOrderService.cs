using AutoMapper;
using SalesOrderApp.Application.DTOs;
using SalesOrderApp.Application.Exceptions;
using SalesOrderApp.Application.Interfaces;
using SalesOrderApp.Domain.Entities;

namespace SalesOrderApp.Application.Services
{
    /// <summary>
    /// Owns all Sales Order business rules, in particular the line/total calculations
    /// required by the spec:
    ///   Excl Amount = Quantity * Price
    ///   Tax Amount  = Excl Amount * TaxRate / 100
    ///   Incl Amount = Excl Amount + Tax Amount
    /// Totals are the sum of each line's amounts. These are always recomputed here,
    /// never trusted from the client payload.
    /// </summary>
    public class SalesOrderService : ISalesOrderService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public SalesOrderService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<SalesOrderListDto>> GetAllAsync()
        {
            var orders = await _uow.SalesOrders.GetAllAsync();
            return _mapper.Map<IReadOnlyList<SalesOrderListDto>>(orders);
        }

        public async Task<SalesOrderDetailDto> GetByIdAsync(int id)
        {
            var order = await _uow.SalesOrders.GetByIdWithItemsAsync(id)
                ?? throw new NotFoundException($"Sales order {id} was not found.");
            return _mapper.Map<SalesOrderDetailDto>(order);
        }

        public async Task<string> GetNextInvoiceNoAsync() =>
            await _uow.SalesOrders.GenerateNextInvoiceNoAsync();

        public async Task<SalesOrderDetailDto> SaveAsync(SaveSalesOrderDto dto)
        {
            if (dto.Items is null || dto.Items.Count == 0)
                throw new ArgumentException("A sales order must contain at least one item.");

            // Validate the customer exists (guards against orphaned/incorrect FK values).
            _ = await _uow.Clients.GetByIdAsync(dto.ClientId)
                ?? throw new NotFoundException($"Client {dto.ClientId} was not found.");

            bool isNew = dto.Id is null or 0;
            SalesOrder order;

            if (isNew)
            {
                order = new SalesOrder { ClientId = dto.ClientId };
            }
            else
            {
                order = await _uow.SalesOrders.GetByIdWithItemsAsync(dto.Id!.Value)
                    ?? throw new NotFoundException($"Sales order {dto.Id} was not found.");
                order.ClientId = dto.ClientId;
                order.Items.Clear();
            }

            order.InvoiceNo = string.IsNullOrWhiteSpace(dto.InvoiceNo)
                ? await _uow.SalesOrders.GenerateNextInvoiceNoAsync()
                : dto.InvoiceNo.Trim();
            order.InvoiceDate = dto.InvoiceDate == default ? DateTime.UtcNow.Date : dto.InvoiceDate;
            order.ReferenceNo = dto.ReferenceNo;
            order.Note = dto.Note;

            decimal totalExcl = 0, totalTax = 0, totalIncl = 0;
            var line = 1;

            foreach (var i in dto.Items)
            {
                if (i.Quantity <= 0)
                    throw new ArgumentException($"Quantity for item '{i.ItemCode}' must be greater than zero.");

                var exclAmount = Math.Round(i.Quantity * i.Price, 2, MidpointRounding.AwayFromZero);
                var taxAmount = Math.Round(exclAmount * i.TaxRate / 100m, 2, MidpointRounding.AwayFromZero);
                var inclAmount = exclAmount + taxAmount;

                order.Items.Add(new SalesOrderItem
                {
                    ItemCode = i.ItemCode,
                    Description = i.Description,
                    Note = i.Note,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    TaxRate = i.TaxRate,
                    ExclAmount = exclAmount,
                    TaxAmount = taxAmount,
                    InclAmount = inclAmount,
                    LineNumber = line++
                });

                totalExcl += exclAmount;
                totalTax += taxAmount;
                totalIncl += inclAmount;
            }

            order.TotalExcl = totalExcl;
            order.TotalTax = totalTax;
            order.TotalIncl = totalIncl;
            order.ModifiedDate = DateTime.UtcNow;

            if (isNew)
                await _uow.SalesOrders.AddAsync(order);
            else
                await _uow.SalesOrders.UpdateAsync(order);

            await _uow.SaveChangesAsync();

            var saved = await _uow.SalesOrders.GetByIdWithItemsAsync(order.Id)
                ?? throw new NotFoundException("Order could not be reloaded after save.");
            return _mapper.Map<SalesOrderDetailDto>(saved);
        }
    }
}
