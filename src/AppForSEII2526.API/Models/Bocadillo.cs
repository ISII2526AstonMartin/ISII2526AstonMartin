using NuGet.Versioning;

namespace AppForSEII2526.API.Models
{
    public class Bocadillo
    {
        public int bocadilloId { get; set; }
        public string nombre { get; set; }
        public float pvp { get; set; }
        public TipoPan tipoPan { get; set; }
        public ResenyaBocadillo reseña { get; set; }

        public List<CompraBocadillo> compraBocadillo {get; set;}
        public int stock { get; set; }

        public Tamanyo tamanyo { get; set; }

        


    }
}
