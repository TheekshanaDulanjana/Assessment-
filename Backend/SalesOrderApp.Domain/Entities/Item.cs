namespace SalesOrderApp.Domain.Entities
{
    /// <summary>
    /// Product/service catalog entry used to populate the Item Code and Description
    /// dropdowns inside the Sales Order line items grid.
    /// </summary>
    public class Item : BaseEntity
    {
        public string ItemCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
