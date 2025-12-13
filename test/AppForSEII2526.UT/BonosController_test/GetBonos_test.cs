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
using RabbitMQ.Client;

namespace AppForSEII2526.UT.BonosController_test
{
    public class GetBonos_test : AppForMovies4SqliteUT
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

            var bonosBocadillos = new List<BonoBocadillo>()
            {
                new BonoBocadillo(1,5,5,"Bono1",14.4f,tipos[0],new List<BonosComprados>()),
                new BonoBocadillo(2,3,7,"Bono2", 29.95f,tipos[1],new List<BonosComprados>()),
                new BonoBocadillo(3,6,6,"Bono3", 3.99f, tipos[2], new List<BonosComprados>()),
                new BonoBocadillo(4, 4,4,"Bono4", 4.44f, tipos[3], new List<BonosComprados>())
            };
            _context.AddRange(tipos);
            _context.AddRange(bonosBocadillos);
            _context.SaveChanges();
        }
        public static IEnumerable<object[]> TestCasesForGetBonosOK()
        {
            var bonosDTOs = new List<BonoBocadillosDTO>()
            {
                new BonoBocadillosDTO(1,5,"Bono1",14.4f,"Vegano"),
                new BonoBocadillosDTO(2,7,"Bono2",29.95f,"Vegetariano"),
                new BonoBocadillosDTO(3,6,"Bono3",3.99f,"Sin Gluten"),
                new BonoBocadillosDTO(4,4,"Bono4",4.44f,"Normal")
            };

            var expected1 = new List<BonoBocadillosDTO>() { bonosDTOs[0], bonosDTOs[1], bonosDTOs[2], bonosDTOs[3] };
            var expected2 = new List<BonoBocadillosDTO>() { bonosDTOs[0] };
            var expected3 = new List<BonoBocadillosDTO>() { bonosDTOs[0] };
            var expected4 = new List<BonoBocadillosDTO>() { bonosDTOs[0] };


            var allTests = new List<object[]>
            {
                new object[] { null, null, expected1 },
                new object[] { "Bono1", null, expected2 },
                new object[] { null, "Vegano", expected3 },
                new object[] { "Bono1", "Vegano", expected4 }
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForGetBonosOK))]
        public async Task getBonosGoodResult_test(string? name, string? type, IList<BonoBocadillosDTO> expectedBonos)
        {
            //arrange

            var mock = new Mock<ILogger<BonosController>>();
            ILogger<BonosController> logger = mock.Object;
            BonosController controller = new BonosController(_context, logger);

            //act

            var result = await controller.GetBonos(name, type);

            //assert

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = Assert.IsType<List<BonoBocadillosDTO>>(okResult.Value);
            Assert.Equal(expectedBonos, actualResult);
        }

        public static IEnumerable<object[]> TestCasesForGetBonosBad()
        {

            var allTests = new List<object[]>
            {
                new object[] { "EsteTipoNoExiste", null},
                new object[] { null, "EsteBonoNoExiste" },
                new object[] { "EsteTipoNoExiste", "EsteBonoNoExiste"}
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForGetBonosBad))]
        public async Task getBonosBadResult_test(string? name, string? type)
        {
            //arrange
            
            var mock = new Mock<ILogger<BonosController>>();
            ILogger<BonosController> logger = mock.Object;
            BonosController controller = new BonosController(_context, logger);

            //act

            var result = await controller.GetBonos(name, type);

            //assert

            var okResult = Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
