using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.UC_Rental;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_CompraBonos
{
    public class UC_CompraBonos_UIT : UC_UIT
    {
        private SelectBonosParaComprar_PO selectbonos_PO;
        private CreateCompraBono_PO comprabonos_PO;
        private const int bonoid = 1;
        private const string nombre1 = "Bono1";
        private const string tipo1 = "Vegano";
        private const string bonoprice1 = "15";
        private const string nbocadillos1 = "6";

        public UC_CompraBonos_UIT(ITestOutputHelper output) : base(output)
        {
            
            selectbonos_PO = new SelectBonosParaComprar_PO(_driver, _output);
            comprabonos_PO = new CreateCompraBono_PO(_driver, _output);
        }
        private void InitialStepsForComprarBonos()
        {
            selectbonos_PO.WaitForBeingVisible(By.Id("CreateCompraBonos"));

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(driver =>
            {
                try
                {
                    var element = driver.FindElement(By.Id("CreateCompraBonos"));
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
                    var createCompraButton = _driver.FindElement(By.Id("CreateCompraBonos"));
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
        public void CU3_FB() //Esc_1, UC3_1
        {
            InitialStepsForComprarBonos();
            var expectedBonos = new List<string[]>
            {
                new string[]
                {
                    "Bono1", "Vegano", "6", "15"
                },
                new string[]
                {
                    "Bono2", "Vegetariano", "2","11"
                },
                new string[]
                {
                    "Bono3", "Sin Gluten", "6","10"
                },

            };

            selectbonos_PO.BuscarBonos("", "");

            Assert.True(selectbonos_PO.CheckListOfBonos(expectedBonos));
            selectbonos_PO.seleccionarBonos("Bono1");
            selectbonos_PO.seleccionarBonos("Bono2");
            selectbonos_PO.seleccionarBonos("Bono3");
            Assert.True(selectbonos_PO.buttonCompraAvailable());
            selectbonos_PO.seleccionarBotonCompra();
        }

        [Theory]
        [InlineData("BonoNoExiste", "")]
        [InlineData("", "TipoNoExiste")]
        [InlineData("BonoNoExiste", "TipoNoExiste")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_FA0_1_2(string filtronombre, string filtrotipo) //Esc_3
        {
            InitialStepsForComprarBonos();
            var expectedBonos = new List<string[]> { };
        }

        [Theory]
        [InlineData(nombre1, tipo1, bonoprice1, nbocadillos1, "Bono1", "")] //UC3_3
        [InlineData(nombre1, tipo1, bonoprice1, nbocadillos1, "", "Vegano")] //UC3_4
        [InlineData(nombre1, tipo1, bonoprice1, nbocadillos1, "Bono1", "Vegano")] //UC3_2
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_FA1_1_2_3_CORRECT(string nombrebono, string tipoboca, string precio, string num, string filtronombre, string filtrotipo) //Esc_2
        {
            InitialStepsForComprarBonos();
            var expectedBonos = new List<string[]>
            {
                new string[]
                {
                    nombrebono,tipoboca, num, precio
                }
            };

            selectbonos_PO.BuscarBonos(filtronombre, filtrotipo);

            Assert.True(selectbonos_PO.CheckListOfBonos(expectedBonos));
        }

        [Theory]
        [InlineData("BonoInexistente", "")] //UC3_5
        [InlineData("", "Normal")] //UC3_6
        [InlineData("BonoInexistente", "Normal")] //UC3_7
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_FA1_1_2_3_fail(string filtronombre, string filtrotipo) //Esc_2
        {
            InitialStepsForComprarBonos();
            var expectedBonos = new List<string[]>{};

            selectbonos_PO.BuscarBonos(filtronombre, filtrotipo);
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));
            Assert.True(selectbonos_PO.checkErrorMessage("Errors: No hay bonos con esos filtros"));
            Assert.True(selectbonos_PO.CheckListOfBonos(expectedBonos));
        }


        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_FA2_1_2_3() //Esc_4, UC3_8
        {
            InitialStepsForComprarBonos();
            
            Assert.False(selectbonos_PO.buttonCompraAvailable());
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_FA3_1_2_3_4() //Esc_5, UC3_9
        {
            InitialStepsForComprarBonos();
            selectbonos_PO.BuscarBonos("", "");
            selectbonos_PO.seleccionarBonos("Bono1");
            selectbonos_PO.seleccionarBonos("Bono2");
            selectbonos_PO.seleccionarBonos("Bono3");
            selectbonos_PO.eliminarBono("Bono2");
            selectbonos_PO.eliminarBono("Bono3");
            Assert.True(selectbonos_PO.buttonCompraAvailable());
            selectbonos_PO.seleccionarBotonCompra();
        }
        
    }
}
