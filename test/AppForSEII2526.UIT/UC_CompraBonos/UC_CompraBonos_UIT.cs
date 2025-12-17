using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.UC_Rental;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.UIT.UC_CompraBonos
{
    public class UC_CompraBonos_UIT : UC_UIT
    {
        private SelectBonosParaComprar_PO selectbonos_PO;
        private CreateCompraBono_PO comprabonos_PO;
        private CompraBonoDetails_PO comprabonodetail_PO;
        private const int bonoid = 1;
        private const string nombre1 = "Bono1";
        private const string tipo1 = "Vegano";
        private const string bonoprice1 = "15";
        private const string nbocadillos1 = "6";

        public UC_CompraBonos_UIT(ITestOutputHelper output) : base(output)
        {
            
            selectbonos_PO = new SelectBonosParaComprar_PO(_driver, _output);
            comprabonos_PO = new CreateCompraBono_PO(_driver, _output);
            comprabonodetail_PO= new CompraBonoDetails_PO(_driver, _output);
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
        public void CU3_FB_1_2_3_4_5_6_7() //Esc_1, UC3_1
        {
            InitialStepsForComprarBonos();
            DateTime fechahoy = DateTime.Today;
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

            var expectedBonosOnCompra = new List<string[]>
            {
                new string[]
                {
                    "Bono1", "Vegano", "15"
                },
                new string[]
                {
                    "Bono2", "Vegetariano", "11"
                },
                new string[]
                {
                    "Bono3", "Sin Gluten", "10"
                },
            };

            var expectedtablebonos = new List<string[]>
            {
                new string[]
                {
                    "Bono1", "Vegano", "15 €", "1"
                },
                new string[]
                {
                    "Bono2", "Vegetariano", "11 €", "1"
                },
                new string[]
                {
                    "Bono3", "Sin Gluten", "10 €", "1"
                }
            };


            //Pagina Select
            selectbonos_PO.BuscarBonos("", "");
            Assert.True(selectbonos_PO.CheckListOfBonos(expectedBonos));
            selectbonos_PO.seleccionarBonos("Bono1");
            selectbonos_PO.seleccionarBonos("Bono2");
            selectbonos_PO.seleccionarBonos("Bono3");
            Assert.True(selectbonos_PO.buttonCompraAvailable());
            selectbonos_PO.seleccionarBotonCompra();

            //Pagina de compra
            Assert.True(comprabonos_PO.CheckListOfBonos(expectedBonosOnCompra));
            comprabonos_PO.rellenarDatosCompra("Daniel", "Martinez", "Bautista", "Tarjeta");
            Assert.True(comprabonos_PO.checkPrecio("36"));
            comprabonos_PO.seleccionarBotonCompra();

            //Pagina de details
            Assert.True(comprabonodetail_PO.CheckListOfDatos("Daniel Martinez Bautista", fechahoy.ToString("dd/MM/yyyy hh:mm:ss"), "Tarjeta", "36"));
            Assert.True(comprabonodetail_PO.CheckListOfBonos(expectedtablebonos));
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
            var expectedBonosOnCompra = new List<string[]>
            {
                new string[]
                {
                    "Bono1", "Vegano", "15"
                }
            };
            InitialStepsForComprarBonos();
            selectbonos_PO.BuscarBonos("", "");
            selectbonos_PO.seleccionarBonos("Bono1");
            selectbonos_PO.seleccionarBonos("Bono2");
            selectbonos_PO.seleccionarBonos("Bono3");
            selectbonos_PO.seleccionarBotonCompra();
            comprabonos_PO.seleccionarBotonVolver();
            selectbonos_PO.eliminarBono("Bono2");
            selectbonos_PO.eliminarBono("Bono3");
            Assert.True(selectbonos_PO.buttonCompraAvailable());
            selectbonos_PO.seleccionarBotonCompra();
            Assert.True(comprabonos_PO.CheckListOfBonos(expectedBonosOnCompra));
            Assert.True(comprabonos_PO.checkPrecio("15"));
        }

        [Theory]
        [InlineData("", "Martinez", "Bautista", "Tarjeta")] //UC3_10
        [InlineData("Daniel", "", "Bautista", "Tarjeta")] //UC3_11
        [InlineData("Andres", "Iniesta", "Lujan", "Tarjeta")] //UC3_13
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_FA4_1_2_3_4_5_6(string nombre, string ap1, string ap2, string mp) //Esc_6
        {
            InitialStepsForComprarBonos();
            selectbonos_PO.BuscarBonos("", "");   
            selectbonos_PO.seleccionarBonos("Bono1");
            selectbonos_PO.seleccionarBonos("Bono2");
            selectbonos_PO.seleccionarBonos("Bono3");
            selectbonos_PO.seleccionarBotonCompra();
            comprabonos_PO.rellenarDatosCompra(nombre, ap1, ap2, mp);
            comprabonos_PO.seleccionarBotonCompra();
            Assert.True(comprabonos_PO.checkErrorMessage("Errors: Error while processing your request, please try again later!"));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_FA4_1_2_3_4_5_6_CantidadCero() //Esc_6 CU3_12
        {
            InitialStepsForComprarBonos();
            selectbonos_PO.BuscarBonos("", "");
            selectbonos_PO.seleccionarBonos("Bono1");
            selectbonos_PO.seleccionarBonos("Bono2");
            selectbonos_PO.seleccionarBonos("Bono3");
            selectbonos_PO.seleccionarBotonCompra();
            comprabonos_PO.rellenarDatosCompra("Daniel", "Martinez", "Bautista", "Tarjeta");
            comprabonos_PO.modificarCantidadBono("1", "0");
            comprabonos_PO.seleccionarBotonCompra();
            Assert.True(comprabonos_PO.checkErrorMessage("Errors: Error while processing your request, please try again later!"));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_FA5_1_2_3_4_5() //Esc_7 CU3_14
        {
          

            var expectedBonosOnCompra = new List<string[]>
            {
                new string[]
                {
                    "Bono1", "Vegano", "15"
                },
                new string[]
                {
                    "Bono2", "Vegetariano", "11"
                },
                new string[]
                {
                    "Bono3", "Sin Gluten", "10"
                },
            };
            InitialStepsForComprarBonos();
            selectbonos_PO.BuscarBonos("", "");
            selectbonos_PO.seleccionarBonos("Bono1");
            selectbonos_PO.seleccionarBonos("Bono2");
            selectbonos_PO.seleccionarBonos("Bono3");
            selectbonos_PO.seleccionarBotonCompra();
            Assert.True(comprabonos_PO.CheckListOfBonos(expectedBonosOnCompra));
            comprabonos_PO.rellenarDatosCompra("Daniel", "Martinez", "Bautista", "Tarjeta");
            comprabonos_PO.seleccionarBotonVolver();
            selectbonos_PO.seleccionarBotonCompra();
            Assert.True(comprabonos_PO.checkDatosUsuario("Daniel", "Martinez", "Bautista","Tarjeta"));
            Assert.True(comprabonos_PO.CheckListOfBonos(expectedBonosOnCompra));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_PruebaExamen()
        {
            DateTime hoy = DateTime.Today;
            var expectedBonos1 = new List<string[]> //Bonos que deben haber en select sin haber filtrado (todos)
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

            var expectedBonos2 = new List<string[]> //Bonos que deben haber en select tras haber filtrado
            {
                new string[]
                {
                    "Bono2", "Vegetariano", "2","11"
                }

            };

            var expectedBonosOnCompra = new List<string[]> //Bonos que deben haber en compra
            {
                new string[]
                {
                    "Bono2", "Vegetariano", "11"
                }
            };

            var expectedtablebonosfinal = new List<string[]> //Bonos que deben haber en la factura (details)
            {
                new string[]
                {
                    "Bono2", "Vegetariano", "11 €", "1"
                }
            };

            InitialStepsForComprarBonos();
            Assert.True(selectbonos_PO.CheckListOfBonos(expectedBonos1));
            selectbonos_PO.seleccionarBonos("Bono1"); //Añade un bocadillo
            selectbonos_PO.BuscarBonos("Bono2", ""); //Filtra por nombre
            Assert.True(selectbonos_PO.CheckListOfBonos(expectedBonos2)); 
            selectbonos_PO.seleccionarBonos("Bono2"); //Añade un nuevo bocadillo (distinto al anterior)
            selectbonos_PO.eliminarBono("Bono1"); //Elimina el primer bocadillo

            //Continua con el proceso de compra de forma correcta
            selectbonos_PO.seleccionarBotonCompra();
            comprabonos_PO.CheckListOfBonos(expectedBonosOnCompra);
            comprabonos_PO.rellenarDatosCompra("Daniel", "Martinez", "Bautista", "Tarjeta");
            Assert.True(comprabonos_PO.checkDatosUsuario("Daniel", "Martinez", "Bautista", "Tarjeta"));
            comprabonos_PO.seleccionarBotonCompra();

            //como resultado se debe haber creado la compra del bono del bocadillo
            Assert.True(comprabonodetail_PO.CheckListOfDatos("Daniel Martinez Bautista", hoy.ToString("dd/MM/yyyy hh:mm:ss"), "Tarjeta", "11"));
            Assert.True(comprabonodetail_PO.CheckListOfBonos(expectedtablebonosfinal));

        }
    }
}
