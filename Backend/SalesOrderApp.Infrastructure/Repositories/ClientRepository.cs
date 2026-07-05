using Microsoft.EntityFrameworkCore;
using SalesOrderApp.Application.Interfaces;
using SalesOrderApp.Domain.Entities;
using SalesOrderApp.Infrastructure.Data;

namespace SalesOrderApp.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _context;
        public ClientRepository(AppDbContext context) => _context = context;

        public async Task<IReadOnlyList<Client>> GetAllAsync() =>
            await _context.Clients.AsNoTracking().OrderBy(c => c.CustomerName).ToListAsync();

        public async Task<Client?> GetByIdAsync(int id) =>
            await _context.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }
}
