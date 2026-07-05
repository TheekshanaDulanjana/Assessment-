using Microsoft.EntityFrameworkCore;
using SalesOrderApp.Application.Interfaces;
using SalesOrderApp.Domain.Entities;
using SalesOrderApp.Infrastructure.Data;

namespace SalesOrderApp.Infrastructure.Repositories
{
    public class SalesOrderRepository : ISalesOrderRepository
    {
        private readonly AppDbContext _context;
        public SalesOrderRepository(AppDbContext context) => _context = context;

        public async Task<IReadOnlyList<SalesOrder>> GetAllAsync() =>
            await _context.SalesOrders
                .Include(o => o.Client)
                .AsNoTracking()
                .OrderByDescending(o => o.InvoiceDate)
                .ThenByDescending(o => o.Id)
                .ToListAsync();

        public async Task<SalesOrder?> GetByIdWithItemsAsync(int id) =>
            await _context.SalesOrders
                .Include(o => o.Client)
                .Include(o => o.Items.OrderBy(i => i.LineNumber))
                .FirstOrDefaultAsync(o => o.Id == id);

        public async Task<SalesOrder> AddAsync(SalesOrder order)
        {
            await _context.SalesOrders.AddAsync(order);
            return order;
        }

        public Task UpdateAsync(SalesOrder order)
        {
            _context.SalesOrders.Update(order);
            return Task.CompletedTask;
        }

        public async Task<string> GenerateNextInvoiceNoAsync()
        {
            var year = DateTime.UtcNow.Year;
            var count = await _context.SalesOrders.CountAsync(o => o.InvoiceDate.Year == year);
            return $"INV-{year}-{(count + 1):D5}";
        }
    }
}
