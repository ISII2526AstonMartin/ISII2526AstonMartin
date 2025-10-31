
namespace AppForSEII2526.API.DTOs.ComprarMerchDTOs
{
    public class ItemMerchDTO
    {
        public string Nombre { get; set; }
        public float PVP { get; set; }
        public TipoProducto TipoProducto { get; set; }
        public int Cantidad { get; set; }
        public ItemMerchDTO() { }

        public ItemMerchDTO(string nombre, float pVP, TipoProducto tipoProducto, int cantidad)
        {
            Nombre = nombre;
            PVP = pVP;
            TipoProducto = tipoProducto;
            Cantidad = cantidad;
        }

        public override bool Equals(object? obj)
        {
            return obj is ItemMerchDTO dTO &&
                   Nombre == dTO.Nombre &&
                   PVP == dTO.PVP &&
                   EqualityComparer<TipoProducto>.Default.Equals(TipoProducto, dTO.TipoProducto) &&
                   Cantidad == dTO.Cantidad;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Nombre, PVP, TipoProducto, Cantidad);
        }
    }
}
