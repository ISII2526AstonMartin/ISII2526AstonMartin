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
        [ProducesResponseType(typeof(IList<Bocadillo>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetBocadillosforResenya()
        {
            IList<Bocadillo> bocadillos = await _context.Bocadillo
                .ToListAsync();
            return Ok(bocadillos);
        }
    }
}
