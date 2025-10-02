namespace AppForSEII2526.API.Models
{
    public class TipoPan
    {
        [Key]
        public int PanId { get; set; }
        [Required]
        public string nombre { get; set; }
        
        public List<Bocadillo> Bocadillos { get; set; }
    }
}
