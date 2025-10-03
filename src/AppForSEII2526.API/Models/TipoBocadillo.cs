using NuGet.Versioning;

namespace AppForSEII2526.API.Models
{
    public class  TipoBocadillo
    {
        /*
        public TipoBocadillo(string idTipo, string nombreTipo)
        {
            this.idTipo = idTipo;
            this.nombreTipo = nombreTipo;
        }
        */
        
        [Key]
        public string IdTipo { get; set; }
        
        [Required]
        public string NombreTipo { get; set; }

        public List<BonoBocadillo> ListaBonoBocadillos { get; set; }

        /*
        public override bool Equals(object? other)
        {
            return other is TipoBocadillo tipo && idTipo == tipo.idTipo;
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(idTipo, nombreTipo);
        }
        */
    }
}
