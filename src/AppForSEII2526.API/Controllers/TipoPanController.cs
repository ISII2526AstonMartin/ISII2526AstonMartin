using Microsoft.AspNetCore.Mvc;
namespace AppForSEII2526.API.Controllers
{
    public class TipoPanController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ILogger _logger;
        public TipoPanController(ApplicationDbContext context, ILogger<CompraBocadillo> logger)
        {
            _context = context;
            _logger = logger;
        }
        // GET: api/Movies/GetMoviesForPurchase

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<string>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetTipoPanes(string? tipoBocadilloString)
        {
            IList<string> tipos = await _context.TipoPan
                .Where(tipob => (tipoBocadilloString == null || tipob.Nombre.Contains(tipoBocadilloString)))
                .OrderBy(tipob => tipob.Nombre)
                .Select(tipob => tipob.Nombre)
                .ToListAsync();
            return Ok(tipos);
        }
    }
}
