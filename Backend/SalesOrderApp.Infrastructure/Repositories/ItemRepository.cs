using Microsoft.EntityFrameworkCore;
using SalesOrderApp.Application.Interfaces;
using SalesOrderApp.Domain.Entities;
using SalesOrderApp.Infrastructure.Data;

namespace SalesOrderApp.Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _context;
        public ItemRepository(AppDbContext context) => _context = context;

        public async Task<IReadOnlyList<Item>> GetAllAsync() =>
            await _context.Items.AsNoTracking().OrderBy(i => i.ItemCode).ToListAsync();

        public async Task<Item?> GetByIdAsync(int id) =>
            await _context.Items.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

        public async Task<Item?> GetByCodeAsync(string itemCode) =>
            await _context.Items.AsNoTracking().FirstOrDefaultAsync(i => i.ItemCode == itemCode);
    }
}
