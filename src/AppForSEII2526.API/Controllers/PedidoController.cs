using AppForSEII2526.API.DTOs.BocadillosParaPedirDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<PedidoController> _logger;

        public PedidoController(ApplicationDbContext context, ILogger<PedidoController> logger)
        {
            _context = context;
            _logger = logger;
        }



        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ItemPedidoDTO), (int)HttpStatusCode.Created)]
        //[ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]

        public async Task<ActionResult> CreatePedido(CreatePedidoDTO pedidoParaCrear)
        {
            var usuario = _context.ApplicationUsers.FirstOrDefault(au => au.Nombre  == pedidoParaCrear.Nombre && au.Apellido1==pedidoParaCrear.Apellido1);
            if ( usuario == null)
                ModelState.AddModelError("RentalApplicationUser", "Error! Usuario no registrado");

            var metodoPago = _context.Compra.FirstOrDefault(mp => mp.MetodoPago == pedidoParaCrear.MetodoPago);
            if (metodoPago == null)
                ModelState.AddModelError("MetodoPago", "Error! Metodo de pago no registrado");


            var pedidoNombre = pedidoParaCrear.ItemPedido.Select(ri => ri.NombreBocadillo).ToList<string>();
            
            var bocadillos = _context.Bocadillo

                .Where(m=> pedidoNombre.Contains(m.Nombre))
                .Include(b => b.TipoPan
                ).ToList();



            Compra compra = new Compra(DateTime.Now, new List<CompraBocadillo>(), pedidoParaCrear.MetodoPago, usuario);
            compra.PrecioTotal = 0;


            foreach(var item in pedidoParaCrear.ItemPedido)
            {
                var bocadillo= bocadillos.FirstOrDefault(p=> p.Nombre == item.NombreBocadillo);
                if (bocadillo == null)
                {
                    ModelState.AddModelError("Bocadillo", $"Error! El bocadillo {item.NombreBocadillo} no está disponible");
                    return ValidationProblem(ModelState);
                }
                else
                {
                    compra.CompraBocadillos.Add(new CompraBocadillo(bocadillo, compra, item.Cantidad));
                    item.PVP= bocadillo.PVP;
                }
                compra.PrecioTotal = compra.CompraBocadillos.Sum(cb => cb.Precio * cb.Cantidad);
            }










        }

    }
}

