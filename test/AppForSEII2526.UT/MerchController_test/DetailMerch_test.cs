using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ComprarMerchDTOs;
using AppForSEII2526.API.Models;
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
    public class DetailMerch_test : AppForMovies4SqliteUT
    {
        public DetailMerch_test()
        {
            // Limpiar y crear base de datos de pruebas
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Crear tipos de producto
            var tipoRopa = new TipoProducto("Ropa", 1, new List<Producto>());
            var tipoAccesorio = new TipoProducto("Accesorios", 2, new List<Producto>());

            _context.TiposProductos.AddRange(tipoRopa, tipoAccesorio);
            _context.SaveChanges();

            // Crear productos
            var camiseta = new Producto("Camiseta UCLM", 1, 8.0f, 48, tipoRopa, new List<Producto_Compra>());
            var gorra = new Producto("Gorra UCLM", 2, 4.0f, 10, tipoAccesorio, new List<Producto_Compra>());

            _context.Productos.AddRange(camiseta, gorra);
            _context.SaveChanges();

            // Crear usuario - CON NombreUsuario asignado
            var user = new ApplicationUser
            {
                UserName = "juan",
                Nombre = "Juan",
                Apellido1 = "Perez",
                Apellido2 = "Muñoz",
                NombreUsuario = "juan",  // ← ESTA LÍNEA FALTABA
                Email = "juan@example.com"
            };

            _context.ApplicationUsers.Add(user);
            _context.SaveChanges();

            // Crear compra
            var compra = new Compra_Producto(user, "Calle Gran Vía 123, Madrid", DateTime.Today, MetodoPago.Tarjeta, new List<Producto_Compra>());
            _context.Compras.Add(compra);
            _context.SaveChanges();

            // Agregar productos a la compra
            compra.Productos_Compras.Add(new Producto_Compra(2, compra.CompraID, camiseta.ProductoID, camiseta.PVP, camiseta, compra));
            compra.Productos_Compras.Add(new Producto_Compra(1, compra.CompraID, gorra.ProductoID, gorra.PVP, gorra, compra));
            compra.PrecioFinal = (camiseta.PVP * 2) + (gorra.PVP * 1);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetMerchDetail_Found_test()
        {
            // Arrange
            var mock = new Mock<ILogger<POSTMerchController>>();
            var controller = new POSTMerchController(_context, mock.Object);

            // Crear el objeto DetailMerchDTO esperado
            var expectedItems = new List<ItemMerchDTO>
            {
                new ItemMerchDTO("Camiseta UCLM", 8.0f, "Ropa", 2),
                new ItemMerchDTO("Gorra UCLM", 4.0f, "Accesorios", 1)
            };

            var expectedDetail = new DetailMerchDTO(
                "juan",           // NombreUsuario
                "Perez",          // Apellido1
                "Muñoz",          // Apellido2
                "Calle Gran Vía 123, Madrid", // DireccionEnvio
                MetodoPago.Tarjeta, // MetodoPago
                expectedItems,    // Items
                1,                // CompraID
                DateTime.Today,   // FechaCompra
                20.0f             // PrecioFinal
            );

            // Act - Buscar compra existente
            var result = await controller.GetMerchDetail(1);

            // Assert - Verificar que devuelve el objeto esperado
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualDetail = Assert.IsType<DetailMerchDTO>(okResult.Value);

            Assert.Equal(expectedDetail, actualDetail);
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetMerchDetail_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<POSTMerchController>>();
            var controller = new POSTMerchController(_context, mock.Object);

            // Act - Buscar compra que no existe
            var result = await controller.GetMerchDetail(999);

            // Assert - Verificar que devuelve NotFound
            Assert.IsType<NotFoundResult>(result);
        }
    }
}