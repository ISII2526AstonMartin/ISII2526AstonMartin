using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.Shared;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading; // Necesario para Thread.Sleep
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_CompraMerch
{
    public class UC_CompraMerch_UIT : UC_UIT
    {
        private SelectMerchParaComprar_PO _selectMerchPO;

     
        private const string merchNombre1 = "Camiseta";
        private const string merchPrecio1 = "8,00€";
        private const string merchTipo1 = "Camiseta";
        private const string merchStock1 = "48";

        private const string merchNombre2 = "Gorra";
        private const string merchPrecio2 = "4,00€";
        private const string merchTipo2 = "Gorra";
        private const string merchStock2 = "10"; 

        private const string merchNombre3 = "Boligrafo";
        private const string merchPrecio3 = "2,00€";
        private const string merchTipo3 = "Boligrafo";
        private const string merchStock3 = "48";

        public UC_CompraMerch_UIT(ITestOutputHelper output) : base(output)
        {
            _selectMerchPO = new SelectMerchParaComprar_PO(_driver, _output);
        }

        private void InitialStepsForMerch()
        {
            // 1. SOLUCIÓN DE NAVEGACIÓN:
            // Ir directamente a la URL final. No vayas a la home primero si no es necesario.
            _driver.Navigate().GoToUrl(_URI + "Merch/SelectMerchParaComprar");

            // 2. ESPERA DE ESTABILIZACIÓN (CRÍTICO PARA BLAZOR):
            // Damos 1.5 segundos para que la página cargue, pinte la tabla y se estabilice
            // antes de que el PO intente buscar elementos.
            Thread.Sleep(1500);
        }

        [Theory]
        [InlineData("Camiseta", "", merchNombre1, merchPrecio1, merchTipo1, merchStock1)]
        // Nota: He cambiado "Gorra" y "6" para asegurar que sale la Gorra.
        [InlineData("Gorra", "6", merchNombre2, merchPrecio2, merchTipo2, merchStock2)]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_FiltrarMerch_Test(string filtroTipo, string filtroPrecio,
                                         string expNombre, string expPrecio, string expTipo, string expStock)
        {
            // Arrange
            InitialStepsForMerch();

            var expectedRows = new List<string[]>
            {
                new string[] { expNombre, expPrecio, expTipo, expStock }
            };

            // Act
            _selectMerchPO.SearchMerch(filtroTipo, filtroPrecio);

            // Assert
            Assert.True(_selectMerchPO.CheckListOfMerch(expectedRows));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_BotonComprarDeshabilitado_SiCarritoVacio_Test()
        {
            // Arrange
            InitialStepsForMerch();

            // Act
            _selectMerchPO.SearchMerch("", ""); // Cargar tabla

            // Añadir y quitar para forzar actualización de estado
            _selectMerchPO.AddMerchToCart(merchNombre1);

            // Pequeña espera entre acciones para evitar StaleElement en el botón de eliminar
            Thread.Sleep(500);

            _selectMerchPO.RemoveMerchFromCart(merchNombre1);

            // Assert
            Assert.True(_selectMerchPO.IsBuyButtonDisabled());
        }
    }
}