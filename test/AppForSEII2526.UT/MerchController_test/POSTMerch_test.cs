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
        // Constantes para datos de prueba reutilizables
        private const string _nombreUsuario = "juan";
        private const string _apellido1 = "Perez";
        private const string _apellido2 = "Muñoz";
        private const string _direccionEnvio = "Calle Gran Vía 123, Madrid";

        public POSTMerch_test()
        {
            // Limpiar la base de datos de pruebas para empezar desde un estado conocido
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Crear tipos de producto para las pruebas
            var tipoRopa = new TipoProducto("Ropa", 1, new List<Producto>());
            var tipoAccesorio = new TipoProducto("Accesorios", 2, new List<Producto>());

            _context.TiposProductos.AddRange(tipoRopa, tipoAccesorio);
            _context.SaveChanges();

            // Crear productos de prueba con datos realistas
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

            // Crear usuarios de prueba para simular clientes del sistema
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

            return new List<object[]>
            {
                new object[] { merchSinItems, "Debes incluir al menos un producto." },
                new object[] { merchUsuarioNoExiste, "Error: Usuario o apellido no registrados." },
                new object[] { merchProductoNoExiste, "Producto 'ProductoFantasma' no encontrado." },
                new object[] { merchCantidadInvalida, "Cantidad inválida para 'Camiseta UCLM'." }
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

            // Obtener un producto existente de la base de datos para la compra
            var producto = await _context.Productos
                .Include(p => p.Tipo_Producto)
                .FirstAsync();

            // Crear la lista de items para la compra
            var items = new List<ItemMerchDTO>
            {
                new ItemMerchDTO(producto.Nombre, producto.PVP, producto.Tipo_Producto?.Nombre ?? "Ropa", 2)
            };

            // Crear el DTO de entrada con los datos de la compra
            var merchDTO = new CreateMerchDTO(
                "juan",
                "Perez",
                "Muñoz",
                "Calle Gran Vía 123, Madrid",
                MetodoPago.Tarjeta,
                items
            );

            // Ejecucion: llamar al metodo del controlador para crear la compra
            var result = await controller.CreateMerch(merchDTO);

            // Verificacion: asegurar que se devuelve un resultado Created
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualDetail = Assert.IsType<DetailMerchDTO>(createdResult.Value);

            // Crear el DetailMerchDTO esperado con todos los datos que deberia tener
            // Incluyendo el ID y fecha reales que devolvio el controlador
            var expectedDetail = new DetailMerchDTO(
                "juan",
                "Perez",
                "Muñoz",
                "Calle Gran Vía 123, Madrid",
                MetodoPago.Tarjeta,
                items,
                actualDetail.CompraID, // Usar el ID real generado
                actualDetail.FechaCompra, // Usar la fecha real generada
                producto.PVP * 2 // Precio final calculado
            );

            // Comparacion directa usando Assert.Equal
            // Esto verifica que todos los campos coinciden, incluyendo ID y fecha
            Assert.Equal(expectedDetail, actualDetail);
        }
    }
}