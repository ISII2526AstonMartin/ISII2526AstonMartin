
namespace AppForSEII2526.API.Models
{
    public class Resenya
    {
        public Resenya()
        {
        }
        public Resenya(string titulo, string descripcion, DateTime fechaPublicacion, ApplicationUser? applicationUser, IList<ResenyaBocadillo> resenyaBocadillo, Valoracion_General valoracionGeneral)
        {
            Titulo = titulo;
            Descripcion = descripcion;
            FechaPublicacion = fechaPublicacion;
            ApplicationUser = applicationUser;
            ResenyaBocadillo = resenyaBocadillo;
            ValoracionGeneral = valoracionGeneral;
        }

        

        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }
        [Required]
        public string Descripcion { get; set; }
        public DateTime FechaPublicacion { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }


        public IList<ResenyaBocadillo> ResenyaBocadillo { get; set; }

        [Required]
        public Valoracion_General ValoracionGeneral { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Resenya resenya &&
                   Id == resenya.Id &&
                   Titulo == resenya.Titulo &&
                   Descripcion == resenya.Descripcion &&
                   FechaPublicacion == resenya.FechaPublicacion &&
                   EqualityComparer<ApplicationUser>.Default.Equals(ApplicationUser, resenya.ApplicationUser) &&
                   EqualityComparer<IList<ResenyaBocadillo>>.Default.Equals(ResenyaBocadillo, resenya.ResenyaBocadillo) &&
                   ValoracionGeneral == resenya.ValoracionGeneral;
        }

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
