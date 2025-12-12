using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_PedirBocadillo
{
    internal class CreatePedido_PO : PageObject
    {
        By inputNombre = By.Id("Nombre");
        By inputApellido1 = By.Id("Apellido1");
        By inputApellido2 = By.Id("Apellido2");
        By inputMetododePago = By.Id("MetodoPago");
        By compraButton = By.Id("Submit");
        By buttonModify = By.Id("ModifyCompra");
        By tablaofBocadillos = By.Id("TableOfRentalItems");
        By contprecio = By.Id("precioTotal");

        public CreatePedido_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }




        public void rellenarDatosParaCompra(string nombre, string apellido1, string apellido2, string metodopago)
        {
            WaitForBeingClickable(inputNombre);
            _driver.FindElement(inputNombre).SendKeys(nombre);
            WaitForBeingClickable(inputApellido1);
            _driver.FindElement(inputApellido1).SendKeys(apellido1);
            WaitForBeingClickable(inputApellido2);
            _driver.FindElement(inputApellido2).SendKeys(apellido2);
            WaitForBeingClickable(inputMetododePago);
            SelectElement se = new SelectElement(_driver.FindElement(inputMetododePago));
            se.SelectByText(metodopago);

        }

        public bool checkDatosUsuario(string nombre, string apellido1, string apellido2, string metodopago)
        {
            WaitForBeingVisible(inputNombre);
            IWebElement containername = _driver.FindElement(inputNombre);
            string actualname = containername.GetAttribute("value");

            WaitForBeingVisible(inputApellido1);
            IWebElement containerap1 = _driver.FindElement(inputApellido1);
            string actualap1 = containerap1.GetAttribute("value"); ;

            WaitForBeingVisible(inputApellido2);
            IWebElement containerap2 = _driver.FindElement(inputApellido2);
            string actualap2 = containerap2.GetAttribute("value"); ;

            WaitForBeingVisible(inputMetododePago);
            IWebElement containerpago = _driver.FindElement(inputMetododePago);
            string actualpago = containerpago.GetAttribute("value");

            return (actualname.Contains(nombre) && actualap1.Contains(apellido1) && actualap2.Contains(apellido2) && actualpago.Contains(metodopago));
        }



        public void modificarCantidadBocadillos(string id, string cantidad)
        {
            By cantidadBocadillosInput = By.Id("Cantidad:" + id);
            _driver.FindElement(cantidadBocadillosInput).Clear();
            _driver.FindElement(cantidadBocadillosInput).SendKeys(cantidad);

        }


        public bool checkPrecio(string precio)
        {
            WaitForBeingVisible(contprecio);
            IWebElement container = _driver.FindElement(contprecio);
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




        public void seleccionarBotonModificar()
        {
            WaitForBeingClickable(buttonModify);
            _driver.FindElement(buttonModify).Click();
        }




        public bool CheckListOfBocadillos(List<string[]> expectedBocadillos)
        {

            return CheckBodyTable(expectedBocadillos, tablaofBocadillos);


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
