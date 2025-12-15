using OpenQA.Selenium;
using System.Collections.Generic;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_CompraMerch
{
    public class DetailMerch_PO : PageObject
    {
        // 1. Selectores EXACTOS (DetailMerch.razor)
        By tablaProductos = By.Id("MerchCompradoTable");
        By nameSurname = By.Id("NameSurname");
        By direccion = By.Id("DeliveryAddress"); // ID corregido
        By pago = By.Id("PaymentMethod");
        By fechaHtml = By.Id("PurchaseDate");    // ID corregido
        By total = By.Id("TotalPrice");

        // El botón no tiene ID en mi html, uso XPath por texto
        By backButton = By.XPath("//button[contains(text(),'Volver a la Tienda')]");

        public DetailMerch_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        // Verifica que la tabla contenga los productos esperados
        public bool CheckListOfProductos(List<string[]> expectedProductos)
        {
            WaitForBeingVisible(tablaProductos);
            return CheckBodyTable(expectedProductos, tablaProductos);
        }

        // Verifica los datos de cabecera del ticket
        public bool CheckTicketDetails(string nombreYApellidos, string direccionEsperada, string fecha, string metodoPago, string precioTotal)
        {
            WaitForBeingVisible(nameSurname);
            string actualName = _driver.FindElement(nameSurname).Text;

            WaitForBeingVisible(direccion);
            string actualDireccion = _driver.FindElement(direccion).Text;

            WaitForBeingVisible(pago);
            string actualPago = _driver.FindElement(pago).Text;

            WaitForBeingVisible(fechaHtml);
            string actualFecha = _driver.FindElement(fechaHtml).Text;

            WaitForBeingVisible(total);
            string actualTotal = _driver.FindElement(total).Text;

            // Debug para ver qué encuentra si falla algo
            _output.WriteLine($"TICKET REAL -> Nombre: {actualName}, Dir: {actualDireccion}, Total: {actualTotal}");

            return actualName.Contains(nombreYApellidos) &&
                   actualDireccion.Contains(direccionEsperada) &&
                   actualPago.Contains(metodoPago) &&
                   actualFecha.Contains(fecha) &&
                   actualTotal.Contains(precioTotal);
        }

        // Hace clic en "Volver a la Tienda"
        public void ClickVolver()
        {
            WaitForBeingClickable(backButton);
            _driver.FindElement(backButton).Click();
        }
    }
}