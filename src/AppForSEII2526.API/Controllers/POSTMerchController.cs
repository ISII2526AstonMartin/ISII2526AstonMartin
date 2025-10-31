using AppForSEII2526.API.DTOs.ComprarMerchDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        // [ProducesResponseType(typeof(MerchDetailDTO), (int)HttpStatusCode.Created)] COMENTADA PORQUE AUN NO ESTA DEFINIDO MerchDetailDTO
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> CreateMerch([FromBody] CreateMerchDTO createMerch)
        {
            if (createMerch.items.Count == 0)
                ModelState.AddModelError("CreateMerch", "Debes incluir al menos un producto");

            // if (!_context.ApplicationUsers.Any(au=>au.UserName==rentalForCreate.CustomerUserName))
            var user = _context.ApplicationUsers.FirstOrDefault(au => au.UserName == createMerch.NombreUsuario && au.Apellido1 == createMerch.Apellido1);
            if (user == null)
                ModelState.AddModelError("CreateMerch", "Error! UserName or surname are not registered");
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));
            if (user == null)
                ModelState.AddModelError("RentalApplicationUser", "Error! UserName is not registered");
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));
            var idProductos = createMerch.items.Select(i => i.Nombre).ToList();

            var Productos = _context.Productos
                .Where(p => idProductos.Contains(p.Nombre))
                .Select(p => new
                {
                    Nombre = p.Nombre,
                    PVP = p.PVP,
                    TipoProducto = p.Tipo_Producto


                })
                .ToList();

            Compra_Producto compra_Producto = new Compra_Producto(user, createMerch.DireccionEnvio, DateTime.Now, createMerch.metodoPago, new List<Producto_Compra>());
            float precioFinal = 0;
            foreach (var item in createMerch.items)
            {
                var producto = Productos.FirstOrDefault(p => p.Nombre == item.Nombre);
                if (producto != null)
                {
                    Producto_Compra producto_Compra = new Producto_Compra
                    {
                        Cantidad = item.Cantidad,
                        ProductoID = producto.Nombre,
                        PVP = producto.PVP,
                        Producto = new Producto
                        {
                            Nombre = producto.Nombre,
                            PVP = producto.PVP,
                            Tipo_Producto = producto.TipoProducto
                        },
                        Compra = compra_Producto
                    };
                    compra_Producto.Productos_Compras.Add(producto_Compra);
                    precioFinal += producto.PVP * item.Cantidad;
                }
                compra_Producto.PrecioFinal = precioFinal;


                if (compra_Producto.Productos_Compras.Count == 0)
                {
                    return Conflict("Ninguno de los productos indicados existe");

                }
                if (ModelState.ErrorCount > 0)
                {
                    return BadRequest(new ValidationProblemDetails(ModelState));    
                }
                _context.Compras.Add(compra_Producto);
                try
                {
                    await _context.SaveChangesAsync();
                } catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    ModelState.AddModelError("Compra_producto", "Error al guardar la compra en la base de datos");
                    return Conflict("Error" + ex.Message);
                }
        }
}
