using AppForSEII2526.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BocadillosController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<BocadillosController> _logger;

        public BocadillosController(ApplicationDbContext context, ILogger<BocadillosController> logger)
        {
            _context = context;
            _logger = logger;
        }




        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<BocadillosDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBocadillosParaPedir(Tamanyo? tamanyo, string? tipoPan)
        {
            IList<BocadillosDTO> bocadillos = await _context.Bocadillo
                .Include(b => b.TipoPan)
                .Where(b =>

                (tipoPan == null || b.TipoPan.Nombre.Contains(tipoPan))


                && (tamanyo == null || b.Tamanyo == tamanyo))


                .Select(b => new BocadillosDTO
                {
                    Id = b.Id,
                    Nombre = b.Nombre,
                    Tamanyo = b.Tamanyo,
                    TipoPan = b.TipoPan.Nombre,
                    PVP = b.PVP,
                })

            .ToListAsync();
            return Ok(bocadillos);

        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<BocadillosDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBocadillosResenya(string? nombre, float? PVP)
        {
            IList<BocadillosDTO> bocadillos = await _context.Bocadillo
                .Include(b => b.TipoPan)
                .Where(b =>
                    (nombre == null || b.Nombre.Contains(nombre)) &&
                    (!PVP.HasValue || b.PVP <= PVP.Value)
                )
                .Select(b => new BocadillosDTO
                {
                    Id = b.Id,
                    Nombre = b.Nombre,
                    Tamanyo = b.Tamanyo,
                    TipoPan = b.TipoPan.Nombre,
                    PVP = b.PVP
                })









































                .ToListAsync();

            if (bocadillos.Count == 0)
            {
                _logger.LogInformation("No hay bocadillos con esos filtros");
                return NotFound("No hay bocadillos con esos filtros");
            }
                return Ok(bocadillos);  
        }


        
    }
}
