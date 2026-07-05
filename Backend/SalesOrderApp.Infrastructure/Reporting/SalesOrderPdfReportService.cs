using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SalesOrderApp.Application.DTOs;
using SalesOrderApp.Application.Interfaces;

namespace SalesOrderApp.Infrastructure.Reporting
{
    /// <summary>
    /// Generates a printable PDF for a Sales Order using QuestPDF (satisfies
    /// requirement 8: "add print option to print each sales order").
    /// </summary>
    public class SalesOrderPdfReportService : IReportService
    {
        public SalesOrderPdfReportService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] GenerateSalesOrderPdf(SalesOrderDetailDto order)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Column(col =>
                    {
                        col.Item().Text("Sales Order").FontSize(20).Bold();
                        col.Item().Text($"Invoice No: {order.InvoiceNo}");
                        col.Item().Text($"Invoice Date: {order.InvoiceDate:yyyy-MM-dd}");
                        if (!string.IsNullOrWhiteSpace(order.ReferenceNo))
                            col.Item().Text($"Reference No: {order.ReferenceNo}");
                    });

                    page.Content().Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().Text("Bill To").Bold();
                        col.Item().Text(order.CustomerName);
                        col.Item().Text(string.Join(", ", new[]
                            {
                                order.Address1, order.Address2, order.Address3,
                                order.Suburb, order.State, order.PostCode
                            }.Where(s => !string.IsNullOrWhiteSpace(s))));

                        col.Item().PaddingTop(10).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1.3f);
                                columns.RelativeColumn(1.3f);
                                columns.RelativeColumn(1.3f);
                            });

                            table.Header(header =>
                            {
                                foreach (var title in new[] { "Item Code", "Description", "Qty", "Price", "Tax %", "Excl", "Tax", "Incl" })
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(4).Text(title).Bold();
                            });

                            foreach (var item in order.Items)
                            {
                                table.Cell().Padding(4).Text(item.ItemCode);
                                table.Cell().Padding(4).Text(item.Description);
                                table.Cell().Padding(4).Text(item.Quantity.ToString("0.##"));
                                table.Cell().Padding(4).Text(item.Price.ToString("0.00"));
                                table.Cell().Padding(4).Text(item.TaxRate.ToString("0.##"));
                                table.Cell().Padding(4).Text(item.ExclAmount.ToString("0.00"));
                                table.Cell().Padding(4).Text(item.TaxAmount.ToString("0.00"));
                                table.Cell().Padding(4).Text(item.InclAmount.ToString("0.00"));
                            }
                        });

                        col.Item().AlignRight().PaddingTop(10).Column(totals =>
                        {
                            totals.Item().Text($"Total Excl: {order.TotalExcl:0.00}");
                            totals.Item().Text($"Total Tax: {order.TotalTax:0.00}");
                            totals.Item().Text($"Total Incl: {order.TotalIncl:0.00}").Bold();
                        });
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Generated on ");
                        x.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
