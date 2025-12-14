using System;
using System.Collections.Generic;
using System.Threading;
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
            // Reintentos para evitar errores si la página se refresca sola
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    WaitForBeingClickable(_inputTipo);
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
                    break; // Si funciona, salimos
                }
                catch (StaleElementReferenceException)
                {
                    Thread.Sleep(1000); // Reintentamos
                }
            }
            Thread.Sleep(500); // Espera para que la tabla cargue
        }

        public void AddMerchToCart(string merchName)
        {
            try
            {
                By btnAdd = By.Id($"merchToAdd_{merchName}");
                WaitForBeingClickable(btnAdd);
                _driver.FindElement(btnAdd).Click();
            }
            catch (StaleElementReferenceException)
            {
                Thread.Sleep(500);
                By btnAdd = By.Id($"merchToAdd_{merchName}");
                _driver.FindElement(btnAdd).Click();
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

        public void ClickComprarMerch()
        {
            WaitForBeingClickable(_btnComprar);
            _driver.FindElement(_btnComprar).Click();
            Thread.Sleep(1000); // Espera para navegar al formulario
        }
    }
}