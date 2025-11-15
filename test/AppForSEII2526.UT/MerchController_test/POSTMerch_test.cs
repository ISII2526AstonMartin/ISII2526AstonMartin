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
            // Estos tipos se utilizaran para categorizar los productos
            var tipoRopa = new TipoProducto("Ropa", Guid.NewGuid().ToString(), new List<Producto>());
            var tipoAccesorio = new TipoProducto("Accesorios", Guid.NewGuid().ToString(), new List<Producto>());

            _context.TiposProductos.AddRange(tipoRopa, tipoAccesorio);
            _context.SaveChanges();

            // Crear productos de prueba con datos realistas
            // Cada producto tiene un ID unico, nombre, precio, stock y tipo asociado
            var camiseta = new Producto(
                Guid.NewGuid().ToString(),  // Identificador unico generado automaticamente
                "Camiseta UCLM",           // Nombre descriptivo del producto
                8,                         // Precio de venta al publico
                48,                        // Cantidad disponible en stock
                tipoRopa,                  // Tipo de producto asociado
                new List<Producto_Compra>() // Lista vacia de compras asociadas
            );

            var gorra = new Producto(
                Guid.NewGuid().ToString(),  // Identificador unico
                "Gorra",                   // Nombre del producto
                4,                         // Precio
                10,                        // Stock disponible
                tipoAccesorio,             // Tipo de producto
                new List<Producto_Compra>() // Lista vacia de compras
            );

            _context.Productos.AddRange(camiseta, gorra);
            _context.SaveChanges();

            // Crear usuarios de prueba para simular clientes del sistema
            // Estos usuarios se utilizaran en las pruebas de compra
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
        // Cada caso prueba una condicion de error especifica
        public static IEnumerable<object[]> TestCasesFor_CreateMerch()
        {
            // Caso 1: Intento de compra sin productos en el carrito
            var merchSinItems = new CreateMerchDTO(
                "rafamartinez", "Martinez", "Muñoz",
                "Calle Gran Vía 123",
                MetodoPago.Tarjeta,
                new List<ItemMerchDTO>() // Lista vacia de productos
            );

            // Caso 2: Usuario no registrado en el sistema
            var merchUsuarioNoExiste = new CreateMerchDTO(
                "usuarioNoExiste", "Martinez", "Muñoz",
                "Calle Sol 123",
                MetodoPago.Paypal,
                new List<ItemMerchDTO> { new ItemMerchDTO("Camiseta UCLM", 8, "Ropa", 1) }
            );

            // Caso 3: Producto que no existe en el catalogo
            var merchProductoNoExiste = new CreateMerchDTO(
                "juan", "Perez", "Muñoz",
                "Calle Luna 45",
                MetodoPago.Gpay,
                new List<ItemMerchDTO> { new ItemMerchDTO("ProductoFantasma", 10f, "Ropa", 1) }
            );

            // Caso 4: Cantidad invalida (cero o negativa)
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

        // Prueba parametrizada para verificar los casos de error
        // Esta prueba ejecuta multiples escenarios de error usando los datos proporcionados
        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateMerch))]
        public async Task CreateMerch_Error_test(CreateMerchDTO merchDTO, string errorEsperado)
        {
            // Preparacion: crear el controlador y logger mock
            var logger = new Mock<ILogger<POSTMerchController>>().Object;
            var controller = new POSTMerchController(_context, logger);

            // Ejecucion: llamar al metodo del controlador
            var result = await controller.CreateMerch(merchDTO);

            // Verificacion: asegurar que se devuelve un error BadRequest
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var errorActual = problemDetails.Errors.First().Value[0];

            // Verificar que el mensaje de error coincide con el esperado
            Assert.StartsWith(errorEsperado, errorActual);
        }

        // Prueba para el caso de exito en la creacion de merchandising
        // Esta prueba verifica que una compra valida se procesa correctamente
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateMerch_Success_test()
        {
            // Preparacion: crear el controlador y logger mock
            var logger = new Mock<ILogger<POSTMerchController>>().Object;
            var controller = new POSTMerchController(_context, logger);

            // Obtener un producto existente de la base de datos para la compra
            var producto = await _context.Productos
                .Include(p => p.Tipo_Producto)
                .FirstAsync();

            // Crear la lista de items para la compra
            // En este caso, se compran 2 unidades del producto seleccionado
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

            // Crear el DetailMerchDTO esperado para comparacion
            // Nota: No podemos predecir valores exactos para CompraID y FechaCompra
            // ya que se generan automaticamente en el controlador
            var expectedDetail = new DetailMerchDTO(
                "juan",
                "Perez",
                "Muñoz",
                "Calle Gran Vía 123, Madrid",
                MetodoPago.Tarjeta,
                items,
                "dummy-id", // Valor temporal para el ID de compra
                DateTime.MinValue, // Valor temporal para la fecha de compra
                producto.PVP * 2 // Precio final calculado (precio unitario * cantidad)
            );

            // Ejecucion: llamar al metodo del controlador para crear la compra
            var result = await controller.CreateMerch(merchDTO);

            // Verificacion: asegurar que se devuelve un resultado Created
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualDetail = Assert.IsType<DetailMerchDTO>(createdResult.Value);

            // Verificar que las propiedades generadas automaticamente tienen valores validos
            // El ID de compra no debe estar vacio y debe ser diferente del valor temporal
            Assert.False(string.IsNullOrEmpty(actualDetail.CompraID));
            Assert.NotEqual("dummy-id", actualDetail.CompraID);

            // La fecha de compra debe ser reciente (dentro del ultimo minuto)
            Assert.True(actualDetail.FechaCompra > DateTime.Now.AddMinutes(-1));

            // Crear un nuevo DetailMerchDTO para comparacion usando los valores reales
            // Esto permite una comparacion exacta usando Assert.Equal
            var expectedForComparison = new DetailMerchDTO(
                expectedDetail.NombreUsuario,
                expectedDetail.Apellido1,
                expectedDetail.Apellido2,
                expectedDetail.DireccionEnvio,
                expectedDetail.MetodoPago,
                expectedDetail.Items,
                actualDetail.CompraID, // Usar el ID real generado por el controlador
                actualDetail.FechaCompra, // Usar la fecha real generada por el controlador
                expectedDetail.PrecioFinal // El precio final calculado debe coincidir
            );

            // Comparacion final: verificar que el objeto devuelto coincide con el esperado
            // Esta comparacion utiliza la implementacion de Equals en los DTOs
            Assert.Equal(expectedForComparison, actualDetail);
        }
    }
}