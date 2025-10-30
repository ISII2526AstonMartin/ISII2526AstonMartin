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


        [Fact]
        public async Task getBocadillosResenyaGoodNameResult_test()
        {
            List<BocadillosDTO> bocadillosEsperados = new List<BocadillosDTO>()
            {
                new BocadillosDTO(2, "BaconQueso", Tamanyo.Normal, "Normal", 6),
                new BocadillosDTO(3, "Bacon", Tamanyo.Pequeño, "Centeno", 2)
            };
            var mock = new Mock<ILogger<BocadillosController>>();
            ILogger<BocadillosController> logger = mock.Object;
            BocadillosController controller = new BocadillosController(_context, logger);


            var result = await controller.GetBocadillosResenya("Bacon", null);


            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = Assert.IsType<List<BocadillosDTO>>(okResult.Value);
            Assert.Equal(bocadillosEsperados, actualResult);

        }


        [Fact]
        public async Task getBocadillosResenyaGoodPVPResult_test()
        {
            List<BocadillosDTO> bocadillosEsperados = new List<BocadillosDTO>()
            {
                new BocadillosDTO(3, "Bacon", Tamanyo.Pequeño, "Centeno", 2),
                new BocadillosDTO(4, "Atun", Tamanyo.Normal, "Integral", 3)
            };

            var mock = new Mock<ILogger<BocadillosController>>();
            ILogger<BocadillosController> logger = mock.Object;
            BocadillosController controller = new BocadillosController(_context, logger);


            var result = await controller.GetBocadillosResenya(null, 5);


            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = Assert.IsType<List<BocadillosDTO>>(okResult.Value);
            Assert.Equal(bocadillosEsperados, actualResult);


        }


        [Fact]
        public async Task getBocadillosNombreNoExiste_test()
        {
            List<BocadillosDTO> bocadillosEsperados = new List<BocadillosDTO>();

            var mock = new Mock<ILogger<BocadillosController>>();
            ILogger<BocadillosController> logger = mock.Object;
            BocadillosController controller = new BocadillosController(_context, logger);


            var result = await controller.GetBocadillosResenya("tortilla", null);


            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = Assert.IsType<List<BocadillosDTO>>(okResult.Value);
            Assert.Equal(bocadillosEsperados, actualResult);
        }
    }
}
