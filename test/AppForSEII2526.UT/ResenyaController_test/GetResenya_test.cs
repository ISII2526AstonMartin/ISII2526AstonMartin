using AppForMovies.UT;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.DTOs.BocadillosResenyaDTOs;


namespace AppForSEII2526.UT.ResenyaController_test
{
    public class GetResenya_test : AppForMovies4SqliteUT
    {
        public GetResenya_test()
        {
            var TipoPan = new List<TipoPan>()
            {
                new TipoPan("Normal"),
                new TipoPan("Integral")
            };

            var bocadillos = new List<Bocadillo>()
            {
                new Bocadillo(1, "Serrano", 5.5f, 5, Tamanyo.Normal, TipoPan[0]),
                new Bocadillo(2, "Bacon", 3f, 6, Tamanyo.Pequeño, TipoPan[1])
            };

            ApplicationUser user = new ApplicationUser("Angel", "Lopez", "Hortelano", "angellohor");

            var Resenya = new Resenya("titulo1", "descripcion1", new DateTime(2025, 11, 04, 12, 30, 00), user, new List<ResenyaBocadillo>()
            , (Resenya.Valoracion_General)Valoracion_General.Tres);
            

            Resenya.ResenyaBocadillo.Add(new ResenyaBocadillo(bocadillos[0], 9, Resenya));

            _context.AddRange(TipoPan);
            _context.AddRange(bocadillos);
            _context.Add(user);
            _context.Add(Resenya);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetResenya_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CrearResenyaController>>();
            ILogger<CrearResenyaController> logger = mock.Object;

            var controller = new CrearResenyaController(_context, logger);

            // Act
            var result = await controller.GetResenya(0);

            //Assert
            //we check that the response type is OK and obtain the list of movies
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetResenya_Found_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CrearResenyaController>>();
            ILogger<CrearResenyaController> logger = mock.Object;
            var controller = new CrearResenyaController(_context, logger);


            var expectedResenya = new DetailResenyaDTO(1, "angellohor", "titulo1", "descripcion1", new DateTime(2025, 11, 04, 12, 30, 00),
                        (CreateResenyaDTO.Valoracion_General)Valoracion_General.Tres, new List<ItemResenyaDTO>());
            expectedResenya.items.Add(new ItemResenyaDTO(1, "Serrano", 9, Tamanyo.Normal, 5.5f));

            // Act 
            var result = await controller.GetResenya(1);

            //Assert
            //we check that the response type is OK and obtain the rental
            var okResult = Assert.IsType<OkObjectResult>(result);
            var ResenyaDTOActual = Assert.IsType<DetailResenyaDTO>(okResult.Value);
            var eq = expectedResenya.Equals(ResenyaDTOActual);
            //we check that the expected and actual are the same
            Assert.Equal(expectedResenya, ResenyaDTOActual);

        }








        public enum Valoracion_General
        {
            Uno,
            Dos,
            Tres,
            Cuatro,
            Cinco
        }
    }
}
