using AppForSEII2526.API.DTOs.BocadillosResenyaDTOs;
using RabbitMQ.Client;

namespace AppForSEII2526.API.DTOs.BocadillosParaPedirDTOs
{
    public class CreatePedidoDTO
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor introduzca su nombre")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre tiene que tener al menos 3 caracteres")]
        public string Nombre { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor introduzca el metodo de pago que desee")]

        public MetodoPago MetodoPago { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor introduzca su primer apellido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El apellido tiene que tener al menos 3 caracteres")]
        public string Apellido1 { get; set; }

        public string? Apellido2 { get; set; }

        public IList<ItemPedidoDTO> ItemPedido { get; set; }


        public CreatePedidoDTO(string nombre, MetodoPago metododepago, string apellido1, string apellido2, IList<ItemPedidoDTO> itempedido)
        {
            Nombre = nombre;
            MetodoPago = metododepago;
            Apellido1 = apellido1;
            Apellido2 = apellido2;
            ItemPedido = itempedido;
        }

        public CreatePedidoDTO()
        {
            ItemPedido = new List<ItemPedidoDTO>();
        }

        public override bool Equals(object? obj)
        {
            return obj is CreatePedidoDTO dTO &&
                   Nombre == dTO.Nombre &&
                   MetodoPago == dTO.MetodoPago &&
                   Apellido1 == dTO.Apellido1 &&
                   Apellido2 == dTO.Apellido2 &&
                   ItemPedido.SequenceEqual(dTO.ItemPedido);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Nombre, MetodoPago, Apellido1, Apellido2, ItemPedido);
        }

        
    }
}
