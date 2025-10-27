using AppForSEII2526.API.DTOs;
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
        [ProducesResponseType(typeof(List<MerchDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProductos(TipoProducto? tipo, float? precio)
        {
            IList<MerchDTO> productos = await _context.Productos
                .Include(p => p.Tipo_Producto)
                .Where(p =>
                (tipo == null || p.Tipo_Producto.Nombre.Contains(tipo.Nombre))
                && (precio == null || p.PVP <= precio))
                .Select(p => new MerchDTO
                {
                    Nombre = p.Nombre,
                    Precio = p.PVP,
                    Stock = p.Stock,
                    Tipo = p.Tipo_Producto
                })
                .ToListAsync();
            if (productos.Count() == 0)
            {
                return NotFound("No hay Productos con esos filtros");
            }

            return Ok(productos);
        }
    }
}
