using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.Shared;
using AppForSEII2526.UIT.UC_CompraBonos;
using AppForSEII2526.UIT.UC_PedirBocadillo;

namespace AppForSEII2526.UIT.UC_Rental
{
    public class UC_PedirBocadillo_UIT : UC_UIT
    {
        private SelectBocadillosParaPedir_PO selectBocadillosParaPedir_PO;
        private CreatePedido_PO compraBocadillos_PO;
        private DetailPedido_PO detailPedido_PO;
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
            compraBocadillos_PO = new CreatePedido_PO(_driver, _output);
            detailPedido_PO = new DetailPedido_PO(_driver, _output);


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

            // Reintenta el click si ocurre un StaleElementReferenceException
            int retries = 3;
            while (retries-- > 0)
            {
                try
                {
                    var createCompraButton = _driver.FindElement(By.Id("CreateCompra"));
                    createCompraButton.Click();
                    break;
                }
                catch (StaleElementReferenceException)
                {
                    if (retries == 0) throw;
                    Thread.Sleep(200); // Espera breve antes de reintentar
                }
            }
        }


        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void TC1_FB_1_2_3_4_5_6_7_8() //Esc_1, UC3_1
        {
            InitialStepsForCompraBocadillos();
            DateTime fechahoy = DateTime.Today;

            var expectedBocadillos = new List<string[]>
            {
                new string[]
                {
                    "Serrano", "Pequeño", "Integral", "5"
                },
                new string[]
                {
                    "Bacon", "Pequeño", "Semilla","2"
                },
                new string[]
                {
                    "BaconQueso", "Normal", "Normal","3"
                },
            }; 


            var expectedBocadillosCompra = new List<string[]>
            {
                new string[]
                {
                    "Serrano","Integral", "5"
                },
                new string[]
                {
                    "BaconQueso","Normal", "3"
                },
            };


            var expectedDatosCompra = new List<string[]>
            {
                new string[]
                {
                    "Serrano", "Integral", "5 €", "1"
                },
                new string[]
                {
                    "BaconQueso", "Normal", "3 €", "1"
                },
            };

            //Pagina del select de bocadillos
            selectBocadillosParaPedir_PO.SearchBocadillos("", "");
            Assert.True(selectBocadillosParaPedir_PO.CheckListOfBocadillos(expectedBocadillos));
            selectBocadillosParaPedir_PO.AddBocadilloParaComprar("Serrano");
            selectBocadillosParaPedir_PO.AddBocadilloParaComprar("BaconQueso");
            selectBocadillosParaPedir_PO.seleccionarBotonCompra();

            //Pagina de la compra de bocadillos
            Assert.True(compraBocadillos_PO.CheckListOfBocadillos(expectedBocadillosCompra));
            compraBocadillos_PO.rellenarDatosParaCompra("Antonio", "Garcia de la Reina", "Aguilar", "Tarjeta");
            Assert.True(compraBocadillos_PO.checkPrecio("8"));
            compraBocadillos_PO.seleccionarBotonCompra();

            //Pagina de detalle del pedido

            Assert.True(detailPedido_PO.CheckListOfDatos("Antonio Garcia de la Reina Aguilar", "Tarjeta", "8", fechahoy.ToString("dd/MM/yyyy hh:mm:ss"), "8"));
            Assert.True(detailPedido_PO.CheckListOfBocadillos(expectedDatosCompra));




        }


        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]

        public void TC1_AF1_TC1_2_CompraNotAvailable() //No hay bocadillos seleccionados
        {
            //Arrange
            InitialStepsForCompraBocadillos();
            //Act
            selectBocadillosParaPedir_PO.AddBocadilloParaComprar(nombreBocadillo1);
            selectBocadillosParaPedir_PO.RemoveBocadilloParaComprar(nombreBocadillo1);

            //Assert

            Assert.True(selectBocadillosParaPedir_PO.CompraNotAvailable());

        }



        //POST

        [Theory]
        [InlineData("", "Garcia de la Reina", "Aguilar", "Tarjeta")] //Falta Nombre
        [InlineData("Antonio", "", "Aguilar", "Tarjeta")] //Falta Primer Apellido
        [InlineData("Julio", "Tarrega", "Peinado", "Tarjeta")] //Usuario Inexistente
        [Trait("LevelTesting", "Funcional Testing")]

        public void TC1_FA3_2_3_4_5_6_7(string nombre, string apellido1, string apellido2, string metodopago) //Esc_6
        {
            InitialStepsForCompraBocadillos();
            selectBocadillosParaPedir_PO.SearchBocadillos("", "");
            selectBocadillosParaPedir_PO.AddBocadilloParaComprar("Serrano");
            selectBocadillosParaPedir_PO.AddBocadilloParaComprar("BaconQueso");
            selectBocadillosParaPedir_PO.seleccionarBotonCompra();
            compraBocadillos_PO.rellenarDatosParaCompra(nombre, apellido1, apellido2, metodopago);
            compraBocadillos_PO.seleccionarBotonCompra();
            Assert.True(compraBocadillos_PO.checkErrorMessage("Errors: Error while processing your request, please try again later!"));
        }




        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void TC1_FA3_2_3_4_5_6_7_8() //Cantidad 0
        {
            InitialStepsForCompraBocadillos();
            selectBocadillosParaPedir_PO.SearchBocadillos("", "");
            selectBocadillosParaPedir_PO.AddBocadilloParaComprar("Serrano");
            selectBocadillosParaPedir_PO.AddBocadilloParaComprar("BaconQueso");
            selectBocadillosParaPedir_PO.seleccionarBotonCompra();
            compraBocadillos_PO.rellenarDatosParaCompra("Antonio", "Garcia de la Reina", "Aguilar", "PayPal");
            compraBocadillos_PO.modificarCantidadBocadillos("1", "0");
            compraBocadillos_PO.seleccionarBotonCompra();
            Assert.True(compraBocadillos_PO.checkErrorMessage("Errors: Error while processing your request, please try again later!"));
        }





    }


    }

