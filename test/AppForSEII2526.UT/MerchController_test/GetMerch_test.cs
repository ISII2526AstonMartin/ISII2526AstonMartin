using AppForMovies.UT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using AppForSEII2526.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.UT.MerchController_test
{
    public class GetMerch_test : AppForMovies4SqliteUT
    {
        public GetMerch_test()
        {
            // Limpiar y crear base de datos de pruebas
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Crear tipos de producto para pruebas
            var tipos = new List<TipoProducto>()
            {
                new TipoProducto("Camiseta", 1, new List<Producto>()),
                new TipoProducto("Gorra", 2, new List<Producto>()),
                new TipoProducto("Boligrafo", 3, new List<Producto>()),
            };

            // Crear productos de prueba
            var productos = new List<Producto>()
            {
                new Producto("Camiseta UCLM",1, 8, 48, tipos[0], new List<Producto_Compra>()),
                new Producto("Gorra UCLM",2, 4, 10, tipos[1], new List<Producto_Compra>()),
                new Producto("Boligrafo UCLM",3, 2, 39, tipos[2], new List<Producto_Compra>()),
                new Producto("Camiseta Verde",4, 12, 5, tipos[0], new List<Producto_Compra>())
            };

            _context.TiposProductos.AddRange(tipos);
            _context.Productos.AddRange(productos);
            _context.SaveChanges();
        }

        // Casos de prueba para filtros correctos
        public static IEnumerable<object[]> TestCasesForGetMerchOK()
        {
            // Crear los objetos MerchDTO esperados para cada caso
            var tipoCamiseta = new TipoProducto("Camiseta", 1, new List<Producto>());
            var tipoGorra = new TipoProducto("Gorra", 2, new List<Producto>());
            var tipoBoligrafo = new TipoProducto("Boligrafo", 3, new List<Producto>());

            var allTests = new List<object[]>
            {
                // Caso 1: Sin filtros - debe devolver todos los productos
                new object[] {
                    null, null,
                    new List<MerchDTO>
                    {
                        new MerchDTO("Camiseta UCLM", 8, tipoCamiseta, 48),
                        new MerchDTO("Gorra UCLM", 4, tipoGorra, 10),
                        new MerchDTO("Boligrafo UCLM", 2, tipoBoligrafo, 39),
                        new MerchDTO("Camiseta Verde", 12, tipoCamiseta, 5)
                    }
                },
                
                // Caso 2: Filtro por tipo "Camiseta" - debe devolver 2 productos
                new object[] {
                    "Camiseta", null,
                    new List<MerchDTO>
                    {
                        new MerchDTO("Camiseta UCLM", 8, tipoCamiseta, 48),
                        new MerchDTO("Camiseta Verde", 12, tipoCamiseta, 5)
                    }
                },
                
                // Caso 3: Filtro por precio maximo 5 - debe devolver 2 productos
                new object[] {
                    null, 5f,
                    new List<MerchDTO>
                    {
                        new MerchDTO("Gorra UCLM", 4, tipoGorra, 10),
                        new MerchDTO("Boligrafo UCLM", 2, tipoBoligrafo, 39)
                    }
                },
                
                // Caso 4: Filtro por tipo "Camiseta" y precio maximo 10 - debe devolver 1 producto
                new object[] {
                    "Camiseta", 10f,
                    new List<MerchDTO>
                    {
                        new MerchDTO("Camiseta UCLM", 8, tipoCamiseta, 48),
                    }
                }
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForGetMerchOK))]
        public async Task GetMerch_GoodResult_test(string? tipo, float? precio, IList<MerchDTO> expectedProducts)
        {
            // Preparacion
            var mock = new Mock<ILogger<MerchController>>();
            ILogger<MerchController> logger = mock.Object;
            MerchController controller = new MerchController(_context, logger);

            // Ejecucion
            var result = await controller.GetProductos(tipo, precio);

            // Verificacion
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = Assert.IsType<List<MerchDTO>>(okResult.Value);

            // Assert.Equivalent verifica que las colecciones tienen los mismos elementos sin importar el orden
            Assert.Equivalent(expectedProducts, actualResult);
        }

        // Casos de prueba para filtros que no devuelven resultados
        public static IEnumerable<object[]> TestCasesForGetMerchBad()
        {
            var allTests = new List<object[]>
            {
                new object[] { "TipoInexistente", null },     // Tipo que no existe en la base de datos
                new object[] { null, 1f },                    // Precio maximo demasiado bajo
                new object[] { "TipoInexistente", 1f }        // Combinacion de filtros sin resultados
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForGetMerchBad))]
        public async Task GetMerch_BadResult_test(string? tipo, float? precio)
        {
            // Preparacion
            var mock = new Mock<ILogger<MerchController>>();
            ILogger<MerchController> logger = mock.Object;
            MerchController controller = new MerchController(_context, logger);

            // Ejecucion
            var result = await controller.GetProductos(tipo, precio);

            // Verificacion: debe devolver NotFound cuando no hay resultados
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        }

        // Test adicional: verificar que sin filtros se devuelven todos los productos
        [Fact]
        public async Task GetMerch_AllProducts_NoFilters()
        {
            // Preparacion
            var mock = new Mock<ILogger<MerchController>>();
            ILogger<MerchController> logger = mock.Object;
            MerchController controller = new MerchController(_context, logger);

            // Ejecucion
            var result = await controller.GetProductos(null, null);

            // Verificacion
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = Assert.IsType<List<MerchDTO>>(okResult.Value);

            // Verificar que se devuelven exactamente los 4 productos esperados
            var tipoCamiseta = new TipoProducto("Camiseta", 1, new List<Producto>());
            var tipoGorra = new TipoProducto("Gorra", 2, new List<Producto>());
            var tipoBoligrafo = new TipoProducto("Boligrafo", 3, new List<Producto>());

            var expectedProducts = new List<MerchDTO>
            {
                 new MerchDTO("Camiseta UCLM", 8, tipoCamiseta, 48),
                 new MerchDTO("Gorra UCLM", 4, tipoGorra, 10),
                 new MerchDTO("Boligrafo UCLM", 2, tipoBoligrafo, 39),
                 new MerchDTO("Camiseta Verde", 12, tipoCamiseta, 5)
            };

            // Assert.Equivalent verifica que las colecciones tienen los mismos elementos sin importar el orden
            Assert.Equivalent(expectedProducts, actualResult);
        }
    }
}