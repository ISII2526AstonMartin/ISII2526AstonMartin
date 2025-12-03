using AppForSEII2526.API.DTOs.CompraBonosDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompraBonosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CompraBonosController> _logger;

        public CompraBonosController(ApplicationDbContext context, ILogger<CompraBonosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(CompraBonoDetailsDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetCompra(int id)
        {
            var compraBono = await _context.ComprasBono
                .Where(cb=>cb.CompraBonoId == id)
                    .Include(cb=>cb.ListaBonosComprados)
                        .ThenInclude(b=>b.BonoBocadillo)
                            .ThenInclude(bono=>bono.Tipo)
                .Select(cb=> new CompraBonoDetailsDTO(
                    cb.CompraBonoId, cb.FechaCompraBono,
                    cb.applicationuser.Nombre, cb.applicationuser.Apellido1, cb.applicationuser.Apellido2,
                    (MetodoPago)cb.MetodoPagoUsuario,
                    cb.ListaBonosComprados.Select(bc=> new CompraBonoItemDTO(
                        bc.BonoId,bc.PrecioBono, bc.BonoBocadillo.NBocadillos, 
                        bc.BonoBocadillo.NombreBono,bc.BonoBocadillo.Tipo.NombreTipo,
                        bc.Cantidad)).ToList<CompraBonoItemDTO>()
                    ))
                .FirstOrDefaultAsync();
            if (compraBono == null)
            {
                return NotFound("No se han encontrado compras");
            }
            return Ok(compraBono);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(CompraBonoDetailsDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateCompra(CompraBonoForCreateDTO dto)
        {
            
            if(dto.nombreCliente.IsNullOrEmpty())
            {
                return BadRequest("El nombre no está definido");
            } else if (dto.apellido1.IsNullOrEmpty()) 
            {
                return BadRequest("El apellido no está definido");
            }else if (!Enum.IsDefined(typeof(MetodoPago), dto.metodoPago))
            {
                return BadRequest("Metodo de pago no valido");
            }

                var user = _context.ApplicationUsers.FirstOrDefault(au =>
                (au.Nombre == dto.nombreCliente) &&
                (au.Apellido1 == dto.apellido1) &&
                (dto.apellido2.IsNullOrEmpty() || au.Apellido2 == dto.apellido2)
                );

            if (user == null)
            {
                return BadRequest("Cliente no registrado");
            }

            var metodoPago = dto.metodoPago;

            var bonosNombres = dto.compraItems.Select(ri => ri.nombreBono).ToList();

            var bonos = _context.BonoBocadillos
                    .Where(bc => bonosNombres.Contains(bc.NombreBono))
                    .Select(m => new
                    {
                        m.BonoID,
                        m.PVP,
                        m.NBocadillos,
                        m.NombreBono,
                        m.Tipo,
                        m.CantidadDisponible
                    }).ToList();

            CompraBono comprabono = new CompraBono(DateTime.Today, dto.compraItems.Count(), 0, metodoPago, new List<BonosComprados>(), user);

            foreach (var item in dto.compraItems)
            {
                var bono = bonos.FirstOrDefault(b => b.NombreBono == item.nombreBono);
                if (bono == null)
                {
                    return BadRequest("Bono no existe");
                }else if (bono.CantidadDisponible==0)
                {
                    return BadRequest("No se puede tener cantidad 0 en un bono"); 
                }

                else
                {
                    comprabono.ListaBonosComprados.Add(new BonosComprados(bono.BonoID, comprabono, item.cantidad, bono.PVP));
                    item.pvp = bono.PVP;
                }
            }

            comprabono.PrecioTotalBono = comprabono.ListaBonosComprados.Sum(cb=>cb.PrecioBono*cb.Cantidad);
            comprabono.NBono = comprabono.ListaBonosComprados.Sum(cb => cb.Cantidad);

            //MODIFICACIÓN DE EXAMEN SPRINT 2
            if (comprabono.PrecioTotalBono <= 3)
            {
                return BadRequest("Error!, El precio del bono debe ser mayor que 3");
            }

            _context.Add(comprabono);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Rental", $"Error! There was an error while saving your rental, plese, try again later");
                return Conflict("Error" + ex.Message);
            }

            var compraItems = comprabono.ListaBonosComprados.Select(bc =>
            {
                var bono = bonos.First(b => b.BonoID == bc.BonoId);
                return new CompraBonoItemDTO(
                    bc.BonoId,
                    (float)bono.PVP,
                    bono.NBocadillos,
                    bono.NombreBono,
                    bono.Tipo.NombreTipo,
                    bc.Cantidad
                );
            }).ToList();

            var compraBonoDetail = new CompraBonoDetailsDTO(comprabono.CompraBonoId, comprabono.FechaCompraBono,
                comprabono.applicationuser.Nombre, comprabono.applicationuser.Apellido1, comprabono.applicationuser.Apellido2,
                comprabono.MetodoPagoUsuario,compraItems);
            
            return CreatedAtAction("GetCompra", new { id = comprabono.CompraBonoId }, compraBonoDetail);
        }
    }
}
