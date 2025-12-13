using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
namespace AppForSEII2526.UIT.UC_Rental
{
    public class SelectBocadillosParaPedir_PO : PageObject
    {
        By inputTamanyo = By.Id("selectTamanyo");
        By inputTipoPan = By.Id("selectTipoPan");
        By buttonSearchBocadillos = By.Id("searchBocadillos");
        By tableOfBocadillos = By.Id("TableOfBocadillos");
        By errorShownBy = By.Id("ErrorsShown");
        By buttonComprarBocadillos = By.Id("purchaseMovieButton");
        public SelectBocadillosParaPedir_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public void SearchBocadillos(string tamanyo, string tipoPan)
        {
            WaitForBeingVisibleIgnoringExeptionTypes(inputTamanyo);
            //wait for the webelement to be clickable
            WaitForBeingClickable(inputTamanyo);
            _driver.FindElement(inputTamanyo).SendKeys(tamanyo);
           


            if (tipoPan == "") tipoPan = "All";
            SelectElement selectElement = new SelectElement(_driver.FindElement(inputTipoPan));
            selectElement.SelectByText(tipoPan);

            if (tamanyo == "") tamanyo = "All";
            selectElement = new SelectElement(_driver.FindElement(inputTamanyo));
            selectElement.SelectByText(tamanyo);

           var createCompraButton=  _driver.FindElement(buttonSearchBocadillos);
                createCompraButton.Click();
        }


        public bool CheckListOfBocadillos(List<string[]> expectedBocadillos)
        {

            return CheckBodyTable(expectedBocadillos, tableOfBocadillos);
        }


        public void AddBocadilloParaComprar(string nombreBocadillo)
        {
            WaitForBeingClickable(By.Id("movieToRent_" + nombreBocadillo));

            _driver.FindElement(By.Id("movieToRent_" + nombreBocadillo)).Click();
        }

        public void seleccionarBotonCompra()
        {
            WaitForBeingVisible(buttonComprarBocadillos);
            WaitForBeingClickable(buttonComprarBocadillos);
            _driver.FindElement(buttonComprarBocadillos).Click();
        }

        public void RemoveBocadilloParaComprar(string nombreBocadillo)
        {
            WaitForBeingClickable(By.Id("removeMovie_" + nombreBocadillo));
            _driver.FindElement(By.Id("removeMovie_" + nombreBocadillo)).Click();
        }

        public bool CompraNotAvailable()
        {
            //the button is not Displayed=hidden

            return _driver.FindElement(buttonComprarBocadillos).Displayed == false;
        }
    }
}