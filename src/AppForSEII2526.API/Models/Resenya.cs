namespace AppForSEII2526.API.Models
{
    public class Resenya
    {
        public string descripcion { get; set; }
        public DateTime fechaPublicacion { get; set; }
        public int Id { get; set; }
        public string nombreUsuario { get; set; }
        public string titulo { get; set; }

        [Range(1, 5)]
        public int valoracionGeneral { get; set; }
        public List<ResenyaBocadillo> resenyaBocadillo { get; set; }
    }
}
