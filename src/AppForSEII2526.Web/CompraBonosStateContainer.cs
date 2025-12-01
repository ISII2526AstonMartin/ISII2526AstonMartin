using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class CompraBonosStateContainer
    {
        public CompraBonoForCreateDTO CreateDTO { get; set; } = new CompraBonoForCreateDTO()
        {
            CompraItems= new List<CompraBonoItemDTO>()
        };
        public decimal TotalPrice
        {
            get
            {
                return Convert.ToDecimal(CreateDTO.CompraItems.Sum(ci=>ci.Pvp));
            }
        }
        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
        public void AddBonoToCompra(BonoBocadillosDTO bono)
        {
            if (!CreateDTO.CompraItems.Any(ri => ri.Id == bono.Id))
            {
                CreateDTO.CompraItems.Add(new CompraBonoItemDTO()
                {
                    Id = bono.Id,
                    Pvp= bono.Pvp,
                    NBocadillos= bono.NBocadillos,
                    NombreBono= bono.NombreBono,
                    Cantidad=1
                });
            }   
        }
        public void RemoveCompraBonoItem(CompraBonoItemDTO item)
        {
            CreateDTO.CompraItems.Remove(item);
        }
        public void ClearRentingCart()
        {
            CreateDTO.CompraItems.Clear();
        }
        //we have already finished the process of renting, thus, we create a new Rental 
        public void CompraProcessed()
        {
            //we have finished the rental process so we create a new object without data
            CreateDTO = new CompraBonoForCreateDTO();
        }
    }
}
