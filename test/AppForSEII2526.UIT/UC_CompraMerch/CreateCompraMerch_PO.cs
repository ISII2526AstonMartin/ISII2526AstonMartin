using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic; // Necesario para List
using System.Linq;
using System.Threading;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_CompraMerch
{
    public class CreateCompraMerch_PO : PageObject
    {
        // 1. Selectores
        By inputNombre = By.Id("NombreUsuario");
        By inputApellido1 = By.Id("Apellido1");
        By inputApellido2 = By.Id("Apellido2");
        By inputDireccion = By.Id("DireccionEnvio");
        By inputPago = By.Id("MetodoPago");

        By compraButton = By.XPath("//button[@type='submit']");
        By ticketHeader = By.XPath("//h4[contains(text(),'Ticket de Compra')]");

        public CreateCompraMerch_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void rellenarDatosParaCompra(string nombre, string ap1, string ap2, string direccion, string pago)
        {
            // Nombre
            WaitForBeingClickable(inputNombre);
            _driver.FindElement(inputNombre).Clear();
            _driver.FindElement(inputNombre).SendKeys(nombre);

            // Apellidos
            WaitForBeingClickable(inputApellido1);
            _driver.FindElement(inputApellido1).Clear();
            _driver.FindElement(inputApellido1).SendKeys(ap1);

            WaitForBeingClickable(inputApellido2);
            _driver.FindElement(inputApellido2).Clear();
            if (!string.IsNullOrEmpty(ap2))
                _driver.FindElement(inputApellido2).SendKeys(ap2);

            // Dirección + TAB
            WaitForBeingClickable(inputDireccion);
            var dirEl = _driver.FindElement(inputDireccion);
            dirEl.Clear();
            dirEl.SendKeys(direccion);
            dirEl.SendKeys(Keys.Tab);
            Thread.Sleep(500);

            // Pago
            WaitForBeingClickable(inputPago);
            SelectElement se = new SelectElement(_driver.FindElement(inputPago));
            se.SelectByText(pago);
        }

        public void seleccionarBotonCompra()
        {
            Thread.Sleep(1000);
            if (IsTicketDisplayed()) return;

            try
            {
                WaitForBeingVisible(compraButton);
                var btn = _driver.FindElement(compraButton);

                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", btn);
                Thread.Sleep(500);
                btn.Click();
            }
            catch (Exception)
            {
                var btnAlt = _driver.FindElement(By.XPath("//button[contains(text(),'Comprar') or contains(text(),'Pagar')]"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", btnAlt);
            }
            Thread.Sleep(2000);
        }

        public bool IsTicketDisplayed()
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));
                return wait.Until(d => d.FindElement(ticketHeader).Displayed);
            }
            catch { return false; }
        }

        // --- MÉTODO CORREGIDO PARA ENCONTRAR EL ERROR ---
        public bool checkErrorMessage(string error)
        {
            // 1. Esperamos un poco a que la validación salte
            Thread.Sleep(1000);

            // 2. Definimos todos los posibles sitios donde sale el error
            var posiblesLugares = new List<By>
            {
                By.Id("ErrorsShown"),             // caja personalizada
                By.ClassName("validation-message"), // Mensajes debajo del input (Blazor estándar)
                By.CssSelector(".alert-danger"),    // Alertas genéricas
                By.ClassName("text-danger")         // Textos rojos genéricos
            };

            foreach (var locator in posiblesLugares)
            {
                try
                {
                    var elementos = _driver.FindElements(locator);
                    foreach (var el in elementos)
                    {
                        // Si el elemento es visible y contiene el texto (ignorando mayúsculas)
                        if (el.Displayed && el.Text.IndexOf(error, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            return true; 
                        }
                    }
                }
                catch { /* Ignorar si no encuentra ese tipo de elemento. */ }
            }

            return false;
        }
    }
}