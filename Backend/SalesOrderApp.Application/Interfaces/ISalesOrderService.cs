using SalesOrderApp.Application.DTOs;

namespace SalesOrderApp.Application.Interfaces
{
    public interface ISalesOrderService
    {
        Task<IReadOnlyList<SalesOrderListDto>> GetAllAsync();
        Task<SalesOrderDetailDto> GetByIdAsync(int id);
        Task<string> GetNextInvoiceNoAsync();
        Task<SalesOrderDetailDto> SaveAsync(SaveSalesOrderDto dto);
    }
}
