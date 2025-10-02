namespace AppForSEII2526.API.Models
{
    public class Bocadillo
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public float PVP { get; set; }
        [Required]
        public int Stock { get; set; }

        [Required]
        public TipoPan TipoPan { get; set; }
        [Required]
        public Tamanyo Tamanyo { get; set; }
        public IList<ResenyaBocadillo> ResenyaBocadillo { get; set; }



    }
    public enum Tamanyo
    {
        Pequeño,
        Normal
    }
}
