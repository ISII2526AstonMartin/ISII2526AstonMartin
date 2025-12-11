using AppForMovies.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_CompraBonos
{
    public class UC_CompraBonos_UIT : UC_UIT
    {
        private SelectBonosParaComprar_PO selectbonos_PO;
        private const int bonoid = 1;
        private const string nombre1 = "Bono1";
        private const string tipo1 = "Vegano";
        private const string bonoprice1 = "15";
        private const string nbocadillos1 = "6";

        public UC_CompraBonos_UIT(ITestOutputHelper output) : base(output)
        {
            
            selectbonos_PO = new SelectBonosParaComprar_PO(_driver, _output);
        }
        private void InitialStepsForComprarBonos()
        {
            selectbonos_PO.WaitForBeingVisible(By.Id("CreateCompraBonos"));
            _driver.FindElement(By.Id("CreateCompraBonos")).Click();
        }
        [Theory]
        [InlineData(nombre1, tipo1, bonoprice1, nbocadillos1, "Bono1", "")]
        [InlineData(nombre1, tipo1, bonoprice1, nbocadillos1, "", "Vegano")]
        [InlineData(nombre1, tipo1, bonoprice1, nbocadillos1, "Bono1", "Vegano")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CuBonos_AF1_2_3_4_5_FILTROS(string nombrebono, string tipoboca, string precio, string num, string filtronombre, string filtrotipo)
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

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CuBonos_AF1_2_3_4_5_NOFILTROS()
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
        }
        [Theory]
        [InlineData("BonoNoExiste", "")]
        [InlineData("", "TipoNoExiste")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CuBonos_AF0(string filtronombre, string filtrotipo)
        {
            InitialStepsForComprarBonos();
            var expectedBonos = new List< string[] >{ };
            Assert.False(selectbonos_PO.CheckListOfBonos(expectedBonos));
        }
    }
}
