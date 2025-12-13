using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_PedirBocadillo
{
    internal class DetailPedido_PO :PageObject
    {

        By tabladeBocadillos = By.Id("BocadilloComprado");
        By nombreYapellido = By.Id("NombreApellidos");
        By metododepago = By.Id("MetodoPago");
        By precioTotal1 = By.Id("PrecioTotal1");
        By preciofinal = By.Id("PrecioTotal");
        By FechaPedido = By.Id("FechaPedido");
        

        public DetailPedido_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }


        public bool CheckListOfBocadillos(List<string[]> expectedBocadillos)
        {

            WaitForBeingVisible(tabladeBocadillos);
            return CheckBodyTable(expectedBocadillos, tabladeBocadillos);

        }

        public bool CheckListOfDatos(string nombreyapellidos, string metodopago, string preciototal1, string fechapedido,string preciototal)
        {
            WaitForBeingVisible(nombreYapellido);
            IWebElement containername = _driver.FindElement(nombreYapellido);
            string actualname = containername.Text;

            WaitForBeingVisible(metododepago);
            IWebElement containerpago = _driver.FindElement(metododepago);
            string actualpago = containerpago.Text;

            WaitForBeingVisible(precioTotal1);
            IWebElement containerprecio = _driver.FindElement(precioTotal1);
            string actualprecio = containerprecio.Text;

            WaitForBeingVisible(FechaPedido);
            IWebElement containerfecha = _driver.FindElement(FechaPedido);
            string actualfecha = containerfecha.Text;

            WaitForBeingVisible(preciofinal);
            IWebElement containertotal = _driver.FindElement(preciofinal);
            string actualtotal = containertotal.Text;

            

            

            return actualname.Contains(nombreyapellidos) && actualpago.Contains(metodopago) && actualprecio.Contains(preciototal1) && actualtotal.Contains(preciototal) && actualfecha.Contains(fechapedido) && actualtotal.Contains(preciototal);
        }






    }
}

