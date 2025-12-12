using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Resenyas
{
    public class SelectBocadillosForResenya_PO : PageObject
    {

        By inputNombre = By.Id("inputNombre");
        By inputPrecio = By.Id("inputPrecio");
        By buttonSearchBocadillos = By.Id("searchBocadillos");
        By tableOfBocadillosBy = By.Id("TableOfBocadillos");
        By buttonCrearResenya = By.Id("createResenyaButton");

        By errorShownBy = By.Id("ErrorsShown");

        public SelectBocadillosForResenya_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SearchBocadillos(string nombre, string precio)
        {
            WaitForBeingClickable(inputNombre);

            _driver.FindElement(inputNombre).SendKeys(nombre);

            _driver.FindElement(inputPrecio).SendKeys(precio);

            
            _driver.FindElement(buttonSearchBocadillos).Click();
        }
        public bool CheckListOfBocadillos(List<string[]> expectedBocadillos)
        {

            return CheckBodyTable(expectedBocadillos, tableOfBocadillosBy);
        }

        public bool CheckMessageError(string errorMessage)
        {
            IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
            _output.WriteLine($"actual Message shown:{actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(errorMessage);
        }



        public void AddMovieToRentingCart(string bocadilloNombre)
        {
            WaitForBeingClickable(By.Id("bocadilloToAdd_" + bocadilloNombre));

            _driver.FindElement(By.Id("bocadilloToAdd_" + bocadilloNombre)).Click();
        }

        public void RemoveMovieFromRentingCart(string bocadilloNombre)
        {
            WaitForBeingClickable(By.Id("removeBocadillo_" + bocadilloNombre));
            _driver.FindElement(By.Id("removeBocadillo_" + bocadilloNombre)).Click();
        }

        public bool ResenyaNotAvailable()
        {
            //the button is not Displayed=hidden

            return _driver.FindElement(buttonCrearResenya).Displayed == false;
        }


    }
}