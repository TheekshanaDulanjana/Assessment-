namespace SalesOrderApp.Application.Interfaces
{
    /// <summary>
    /// Coordinates repositories that must be saved together within a single
    /// EF Core DbContext/transaction (e.g. a SalesOrder plus its line items).
    /// </summary>
    public interface IUnitOfWork
    {
        IClientRepository Clients { get; }
        IItemRepository Items { get; }
        ISalesOrderRepository SalesOrders { get; }
        Task<int> SaveChangesAsync();
    }
}
