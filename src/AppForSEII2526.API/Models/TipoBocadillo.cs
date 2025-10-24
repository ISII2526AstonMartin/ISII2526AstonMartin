using NuGet.Versioning;

namespace AppForSEII2526.API.Models
{
    public class  TipoBocadillo
    {
        public TipoBocadillo(string idTipo, string nombreTipo)
        {
            IdTipo = idTipo;
            NombreTipo = nombreTipo;
        }

        [Key]
        public string IdTipo { get; set; }
        
        public string NombreTipo { get; set; }

        public List<BonoBocadillo> ListaBonoBocadillos { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is TipoBocadillo bocadillo &&
                   IdTipo == bocadillo.IdTipo &&
                   NombreTipo == bocadillo.NombreTipo &&
                   EqualityComparer<List<BonoBocadillo>>.Default.Equals(ListaBonoBocadillos, bocadillo.ListaBonoBocadillos);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdTipo, NombreTipo, ListaBonoBocadillos);
        }
    }
}
