using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AppForSEII2526.UIT.UC_CompraBonos
{
    public class SelectBonosParaComprar_PO:PageObject
    {
        By inputnombre = By.Id("inputTitle");
        By inputTipo = By.Id("selectTipoBocadillo");
        By buscaButton = By.Id("searchBonos");
        By tablaofBonos = By.Id("TableOfBonos");
        By compraButton = By.Id("compraButton");

        public SelectBonosParaComprar_PO(IWebDriver driver, ITestOutputHelper output) :base(driver, output)
        {
        }

        public void BuscarBonos(string nombre, string tipo)
        {
            WaitForBeingClickable(inputnombre);
            _driver.FindElement(inputnombre).SendKeys(nombre);
            if (tipo == "") tipo = "Todos";
            WaitForBeingClickable(inputTipo);
            SelectElement se = new SelectElement(_driver.FindElement(inputTipo));
            se.SelectByText(tipo);
            _driver.FindElement(buscaButton).Click();
        }

        public void seleccionarBonos(string nombre)
        {
            By buttonBocadillo = By.Id("BonoToBuy_"+nombre);
            WaitForBeingVisible(buttonBocadillo);
            WaitForBeingClickable(buttonBocadillo);
            _driver.FindElement(buttonBocadillo).Click();
        }

        public void seleccionarBotonCompra()
        {
            WaitForBeingVisible(compraButton);
            WaitForBeingClickable(compraButton);
            _driver.FindElement(compraButton).Click();
        }

        public void eliminarBono(string nombre)
        {
            By buttonBocadilloRemove = By.Id("removeBono_" + nombre);
            WaitForBeingVisible(buttonBocadilloRemove);
            WaitForBeingClickable(buttonBocadilloRemove);
            _driver.FindElement(buttonBocadilloRemove).Click();
        }
        
        public bool CheckListOfBonos(List<string[]> expectedBonos)
        {
            return CheckBodyTable(expectedBonos, tablaofBonos);
        }

        public bool buttonCompraAvailable()
        {
            return _driver.FindElement(compraButton).Displayed == true;
        }
        public bool checkErrorMessage(string error)
        {
            By errorContainer = By.Id("ErrorsShown");
            WaitForBeingVisible(errorContainer);
            IWebElement container= _driver.FindElement(errorContainer);
            string actualmessage = container.Text;
            return actualmessage.Contains(error);
        }
    }
}
