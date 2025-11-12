
namespace AppForSEII2526.API.DTOs.BocadillosParaPedirDTOs
{
    public class PedidoDetailDTO: CreatePedidoDTO
    {
        public PedidoDetailDTO(string nombre, MetodoPago metodoPago,string apellido1,string? apellido2, DateTime fechaPedido, float precioTotal, List<ItemPedidoDTO> itemPedido):base(nombre,metodoPago,apellido1,apellido2,itemPedido)
        {

            FechaPedido = fechaPedido;
            PrecioTotal = precioTotal;
        }

        public DateTime FechaPedido { get; set; }

        public float PrecioTotal { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PedidoDetailDTO dTO &&
                   base.Equals(obj) &&
                   Nombre == dTO.Nombre &&
                   MetodoPago == dTO.MetodoPago &&
                   Apellido1 == dTO.Apellido1 &&
                   Apellido2 == dTO.Apellido2 &&
                    ItemPedido.SequenceEqual(dTO.ItemPedido) &&                  
                    FechaPedido == dTO.FechaPedido &&
                   PrecioTotal == dTO.PrecioTotal;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Nombre, MetodoPago, Apellido1, Apellido2, ItemPedido, FechaPedido, PrecioTotal);
        }
    }
}
