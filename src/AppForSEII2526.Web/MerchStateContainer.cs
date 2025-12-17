using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class MerchStateContainer
    {
        public CreateMerchDTO CreateDTO { get; set; } = new CreateMerchDTO()
        {
            Items = new List<ItemMerchDTO>()
        };

        public float TotalPrice
        {
            get
            {
                return CreateDTO.Items.Sum(item => item.Pvp * item.Cantidad);
            }
        }

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddMerchToCart(MerchDTO merch)
        {
            if (!CreateDTO.Items.Any(item => item.Nombre == merch.Nombre))
            {
                CreateDTO.Items.Add(new ItemMerchDTO()
                {
                    Nombre = merch.Nombre,
                    Pvp = merch.Precio,
                    TipoProducto = merch.Tipo.Nombre,
                    Cantidad = 1
                });
                NotifyStateChanged();
            }
        }

        public void UpdateItemQuantity(ItemMerchDTO item, int nuevaCantidad)
        {
            if (nuevaCantidad > 0)
            {
                item.Cantidad = nuevaCantidad;
            }
            else
            {
                RemoveMerchItemFromCart(item);
                return;
            }
            NotifyStateChanged();
        }

        public void RemoveMerchItemFromCart(ItemMerchDTO item)
        {
            CreateDTO.Items.Remove(item);
            NotifyStateChanged();
        }

        public void ClearMerchCart()
        {
            CreateDTO.Items.Clear();
            NotifyStateChanged();
        }

        public void MerchProcessed()
        {
            // Creamos un nuevo objeto sin datos
            CreateDTO = new CreateMerchDTO()
            {
                Items = new List<ItemMerchDTO>()
            };
            NotifyStateChanged();
        }

        // Método para establecer los datos del usuario antes de enviar
        public void SetUserInfo(string nombreUsuario, string apellido1, string? apellido2,
                               string direccionEnvio, MetodoPago metodoPago)
        {
            CreateDTO.NombreUsuario = nombreUsuario;
            CreateDTO.Apellido1 = apellido1;
            CreateDTO.Apellido2 = apellido2;
            CreateDTO.DireccionEnvio = direccionEnvio;
            CreateDTO.MetodoPago = metodoPago;
        }

        // Verificar si el carrito tiene items
        public bool HasItems()
        {
            return CreateDTO.Items.Any();
        }

        // Obtener cantidad de items en el carrito
        public int ItemCount()
        {
            return CreateDTO.Items.Sum(item => item.Cantidad);
        }

        // Obtener cantidad de productos distintos
        public int DistinctItemCount()
        {
            return CreateDTO.Items.Count;
        }
    }
}