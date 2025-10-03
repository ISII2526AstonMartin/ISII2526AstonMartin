namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(BocadilloId),nameof(CompraId))]
    public class CompraBocadillo
    {
        
        public Bocadillo Bocadillo { get; set; }

        public int BocadilloId { get; set; }


        public Compra Compra { get; set; }
        public int CompraId { get; set; }

        [Required]
        public int Cantidad {  get; set; }
        [Required]
        public float Precio { get; set; }

        [Required]
        public string TipoPan { get; set; }
        [Required]
        public string NombreBocadillo { get; set; }

        
    }
}
