using NuGet.Versioning;

namespace AppForSEII2526.API.Models
{
    public class Bocadillo
    {
        public int BocadilloId { get; set; }
        public string Nombre { get; set; }
        public float PVP { get; set; }
        public TipoPan TipoPan { get; set; }
        

        public IList<CompraBocadillo> CompraBocadillo {get; set;}
        public int Stock { get; set; }


    }
}
