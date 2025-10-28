namespace AppForSEII2526.API.DTOs.ComprarMerchDTOs
{
    public class ItemMerchDTO
    {
        public string Nombre { get; set; }
        public float PVP { get; set; }
        public TipoProducto TipoProducto { get; set; }
        public ItemMerchDTO() { }
        public ItemMerchDTO(string nombre, float pVP, TipoProducto tipoProducto)
        {
            Nombre = nombre;
            PVP = pVP;
            TipoProducto = tipoProducto;
        }
        public override bool Equals(object? obj)
        {
            return obj is ItemMerchDTO dTO &&
                   Nombre == dTO.Nombre &&
                   PVP == dTO.PVP &&
                   TipoProducto == dTO.TipoProducto;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Nombre, PVP, TipoProducto);
        }
        
    }
}
