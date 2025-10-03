namespace AppForSEII2526.API.Models
{
    public class TipoPan
    {

        public int Id { get; set; }
        public List<Bocadillo> Bocadillos { get; set; }
        [Required]
        public string Nombre { get; set; }

        

        

    }
}
