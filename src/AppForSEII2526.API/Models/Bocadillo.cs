using NuGet.Versioning;

namespace AppForSEII2526.API.Models
{
    public class Bocadillo
    {

        public Bocadillo(string nombre, float pVP, int stock, Tamanyo tamanyo, TipoPan tipoPan,int id)
        {
            Id = id;
            Nombre = nombre;
            PVP = pVP;
            Stock = stock;
            Tamanyo = tamanyo;
            ResenyaBocadillo = new List<ResenyaBocadillo>();
            CompraBocadillo = new List<CompraBocadillo>();
            TipoPan = tipoPan;
        }

        public Bocadillo(string nombre, float pVP, int stock, Tamanyo tamanyo,TipoPan tipoPan)
        {
            
            Nombre = nombre;
            PVP = pVP;
            Stock = stock;
            Tamanyo = tamanyo;
            ResenyaBocadillo = new List<ResenyaBocadillo>();
            CompraBocadillo = new List<CompraBocadillo>();
            TipoPan = tipoPan;
        }
        public Bocadillo(string nombre,float pVP, int stock, Tamanyo tamanyo)
        {
            Nombre = nombre;
            PVP = pVP;
            Stock = stock;
            Tamanyo = tamanyo;
            ResenyaBocadillo = new List<ResenyaBocadillo>();
            CompraBocadillo = new List<CompraBocadillo>();
            TipoPan = null!;
        }
        
        public int Id { get; set; }
        
        public string Nombre { get; set; }
        
        public float PVP { get; set; }
        
        public int Stock { get; set; }

       
        public TipoPan TipoPan { get; set; }
        
        public Tamanyo Tamanyo { get; set; }
        public IList<ResenyaBocadillo> ResenyaBocadillo { get; set; }

        public List<CompraBocadillo> CompraBocadillo {get; set;}



        public override bool Equals(object? obj)
        {
            return obj is Bocadillo bocadillo &&
                   Id == bocadillo.Id &&
                   Nombre == bocadillo.Nombre &&
                   PVP == bocadillo.PVP &&
                   Stock == bocadillo.Stock &&
                   EqualityComparer<TipoPan>.Default.Equals(TipoPan, bocadillo.TipoPan) &&
                   Tamanyo == bocadillo.Tamanyo;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, PVP, Stock, TipoPan, Tamanyo);
        }
    }
    public enum Tamanyo
    {
        Pequeño,
        Normal
    }
}
