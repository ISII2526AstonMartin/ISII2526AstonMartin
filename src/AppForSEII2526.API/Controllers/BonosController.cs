using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.DTOs.CompraBonosDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BonosController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<BonosController> _logger;
        public BonosController(ApplicationDbContext context, ILogger<BonosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<BonoBocadillosDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType ((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetBonos(string? nombre, string? nombretipo)
        {
            List<BonoBocadillosDTO> bonobocadillos = await _context.BonoBocadillos
                .Include(bb=>bb.Tipo)
                .Where(bb=>
                (nombre==null || bb.NombreBono.Contains(nombre)) && 
                (nombretipo==null || bb.Tipo.NombreTipo.Contains(nombretipo)))
                .Select(bb=> 
                new BonoBocadillosDTO(bb.BonoID, bb.CantidadDisponible, bb.NBocadillos, bb.NombreBono, bb.PVP, bb.Tipo.NombreTipo)
                )
                .ToListAsync();

            if (bonobocadillos.Count() == 0)
            {
                return NotFound("No hay bocadillos con esos filtros");
            }

            return Ok(bonobocadillos);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(CompraBonoDetailsDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateCompra(CompraBonoForCreateDTO dto)
        {
            var user= _context.ApplicationUsers.FirstOrDefault(au=>
            (au.Nombre == dto.nombreCliente)&&
            (au.Apellido1 == dto.apellido1)&&
            (au.Apellido2 == dto.apellido2)
            );

            if (user == null)
            {
                return BadRequest("Cliente no registrado");
            }
            var metodoPago = _context.ComprasBono.FirstOrDefault(bc=>
            bc.MetodoPagoUsuario==dto.metodoPago
            );

            if (metodoPago == null)
            {
                return BadRequest("Metodo de pago no registrado");
            }

            var bonosNombres = dto.compraItems.Select(ri=>ri.nombreBocadillo).ToList<string>();

            var bonos = _context.BonoBocadillos
                .Include(bc => bc.ListaBonosComprados)
                    .ThenInclude(c=>c.Comprabono)
                    .Where(bc=>bc.NombreBono.Contains(bc.NombreBono))
                    .Select(m=> new 
                    {
                        m.BonoID,
                        m.PVP,
                        m.NBocadillos,
                        m.NombreBono,
                        m.Tipo,
                        m.CantidadDisponible
                    })
                    .ToList();

            CompraBono comprabono = new CompraBono(dto.nombreCliente, DateTime.Today, dto.compraItems.Count(), 0, dto.metodoPago, new List<BonosComprados>(), user);

            foreach (var item in dto.compraItems)
            {
                var bono = bonos.FirstOrDefault(b => b.NombreBono == item.nombreBocadillo);
                if (bono == null)
                {
                    return BadRequest("Bono no existe");
                }
                
                else
                {
                    comprabono.ListaBonosComprados.Add(new BonosComprados(bono.BonoID, comprabono.CompraBonoId, item.cantidad, bono.PVP));
                    comprabono.PrecioTotalBono += bono.PVP;
                }
            }
            _context.Add(comprabono);
            try
            {
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Rental", $"Error! There was an error while saving your rental, plese, try again later");
                return Conflict("Error" + ex.Message);
            }

            var compraBonoDetail = new CompraBonoDetailsDTO(comprabono.CompraBonoId, comprabono.FechaCompraBono, 
                comprabono.applicationuser.Nombre, comprabono.applicationuser.Apellido1, comprabono.applicationuser.Apellido2,
                comprabono.MetodoPagoUsuario,
                comprabono.ListaBonosComprados.Select(bc => new CompraBonoItemDTO(
                    bc.BonoId.ToString(),
                    (float)bc.BonoBocadillo.PVP,
                    bc.BonoBocadillo.NBocadillos,
                    bc.BonoBocadillo.NombreBono,
                    bc.BonoBocadillo.Tipo.NombreTipo,
                    bc.Cantidad
                    )).ToList()
                );

            return CreatedAtAction("GetCompra", new { id = comprabono.CompraBonoId }, comprabono);
        }
    }
}
