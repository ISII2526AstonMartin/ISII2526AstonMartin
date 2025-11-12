using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.BocadillosParaPedirDTOs;
using Microsoft.EntityFrameworkCore.Query.Internal;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.PedidoController_test
{
    public class PostPedidos_test : AppForMovies4SqliteUT
    {

        private const string nombre = "Antonio";
        private const string apellido1 = "Garcia de la Reina";


        private const string nombreBocadillo1 = "Atun";
        private const string nombreBocadillo2 = "Vegetal";
        private const string tipoPan1 = "Semillas";
        private const string tipoPan2 = "Integral";




        public PostPedidos_test()
        {
            var tipopan = new List<TipoPan>()
            {
                new TipoPan(tipoPan1),
                new TipoPan(tipoPan2),

            };


            var bocadillo = new List<Bocadillo>()
            {
                new Bocadillo(nombreBocadillo1,2.0f, 3,Tamanyo.Normal, tipopan[0],1),
                new Bocadillo(nombreBocadillo2,3.0f, 5,Tamanyo.Pequeño, tipopan[1],2),

            };



            ApplicationUser user = new ApplicationUser("Antonio", "Garcia de la Reina", "Aguilar", "antonio@uclm.es");


            var rental = new Compra(DateTime.Today, new List<CompraBocadillo>(), MetodoPago.Tarjeta, user);


            rental.CompraBocadillos.Add(new CompraBocadillo(bocadillo[0], rental, 2));


            _context.ApplicationUsers.Add(user);
            _context.AddRange(tipopan);
            _context.AddRange(bocadillo);
            _context.Add(rental);
            _context.SaveChanges();

        }

        public static IEnumerable<object[]> TestCasesFor_CreatePedido()
        {
            var bocadilloPedido = new List<ItemPedidoDTO>()
            {
                new ItemPedidoDTO(2, "Vegetal", 2, 3.0f, "Integral")
            };

            var pedidoUsuarioNoRegistrado = new CreatePedidoDTO(
                "NoExiste",
                MetodoPago.Tarjeta,

                "ApellidoFalso",
                "Falso2",

                bocadilloPedido
            );

            var pedidoMetodoPagoInvalido = new CreatePedidoDTO(
                "Antonio",
                (MetodoPago)999,
                "Garcia de la Reina",
                "Aguilar",

                bocadilloPedido
            );

            var pedidoBocadilloInexistente = new CreatePedidoDTO(
                "Antonio",
                MetodoPago.Paypal,
                "Garcia de la Reina",
                "Aguilar",

                new List<ItemPedidoDTO>()
                {
                    new ItemPedidoDTO(999, "FalsoBocadillo", 1, 2.0f, "Normal")
                }
            );

            var pedidoSinBocadillos = new CreatePedidoDTO(
                "Antonio",
                MetodoPago.Tarjeta,
                "Garcia de la Reina",

                "Aguilar",


                new List<ItemPedidoDTO>()
            );

            return new List<object[]>
            {
                new object[] { pedidoUsuarioNoRegistrado, "Error! Usuario no registrado" },
                new object[] { pedidoMetodoPagoInvalido, "Error! Método de pago no válido. Usa: Tarjeta, Paypal o Gpay." },
                new object[] { pedidoBocadilloInexistente, "Error! El bocadillo FalsoBocadillo no está disponible" },

            };


        }


        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreatePedido))]
        public async Task CreatePedido_Error_test(CreatePedidoDTO pedidoDTO, string errorEsperado)
        {
            // Arrange
            var mock = new Mock<ILogger<PedidoController>>();
            ILogger<PedidoController> logger = mock.Object;
            var controller = new PedidoController(_context, logger);

            // Act
            var result = await controller.CreatePedido(pedidoDTO);

            // Assert
            var badRequestResult = Assert.IsAssignableFrom<ObjectResult>(result);


            if (badRequestResult.Value is ValidationProblemDetails problemDetails)
            {
                var errorActual = problemDetails.Errors.First().Value[0];
                Assert.StartsWith(errorEsperado, errorActual);
            }
            else if (badRequestResult.Value is string errorMessage)
            {
                Assert.StartsWith(errorEsperado, errorMessage);
            }
            else
            {
                Assert.True(false, "Unexpected error response type");
            }
        }

        
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreatePedido_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<PedidoController>>();
            ILogger<PedidoController> logger = mock.Object;
            var controller = new PedidoController(_context, logger);

            var items = new List<ItemPedidoDTO>()
            {
                new ItemPedidoDTO(2, "Vegetal", 2, 3.0f, "Integral")
            };

            var pedidoDTO = new CreatePedidoDTO(
                nombre,
                MetodoPago.Tarjeta,
                apellido1,
                "Aguilar",

                items
            );

            var expectedpedidoDetailDTO = new PedidoDetailDTO(nombre, MetodoPago.Tarjeta, apellido1,"Aguilar", DateTime.Today, 6.0f, new List<ItemPedidoDTO>() {new ItemPedidoDTO(2,"Vegetal",2,3.0f,"Integral" )});

            var result = await controller.CreatePedido(pedidoDTO);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualPedidoDetailDTO = Assert.IsType<PedidoDetailDTO>(createdResult.Value);



            

            Assert.Equal(expectedpedidoDetailDTO, actualPedidoDetailDTO);


        }
        
        

    }
}

