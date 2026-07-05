using Microsoft.AspNetCore.Mvc;
using SalesOrderApp.Application.Interfaces;

namespace SalesOrderApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;
        public ItemsController(IItemService itemService) => _itemService = itemService;

        /// <summary>Populates the Item Code / Description dropdowns in the order lines grid.</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _itemService.GetAllAsync());
    }
}
