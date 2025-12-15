using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ComprarMerchDTOs;
using AppForSEII2526.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.MerchController_test
{
    public class POSTMerch_test : AppForMovies4SqliteUT
    {
        public POSTMerch_test()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var tipoRopa = new TipoProducto("Ropa", 1, new List<Producto>());
            var tipoAccesorio = new TipoProducto("Accesorios", 2, new List<Producto>());

            _context.TiposProductos.AddRange(tipoRopa, tipoAccesorio);
            _context.SaveChanges();

            var camiseta = new Producto(
                "Camiseta UCLM",
                1,
                8,
                48,
                tipoRopa,
                new List<Producto_Compra>()
            );

            var gorra = new Producto(
                "Gorra",
                2,
                4,
                10,
                tipoAccesorio,
                new List<Producto_Compra>()
            );

            _context.Productos.AddRange(camiseta, gorra);
            _context.SaveChanges();

            var usuarios = new List<ApplicationUser>
            {
                new ApplicationUser {
                    UserName = "juan",
                    Nombre = "Juan",
                    Apellido1 = "Perez",
                    Apellido2 = "Muñoz",
                    Email = "juan@example.com"
                },
                new ApplicationUser {
                    UserName = "rafamartinez",
                    Nombre = "Rafa",
                    Apellido1 = "Martinez",
                    Apellido2 = "Muñoz",
                    Email = "rafa@example.com"
                },
                new ApplicationUser {
                    UserName = "maria.gomez",
                    Nombre = "Maria",
                    Apellido1 = "Gomez",
                    Apellido2 = "Ruiz",
                    Email = "maria@example.com"
                }
            };
            _context.ApplicationUsers.AddRange(usuarios);
            _context.SaveChanges();
        }

        // Casos de prueba para escenarios de error en la creacion de merchandising
        public static IEnumerable<object[]> TestCasesFor_CreateMerch()
        {
            var merchSinItems = new CreateMerchDTO(
                "rafamartinez", "Martinez", "Muñoz",
                "Calle Gran Vía 123",
                MetodoPago.Tarjeta,
                new List<ItemMerchDTO>()
            );

            var merchUsuarioNoExiste = new CreateMerchDTO(
                "usuarioNoExiste", "Martinez", "Muñoz",
                "Calle Sol 123",
                MetodoPago.Paypal,
                new List<ItemMerchDTO> { new ItemMerchDTO("Camiseta UCLM", 8, "Ropa", 1) }
            );

            var merchProductoNoExiste = new CreateMerchDTO(
                "juan", "Perez", "Muñoz",
                "Calle Luna 45",
                MetodoPago.Gpay,
                new List<ItemMerchDTO> { new ItemMerchDTO("ProductoFantasma", 10f, "Ropa", 1) }
            );

            var merchCantidadInvalida = new CreateMerchDTO(
                "juan", "Perez", "Muñoz",
                "Calle Falsa 123",
                MetodoPago.Tarjeta,
                new List<ItemMerchDTO> { new ItemMerchDTO("Camiseta UCLM", 8, "Ropa", 0) }
            );

            var merchDireccionInvalida = new CreateMerchDTO(
                "juan", "Perez", "Muñoz",
                "C/ Rosario",
                MetodoPago.Tarjeta,
                new List<ItemMerchDTO> { new ItemMerchDTO("Camiseta UCLM", 8, "Ropa", 1) }
            );

            // Caso para validar que el Backend rechaza métodos de pago no definidos en el Enum (ej: 999)
            var merchPagoInvalido = new CreateMerchDTO(
                "juan", "Perez", "Muñoz",
                "Calle Correcta 123",
                (MetodoPago)999,
                new List<ItemMerchDTO> { new ItemMerchDTO("Camiseta UCLM", 8, "Ropa", 1) }
            );

            return new List<object[]>
            {
                new object[] { merchSinItems, "Debes incluir al menos un producto." },
                new object[] { merchUsuarioNoExiste, "Error: Usuario o apellido no registrados." },
                new object[] { merchProductoNoExiste, "Producto 'ProductoFantasma' no encontrado." },
                new object[] { merchCantidadInvalida, "Cantidad inválida para 'Camiseta UCLM'." },
                new object[] { merchDireccionInvalida, "Error!, por favor introduce una dirección de envío válida"},
                new object[] { merchPagoInvalido, "Error: Método de pago no válido o no soportado." }
            };
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateMerch))]
        public async Task CreateMerch_Error_test(CreateMerchDTO merchDTO, string errorEsperado)
        {
            var logger = new Mock<ILogger<POSTMerchController>>().Object;
            var controller = new POSTMerchController(_context, logger);

            var result = await controller.CreateMerch(merchDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var errorActual = problemDetails.Errors.First().Value[0];

            Assert.StartsWith(errorEsperado, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateMerch_Success_test()
        {
            var logger = new Mock<ILogger<POSTMerchController>>().Object;
            var controller = new POSTMerchController(_context, logger);

            var producto = await _context.Productos
                .Include(p => p.Tipo_Producto)
                .FirstAsync();

            var items = new List<ItemMerchDTO>
            {
                new ItemMerchDTO(producto.Nombre, producto.PVP, producto.Tipo_Producto?.Nombre ?? "Ropa", 2)
            };

            var merchDTO = new CreateMerchDTO(
                "juan",
                "Perez",
                "Muñoz",
                "Calle Gran Vía 123, Madrid",
                MetodoPago.Tarjeta,
                items
            );

            var result = await controller.CreateMerch(merchDTO);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualDetail = Assert.IsType<DetailMerchDTO>(createdResult.Value);

            var expectedDetail = new DetailMerchDTO(
                "juan",
                "Perez",
                "Muñoz",
                "Calle Gran Vía 123, Madrid",
                MetodoPago.Tarjeta,
                items,
                1,
                DateTime.Today,
                producto.PVP * 2
            );

            Assert.Equal(expectedDetail, actualDetail);
        }
    }
}