
using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.Shared;
using AppForSEII2526.UIT.UC_Resenyas;
using OpenQA.Selenium.BiDi.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UIT.UC_CrearResenya
{
    public class UC_CrearResenya_UIT : UC_UIT
    {
        private SelectBocadillosForResenya_PO SelectBocadillosForResenya_PO;
        private CreateResenya_PO CreateResenya_PO;
        private ResenyaDetail_PO ResenyaDetail_PO;

        private const string bocadilloNombre1 = "Serrano";
        private const string bocadilloTamanyo1 = "Pequeño";
        private const string bocadilloTipoPan1 = "Integral";
        private const string bocadilloPrecio1 = "5";

        private const string bocadilloNombre2 = "Bacon";
        private const string bocadilloTamanyo2 = "Pequeño";
        private const string bocadilloTipoPan2 = "Semilla";
        private const string bocadilloPrecio2 = "2";


        public UC_CrearResenya_UIT(ITestOutputHelper output) : base(output)
        {
            SelectBocadillosForResenya_PO = new SelectBocadillosForResenya_PO(_driver, _output);
            CreateResenya_PO = new CreateResenya_PO(_driver, _output);
            ResenyaDetail_PO = new ResenyaDetail_PO(_driver, _output);



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
        public void UC2_2_3_Filtrar_Bocadillos(string nombre, string tamanyo, string tipoPan, string pvp,
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

        public void UC2_4_Carrito_Vacio()
        {
            //Arrange
            InitialStepsForCrearResenya();
            //Act
            SelectBocadillosForResenya_PO.AddMovieToRentingCart(bocadilloNombre1);
            SelectBocadillosForResenya_PO.RemoveMovieFromRentingCart(bocadilloNombre1);

            //Assert

            Assert.True(SelectBocadillosForResenya_PO.ResenyaNotAvailable());

        }



        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_1_CrearResenya_Correcta()
        {
            InitialStepsForCrearResenya();
            string usuario = "Juan";
            string titulo = "Sugerencia para cenar"; 
            string descripcion = "Estaba todo muy rico";
            string valGeneral = "5";
            string puntuacionBocata = "9";

            SelectBocadillosForResenya_PO.AddMovieToRentingCart(bocadilloNombre1);

            CreateResenya_PO.ClickIrACrearResenya();

            CreateResenya_PO.RellenarFormulario(usuario, titulo, descripcion, valGeneral);

            CreateResenya_PO.PuntuarBocadillo(bocadilloNombre1, "9");

            CreateResenya_PO.PulsarCrearResenyas();
            CreateResenya_PO.ConfirmarDialogo();

            Assert.True(ResenyaDetail_PO.VerifyResenyaData(titulo, descripcion, "Cinco"),
                "Los datos en la vista de Detalle no coinciden con lo creado.");

            Assert.True(ResenyaDetail_PO.VerifyBocadilloPuntuado(bocadilloNombre1, puntuacionBocata),
                $"El bocadillo {bocadilloNombre1} no aparece correctamente puntuado en el detalle.");
        }


        [Theory]
        [Trait("LevelTesting", "Funcional Testing")]
        [InlineData("Juan", "", "Todo rico", "5", "9", "The Título field is required.")]
        [InlineData("Juan", "Sugerencia para cenar", "", "5", "9", "The Descripción field is required.")]
        [InlineData("Juan", "Mi Opinión", "Todo rico", "5", "9", "Error while processing your request")]
        public void UC2_6_7_9_Resenya_Errores(
            string usuario,
            string titulo,
            string descripcion,
            string valoracion,
            string puntuacionBocata,
            string mensajeErrorEsperado)
        {
            
            InitialStepsForCrearResenya();

            
            SelectBocadillosForResenya_PO.AddMovieToRentingCart(bocadilloNombre1);
            CreateResenya_PO.ClickIrACrearResenya();

            CreateResenya_PO.RellenarFormulario(usuario, titulo, descripcion, valoracion);
            CreateResenya_PO.PuntuarBocadillo(bocadilloNombre1, puntuacionBocata);

            CreateResenya_PO.PulsarCrearResenyas();

            CreateResenya_PO.ConfirmarDialogo();

            
            bool errorEncontrado = CreateResenya_PO.CheckMessageError(mensajeErrorEsperado);

            Assert.True(errorEncontrado,
                $"Fallo del test: Se esperaba encontrar el mensaje de error '{mensajeErrorEsperado}', pero no apareció en la pantalla.");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_5_ModificarCarrito()
        {
           
            InitialStepsForCrearResenya();

            
            SelectBocadillosForResenya_PO.AddMovieToRentingCart(bocadilloNombre1); 
            SelectBocadillosForResenya_PO.AddMovieToRentingCart(bocadilloNombre2); 

            
            SelectBocadillosForResenya_PO.RemoveMovieFromRentingCart(bocadilloNombre1);

            
            CreateResenya_PO.ClickIrACrearResenya();

            Assert.False(CreateResenya_PO.EsBocadilloVisible(bocadilloNombre1), "Serrano sigue visible en el formulario");
            Assert.True(CreateResenya_PO.EsBocadilloVisible(bocadilloNombre2), "Bacon no está visible en el formulario");

            CreateResenya_PO.RellenarFormulario("Juan", "Sugerencia para cenar", "Solo quiero Bacon", "5");
            CreateResenya_PO.PuntuarBocadillo(bocadilloNombre2, "8");

            CreateResenya_PO.PulsarCrearResenyas();
            CreateResenya_PO.ConfirmarDialogo();


            Assert.True(ResenyaDetail_PO.VerifyBocadilloPuntuado(bocadilloNombre2, "8"),
                "El bocadillo Bacon debería aparecer en el detalle.");

            Assert.True(ResenyaDetail_PO.VerifyBocadilloNoExiste(bocadilloNombre1),
                "El bocadillo Serrano NO debería aparecer en el detalle.");
        }


        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_10_NavegarAtras()
        {
            InitialStepsForCrearResenya();

           
            SelectBocadillosForResenya_PO.AddMovieToRentingCart(bocadilloNombre1); 

            CreateResenya_PO.ClickIrACrearResenya();
            CreateResenya_PO.PulsarModificarResenyas();

            Assert.True(SelectBocadillosForResenya_PO.EstaEnLaPaginaBusqueda(),
                "Error: El sistema no redirigió de vuelta a la selección de bocadillos.");

            Assert.True(SelectBocadillosForResenya_PO.EstaEnElCarrito(bocadilloNombre1),
                "Error: Al volver atrás se perdió el bocadillo seleccionado del carrito.");
        }


        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_8_Error_Puntuacion_MenorQueUno()
        {
            InitialStepsForCrearResenya();

            SelectBocadillosForResenya_PO.AddMovieToRentingCart(bocadilloNombre2); 
            CreateResenya_PO.ClickIrACrearResenya();

            CreateResenya_PO.RellenarFormulario("Juan", "Sugerencia para test", "Todo correcto", "5");

            CreateResenya_PO.PuntuarBocadillo(bocadilloNombre2, "0");

            CreateResenya_PO.PulsarCrearResenyas();

            
            string mensajeNativo = CreateResenya_PO.ObtenerMensajeValidacionHtml5(bocadilloNombre2);

         
            Assert.True(mensajeNativo.Contains("1"),
                $"Se esperaba un error de validación HTML5 indicando el mínimo de 1. Mensaje recibido: '{mensajeNativo}'");
        }





        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_Examen_CrearResenya_Correcta()
        {
            InitialStepsForCrearResenya();
            string usuario = "";
            string titulo = "Sugerencia para cenar";
            string descripcion = "Estaba todo muy rico";
            string valGeneral = "5";
            string puntuacionBocata = "9";

            SelectBocadillosForResenya_PO.AddMovieToRentingCart(bocadilloNombre1);


            SelectBocadillosForResenya_PO.SearchBocadillos("", "3");

            SelectBocadillosForResenya_PO.AddMovieToRentingCart(bocadilloNombre2);

            SelectBocadillosForResenya_PO.RemoveMovieFromRentingCart(bocadilloNombre1);


            CreateResenya_PO.ClickIrACrearResenya();

            CreateResenya_PO.RellenarFormulario(usuario, titulo, descripcion, valGeneral);

            CreateResenya_PO.PuntuarBocadillo(bocadilloNombre2, "9");

            CreateResenya_PO.PulsarCrearResenyas();
            CreateResenya_PO.ConfirmarDialogo();

            Assert.True(ResenyaDetail_PO.VerifyResenyaData(titulo, descripcion, "Cinco"),
                "Los datos en la vista de Detalle no coinciden con lo creado.");

            Assert.True(ResenyaDetail_PO.VerifyBocadilloPuntuado(bocadilloNombre2, puntuacionBocata),
                $"El bocadillo {bocadilloNombre2} no aparece correctamente puntuado en el detalle.");
        }

    }


}
