using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            this._context = context;
            this._logger = logger;
        }
       
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<Bocadillo>), (int)HttpStatusCode.OK)]
            public async Task<IActionResult> GetBocadillosParaPedir()
        {
            IList<Bocadillo > bocadillos = await _context.Bocadillo
                .ToListAsync();
                return Ok(bocadillos);  
        }

    }
}
