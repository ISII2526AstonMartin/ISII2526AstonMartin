using AppForSEII2526.API.DTOs.BonosDTOs;
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

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(CompraBonoDetailsDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetCompras(string id)
        {
            if (id == null)
            {
                return BadRequest("id no valido");
            }
            if (_context.ComprasBono == null)
            {
                return NotFound("Base de datos vacía");
            }
            var compras= await _context.ComprasBono
                .Where(cb=>cb.CompraBonoId==id)
                    .Include(cb=> cb.ListaBonosComprados)
                        .ThenInclude(cbb=>cbb.BonoBocadillo)
                            .ThenInclude(cbb=>cbb.Tipo)
                .Select(cb=>new CompraBonoDetailsDTO(cb.CompraBonoId, 
                    cb.applicationuser.Nombre, cb.applicationuser.Apellido1, cb.applicationuser.Apellido2, 
                    cb.MetodoPagoUsuario, cb.FechaCompraBono, 
                    cb.ListaBonosComprados
                        .Select(cbb=> new CompraBonoItemDTO(cbb.BonoId, cbb.BonoBocadillo.NombreBono, cbb.BonoBocadillo.PVP, 
                        cbb.BonoBocadillo.NBocadillos, cbb.BonoBocadillo.Tipo.NombreTipo, cbb.Cantidad)).ToList<CompraBonoItemDTO>()
                    )).FirstOrDefaultAsync();
            if (compras == null)
            {
                return NotFound("No se han encontrado compras");
            }
            return Ok(compras);
        }
        /*
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(CompraBonoItemDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateCompra(CompraBonoForCreateDTO dto)
        {

        }
        */
    }
}
