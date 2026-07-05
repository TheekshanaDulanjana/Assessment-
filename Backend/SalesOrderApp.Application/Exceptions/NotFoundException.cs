namespace SalesOrderApp.Application.Exceptions
{
    /// <summary>Thrown when a requested aggregate (client, order, etc.) does not exist.</summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}
