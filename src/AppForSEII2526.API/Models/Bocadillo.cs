namespace AppForSEII2526.API.Models
{
    public class Bocadillo
    {
        public int Id { get; set; }
        public string nombre { get; set; }
        public float pvp { get; set; }
        public int stock { get; set; }
        //public List<CompraBocadillo> comprasDelBocadillo { get; set; }

        public TipoPan tipoPan { get; set; }

        public Tamanyo tamanyo { get; set; }
        public List<ResenyaBocadillo> resenyaBocadillo { get; set; }



    }
    public enum Tamanyo
    {
        Pequeño,
        Normal
    }
}
