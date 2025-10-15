namespace AppForSEII2526.API.Models
{
    public class Resenya
    {
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }
        [Required]
        public string Descripcion { get; set; }
        public DateTime FechaPublicacion { get; set; }

        public ApplicationUser ApplicationUser { get; set; }


        public IList<ResenyaBocadillo> ResenyaBocadillo { get; set; }

        [Required]
        public Valoracion_General ValoracionGeneral { get; set; }
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
