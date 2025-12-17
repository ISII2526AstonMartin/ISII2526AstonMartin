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
        private DetailMerch_PO _detailMerchPO;

        // Datos GET
        private const string merchNombre1 = "Camiseta";
        private const string merchPrecio1 = "8,00 €";
        private const string merchTipo1 = "Camiseta";

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
            _detailMerchPO = new DetailMerch_PO(_driver, _output);
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

        // --- TESTS EXISTENTES (Sin cambios) ---
        [Theory]
        [InlineData("Camiseta", "", "Camiseta", "8,00€", "Camiseta", "48")]
        [InlineData("Gorra", "6", "Gorra", "4,00€", "Gorra", "10")]
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
            _selectMerchPO.AddMerchToCart("Camiseta");
            Thread.Sleep(500);
            _selectMerchPO.RemoveMerchFromCart("Camiseta");
            Assert.True(_selectMerchPO.IsBuyButtonDisabled());
        }
        //TEST DEL EXAMEN
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_MODIFICACION_EXAMEN_Test()
        {
            InitialStepsForMerch();
            _selectMerchPO.AddMerchToCart("Camiseta");
            _selectMerchPO.SearchMerch("", "5");
            _selectMerchPO.AddMerchToCart("Gorra");
            _selectMerchPO.RemoveMerchFromCart("Camiseta");
            _selectMerchPO.ClickComprarMerch();
            Assert.True(_selectMerchPO.IsBuyButtonDisabled());
            _createMerchPO.rellenarDatosParaCompra(userValido, ap1Valido, "", dirValida, pagoValido);
            _createMerchPO.seleccionarBotonCompra();
            var productoEsperado = new List<string[]>
            {
                new string[] { "Gorra", "Gorra", "4,00 €", "1" }
            };
            Assert.True(_detailMerchPO.CheckListOfProductos(productoEsperado));

        }
        // --- TESTS NUEVOS DEL DETAILS ---

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_RealizarCompra_Exitosa_Test()
        {
            // 1. Comprar
            Precondicion_LlegarAlFormulario();
            _createMerchPO.rellenarDatosParaCompra(userValido, ap1Valido, "", dirValida, pagoValido);
            _createMerchPO.seleccionarBotonCompra();

            // 2. Verificar Ticket

            var productoEsperado = new List<string[]>
            {
                new string[] { merchNombre1, merchTipo1, merchPrecio1, "1" }
            };
            Assert.True(_detailMerchPO.CheckListOfProductos(productoEsperado));

            // B) Cabecera: Verificar datos usuario y total
            string nombreCompleto = userValido + " " + ap1Valido;
            string fechaHoy = DateTime.Today.ToString("dd/MM/yyyy");
            string precioTotal = "8,00 €";

            Assert.True(_detailMerchPO.CheckTicketDetails(
                nombreCompleto,
                dirValida,
                fechaHoy,
                pagoValido,
                precioTotal
            ));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_RealizarCompra_Fallida_DireccionMal_Test()
        {
            Precondicion_LlegarAlFormulario();
            _createMerchPO.rellenarDatosParaCompra(userValido, ap1Valido, "", dirInvalida, pagoValido);
            _createMerchPO.seleccionarBotonCompra();

            Assert.True(_createMerchPO.checkErrorMessage("válida"));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_VolverAlListado_DesdeTicket_Test()
        {
            Precondicion_LlegarAlFormulario();
            _createMerchPO.rellenarDatosParaCompra(userValido, ap1Valido, "", dirValida, pagoValido);
            _createMerchPO.seleccionarBotonCompra();

            // Pulsar "Volver a la Tienda"
            _detailMerchPO.ClickVolver();

            // Verificar que estamos en la búsqueda (necesita IsSearchInputVisible en el PO)
            Assert.True(_selectMerchPO.IsSearchInputVisible());
        }
    }
}