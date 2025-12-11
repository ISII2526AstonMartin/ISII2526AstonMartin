using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Rental
{
    public class UC_PedirBocadillo_UIT : UC_UIT
    {
        private SelectBocadillosParaPedir_PO selectBocadillosParaPedir_PO;
        private const int bocadilloID = 1;
        private const string nombreBocadillo1 = "BaconQueso";
        private const string tipoPan1 = "Normal";
        private const string precio1 = "3";
        private const string tamanyo1 = "Normal";

        private const string nombreBocadillo2 = "Serrano";
        private const string tipoPan2 = "Integral";
        private const string precio2 = "5";
        private const string tamanyo2 = "Pequeño";



        public UC_PedirBocadillo_UIT(ITestOutputHelper output) : base(output)
        {
            selectBocadillosParaPedir_PO = new SelectBocadillosParaPedir_PO(_driver, _output);


        }


        private void InitialStepsForCompraBocadillos()
        {
            _driver.Navigate().GoToUrl("https://localhost:7081/");
            selectBocadillosParaPedir_PO.WaitForBeingVisible(By.Id("CreateCompra"));

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(driver =>
            {
                try
                {
                    var element = driver.FindElement(By.Id("CreateCompra"));
                    return element != null && element.Displayed && element.Enabled;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            });

            // Localiza el botón justo antes de hacer clic para evitar stale element
            var createCompraButton = _driver.FindElement(By.Id("CreateCompra"));
            createCompraButton.Click();
        }




        [Theory]
        [InlineData(nombreBocadillo1, tipoPan1, precio1, tamanyo1, "Normal", "")]
        [InlineData(nombreBocadillo2, tipoPan2, precio2, tamanyo2, "", "Integral")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_AF1_UC2_4_5_6_filtering(string nombreBocadillo, string tipoPan, string precio, string tamanyo,
            string filterTamanyo, string filterTipoPan)
        {
            //Arrange
             InitialStepsForCompraBocadillos();
             var expectedBocadillos = new List<string[]> { new string[] { nombreBocadillo, tamanyo,tipoPan,precio }, };

            //Act

            selectBocadillosParaPedir_PO.SearchBocadillos(filterTamanyo, filterTipoPan);

            //Assert

            Assert.True(selectBocadillosParaPedir_PO.CheckListOfBocadillos(expectedBocadillos));

        }



        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]

        public void UC2_AF1_UC2_11_CompraNotAvailable()
        {
            //Arrange
            InitialStepsForCompraBocadillos();
            //Act
            selectBocadillosParaPedir_PO.AddBocadilloParaComprar(nombreBocadillo1);
            selectBocadillosParaPedir_PO.RemoveBocadilloParaComprar(nombreBocadillo1);

            //Assert

            Assert.True(selectBocadillosParaPedir_PO.CompraNotAvailable());

        }






    }


    }

