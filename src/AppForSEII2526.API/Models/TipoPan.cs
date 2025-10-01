namespace AppForSEII2526.API.Models
{
    public class TipoPan
    {
        public string nombre { get; set; }
        public int PanId { get; set; }
        public List<Bocadillo> Bocadillos { get; set; }
    }
}
