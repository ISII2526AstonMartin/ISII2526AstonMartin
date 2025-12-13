using AppForSEII2526.API.DTOs.ComprarMerchDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class POSTMerchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<POSTMerchController> _logger;

        public POSTMerchController(ApplicationDbContext context, ILogger<POSTMerchController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Obtiene el detalle de una compra específica por ID
        [HttpGet]
        [Route("[action]")] // Cambiado: se quitó el {id} de la ruta
        [ProducesResponseType(typeof(DetailMerchDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetMerchDetail(int id) // El id viene como parámetro de query
        {
            // Verificar si la tabla de Compras existe en la base de datos
            if (_context.Compras == null)
            {
                _logger.LogError("Error: La tabla de Compras no existe en la base de datos.");
                return NotFound();
            }

            // Buscar la compra con el ID proporcionado, incluyendo las relaciones necesarias
            var compra = await _context.Compras
                .Where(c => c.CompraID == id)
                .Include(c => c.Usuario)
                .Include(c => c.Productos_Compras)
                    .ThenInclude(pc => pc.Producto)
                        .ThenInclude(p => p.Tipo_Producto)
                .FirstOrDefaultAsync();

            // Si no se encuentra la compra, retornar NotFound
            if (compra == null)
            {
                _logger.LogError($"Error: La compra con ID {id} no existe.");
                return NotFound();
            }

            // Mapear los productos de la compra a ItemMerchDTO
            var items = compra.Productos_Compras.Select(pc => new ItemMerchDTO(
                pc.Producto.Nombre,
                pc.PVP,
                pc.Producto.Tipo_Producto.Nombre,
                pc.Cantidad
            )).ToList();

            // Crear el DTO de detalle con toda la información de la compra
            var detalle = new DetailMerchDTO(
                compra.Usuario.NombreUsuario,
                compra.Usuario.Apellido1,
                compra.Usuario.Apellido2,
                compra.Direccion_Envio,
                compra.Metodo_Pago,
                items,
                compra.CompraID,
                compra.FechaCompra,
                compra.PrecioFinal
            );

            return Ok(detalle);
        }

        // POST: Crea una nueva compra de merchandising
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(DetailMerchDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateMerch(CreateMerchDTO createMerch)
        {
            // Validar que el objeto recibido no sea nulo
            if (createMerch == null)
                return BadRequest("El cuerpo de la solicitud está vacío.");

            // Validar que la compra contenga al menos un producto
            if (createMerch.Items == null || createMerch.Items.Count == 0)
            {
                ModelState.AddModelError("CreateMerch", "Debes incluir al menos un producto.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Validar que la direccion de envio no es nula y no contiene la palabra "Calle" //EXAMEN
            if (createMerch.DireccionEnvio == null || !createMerch.DireccionEnvio.Contains("Calle"))
            {
                // Devolvemos un bad request.
                ModelState.AddModelError("CreateMerch", "Error!, por favor introduce una dirección de envío válida");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Buscar el usuario en la base de datos
            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(au =>
                    au.UserName == createMerch.NombreUsuario &&
                    au.Apellido1 == createMerch.Apellido1);

            // Si el usuario no existe, retornar error
            if (user == null)
            {
                ModelState.AddModelError("CreateMerch", "Error: Usuario o apellido no registrados.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Obtener los nombres de los productos para buscar en la base de datos
            var nombres = createMerch.Items.Select(i => i.Nombre).ToList();

            // Buscar los productos en la base de datos
            var productosEnBd = await _context.Productos
                .Include(p => p.Tipo_Producto)
                .Where(p => nombres.Contains(p.Nombre))
                .ToListAsync();

            // Crear una nueva compra con ID único

            var compra = new Compra_Producto(user, createMerch.DireccionEnvio, DateTime.Today, createMerch.MetodoPago, new List<Producto_Compra>());

            float precioFinal = 0f;

            // Procesar cada item de la compra
            foreach (var item in createMerch.Items)
            {
                // Validar que la cantidad sea válida
                if (item.Cantidad <= 0)
                {
                    ModelState.AddModelError("CreateMerch", $"Cantidad inválida para '{item.Nombre}'.");
                    continue;
                }

                // Buscar el producto en la lista de productos de la base de datos
                var producto = productosEnBd.FirstOrDefault(p => p.Nombre == item.Nombre);
                if (producto == null)
                {
                    ModelState.AddModelError("CreateMerch", $"Producto '{item.Nombre}' no encontrado.");
                    continue;
                }

                // Crear la relación Producto_Compra
                var productoCompra = new Producto_Compra
                {
                    Cantidad = item.Cantidad,
                    ProductoID = producto.ProductoID,
                    PVP = producto.PVP,
                    Producto = producto,
                    Compra = compra,
                    CompraID = compra.CompraID
                };

                // Agregar el producto a la compra y calcular el precio
                compra.Productos_Compras.Add(productoCompra);
                precioFinal += producto.PVP * item.Cantidad;
            }

            // Si hay errores de validación, retornar BadRequest
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // Validar que al menos un producto fue agregado a la compra
            if (!compra.Productos_Compras.Any())
                return BadRequest("Ninguno de los productos indicados existe.");

            // Asignar el precio final calculado a la compra
            compra.PrecioFinal = precioFinal;

            // Agregar la compra al contexto
            _context.Compras.Add(compra);

            try
            {
                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log del error y retornar Conflict si hay problemas al guardar
                _logger.LogError(ex, "Error al guardar la compra.");
                return Conflict("Error al guardar la compra: " + ex.Message);
            }

            // Crear el DTO de respuesta con los detalles de la compra creada
            var merchDetail = new DetailMerchDTO(
                createMerch.NombreUsuario!,
                createMerch.Apellido1,
                createMerch.Apellido2,
                createMerch.DireccionEnvio,
                createMerch.MetodoPago,
                createMerch.Items,
                compra.CompraID,
                DateTime.Today, // Cambiado: DateTime.Now por DateTime.Today
                compra.PrecioFinal
            );

            // Retornar respuesta Created con referencia al endpoint GetMerchDetail
            return CreatedAtAction("GetMerchDetail", new { id = compra.CompraID }, merchDetail);
        }
    }
}