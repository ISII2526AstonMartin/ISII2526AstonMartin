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
        private readonly ILogger<BocadillosController> _logger;
        public BonosController(ApplicationDbContext context, ILogger<BocadillosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<Bocadillo>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBonos()
        {
            IList<BonoBocadillo> bonobocadillos = await _context.BonoBocadillos
                .ToListAsync();
            return Ok(bonobocadillos);
        }
    }
}
