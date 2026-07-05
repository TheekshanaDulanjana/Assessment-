using SalesOrderApp.Domain.Entities;

namespace SalesOrderApp.Application.Interfaces
{
    public interface ISalesOrderRepository
    {
        Task<IReadOnlyList<SalesOrder>> GetAllAsync();
        Task<SalesOrder?> GetByIdWithItemsAsync(int id);
        Task<SalesOrder> AddAsync(SalesOrder order);
        Task UpdateAsync(SalesOrder order);
        Task<string> GenerateNextInvoiceNoAsync();
    }
}
