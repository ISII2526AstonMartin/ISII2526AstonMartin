using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_CrearResenya
{
    public class ResenyaDetail_PO : PageObject
    {
        // Localizadores
        private By _labelTitulo = By.Id("Titulo");
        private By _labelDescripcion = By.Id("Descripcion");
        private By _labelValoracion = By.Id("ValoracionGeneral");

        public ResenyaDetail_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void WaitForDetailsPage()
        {
            WaitForBeingVisible(_labelTitulo);
        }

        public bool VerifyResenyaData(string tituloEsperado, string descripcionEsperada, string valoracionEsperada)
        {
            try
            {
                WaitForDetailsPage();

                string tituloActual = _driver.FindElement(_labelTitulo).Text;
                string descActual = _driver.FindElement(_labelDescripcion).Text;
                string valActual = _driver.FindElement(_labelValoracion).Text;

                _output.WriteLine($"Verificando Detalle -> Título: {tituloActual}, Val: {valActual}");

                bool checkTitulo = tituloActual == tituloEsperado;
                bool checkDesc = descActual == descripcionEsperada;

                bool checkVal = valActual.Contains(valoracionEsperada) || valoracionEsperada.Contains(valActual);

                return checkTitulo && checkDesc && checkVal;
            }
            catch (Exception ex)
            {
                _output.WriteLine("Error verificando datos de la reseña: " + ex.Message);
                return false;
            }
        }

        public bool VerifyBocadilloPuntuado(string nombreBocadillo, string puntuacionEsperada)
        {
            try
            {
                By rowLocator = By.Id($"ItemResenya_{nombreBocadillo}");
                WaitForBeingVisible(rowLocator);
                IWebElement row = _driver.FindElement(rowLocator);
                return row.Text.Contains(puntuacionEsperada);
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }


        public bool VerifyBocadilloNoExiste(string nombreBocadillo)
        {
           
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);

            try
            {
                By rowLocator = By.Id($"ItemResenya_{nombreBocadillo}");
                var elementos = _driver.FindElements(rowLocator);

                bool noExiste = elementos.Count == 0;

                if (!noExiste) _output.WriteLine($"Error: El bocadillo {nombreBocadillo} aparece en el detalle y no debería.");

                return noExiste;
            }
            finally
            {
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            }
        }


    }
}
