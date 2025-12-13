using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading; // Necesario para Thread.Sleep
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_CompraMerch
{
    public class SelectMerchParaComprar_PO : PageObject
    {
        private By _inputTipo = By.Id("inputTipo");
        private By _inputPrecio = By.Id("inputPrecio");
        private By _btnBuscar = By.Id("searchMerch");
        private By _tablaMerch = By.Id("TableOfMerch");
        private By _btnComprar = By.Id("comprarMerchButton");
        private By _errorShown = By.Id("ErrorsShown");

        public SelectMerchParaComprar_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SearchMerch(string tipo, string precioMax)
        {
            // Intentamos hasta 3 veces por si la página se está refrescando
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    // 1. Esperamos a que sea clickable
                    WaitForBeingClickable(_inputTipo);

                    // 2. Interactuamos
                    var tipoElement = _driver.FindElement(_inputTipo);
                    tipoElement.Clear();
                    tipoElement.SendKeys(tipo);

                    var precioElement = _driver.FindElement(_inputPrecio);
                    precioElement.Clear();
                    if (!string.IsNullOrEmpty(precioMax))
                    {
                        precioElement.SendKeys(precioMax);
                    }

                    _driver.FindElement(_btnBuscar).Click();

                    // Si llegamos aquí sin error, salimos del bucle
                    break;
                }
                catch (StaleElementReferenceException)
                {
                    // Si ocurre el error, esperamos un poco y el bucle (i) lo intentará de nuevo
                    _output.WriteLine($"Intento {i + 1}: Elemento obsoleto, reintentando...");
                    Thread.Sleep(1000);
                }
            }

            // Espera final para que la tabla se refresque tras el click en buscar
            Thread.Sleep(500);
        }

        public void AddMerchToCart(string merchName)
        {
            // También protegemos este método
            try
            {
                By btnAdd = By.Id($"merchToAdd_{merchName}");
                WaitForBeingClickable(btnAdd);
                _driver.FindElement(btnAdd).Click();
            }
            catch (StaleElementReferenceException)
            {
                Thread.Sleep(500); // Esperamos a que se asiente
                By btnAdd = By.Id($"merchToAdd_{merchName}");
                _driver.FindElement(btnAdd).Click(); // Reintentamos
            }
        }

        public void RemoveMerchFromCart(string merchName)
        {
            By btnRemove = By.Id($"removeMerch_{merchName}");
            WaitForBeingClickable(btnRemove);
            _driver.FindElement(btnRemove).Click();
        }

        public bool CheckListOfMerch(List<string[]> expectedMerch)
        {
            return CheckBodyTable(expectedMerch, _tablaMerch);
        }

        public bool CheckMessageError(string errorMessage)
        {
            try
            {
                IWebElement errorElement = _driver.FindElement(_errorShown);
                return errorElement.Text.Contains(errorMessage);
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool IsBuyButtonDisabled()
        {
            try
            {
                var btn = _driver.FindElement(_btnComprar);
                return !btn.Enabled;
            }
            catch (NoSuchElementException)
            {
                return true;
            }
        }
    }
}