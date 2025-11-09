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
        private readonly ILogger<POSTMerchController> _logger; // ✅ corregido el tipo de logger

        public POSTMerchController(ApplicationDbContext context, ILogger<POSTMerchController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // POST: api/POSTMerch/CreateMerch
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CreateMerch(CreateMerchDTO createMerch)
        {
            if (createMerch == null)
                return BadRequest("El cuerpo de la solicitud está vacío.");

            if (createMerch.Items == null || createMerch.Items.Count == 0)
            {
                ModelState.AddModelError("CreateMerch", "Debes incluir al menos un producto.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Buscar usuario por nombre y primer apellido
            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(au =>
                    au.UserName == createMerch.NombreUsuario &&
                    au.Apellido1 == createMerch.Apellido1);

            if (user == null)
            {
                ModelState.AddModelError("CreateMerch", "Error: Usuario o apellido no registrados.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Obtener nombres de los productos solicitados
            var nombres = createMerch.Items.Select(i => i.Nombre).ToList();

            // Cargar productos de la base de datos (incluyendo su tipo)
            var productosEnBd = await _context.Productos
                .Include(p => p.Tipo_Producto)
                .Where(p => nombres.Contains(p.Nombre))
                .ToListAsync();

            // Crear la compra
            var compra = new Compra_Producto(user, createMerch.DireccionEnvio, DateTime.Now, createMerch.MetodoPago, new List<Producto_Compra>())
            {
                CompraID = Guid.NewGuid().ToString()
            };

            float precioFinal = 0f;

            foreach (var item in createMerch.Items)
            {
                var producto = productosEnBd.FirstOrDefault(p => p.Nombre == item.Nombre);
                if (producto == null)
                {
                    ModelState.AddModelError("CreateMerch", $"Producto '{item.Nombre}' no encontrado.");
                    continue;
                }

                if (item.Cantidad <= 0)
                {
                    ModelState.AddModelError("CreateMerch", $"Cantidad inválida para '{item.Nombre}'.");
                    continue;
                }

                var productoCompra = new Producto_Compra
                {
                    Cantidad = item.Cantidad,
                    ProductoID = producto.Nombre,
                    PVP = producto.PVP,
                    Producto = producto,
                    Compra = compra,
                    CompraID = compra.CompraID
                };

                compra.Productos_Compras.Add(productoCompra);
                precioFinal += producto.PVP * item.Cantidad;
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            if (!compra.Productos_Compras.Any())
                return BadRequest("Ninguno de los productos indicados existe.");

            compra.PrecioFinal = precioFinal;

            _context.Compras.Add(compra);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la compra.");
                return Conflict("Error al guardar la compra: " + ex.Message);
            }

            return Ok(new
            {
                message = "Compra realizada correctamente.",
                precioFinal = compra.PrecioFinal,
                compraId = compra.CompraID
            });
        }
    }
}
