using Microsoft.AspNetCore.Mvc;
namespace AppForSEII2526.API.Controllers
{
    public class TipoBocadillosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ILogger _logger;
        public TipoBocadillosController(ApplicationDbContext context, ILogger<BonosComprados> logger)
        {
            _context = context;
            _logger = logger;
        }
        // GET: api/Movies/GetMoviesForPurchase

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<string>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetTiposBocadillos(string? tipoBocadilloString)
        {
            IList<string> tipos = await _context.TiposBocadillos
                .Where(tipob => (tipoBocadilloString == null || tipob.NombreTipo.Contains(tipoBocadilloString)))            
                .OrderBy(tipob => tipob.NombreTipo)
                .Select(tipob => tipob.NombreTipo)
                .ToListAsync();
            return Ok(tipos);
        }
    }
}