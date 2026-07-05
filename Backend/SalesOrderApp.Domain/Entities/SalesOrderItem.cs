namespace SalesOrderApp.Domain.Entities
{
    /// <summary>
    /// A single line item of a Sales Order. ExclAmount/TaxAmount/InclAmount are
    /// calculated fields (Quantity * Price, etc.) persisted for reporting/history
    /// purposes but always recomputed by the service layer before saving.
    /// </summary>
    public class SalesOrderItem : BaseEntity
    {
        public int SalesOrderId { get; set; }
        public SalesOrder? SalesOrder { get; set; }

        public string ItemCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Note { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TaxRate { get; set; }

        public decimal ExclAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal InclAmount { get; set; }

        public int LineNumber { get; set; }
    }
}
