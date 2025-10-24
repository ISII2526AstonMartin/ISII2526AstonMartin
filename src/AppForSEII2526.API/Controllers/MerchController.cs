using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BocadillosController> _logger;


        public MerchController(ApplicationDbContext context, ILogger<BocadillosController> logger)
        {
            this._context = context;
            this._logger = logger;
        }
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<Producto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductos()
        {
            IList<Producto> productos = await _context.Productos
                .ToListAsync();
            return Ok(productos);
        }
    }
}
