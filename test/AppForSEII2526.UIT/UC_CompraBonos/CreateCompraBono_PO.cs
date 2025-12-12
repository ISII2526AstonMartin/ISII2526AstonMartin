using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_CompraBonos
{
    public class CreateCompraBono_PO : PageObject
    {
        public CreateCompraBono_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public bool CheckListOfBonos(List<string[]> expectedBonos)
        {
            //return CheckBodyTable(expectedBonos, tablaofBonos);
            return false; //PLACEHOLDER, ESTO ESTA SIN TERMINAR
        }
    }
}
