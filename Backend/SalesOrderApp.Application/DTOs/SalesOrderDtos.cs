namespace SalesOrderApp.Application.DTOs
{
    /// <summary>Row shape used by the Home screen (Screen 2) grid.</summary>
    public class SalesOrderListDto
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string? ReferenceNo { get; set; }
        public decimal TotalIncl { get; set; }
    }

    /// <summary>Full order detail returned when opening/editing a Sales Order (Screen 1).</summary>
    public class SalesOrderDetailDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Address3 { get; set; }
        public string? Suburb { get; set; }
        public string? State { get; set; }
        public string? PostCode { get; set; }
        public string InvoiceNo { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public string? ReferenceNo { get; set; }
        public string? Note { get; set; }
        public decimal TotalExcl { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalIncl { get; set; }
        public List<SalesOrderItemDto> Items { get; set; } = new();
    }

    public class SalesOrderItemDto
    {
        public int Id { get; set; }
        public string ItemCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Note { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TaxRate { get; set; }
        public decimal ExclAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal InclAmount { get; set; }
    }

    /// <summary>
    /// Payload sent by the frontend to create (Id = null) or update (Id = value) an order.
    /// NOTE: Excl/Tax/Incl amounts are intentionally NOT part of this DTO — they are
    /// always recalculated server-side from Quantity/Price/TaxRate so a tampered
    /// client request can never persist incorrect totals.
    /// </summary>
    public class SaveSalesOrderDto
    {
        public int? Id { get; set; }
        public int ClientId { get; set; }
        public string InvoiceNo { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public string? ReferenceNo { get; set; }
        public string? Note { get; set; }
        public List<SaveSalesOrderItemDto> Items { get; set; } = new();
    }

    public class SaveSalesOrderItemDto
    {
        public string ItemCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Note { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TaxRate { get; set; }
    }
}
