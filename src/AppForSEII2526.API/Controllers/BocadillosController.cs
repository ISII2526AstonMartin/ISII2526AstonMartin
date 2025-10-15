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
        [ProducesResponseType(typeof(decimal),(int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetDivision(decimal op1, decimal op2)
        {
           if (op2 == 0)
            {
                _logger.LogError($"{DateTime.Now} Exception: op2=0, division by 0");
                return BadRequest("op2 must be different from 0");
            }
            decimal result = decimal.Round(op1 / op2, 2);
            return Ok(result);
        }
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<Bocadillo>), (int)HttpStatusCode.OK)]
            public async Task<IActionResult> GetAllBocadillos()
        {
            IList<Bocadillo > bocadillos = await _context.Bocadillo
                .ToListAsync();
                return Ok(bocadillos);  
        }

    }
}
