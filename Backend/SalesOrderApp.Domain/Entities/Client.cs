namespace SalesOrderApp.Domain.Entities
{
    /// <summary>
    /// Represents a customer record used to populate the Sales Order "Customer Name"
    /// dropdown and to auto-fill the address block once a customer is selected.
    /// </summary>
    public class Client : BaseEntity
    {
        public string CustomerName { get; set; } = string.Empty;
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Address3 { get; set; }
        public string? Suburb { get; set; }
        public string? State { get; set; }
        public string? PostCode { get; set; }

        public ICollection<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();
    }
}
