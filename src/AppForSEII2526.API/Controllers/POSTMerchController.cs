using AppForSEII2526.API.DTOs.ComprarMerchDTOs;
using AppForSEII2526.API.DTOs.ComprarMerchDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class POSTMerchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MerchController> _logger;
        public POSTMerchController(ApplicationDbContext context, ILogger<MerchController> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CreateMerch([FromBody] CreateMerchDTO createMerch)
        {
            if (createMerch == null)
                return BadRequest("Request vacío");

            if (createMerch.items == null || createMerch.items.Count == 0)
            {
                ModelState.AddModelError("CreateMerch", "Debes incluir al menos un producto");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(au => au.UserName == createMerch.NombreUsuario && au.Apellido1 == createMerch.Apellido1);

            if (user == null)
            {
                ModelState.AddModelError("CreateMerch", "Error! UserName o apellido no registrados");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var nombres = createMerch.items.Select(i => i.Nombre).ToList();

            // Cargar las entidades Producto completas desde la BD (incluyendo Tipo_Producto)
            var productosEnBd = await _context.Productos
                .Include(p => p.Tipo_Producto)
                .Where(p => nombres.Contains(p.Nombre))
                .ToListAsync();

            var compra = new Compra_Producto(user, createMerch.DireccionEnvio, DateTime.Now, createMerch.metodoPago, new List<Producto_Compra>());
            compra.CompraID = Guid.NewGuid().ToString();
            float precioFinal = 0f;

            foreach (var item in createMerch.items)
            {
                var producto = productosEnBd.FirstOrDefault(p => p.Nombre == item.Nombre);
                if (producto == null)
                {
                    ModelState.AddModelError("CreateMerch", $"Producto '{item.Nombre}' no encontrado");
                    continue;
                }

                if (item.Cantidad <= 0)
                {
                    ModelState.AddModelError("CreateMerch", $"Cantidad inválida para '{item.Nombre}'");
                    continue;
                }

                var pc = new Producto_Compra
                {
                    Cantidad = item.Cantidad,
                    ProductoID = producto.Nombre,
                    PVP = producto.PVP,
                    Producto = producto,
                    Compra = compra,
                    CompraID = compra.CompraID
                };

                compra.Productos_Compras.Add(pc);
                precioFinal += producto.PVP * item.Cantidad;
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            if (!compra.Productos_Compras.Any())
                return Conflict("Ninguno de los productos indicados existe");

            compra.PrecioFinal = precioFinal;

            _context.Compras.Add(compra);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la compra");
                return Conflict("Error al guardar la compra: " + ex.Message);
            }

            return Ok(new { message = "Compra realizada correctamente", precioFinal = compra.PrecioFinal, compraId = compra.CompraID });
        }
    }
}