using AppForSEII2526.API.DTOs;
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


    }
}
