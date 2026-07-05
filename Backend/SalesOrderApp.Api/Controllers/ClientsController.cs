using Microsoft.AspNetCore.Mvc;
using SalesOrderApp.Application.Interfaces;

namespace SalesOrderApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;
        public ClientsController(IClientService clientService) => _clientService = clientService;

        /// <summary>Populates the "Customer Name" dropdown on the Sales Order screen.</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _clientService.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _clientService.GetByIdAsync(id);
            return client is null ? NotFound() : Ok(client);
        }
    }
}
