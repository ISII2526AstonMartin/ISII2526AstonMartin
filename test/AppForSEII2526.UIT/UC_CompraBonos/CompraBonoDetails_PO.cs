using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_CompraBonos
{
    public class CompraBonoDetails_PO : PageObject
    {
        By tablaofBonos = By.Id("BonosCompradosTable");
        By namesurname = By.Id("NameSurname");
        By pago = By.Id("PaymentMethod");
        By fechahtml = By.Id("RentalDate");
        By total = By.Id("TotalPrice");

        public CompraBonoDetails_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public bool CheckListOfBonos(List<string[]> expectedBonos)
        {
            
            WaitForBeingVisible(tablaofBonos);
            return CheckBodyTable(expectedBonos, tablaofBonos);
            
        }
        public bool CheckListOfDatos(string nombreyapellidos, string fecha, string metodopago, string precio)
        {
            WaitForBeingVisible(namesurname);
            IWebElement containername = _driver.FindElement(namesurname);
            string actualname = containername.Text;
            
            WaitForBeingVisible(pago);
            IWebElement containerpago = _driver.FindElement(pago);
            string actualpago = containerpago.Text;

            WaitForBeingVisible(fechahtml);
            IWebElement containerfecha = _driver.FindElement(fechahtml);
            string actualfecha = containerfecha.Text;

            WaitForBeingVisible(total);
            IWebElement containertotal = _driver.FindElement(total);
            string actualtotal = containertotal.Text;
            
            return actualname.Contains(nombreyapellidos)&&actualpago.Contains(metodopago)&&actualfecha.Contains(fecha)&&actualtotal.Contains(precio);
        }
    }
}
