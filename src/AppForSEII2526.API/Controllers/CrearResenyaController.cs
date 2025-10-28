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



        [HttpPost]
        [Route("[action]")]
        //[ProducesResponseType(typeof(RentalDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateResenya(CreateResenyaDTO createResenyaDTO)
        {
            if (createResenyaDTO.items.Count == 0)
            {
                return BadRequest("Debe incluir al menos un bocadillo en la reseña.");
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

        }
    }
}
