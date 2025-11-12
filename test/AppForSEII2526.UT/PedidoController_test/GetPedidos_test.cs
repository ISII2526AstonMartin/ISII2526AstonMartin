using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.BocadillosParaPedirDTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.PedidoController_test
{
    public class GetPedidos_test : AppForMovies4SqliteUT
    {
        public GetPedidos_test()
        {
            var tipopan = new List<TipoPan>()
            {
                new TipoPan("Semillas"),
                new TipoPan("Integral"),

            };

            var bocadillo = new List<Bocadillo>()
            {
                new Bocadillo("Atun",2.0f, 3,Tamanyo.Normal, tipopan[0],1),
                new Bocadillo("Vegetal",3.0f, 5,Tamanyo.Pequeño, tipopan[1],2),

            };

            ApplicationUser user = new ApplicationUser("Antonio", "Garcia de la Reina", "Aguilar", "antonio@uclm.es");

            var compra = new Compra(DateTime.Today,new List<CompraBocadillo>() , MetodoPago.Paypal, user);
            compra.CompraBocadillos.Add(new CompraBocadillo(bocadillo[0], compra, 2));

            compra.PrecioTotal = compra.CompraBocadillos.Sum(cb => cb.Precio * cb.Cantidad);

            _context.Add(user);
            _context.AddRange(bocadillo);
            _context.AddRange(tipopan);
            _context.Add(compra);
            _context.SaveChanges();
        }


        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]


        public async Task GetPedidos_NotFound_Test()
        {
            var mock = new Mock<ILogger<PedidoController>>();
            ILogger<PedidoController> logger = mock.Object;

            var controller = new PedidoController(_context, logger);

            // Act
            var result = await controller.GetPedidos(0);

            //Assert
            //we check that the response type is OK and obtain the list of movies
            Assert.IsType<NotFoundResult>(result);
        }




        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]

        public async Task GetPedidos_Found_test()
        {

            var mock = new Mock<ILogger<PedidoController>>();
            ILogger<PedidoController> logger = mock.Object;

            var controller = new PedidoController(_context, logger);

            var expectedPedido = new PedidoDetailDTO("Antonio", MetodoPago.Paypal,"Garcia de la Reina", "Aguilar", DateTime.Today, 4.0f , new List<ItemPedidoDTO>());

            expectedPedido.ItemPedido.Add(new ItemPedidoDTO(1,"Atun",2, 2.0f,"Semillas"));


            var result = await controller.GetPedidos(1);

            var okResult= Assert.IsType<OkObjectResult>(result);
            var pedidoDTOActual = Assert.IsType<PedidoDetailDTO>(okResult.Value);
            var eq = expectedPedido.Equals(pedidoDTOActual);
            Assert.Equal(expectedPedido, pedidoDTOActual);





        }

    }
}

