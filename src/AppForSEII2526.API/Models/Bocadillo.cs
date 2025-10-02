namespace AppForSEII2526.API.Models
{
    public class Bocadillo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public float PVP { get; set; }
        public int Stock { get; set; }

        public TipoPan TipoPan { get; set; }

        public Tamanyo Tamanyo { get; set; }
        public IList<ResenyaBocadillo> ResenyaBocadillo { get; set; }



    }
    public enum Tamanyo
    {
        Pequeño,
        Normal
    }
}
