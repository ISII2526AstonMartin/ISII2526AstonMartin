using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_CompraMerch
{
    public class SelectMerchParaComprar_PO : PageObject
    {
        // Selectores
        private By _inputTipo = By.Id("inputTipo");
        private By _inputPrecio = By.Id("inputPrecio");
        private By _btnBuscar = By.Id("searchMerch");
        private By _tablaMerch = By.Id("TableOfMerch");
        private By _btnComprar = By.Id("comprarMerchButton");

        public SelectMerchParaComprar_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SearchMerch(string tipo, string precioMax)
        {
            WaitForBeingClickable(_inputTipo);
            _driver.FindElement(_inputTipo).Clear();
            _driver.FindElement(_inputTipo).SendKeys(tipo);

            var precioElement = _driver.FindElement(_inputPrecio);
            precioElement.Clear();
            if (!string.IsNullOrEmpty(precioMax)) precioElement.SendKeys(precioMax);

            _driver.FindElement(_btnBuscar).Click();
            Thread.Sleep(500);
        }

        public void AddMerchToCart(string merchName)
        {
            By btnAdd = By.Id($"merchToAdd_{merchName}");
            WaitForBeingClickable(btnAdd);
            _driver.FindElement(btnAdd).Click();
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
            catch (NoSuchElementException) { return true; }
        }

        public void ClickComprarMerch()
        {
            WaitForBeingClickable(_btnComprar);
            _driver.FindElement(_btnComprar).Click();
            Thread.Sleep(1000);
        }

        // --- MÉTODO NECESARIO PARA EL TEST DE "VOLVER" ---
        public bool IsSearchInputVisible()
        {
            try
            {
                WaitForBeingVisible(_inputTipo);
                return _driver.FindElement(_inputTipo).Displayed;
            }
            catch { return false; }
        }
    }
}