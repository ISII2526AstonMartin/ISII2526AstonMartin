namespace AppForSEII2526.API.DTOs.ComprarMerchDTOs
{
    public class ItemMerchDTO
    {
        public string Nombre { get; set; }
        public float PVP { get; set; }
        public string TipoProducto { get; set; }  // ✅ CAMBIO: ahora es solo un string (nombre del tipo)
        public int Cantidad { get; set; }

        public ItemMerchDTO() { }

        public ItemMerchDTO(string nombre, float pvp, string tipoProducto, int cantidad)
        {
            Nombre = nombre;
            PVP = pvp;
            TipoProducto = tipoProducto;
            Cantidad = cantidad;
        }

        public override bool Equals(object? obj)
        {
            return obj is ItemMerchDTO dto &&
                   Nombre == dto.Nombre &&
                   PVP == dto.PVP &&
                   TipoProducto == dto.TipoProducto &&
                   Cantidad == dto.Cantidad;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Nombre, PVP, TipoProducto, Cantidad);
        }
    }
}
