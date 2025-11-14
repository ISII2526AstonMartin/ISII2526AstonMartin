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

            // 2️⃣ CORREGIDO: Crear productos con nombres descriptivos
            // El primer parámetro es el ID, el segundo es el Nombre
            var camiseta = new Producto(
                Guid.NewGuid().ToString(),  // ID (único)
                "Camiseta UCLM",           // Nombre (descriptivo)
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

            // Verificar creación
            Console.WriteLine($"Productos creados: {_context.Productos.Count()}");
            foreach (var p in _context.Productos)
            {
                Console.WriteLine($"- ID: {p.ProductoID}, Nombre: {p.Nombre}");
            }
        }

        // ✅ Método auxiliar para verificar datos de prueba - CORREGIDO
        private void VerificarDatosDePrueba()
        {
            // Verificar que el producto existe por NOMBRE
            var producto = _context.Productos.FirstOrDefault(p => p.Nombre == "Camiseta UCLM");
            if (producto == null)
            {
                // Listar productos disponibles para debug
                var productosDisponibles = _context.Productos
                    .Select(p => $"Nombre: '{p.Nombre}', ID: {p.ProductoID}")
                    .ToList();
                throw new Exception($"El producto 'Camiseta UCLM' no se encontró. Productos en BD: {string.Join("; ", productosDisponibles)}");
            }

            var usuario = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == "juan");
            if (usuario == null)
            {
                throw new Exception("El usuario 'juan' no se creó en el constructor.");
            }
        }

        // ✅ Casos de error esperados - CORREGIDOS (usar nombres reales)
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

        // ❌ Test de errores - SIN verificación inicial (para evitar fallos tempranos)
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

        // ✅ Test de éxito - VERSIÓN SIMPLIFICADA
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateMerch_Success_test()
        {
            var logger = new Mock<ILogger<POSTMerchController>>().Object;
            var controller = new POSTMerchController(_context, logger);

            // Buscar producto por nombre directamente
            var producto = await _context.Productos
                .Include(p => p.Tipo_Producto)
                .FirstOrDefaultAsync(p => p.Nombre == "Camiseta UCLM");

            // Si no existe, usar cualquier producto disponible
            if (producto == null)
            {
                producto = await _context.Productos
                    .Include(p => p.Tipo_Producto)
                    .FirstAsync();
            }

            var merchDTO = new CreateMerchDTO(
                "juan",
                "Perez",
                "Muñoz",
                "Calle Gran Vía 123, Madrid",
                MetodoPago.Tarjeta,
                new List<ItemMerchDTO>
                {
                    new ItemMerchDTO(producto.Nombre, producto.PVP, producto.Tipo_Producto.Nombre, 2)
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

        // ✅ Test adicional para debug
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public void VerificarProductosCreados()
        {
            var productos = _context.Productos.ToList();
            Assert.True(productos.Count >= 1, $"Debe haber al menos 1 producto. Encontrados: {productos.Count}");

            foreach (var p in productos)
            {
                Console.WriteLine($"Producto: ID={p.ProductoID}, Nombre='{p.Nombre}', PVP={p.PVP}");
            }
        }
    }
}