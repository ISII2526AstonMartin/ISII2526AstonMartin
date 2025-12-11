using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.UC_CompraBonos
{
    public class SelectBonosParaComprar_PO:PageObject
    {
        By inputnombre = By.Id("inputTitle");
        By inputTipo = By.Id("selectTipoBocadillo");
        By compraButton = By.Id("searchBonos");
        By tablaofBonos = By.Id("TableOfBonos");

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
            _driver.FindElement(compraButton).Click();
        }
        
        public bool CheckListOfBonos(List<string[]> expectedBonos)
        {
            return CheckBodyTable(expectedBonos, tablaofBonos);
        }
    }
}
