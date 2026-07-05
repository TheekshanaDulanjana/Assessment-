using SalesOrderApp.Domain.Entities;

namespace SalesOrderApp.Application.Interfaces
{
    public interface IClientRepository
    {
        Task<IReadOnlyList<Client>> GetAllAsync();
        Task<Client?> GetByIdAsync(int id);
    }
}
