using SalesOrderApp.Application.Interfaces;
using SalesOrderApp.Infrastructure.Data;

namespace SalesOrderApp.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(
            AppDbContext context,
            IClientRepository clients,
            IItemRepository items,
            ISalesOrderRepository salesOrders)
        {
            _context = context;
            Clients = clients;
            Items = items;
            SalesOrders = salesOrders;
        }

        public IClientRepository Clients { get; }
        public IItemRepository Items { get; }
        public ISalesOrderRepository SalesOrders { get; }

        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
