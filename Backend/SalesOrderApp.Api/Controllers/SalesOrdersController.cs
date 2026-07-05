using Microsoft.AspNetCore.Mvc;
using SalesOrderApp.Application.DTOs;
using SalesOrderApp.Application.Interfaces;

namespace SalesOrderApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesOrdersController : ControllerBase
    {
        private readonly ISalesOrderService _orderService;
        private readonly IReportService _reportService;

        public SalesOrdersController(ISalesOrderService orderService, IReportService reportService)
        {
            _orderService = orderService;
            _reportService = reportService;
        }

        /// <summary>Feeds the Home screen (Screen 2) grid.</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _orderService.GetAllAsync());

        [HttpGet("next-invoice-no")]
        public async Task<IActionResult> GetNextInvoiceNo() =>
            Ok(new { invoiceNo = await _orderService.GetNextInvoiceNoAsync() });

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _orderService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SaveSalesOrderDto dto)
        {
            dto.Id = null;
            var result = await _orderService.SaveAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] SaveSalesOrderDto dto)
        {
            dto.Id = id;
            var result = await _orderService.SaveAsync(dto);
            return Ok(result);
        }

        /// <summary>Requirement 8: print option for a saved sales order (returns a PDF).</summary>
        [HttpGet("{id:int}/print")]
        public async Task<IActionResult> Print(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            var pdf = _reportService.GenerateSalesOrderPdf(order);
            return File(pdf, "application/pdf", $"SalesOrder-{order.InvoiceNo}.pdf");
        }
    }
}
