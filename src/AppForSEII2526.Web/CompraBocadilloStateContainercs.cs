using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class CompraBocadilloStateContainercs
    {






        //we create an instance of Rental when an instance of RentalStateContainer is created
        public CreatePedidoDTO Compra { get; private set; } = new CreatePedidoDTO()
        {
            ItemPedido = new List<ItemPedidoDTO>()
        };

        //we compute the TotalPrice of the movies we have selected for renting them
        public decimal TotalPrice
        {
            get
            {
              
                return Convert.ToDecimal(Compra.ItemPedido.Sum(ri => ri.Pvp));
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();



        public void AddBocadilloToComprar(BocadillosDTO bocadillo)
        {
            //before adding a movie we checked whether it has been already added
            if (!Compra.ItemPedido.Any(ri => ri.Id == bocadillo.Id))
                //we add it if it is not in the list
                Compra.ItemPedido.Add(new ItemPedidoDTO()
                {
                    Id = bocadillo.Id,
                    TipoPan = bocadillo.TipoPan,
                    Pvp = bocadillo.Pvp,
                    NombreBocadillo = bocadillo.Nombre,
                    Cantidad = 1
                }
            );

        }

        //to delete movies from the list of selected movies
        public void RemoveCompralItemToRent(ItemPedidoDTO item)
        {
            Compra.ItemPedido.Remove(item);

        }

        //we eliminate all the movies from the list
        public void ClearRentingCart()
        {
            Compra.ItemPedido.Clear();

        }

        //we have already finished the process of renting, thus, we create a new Rental 
        public void RentalProcessed()
        {
            //we have finished the rental process so we create a new object without data
            Compra = new CreatePedidoDTO()
            {
                ItemPedido = new List<ItemPedidoDTO>()
            };
        }
    }
}

    

