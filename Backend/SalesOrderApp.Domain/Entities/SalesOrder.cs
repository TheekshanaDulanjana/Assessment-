namespace SalesOrderApp.Domain.Entities
{
    /// <summary>
    /// Header record for a sales order (Screen 1). Totals are always derived from
    /// the child SalesOrderItem lines and recalculated server-side on every save.
    /// </summary>
    public class SalesOrder : BaseEntity
    {
        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public string InvoiceNo { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public string? ReferenceNo { get; set; }
        public string? Note { get; set; }

        public decimal TotalExcl { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalIncl { get; set; }

        public ICollection<SalesOrderItem> Items { get; set; } = new List<SalesOrderItem>();
    }
}
