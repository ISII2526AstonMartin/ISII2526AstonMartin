using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.DTOs.ComprarMerchDTOs;
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
        private readonly ILogger<MerchController> _logger;


        public MerchController(ApplicationDbContext context, ILogger<MerchController> logger)
        {
            this._context = context;
            this._logger = logger;
        }
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<MerchDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProductos(string? tipo, float? precio)
        {
            IList<MerchDTO> productos = await _context.Productos
                .Include(p => p.Tipo_Producto)
                .Where(p =>
                (tipo == null || p.Tipo_Producto.Nombre.Contains(tipo))
                && (precio == null || p.PVP <= precio))
                .Select(p =>
                new MerchDTO(p.Nombre, p.PVP, p.Tipo_Producto, p.Stock)
                )
                .ToListAsync();
            if (productos.Count() == 0)
            {
                return NotFound("No hay Productos con esos filtros");
            }

            return Ok(productos);
        }
        [HttpPost]
        [Route("[action]")]
        // [ProducesResponseType(typeof(MerchDetailDTO), (int)HttpStatusCode.Created)] COMENTADA PORQUE AUN NO ESTA DEFINIDO MerchDetailDTO
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> CreateMerch([FromBody] CreateMerchDTO createMerch)
        {
            // if (!_context.ApplicationUsers.Any(au=>au.UserName==rentalForCreate.CustomerUserName))
            var user = _context.ApplicationUsers.FirstOrDefault(au => au.UserName == createMerch.CustomerUserName);
            if (user == null)
                ModelState.AddModelError("RentalApplicationUser", "Error! UserName is not registered");
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            
        }
    }
}
