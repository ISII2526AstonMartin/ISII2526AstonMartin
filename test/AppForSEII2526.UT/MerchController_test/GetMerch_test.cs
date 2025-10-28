using AppForMovies.UT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;

namespace AppForSEII2526.UT.MerchController_test
{
    public class GetMerch_test:AppForMovies4SqliteUT
    {
        public GetMerch_test()
        {
            var tipos = new List<TipoProducto>()
           {
                new TipoProducto("Camiseta", "1", new List<Producto>()),
                new TipoProducto("Gorra", "2", new List<Producto>()),
                new TipoProducto("Boligrafo", "3", new List<Producto>()),
           };
            var productos = new List<Producto>()
           {
                new Producto("Camiseta", "1", 8, 48, tipos[0], new List<Producto_Compra>()),
                new Producto("Gorra", "2" , 4, 10, tipos[1], new List<Producto_Compra>()),
                new Producto("Boligrafo", "3", 2, 39, tipos[2], new List<Producto_Compra>())
            };
            _context.AddRange(tipos);
            _context.AddRange(productos);
            _context.SaveChanges();
        }
        [Fact]
        public async Task GetMerch_nombre()
        {
            //arrange
            List<MerchDTO> expectedMerch = new List<MerchDTO>()
            {
                new MerchDTO("Gorra", 4, new TipoProducto("Gorra", "2", new List<Producto>()), 10)
            };
            var mock = new Mock<ILogger<MerchController>>();
            ILogger<MerchController> logger = mock.Object;
            MerchController controller = new MerchController(_context, logger);
            //act
            var result =   await controller.GetProductos("Gorra", null);
            //assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var MerchActualResult = Assert.IsType<List<MerchDTO>>(okResult.Value);
            Assert.Equal(expectedMerch, MerchActualResult);
        }
            
        
    }
}
