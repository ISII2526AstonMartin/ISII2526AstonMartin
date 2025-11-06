using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.DTOs.CompraBonosDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.BonosCompraController_test
{
    public class CreateCompra_test : AppForMovies4SqliteUT
    {
        public CreateCompra_test()
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
                new BonoBocadillo(4, 4,4,"Bono 4", 4.44f, tipos[3], new List<BonosComprados>())
            };
            var applicationUsers = new ApplicationUser("Andres", "Iniesta", "Lujan", "AIniesta");

            _context.Add(applicationUsers);
            _context.AddRange(tipos);
            _context.AddRange(bonosBocadillos);

            _context.SaveChanges();
        }

        [Fact]
        public async Task CreateCompraGood()
        {
            var expected = new CompraBonoDetailsDTO(1, DateTime.Today, "Andres", "Iniesta", "Lujan", MetodoPago.Tarjeta, new List<CompraBonoItemDTO>());
            expected.compraItems.Add(new CompraBonoItemDTO(1, 14.4f, 5, "Bono1", "Vegano", 4));

            //assert
            var argdto = new CompraBonoForCreateDTO("Andres", "Iniesta", "Lujan", MetodoPago.Tarjeta, new List<CompraBonoItemDTO>());
            argdto.compraItems.Add(new CompraBonoItemDTO(1, 14.4f, 5, "Bono1", "Vegano", 4));

            var mock = new Mock<ILogger<CompraBonosController>>();
            ILogger<CompraBonosController> logger = mock.Object;
            CompraBonosController controller = new CompraBonosController(_context, logger);

            //act
            var actual = await controller.CreateCompra(argdto);

            //assert
            var createdObject=Assert.IsType<CreatedAtActionResult>(actual);
            var actualResult=Assert.IsType<CompraBonoDetailsDTO>(createdObject.Value);
            Assert.Equal(expected, actualResult);
        }

        public static IEnumerable<object[]> badPostCompras()
        {
            var nuevo = new CompraBonoForCreateDTO("Andres", "Iniesta", "Lujan", MetodoPago.Tarjeta, new List<CompraBonoItemDTO>());
            nuevo.compraItems.Add(new CompraBonoItemDTO(1, 14.4f, 5, "BonoNoExiste", "Vegano", 4));
            var allTests = new List<object[]>()
            {
                new object[]{ new CompraBonoForCreateDTO(null, "Iniesta", "Lujan", MetodoPago.Tarjeta, new List<CompraBonoItemDTO>()), "El nombre no está definido" },
                new object[]{ new CompraBonoForCreateDTO("Andres", null, "Lujan", MetodoPago.Tarjeta, new List<CompraBonoItemDTO>()), "El apellido no está definido" },
                new object[]{ new CompraBonoForCreateDTO("Andres", "Iniesta", "Lujan", (MetodoPago)9999999, new List<CompraBonoItemDTO>()), "Metodo de pago no valido" },
                new object[]{ new CompraBonoForCreateDTO("Lionel Andres", "Messi", "Cuccittini", MetodoPago.Tarjeta, new List<CompraBonoItemDTO>()), "Cliente no registrado" },
                new object[]{ nuevo , "Bono no existe" }
            };
            
            return allTests;
        }

        [Theory]
        [MemberData(nameof(badPostCompras))]
        public async Task CreateCompraErrors(CompraBonoForCreateDTO dto, string expectedString)
        {
            var mock = new Mock<ILogger<CompraBonosController>>();
            ILogger<CompraBonosController> logger = mock.Object;
            CompraBonosController controller = new CompraBonosController(_context, logger);

            //act
            var actual = await controller.CreateCompra(dto);

            //assert
            var actualResult=Assert.IsType<BadRequestObjectResult>(actual);
            Assert.Contains(expectedString,actualResult.Value.ToString());
        }
    }
}
