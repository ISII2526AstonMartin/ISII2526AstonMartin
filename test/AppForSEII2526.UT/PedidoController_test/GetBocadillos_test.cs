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

namespace AppForSEII2526.UT.PedidoController_test
{
    public class GetBocadillos_test : AppForMovies4SqliteUT
    {
        public GetBocadillos_test()
        {
            var tipopan = new List<TipoPan>()
            {
            new TipoPan("Semillas"),
            new TipoPan("Integral"),
            new TipoPan("Normal"),
            new TipoPan("Baguette")
        };


            var bocadillos = new List<Bocadillo>()
            {

                new Bocadillo("Serranito", 2.5f, 5, Tamanyo.Pequeño, tipopan[0],1),

                new Bocadillo("Vegetal", 2.0f, 4, Tamanyo.Normal, tipopan[1],2),

                new Bocadillo("Atún", 1.5f, 10, Tamanyo.Normal, tipopan[2],3),
                new Bocadillo("Pollo", 2.0f, 7, Tamanyo.Pequeño, tipopan[0],4),




            };


            ApplicationUser user = new ApplicationUser("Antonio", "Garcia de la Reina", "Aguilar", "antonio@uclm.es");


            _context.Add(user);
            _context.AddRange(bocadillos);
            _context.AddRange(tipopan);
            _context.SaveChanges();




        }



        public static IEnumerable<object[]> TestCasesFor_GetBocadillosParaPedir_OK()
        {
            var bocadilloDTOs = new List<BocadillosDTO>()
            {

                new BocadillosDTO(1, "Serranito", Tamanyo.Pequeño, "Semillas", 2.5f),

                new BocadillosDTO(2, "Vegetal", Tamanyo.Normal, "Integral", 2.0f),

                new BocadillosDTO(3, "Atún", Tamanyo.Normal, "Normal", 1.5f),
                new BocadillosDTO(4, "Pollo", Tamanyo.Pequeño, "Semillas", 2.0f),

            };

            var bocadilloDTOsTC1 = new List<BocadillosDTO>() { bocadilloDTOs[1], bocadilloDTOs[2] };

            var bocadilloDTOsTC2 = new List<BocadillosDTO>() { bocadilloDTOs[1] };

            var bocadadilloDTOsTC3 = new List<BocadillosDTO>() { bocadilloDTOs[2] };
            var bocadilloDTOsTC4 = new List<BocadillosDTO>() { bocadilloDTOs[0], bocadilloDTOs[1], bocadilloDTOs[2], bocadilloDTOs[3] };


            var allTests = new List<object[]>
            {
                new object[] {null, null, bocadilloDTOsTC4 },
                new object[] {Tamanyo.Normal, null, bocadilloDTOsTC1 },
                new object[] {null, "Integral", bocadilloDTOsTC2 },
                new object[] {null, "Normal", bocadadilloDTOsTC3 },

            };

            return allTests;


        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetBocadillosParaPedir_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]

        public async Task GetBocadillosParaPedir_OK(Tamanyo? tamanyo, string? tipoPan, List<BocadillosDTO> expectedBocadillo)
        {
            
            var controller = new BocadillosController(_context, null);

            //Ejecucion
            var resultado = await controller.GetBocadillosParaPedir(tamanyo, tipoPan);

            //Comprobacion
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var bocadilloDTOsActual = Assert.IsType<List<BocadillosDTO>>(okResult.Value);
            Assert.Equal(expectedBocadillo, bocadilloDTOsActual);

        }




        /*


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]

        public async Task GetBocadillosParaPedir_badrequest_test()
        {
            var mock = new Mock <ILogger<BocadillosController>>();
            ILogger<BocadillosController> logger = mock.Object;
            var controller = new BocadillosController(_context, logger);



            var resultado = await controller.GetBocadillosParaPedir(Tamanyo.Normal, "NoExiste");

        }
        */

    }

}