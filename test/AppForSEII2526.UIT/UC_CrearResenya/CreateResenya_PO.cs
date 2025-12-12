using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_CrearResenya
{
    public class CreateResenya_PO : PageObject
    {
        By buttonCrearResenya = By.Id("createResenyaButton");

        By inputUsuario = By.Id("NombreUsuario");
        By inputTitulo = By.Id("Titulo");
        By inputDescripcion = By.Id("Descripcion");

        By selectValoracion = By.Id("ValoracionGeneral");

        By btnSubmit = By.Id("Submit"); 
        By btnModificar = By.Id("ModificarResenyas"); 

        By errorBox = By.Id("ErrorsShown");

        public CreateResenya_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public void RellenarFormulario(string usuario, string titulo, string descripcion, string valoracionGeneral)
        {
            WaitForBeingVisible(inputUsuario);

            if (usuario != null)
            {
                _driver.FindElement(inputUsuario).Clear();
                _driver.FindElement(inputUsuario).SendKeys(usuario);
            }

            if (titulo != null)
            {
                _driver.FindElement(inputTitulo).Clear();
                _driver.FindElement(inputTitulo).SendKeys(titulo);
            }

            if (descripcion != null)
            {
                _driver.FindElement(inputDescripcion).Clear();
                _driver.FindElement(inputDescripcion).SendKeys(descripcion);
            }

            if (!string.IsNullOrEmpty(valoracionGeneral))
            {
                
                var selectElement = new SelectElement(_driver.FindElement(selectValoracion));

                try
                {

                    foreach (var option in selectElement.Options)
                    {
                        if (option.Text.Contains(valoracionGeneral))
                        {
                            option.Click();
                            break;
                        }
                    }
                }
                catch
                {
                    _driver.FindElement(selectValoracion).SendKeys(valoracionGeneral);
                }
            }
        }

        public void PuntuarBocadillo(string nombreBocadillo, string puntuacion)
        {
            string xpathInput = $"//tr[@id='BocadilloData_{nombreBocadillo}']//input";
            By inputLocator = By.XPath(xpathInput);

            WaitForBeingVisible(inputLocator);

            var inputElement = _driver.FindElement(inputLocator);

            inputElement.SendKeys(Keys.Control + "a");
            inputElement.SendKeys(Keys.Delete);

            inputElement.SendKeys(puntuacion);
        }

        public void PulsarCrearResenyas()
        {
            WaitForBeingClickable(btnSubmit);
            _driver.FindElement(btnSubmit).Click();
        }

        public void PulsarModificarResenyas()
        {
            WaitForBeingClickable(btnModificar);
            _driver.FindElement(btnModificar).Click();
        }

        public void ConfirmarDialogo()
        {
           
            PressOkModalDialog();
        }



        public bool CheckMessageError(string errorMessage)
        {
            try
            {
                WaitForBeingVisibleIgnoringExeptionTypes(errorBox);
                IWebElement actualErrorShown = _driver.FindElement(errorBox);
                _output.WriteLine($"Actual Error: {actualErrorShown.Text}");
                return actualErrorShown.Text.Contains(errorMessage);
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public void ClickIrACrearResenya()
        {
            WaitForBeingClickable(buttonCrearResenya);
            _driver.FindElement(buttonCrearResenya).Click();
        }

        public bool EsBocadilloVisible(string nombreBocadillo)
        {
          
            By rowLocator = By.Id($"BocadilloData_{nombreBocadillo}");

        
            var elementos = _driver.FindElements(rowLocator);
            return elementos.Count > 0 && elementos[0].Displayed;
        }

        public string ObtenerMensajeValidacionHtml5(string nombreBocadillo)
        {
            string xpathInput = $"//tr[@id='BocadilloData_{nombreBocadillo}']//input";
            IWebElement inputElement = _driver.FindElement(By.XPath(xpathInput));

            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            string validacion = (string)js.ExecuteScript("return arguments[0].validationMessage;", inputElement);

            return validacion;
        }
    }
}
