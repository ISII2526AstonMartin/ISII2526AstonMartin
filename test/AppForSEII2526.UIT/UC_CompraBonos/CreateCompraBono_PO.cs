using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.UIT.UC_CompraBonos
{
    public class CreateCompraBono_PO : PageObject
    {
        By inputNombre = By.Id("Name");
        By inputap1 = By.Id("Surname");
        By inputap2 = By.Id("Surname2");
        By inputPago = By.Id("PaymentMethod");
        By tablaofBonos = By.Id("TableOfRentalItems");
        By compraButton = By.Id("Submit");
        By preciocont = By.Id("precioTotalID");
        By buttonVolver = By.Id("ModifyMovies");
        public CreateCompraBono_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void rellenarDatosCompra(string name, string ap1, string ap2, string mp)
        {
            WaitForBeingClickable(inputNombre);
            _driver.FindElement(inputNombre).SendKeys(name);
            WaitForBeingClickable(inputap1);
            _driver.FindElement(inputap1).SendKeys(ap1);
            WaitForBeingClickable(inputap2);
            _driver.FindElement(inputap2).SendKeys(ap2);
            WaitForBeingClickable(inputPago);
            SelectElement se = new SelectElement(_driver.FindElement(inputPago));
            se.SelectByText(mp);

        }

        public bool checkDatosUsuario(string name, string ap1, string ap2, string mp)
        {
            WaitForBeingVisible(inputNombre);
            IWebElement containername= _driver.FindElement(inputNombre);
            string actualname = containername.GetAttribute("value");

            WaitForBeingVisible(inputap1);
            IWebElement containerap1 = _driver.FindElement(inputap1);
            string actualap1 = containerap1.GetAttribute("value"); ;

            WaitForBeingVisible(inputap2);
            IWebElement containerap2 = _driver.FindElement(inputap2);
            string actualap2 = containerap2.GetAttribute("value"); ;

            WaitForBeingVisible(inputPago);
            IWebElement containerpago = _driver.FindElement(inputPago);
            string actualpago = containerpago.GetAttribute("value");

            return (actualname.Contains(name)&&actualap1.Contains(ap1)&& actualap2.Contains(ap2) && actualpago.Contains(mp));
        }

        public void modificarCantidadBono(string id, string cantidad)
        {
            By cantidadBonoInput = By.Id("cantidad_"+id);
            _driver.FindElement(cantidadBonoInput).Clear();
            _driver.FindElement(cantidadBonoInput).SendKeys(cantidad);

        }

        public bool checkPrecio(string precio)
        {
            WaitForBeingVisible(preciocont);
            IWebElement container = _driver.FindElement(preciocont);
            string actualmessage = container.Text;
            return actualmessage.Contains(precio);
        }

        public void seleccionarBotonCompra()
        {
            _driver.FindElement(compraButton).Click();
            By buttondialog = By.Id("Button_DialogOK");
            WaitForBeingVisible(buttondialog);
            WaitForBeingClickable(buttondialog);
            _driver.FindElement(buttondialog).Click();
        }
        public void seleccionarBotonVolver()
        {
            WaitForBeingClickable(buttonVolver);
            _driver.FindElement(buttonVolver).Click();
        }

        public bool CheckListOfBonos(List<string[]> expectedBonos)
        {
            return CheckBodyTable(expectedBonos, tablaofBonos);
        }

        public bool checkErrorMessage(string error)
        {
            By errorContainer = By.Id("ErrorsShown");
            WaitForBeingVisible(errorContainer);
            IWebElement container = _driver.FindElement(errorContainer);
            string actualmessage = container.Text;
            return actualmessage.Contains(error);
        }
    }
}
