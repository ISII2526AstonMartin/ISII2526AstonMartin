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
        private const string _nombreUsuario = "juan";
        private const string _apellido1 = "Perez";
        private const string _apellido2 = "Muñoz";
        private const string _direccionEnvio = "Calle Gran Vía 123, Madrid";

        public POSTMerch_test()
        {
            // Limpiar la base de datos primero
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // 1️⃣ Crear tipos de producto
            var tipoRopa = new TipoProducto("Ropa", Guid.NewGuid().ToString(), new List<Producto>());
            var tipoAccesorio = new TipoProducto("Accesorios", Guid.NewGuid().ToString(), new List<Producto>());

            _context.TiposProductos.AddRange(tipoRopa, tipoAccesorio);
            _context.SaveChanges();

            // 2️⃣ Crear productos con nombres descriptivos
            var camiseta = new Producto(
                Guid.NewGuid().ToString(),  // ID único
                "Camiseta UCLM",           // Nombre descriptivo
                8,                         // PVP
                48,                        // Stock
                tipoRopa,                  // Tipo
                new List<Producto_Compra>()
            );

            var gorra = new Producto(
                Guid.NewGuid().ToString(),  // ID
                "Gorra",                   // Nombre
                4,                         // PVP  
                10,                        // Stock
                tipoAccesorio,             // Tipo
                new List<Producto_Compra>()
            );

            _context.Productos.AddRange(camiseta, gorra);
            _context.SaveChanges();

            // 3️⃣ Crear usuarios
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

        // ✅ Casos de error esperados
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

        // ❌ Test de errores
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

        // ✅ Test de éxito
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateMerch_Success_test()
        {
            var logger = new Mock<ILogger<POSTMerchController>>().Object;
            var controller = new POSTMerchController(_context, logger);

            // Tomar el primer producto disponible (sea cual sea su nombre)
            var producto = await _context.Productos
                .Include(p => p.Tipo_Producto)
                .FirstAsync();

            var merchDTO = new CreateMerchDTO(
                "juan",
                "Perez",
                "Muñoz",
                "Calle Gran Vía 123, Madrid",
                MetodoPago.Tarjeta,
                new List<ItemMerchDTO>
                {
            new ItemMerchDTO(producto.Nombre, producto.PVP, producto.Tipo_Producto?.Nombre ?? "Ropa", 2)
                }
            );

            var result = await controller.CreateMerch(merchDTO);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var detail = Assert.IsType<DetailMerchDTO>(createdResult.Value);

            Assert.Equal(merchDTO.NombreUsuario, detail.NombreUsuario);
            Assert.Equal(merchDTO.DireccionEnvio, detail.DireccionEnvio);
            Assert.Equal(merchDTO.Items.Count, detail.Items.Count);
            Assert.True(detail.PrecioFinal > 0);
        }
    }
}