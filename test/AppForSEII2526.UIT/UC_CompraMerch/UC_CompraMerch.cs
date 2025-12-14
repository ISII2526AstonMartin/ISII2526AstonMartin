using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.Shared;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_CompraMerch
{
    public class UC_CompraMerch_UIT : UC_UIT
    {
        private SelectMerchParaComprar_PO _selectMerchPO;
        private CreateCompraMerch_PO _createMerchPO;

        // Datos GET
        private const string merchNombre1 = "Camiseta";
        private const string merchPrecio1 = "8,00€";
        private const string merchTipo1 = "Camiseta";
        private const string merchStock1 = "48";
        private const string merchNombre2 = "Gorra";
        private const string merchPrecio2 = "4,00€";
        private const string merchTipo2 = "Gorra";
        private const string merchStock2 = "10";

        // Datos POST
        private const string userValido = "rafamartinez";
        private const string ap1Valido = "Martinez";
        private const string dirValida = "Calle Principal 123";
        private const string dirInvalida = "Avenida Falsa";
        private const string pagoValido = "Tarjeta";

        public UC_CompraMerch_UIT(ITestOutputHelper output) : base(output)
        {
            _selectMerchPO = new SelectMerchParaComprar_PO(_driver, _output);
            _createMerchPO = new CreateCompraMerch_PO(_driver, _output);
        }

        private void InitialStepsForMerch()
        {
            _driver.Navigate().GoToUrl(_URI + "Merch/SelectMerchParaComprar");
            Thread.Sleep(1500);
        }

        private void Precondicion_LlegarAlFormulario()
        {
            InitialStepsForMerch();
            _selectMerchPO.SearchMerch("Camiseta", "");
            _selectMerchPO.AddMerchToCart("Camiseta");
            _selectMerchPO.ClickComprarMerch();
        }

        // --- TESTS GET ---

        [Theory]
        [InlineData("Camiseta", "", merchNombre1, merchPrecio1, merchTipo1, merchStock1)]
        [InlineData("Gorra", "6", merchNombre2, merchPrecio2, merchTipo2, merchStock2)]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_FiltrarMerch_Test(string fTipo, string fPrecio, string eNom, string ePre, string eTip, string eSto)
        {
            InitialStepsForMerch();
            var expected = new List<string[]> { new string[] { eNom, ePre, eTip, eSto } };
            _selectMerchPO.SearchMerch(fTipo, fPrecio);
            Assert.True(_selectMerchPO.CheckListOfMerch(expected));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_BotonComprarDeshabilitado_SiCarritoVacio_Test()
        {
            InitialStepsForMerch();
            _selectMerchPO.SearchMerch("", "");
            _selectMerchPO.AddMerchToCart(merchNombre1);
            Thread.Sleep(500);
            _selectMerchPO.RemoveMerchFromCart(merchNombre1);
            Assert.True(_selectMerchPO.IsBuyButtonDisabled());
        }

        // --- TESTS POST ---

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_RealizarCompra_Exitosa_Test()
        {
            Precondicion_LlegarAlFormulario();
            _createMerchPO.rellenarDatosParaCompra(userValido, ap1Valido, "", dirValida, pagoValido);
            _createMerchPO.seleccionarBotonCompra();

            Assert.True(_createMerchPO.IsTicketDisplayed());
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_RealizarCompra_Fallida_DireccionMal_Test()
        {
            Precondicion_LlegarAlFormulario();

            // 1. Rellenamos con dirección mala (sin "Calle")
            _createMerchPO.rellenarDatosParaCompra(userValido, ap1Valido, "", dirInvalida, pagoValido);

            // 2. Intentamos comprar
            _createMerchPO.seleccionarBotonCompra();

            // 3. Verificamos el mensaje de error.
            // CAMBIO: Buscamos "válida" en lugar de "Calle", porque tu Controller devuelve:
            // "Error!, por favor introduce una dirección de envío válida"
            Assert.True(_createMerchPO.checkErrorMessage("válida"));
        }
    }
}