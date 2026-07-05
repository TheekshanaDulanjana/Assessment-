using SalesOrderApp.Domain.Entities;

namespace SalesOrderApp.Application.Interfaces
{
    public interface IItemRepository
    {
        Task<IReadOnlyList<Item>> GetAllAsync();
        Task<Item?> GetByIdAsync(int id);
        Task<Item?> GetByCodeAsync(string itemCode);
    }
}
