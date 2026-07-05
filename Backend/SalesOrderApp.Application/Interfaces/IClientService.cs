using SalesOrderApp.Application.DTOs;

namespace SalesOrderApp.Application.Interfaces
{
    public interface IClientService
    {
        Task<IReadOnlyList<ClientDto>> GetAllAsync();
        Task<ClientDto?> GetByIdAsync(int id);
    }
}
