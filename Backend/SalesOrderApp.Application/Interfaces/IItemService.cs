using SalesOrderApp.Application.DTOs;

namespace SalesOrderApp.Application.Interfaces
{
    public interface IItemService
    {
        Task<IReadOnlyList<ItemDto>> GetAllAsync();
    }
}
