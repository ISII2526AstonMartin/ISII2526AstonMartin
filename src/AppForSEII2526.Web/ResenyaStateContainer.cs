using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class ResenyaStateContainer
    {
        // Creamos una instancia de Resenya cuando se crea una instancia de ResenyaStateContainer
        public CreateResenyaDTO Resenya { get; private set; } = new CreateResenyaDTO()
        {
            Items = new List<ItemResenyaDTO>()
        };

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddBocadilloToResenya(ItemResenyaDTO bocadillo)
        {
            if (!Resenya.Items.Any(i => i.BocadilloId == bocadillo.BocadilloId))
            {
                Resenya.Items.Add(new ItemResenyaDTO()
                {
                    BocadilloId = bocadillo.BocadilloId,
                    BocadilloNombre = bocadillo.BocadilloNombre,
                    Puntuacion = bocadillo.Puntuacion,
                    Tamanyo = bocadillo.Tamanyo,
                    Pvp = bocadillo.Pvp
                });
            }

        }

        public void RemoveResenyaItemToAdd(ItemResenyaDTO item)
        {
            Resenya.Items.Remove(item);
            
        }

        public void ClearResenyaCart()
        {
            Resenya.Items.Clear();
            
        }

       
        

        public void ResenyaProcessed()
        {
           
            Resenya = new CreateResenyaDTO()
            {
                Items = new List<ItemResenyaDTO>()
            };

        }

       
    }
}
