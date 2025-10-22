namespace AppForSEII2526.API.DTOs
{
    public class MerchDTO
    {
        public string Nombre { get; set; }
        public float Precio { get; set; }
        public TipoProducto Tipo { get; set; }
        public int Stock { get; set; }
   
    public MerchDTO()
        {

        }
        public MerchDTO(string nombre, float precio, TipoProducto tipo, int stock)
        {
            Nombre = nombre;
            Precio = precio;
            Tipo = tipo;
            Stock = stock;
        }
        public override bool Equals(object? obj)
        {
            return obj is MerchDTO dTO &&
                   Nombre == dTO.Nombre &&
                   Precio == dTO.Precio &&
                   EqualityComparer<TipoProducto>.Default.Equals(Tipo, dTO.Tipo) &&
                   Stock == dTO.Stock;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Nombre, Precio, Tipo, Stock);
        }
    }
}
