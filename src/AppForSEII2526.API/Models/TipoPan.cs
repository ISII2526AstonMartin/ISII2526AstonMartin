
namespace AppForSEII2526.API.Models
{
    public class TipoPan
    {

        public int Id { get; set; }
        public List<Bocadillo> Bocadillos { get; set; }
        public string Nombre { get; set; }

        public TipoPan(string nombre)
        {
            Nombre = nombre;
        }

        public override bool Equals(object? obj)
        {
            return obj is TipoPan pan &&
                   Id == pan.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
