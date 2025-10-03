namespace AppForSEII2526.API.Models
{
    public class Resenya
    {

        [Required]
        public string Descripcion { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        [Required]
        public string Titulo { get; set; }

        public IList<ResenyaBocadillo> ResenyaBocadillo { get; set; }

        [Required]
        public string ValoracionGeneral { get; set; }
        public enum Valoracion_General
        {
            Uno,
            Dos,
            Tres,
            Cuatro,
            Cinco
        }
    }
}
