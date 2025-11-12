using AppForMovies.UT;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.Controllers;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace AppForSEII2526.UT.ResenyaController_test
{
    public class GetBocadillosResenya_test : AppForMovies4SqliteUT
    {
        public GetBocadillosResenya_test()
        {


            var tipoPan = new List<TipoPan>()
            {
                new TipoPan(1, "Integral"),
                new TipoPan(2, "Normal"),
                new TipoPan(3, "Centeno")
            };
            var bocadillos = new List<Bocadillo>()
            {
                new Bocadillo(1, "Serrano", 5.5f, 10,Tamanyo.Pequeño, tipoPan[0]),
                new Bocadillo(2, "BaconQueso", 6, 10,Tamanyo.Normal, tipoPan[1]),
                new Bocadillo(3, "Bacon", 2, 10,Tamanyo.Pequeño, tipoPan[2]),
                new Bocadillo(4, "Atun", 3, 10,Tamanyo.Normal, tipoPan[0])
            };

            ApplicationUser user = new ApplicationUser("usuario1", "apellido1", "apellido2", "nombreusuario1");

            _context.AddRange(tipoPan);
            _context.AddRange(bocadillos);
            _context.Add(user);
            _context.SaveChanges();
        }


        public static IEnumerable<object[]> TestCasesFor_GetBocadillos_OK()
        {
            var bocadillosDTOs = new List<BocadillosDTO>
            {
                new BocadillosDTO(1, "Serrano", Tamanyo.Pequeño, "Integral", 5.5f),
                new BocadillosDTO(2, "BaconQueso", Tamanyo.Normal, "Normal", 6f),
                new BocadillosDTO(3, "Bacon", Tamanyo.Pequeño, "Centeno", 2f),
                new BocadillosDTO(4, "Atun", Tamanyo.Normal, "Integral", 3f)
            };

            var bocadillosDTOsTC1 = new List<BocadillosDTO>()
            {
                bocadillosDTOs[0],
                bocadillosDTOs[1],
                bocadillosDTOs[2],
                bocadillosDTOs[3]
            };

            var bocadillosDTOsTC2 = new List<BocadillosDTO>()
            {
                bocadillosDTOs[1],
                bocadillosDTOs[2]
            };

            var bocadillosDTOsTC3 = new List<BocadillosDTO>()
            {
                bocadillosDTOs[2],
                bocadillosDTOs[3]
            };

            var bocadillosDTOsTC4 = new List<BocadillosDTO>()
            {
                 bocadillosDTOs[2],
            };

            var allTests = new List<object[]>
            {
                new object[] {null, null, bocadillosDTOsTC1 },
                new object[] {"Bacon",null, bocadillosDTOsTC2 },
                new object[] {null, 5f, bocadillosDTOsTC3 },
                new object[] {"Bacon", 4f, bocadillosDTOsTC4 }
            };

            return allTests;


        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetBocadillos_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetBocadillos_OK(string? nombreBocadillo, float? precioMaximo, List<BocadillosDTO> expectedBocadillos)
        {
            // Arrange
            var mock = new Mock<ILogger<BocadillosController>>();
            ILogger<BocadillosController> logger = mock.Object;
            var controller = new BocadillosController(_context, logger);
            var result = await controller.GetBocadillosResenya(nombreBocadillo, precioMaximo);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBocadillos = Assert.IsAssignableFrom<IEnumerable<BocadillosDTO>>(okResult.Value);
            Assert.Equal(expectedBocadillos.Count, returnedBocadillos.Count());

        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetBocadillos_NotFound()
        {
            var mock = new Mock<ILogger<BocadillosController>>();
            ILogger<BocadillosController> logger = mock.Object;
            var controller = new BocadillosController(_context, logger);

            var nombreBocadillo = "NoExiste";
            var precioMaximo = 1f;
            var result = await controller.GetBocadillosResenya(nombreBocadillo, precioMaximo);


            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No hay bocadillos con esos filtros", notFoundResult.Value);

        }
    }
}
