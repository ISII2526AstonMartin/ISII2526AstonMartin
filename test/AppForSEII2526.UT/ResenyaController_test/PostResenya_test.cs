using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForMovies.UT;
using AppForSEII2526.API.DTOs.BocadillosResenyaDTOs;
using Moq;
using System.Net;
using Microsoft.AspNetCore.Mvc;


namespace AppForSEII2526.UT.ResenyaController_test
{
    public class PostResenya_test : AppForMovies4SqliteUT
    {
        private const string _nombreUsuario = "angellohor";


        private const string _nombreBocadillo1 = "Bocadillo1";
        private const string _tipoPan1 = "Integral";
        private const string _nombreBocadillo2 = "Bocadillo2";
        private const string _tipoPan2 = "Normal";


        public PostResenya_test()
        {
            var tipoPan = new List<TipoPan>()
            {
                new TipoPan(_tipoPan1),
                new TipoPan(_tipoPan2),

            };

            var bocadillos = new List<Bocadillo>()
            {
                new Bocadillo(_nombreBocadillo1, 2.5f, 5, Tamanyo.Pequeño, tipoPan[0],1),
                new Bocadillo(_nombreBocadillo2, 2.0f, 4, Tamanyo.Normal, tipoPan[1],2),
            };

            ApplicationUser user = new ApplicationUser("Angel", "Garcia", "Lopez", _nombreUsuario);

            var Resenya = new Resenya("titulo", "descripcion", new DateTime(2025, 11, 05),
                user, new List<ResenyaBocadillo>(), (Resenya.Valoracion_General)Valoracion_General.Cinco);

            Resenya.ResenyaBocadillo.Add(new ResenyaBocadillo(bocadillos[0], 9, Resenya));

            _context.AddRange(tipoPan);
            _context.AddRange(bocadillos);
            _context.Add(user);
            _context.Add(Resenya);
            _context.SaveChanges();

        }

        public static IEnumerable<object[]> TestCasesFor_CreateResenya()
        {
            var resenyaNoItems = new CreateResenyaDTO("angellohor", "titulo", "descripcion",
                (CreateResenyaDTO.Valoracion_General)Valoracion_General.Cinco, new List<ItemResenyaDTO>());

            var resenyaBocadillo = new List<ItemResenyaDTO>()
            {
                new ItemResenyaDTO(2, "Bocadillo2", 8, Tamanyo.Normal, 2.0f)
            };

            var resenyaNoUser = new CreateResenyaDTO(null, "titulo", "descripcion",
                (CreateResenyaDTO.Valoracion_General)Valoracion_General.Cuatro, resenyaBocadillo);

            var resenyaUser = new CreateResenyaDTO("angellohor", "titulo", "descripcion",
                (CreateResenyaDTO.Valoracion_General)Valoracion_General.Cinco, resenyaBocadillo);

            var resenyaBocadilloNotExist = new List<ItemResenyaDTO>()
            {
                new ItemResenyaDTO(999, "BocadilloNoExiste", 8, Tamanyo.Normal, 2.0f)
            };

            var resenyaBocadilloIdNotExist = new CreateResenyaDTO("angellohor", "titulo", "descripcion",
                (CreateResenyaDTO.Valoracion_General)Valoracion_General.Cinco, resenyaBocadilloNotExist);

            var allTests = new List<object[]>
            {
                // input for createResenya - Error expected
                new object[] { resenyaNoItems, "Debe incluir al menos un bocadillo en la reseña." },
                new object[] { resenyaBocadilloIdNotExist, "Error! El bocadillo con ID '999' no existe" },
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateResenya))]
        public async Task CreateResenya_Error_test(CreateResenyaDTO resenyaDTO, string errorExpected)
        {
            // Arrange
            var mock = new Mock<ILogger<CrearResenyaController>>();
            ILogger<CrearResenyaController> logger = mock.Object;

            var controller = new CrearResenyaController(_context, logger);

            // Act
            var result = await controller.CreateResenya(resenyaDTO);

            // Assert
            // we check that the response type is BadRequest and obtain the error returned
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            
            // Check if it's a ValidationProblemDetails (for structured errors)
            if (badRequestResult.Value is ValidationProblemDetails problemDetails)
            {
                var errorActual = problemDetails.Errors.First().Value[0];
                Assert.StartsWith(errorExpected, errorActual);
            }
            // Or if it's a simple string error message
            else if (badRequestResult.Value is string errorMessage)
            {
                Assert.StartsWith(errorExpected, errorMessage);
            }
            else
            {
                Assert.True(false, "Unexpected error response type");
            }
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateResenya_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CrearResenyaController>>();
            ILogger<CrearResenyaController> logger = mock.Object;

            var controller = new CrearResenyaController(_context, logger);

            var resenyaBocadillo = new List<ItemResenyaDTO>()
            {
                new ItemResenyaDTO(2, "Bocadillo2", 8, Tamanyo.Normal, 2.0f)
            };

            var resenyaDTO = new CreateResenyaDTO(_nombreUsuario, "titulo nuevo", "descripcion nueva",
                (CreateResenyaDTO.Valoracion_General)Valoracion_General.Cuatro, resenyaBocadillo);

            // Act
            var result = await controller.CreateResenya(resenyaDTO);

            // Assert
            // we check that the response type is CreatedAtAction
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualResenyaDetailDTO = Assert.IsType<DetailResenyaDTO>(createdResult.Value);

            var expectedResenyaDetailDTO = new DetailResenyaDTO(actualResenyaDetailDTO.Id, _nombreUsuario, 
                "titulo nuevo", "descripcion nueva", actualResenyaDetailDTO.FechaPublicacion,
                (CreateResenyaDTO.Valoracion_General)Valoracion_General.Cuatro, resenyaBocadillo);

            Assert.Equal(expectedResenyaDetailDTO, actualResenyaDetailDTO);
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
