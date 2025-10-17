
namespace AppForSEII2526.API.DTOs
{
    public class BocadillosParaPedirDTO
    {

        public string Nombre { get; set; }
        public Tamanyo Tamanyo { get; set; }
        public TipoPan TipoPan { get; set; }
        public float Precio { get; set; }

        public BocadillosParaPedirDTO()
        {

        }  

        public BocadillosParaPedirDTO(string nombre, Tamanyo tamanyo, TipoPan tipoPan, float precio)
        {
            Nombre = nombre;
            Tamanyo = tamanyo;
            TipoPan = tipoPan;
            Precio = precio;

        }

        public override bool Equals(object? obj)
        {
            return obj is BocadillosParaPedirDTO dTO &&
                   Nombre == dTO.Nombre &&
                   Tamanyo == dTO.Tamanyo &&
                   EqualityComparer<TipoPan>.Default.Equals(TipoPan, dTO.TipoPan) &&
                   Precio == dTO.Precio;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Nombre, Tamanyo, TipoPan, Precio);
        }
    }
}
