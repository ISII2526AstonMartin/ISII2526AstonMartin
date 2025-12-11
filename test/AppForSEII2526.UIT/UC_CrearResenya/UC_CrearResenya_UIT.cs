
using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.Shared;
using AppForSEII2526.UIT.UC_Resenyas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_CrearResenya
{
    public class UC_CrearResenya_UIT : UC_UIT
    {
        private SelectBocadillosForResenya_PO SelectBocadillosForResenya_PO;
        private const string bocadilloNombre1 = "Serrano";
        private const string bocadilloTamanyo1 = "Normal";
        private const string bocadilloTipoPan1 = "Integral";
        private const string bocadilloPrecio1 = "5";

        private const string bocadilloNombre2 = "Bacon";
        private const string bocadilloTamanyo2 = "Pequeño";
        private const string bocadilloTipoPan2 = "Semilla";
        private const string bocadilloPrecio2 = "2";


        public UC_CrearResenya_UIT(ITestOutputHelper output) : base(output)
        {
            SelectBocadillosForResenya_PO = new SelectBocadillosForResenya_PO(_driver, _output);



        }

        private void InitialStepsForCrearResenya()
        {
            _driver.Navigate().GoToUrl("https://localhost:7081/");
            SelectBocadillosForResenya_PO.WaitForBeingVisible(By.Id("CreateResenya"));
            //we click on the menu
            _driver.FindElement(By.Id("CreateResenya")).Click();
        }



        [Theory]
        [InlineData(bocadilloNombre1, bocadilloTamanyo1, bocadilloTipoPan1, bocadilloPrecio1, "Serrano", "")]
        [InlineData(bocadilloNombre2, bocadilloTamanyo2, bocadilloTipoPan2, bocadilloPrecio2, "", "2")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_FiltrarBocadillos(string nombre, string tamanyo, string tipoPan, string pvp,
            string filterNombre, string filterPrecio)
        {
            InitialStepsForCrearResenya();

            var expectedBocadillos = new List<string[]> {
                new string[] { nombre, tamanyo, tipoPan, pvp }
            };

            
            SelectBocadillosForResenya_PO.SearchBocadillos(filterNombre, filterPrecio);

            Assert.True(SelectBocadillosForResenya_PO.CheckListOfBocadillos(expectedBocadillos));
        }



        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]

        public void UC2_ResenyaNotavailable()
        {
            //Arrange
            InitialStepsForCrearResenya();
            //Act
            SelectBocadillosForResenya_PO.AddMovieToRentingCart(bocadilloNombre1);
            SelectBocadillosForResenya_PO.RemoveMovieFromRentingCart(bocadilloNombre1);

            //Assert

            Assert.True(SelectBocadillosForResenya_PO.ResenyaNotAvailable());

        }

    }


}
