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
                new TipoProducto("Camiseta", "1", new List<Producto>()),
                new TipoProducto("Gorra", "2", new List<Producto>()),
                new TipoProducto("Boligrafo", "3", new List<Producto>()),
            };

            // Crear productos de prueba
            // NOTA: Los productos se guardan con IDs como nombres ("1", "2", "3", "4")
            // debido a la configuracion del modelo Producto
            var productos = new List<Producto>()
            {
                new Producto("1", "Camiseta UCLM", 8, 48, tipos[0], new List<Producto_Compra>()),
                new Producto("2", "Gorra UCLM", 4, 10, tipos[1], new List<Producto_Compra>()),
                new Producto("3", "Boligrafo UCLM", 2, 39, tipos[2], new List<Producto_Compra>()),
                new Producto("4", "Camiseta Verde", 12, 5, tipos[0], new List<Producto_Compra>())
            };

            _context.TiposProductos.AddRange(tipos);
            _context.Productos.AddRange(productos);
            _context.SaveChanges();
        }

        // Casos de prueba para filtros correctos
        public static IEnumerable<object[]> TestCasesForGetMerchOK()
        {
            var allTests = new List<object[]>
            {
                // Caso 1: Sin filtros - debe devolver todos los productos
                new object[] {
                    null, null,
                    new List<(string nombre, float precio, string tipo, int stock)>
                    {
                        ("1", 8, "Camiseta", 48),
                        ("2", 4, "Gorra", 10),
                        ("3", 2, "Boligrafo", 39),
                        ("4", 12, "Camiseta", 5)
                    }
                },
                
                // Caso 2: Filtro por tipo "Camiseta" - debe devolver 2 productos
                new object[] {
                    "Camiseta", null,
                    new List<(string nombre, float precio, string tipo, int stock)>
                    {
                        ("1", 8, "Camiseta", 48),
                        ("4", 12, "Camiseta", 5)
                    }
                },
                
                // Caso 3: Filtro por precio maximo 5 - debe devolver 2 productos
                new object[] {
                    null, 5f,
                    new List<(string nombre, float precio, string tipo, int stock)>
                    {
                        ("2", 4, "Gorra", 10),
                        ("3", 2, "Boligrafo", 39)
                    }
                },
                
                // Caso 4: Filtro por tipo "Camiseta" y precio maximo 10 - debe devolver 1 producto
                new object[] {
                    "Camiseta", 10f,
                    new List<(string nombre, float precio, string tipo, int stock)>
                    {
                        ("1", 8, "Camiseta", 48)
                    }
                }
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForGetMerchOK))]
        public async Task GetMerch_GoodResult_test(string? tipo, float? precio, IList<(string nombre, float precio, string tipo, int stock)> expectedProducts)
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

            // Verificar que se devuelve la cantidad correcta de productos
            Assert.Equal(expectedProducts.Count, actualResult.Count);

            // Verificar que cada producto esperado esta presente en el resultado
            foreach (var expected in expectedProducts)
            {
                var actualProduct = actualResult.FirstOrDefault(p =>
                    p.Nombre == expected.nombre &&
                    p.Precio == expected.precio);

                // Si el producto no se encuentra, el test fallara con este mensaje
                Assert.NotNull(actualProduct);

                // Verificar todas las propiedades del producto
                Assert.Equal(expected.nombre, actualProduct.Nombre);
                Assert.Equal(expected.precio, actualProduct.Precio);
                Assert.Equal(expected.tipo, actualProduct.Tipo?.Nombre);
                Assert.Equal(expected.stock, actualProduct.Stock);
            }
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

            // Deben devolverse 4 productos cuando no hay filtros
            Assert.Equal(4, actualResult.Count);
        }
    }
}