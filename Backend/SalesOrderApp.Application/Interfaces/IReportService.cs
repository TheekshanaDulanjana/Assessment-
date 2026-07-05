using SalesOrderApp.Application.DTOs;

namespace SalesOrderApp.Application.Interfaces
{
    /// <summary>
    /// Abstraction over the reporting engine so the Application layer never
    /// depends directly on a specific PDF/reporting library (implemented in Infrastructure).
    /// </summary>
    public interface IReportService
    {
        byte[] GenerateSalesOrderPdf(SalesOrderDetailDto order);
    }
}
