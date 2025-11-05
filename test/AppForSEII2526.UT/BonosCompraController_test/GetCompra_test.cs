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
    public class GetCompra_test : AppForMovies4SqliteUT
    {
        public GetCompra_test() 
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


            var comprasBonos = new CompraBono(1, new DateTime(2025, 10, 6), 4, 15.5f, MetodoPago.Tarjeta, new List<BonosComprados>() { }, applicationUsers);
            comprasBonos.ListaBonosComprados.Add(new BonosComprados(bonosBocadillos[0], comprasBonos, 4, 14.4f));

            _context.AddRange(tipos);
            _context.Add(applicationUsers);
            _context.AddRange(bonosBocadillos);
            _context.Add(comprasBonos);
            _context.SaveChanges();
        }


        [Fact]
        public async Task GetCompraOk()
        {
            //arrange

            CompraBonoDetailsDTO expectedCompraBono = new CompraBonoDetailsDTO(1, new DateTime(2025, 10, 6), "Andres", "Iniesta", "Lujan", MetodoPago.Tarjeta, new List<CompraBonoItemDTO>());
            expectedCompraBono.compraItems.Add(new CompraBonoItemDTO(1, 14.4f, 5, "Bono1", "Vegano", 4));

            var mock = new Mock<ILogger<CompraBonosController>>();
            ILogger<CompraBonosController> logger = mock.Object;
            CompraBonosController controller = new CompraBonosController(_context, logger);

            //act
            var results = await controller.GetCompra(1);

            //assert
            var okResult = Assert.IsType<OkObjectResult>(results);
            var actualResult = Assert.IsType<CompraBonoDetailsDTO>(okResult.Value);
            Assert.Equivalent(expectedCompraBono, actualResult);
        }
        
        [Fact]
        public async Task GetCompraBad()
        {
            var mock = new Mock<ILogger<CompraBonosController>>();
            ILogger<CompraBonosController> logger = mock.Object;
            CompraBonosController controller = new CompraBonosController(_context, logger);

            var results = await controller.GetCompra(123456789);
            var okResult = Assert.IsType<NotFoundObjectResult>(results);
            Assert.Contains("No se han encontrado compras", okResult.Value.ToString());
        }

    }
}
