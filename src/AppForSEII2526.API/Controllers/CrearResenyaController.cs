
using AppForSEII2526.API.DTOs.BocadillosResenyaDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrearResenyaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CrearResenyaController> _logger;

        public CrearResenyaController(ApplicationDbContext context, ILogger<CrearResenyaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(DetailResenyaDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetResenya(int id)
        {

            if (_context.Resenyas == null)
            {
                _logger.LogError("Error: Rentals table does not exist");
                return NotFound();
            }

            var resenya = await _context.Resenyas
                .Where(r => r.Id == id)
                .Include(r => r.ResenyaBocadillo)
                .ThenInclude(rb => rb.Bocadillo)
                .ThenInclude(b => b.TipoPan)
                .Select(r => new DetailResenyaDTO(id, r.ApplicationUser != null ? r.ApplicationUser.NombreUsuario : "Usuario desconocido"
                , r.Titulo, 
                r.Descripcion, r.FechaPublicacion, (CreateResenyaDTO.Valoracion_General)r.ValoracionGeneral,
                r.ResenyaBocadillo.Select(
                    rb => new ItemResenyaDTO(rb.BocadilloId, rb.Bocadillo.Nombre, rb.Puntuacion, rb.Bocadillo.Tamanyo, rb.Bocadillo.PVP
                )).ToList()
                ))
            .FirstOrDefaultAsync();


            if (resenya == null)
            {
                _logger.LogError($"Error: Rental with id {id} does not exist");
                return NotFound();
            }

            return Ok(resenya);

        }














        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(DetailResenyaDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateResenya(CreateResenyaDTO createResenyaDTO)
        {
            if (createResenyaDTO.items.Count == 0)
            {
                return BadRequest("Debe incluir al menos un bocadillo en la reseña.");
            }

            // Validar que la descripción no está vacía o nula
            if (string.IsNullOrWhiteSpace(createResenyaDTO.descripcion))
            {
                return BadRequest("La descripción es obligatoria");
            }

            // Validar que la valoración general es válida
            if (!Enum.IsDefined(typeof(CreateResenyaDTO.Valoracion_General), createResenyaDTO.valoracion_General))
            {
                return BadRequest("La valoración general no es válida");
            }

            var user = _context.ApplicationUsers.FirstOrDefault(au => au.NombreUsuario == createResenyaDTO.NombreUsuario);


            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var bocadillosIds = createResenyaDTO.items.Select(i => i.BocadilloId).ToList();

            var bocadillos = _context.Bocadillo
                .Where(b => bocadillosIds.Contains(b.Id))
                .Select(b => new
                {
                    b.Id,
                    b.Nombre,
                    b.Tamanyo,
                    b.PVP
                })
                .ToList();


            if (!createResenyaDTO.Titulo.StartsWith("Sugerencia para"))
            {
                return BadRequest("Error!, el título de la reseña debe empezar por sugerencia para");
            }

            Resenya resenya = new Resenya
                (
                    createResenyaDTO.Titulo,
                    createResenyaDTO.descripcion,
                    DateTime.Now,
                    user,
                    new List<ResenyaBocadillo>(),
                    (Resenya.Valoracion_General)createResenyaDTO.valoracion_General
                );

            foreach (var item in createResenyaDTO.items)
            {
                var bocadillo = bocadillos.FirstOrDefault(b => b.Id == item.BocadilloId);
                if (bocadillo == null)
                {
                    ModelState.AddModelError("Items", $"Error! El bocadillo con ID '{item.BocadilloId}' no existe");
                }

                else
                { 
                    resenya.ResenyaBocadillo.Add(new ResenyaBocadillo(bocadillo.Id, item.puntuacion, resenya));
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(resenya);

            
            try
            {
                //we store in the database both rental and its rentalitems
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Rental", $"Error! There was an error while saving your rental, plese, try again later");
                return Conflict("Error" + ex.Message);

            }



            var resenyaDetail = new DetailResenyaDTO(resenya.Id, resenya.Titulo,
                    resenya.Descripcion, resenya.FechaPublicacion, (CreateResenyaDTO.Valoracion_General)resenya.ValoracionGeneral,
                    createResenyaDTO.items);

            if (user != null)
            {
                resenyaDetail.NombreUsuario = resenya.ApplicationUser.NombreUsuario;
            }
                




                return CreatedAtAction("GetResenya", new { id = resenya.Id }, resenyaDetail);


        }
    }
}
