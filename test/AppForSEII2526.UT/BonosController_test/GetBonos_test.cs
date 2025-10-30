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

namespace AppForSEII2526.UT.BonosController_test
{
    public class GetBonos_test: AppForMovies4SqliteUT
    {
        public GetBonos_test() 
        {
            var tipos = new List<TipoBocadillo>()
            {
                new TipoBocadillo(1,"Vegano", new List<BonoBocadillo>()),
                new TipoBocadillo(2, "Vegetariano", new List<BonoBocadillo>()),
                new TipoBocadillo(3, "Sin Gluten", new List<BonoBocadillo>()),
                new TipoBocadillo(4, "Normal", new List<BonoBocadillo>())
            };

            var bonosBocadillos= new List<BonoBocadillo>()
            {
                new BonoBocadillo(1,5,5,"Bono1",14.4f,tipos[0],new List<BonosComprados>()),
                new BonoBocadillo(2,3,7,"Bono2", 29.95f,tipos[1],new List<BonosComprados>()),
                new BonoBocadillo(3,6,6,"Bono3", 3.99f, tipos[2], new List<BonosComprados>()),
                new BonoBocadillo(4, 4,4,"Bono 4", 4.44f, tipos[3], new List<BonosComprados>())
            };
            _context.AddRange(tipos);
            _context.AddRange(bonosBocadillos);
            _context.SaveChanges();
        }

        [Fact]
        public async Task getBonosGoodNameResult_test()
        {
            //arrange
            List<BonoBocadillosDTO> expectedbonos = new List<BonoBocadillosDTO>()
            {
                new BonoBocadillosDTO(1,5,5,"Bono1",14.4f,"Vegano")
            };

            var mock =new Mock<ILogger<BonosController>>();
            ILogger<BonosController> logger=mock.Object;
            BonosController controller = new BonosController(_context, logger);

            //act

            var result = await controller.GetBonos("Bono1", null);

            //assert

            var okResult= Assert.IsType<OkObjectResult>(result);
            var actualResult= Assert.IsType<List<BonoBocadillosDTO>>(okResult.Value);
            Assert.Equal(expectedbonos, actualResult);
        }

        [Fact]
        public async Task getBonosGoodTypeResult_test()
        {
            //arrange
            List<BonoBocadillosDTO> expectedbonos = new List<BonoBocadillosDTO>()
            {
                new BonoBocadillosDTO(1,5,5,"Bono1",14.4f, "Vegano")
            };

            var mock = new Mock<ILogger<BonosController>>();
            ILogger<BonosController> logger = mock.Object;
            BonosController controller = new BonosController(_context, logger);

            //act

            var result = await controller.GetBonos(null, "Vegano");

            //assert

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = Assert.IsType<List<BonoBocadillosDTO>>(okResult.Value);
            Assert.Equal(expectedbonos, actualResult);
        }
        [Fact]
        public async Task getBonosNombreNoexiste_test()
        {
            //arrange
            
            var mock = new Mock<ILogger<BonosController>>();
            ILogger<BonosController> logger = mock.Object;
            BonosController controller = new BonosController(_context, logger);

            //act

            var result = await controller.GetBonos("EsteBonoNoExiste", null);

            //assert

            var okResult = Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task getBonosTipoNoexiste_test()
        {
            //arrange

            var mock = new Mock<ILogger<BonosController>>();
            ILogger<BonosController> logger = mock.Object;
            BonosController controller = new BonosController(_context, logger);

            //act

            var result = await controller.GetBonos(null, "EsteTipoNoExiste");

            //assert

            var okResult = Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
