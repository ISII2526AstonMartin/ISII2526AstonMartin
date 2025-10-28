namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(BocadilloId),nameof(CompraId))]
    public class CompraBocadillo
    {
        public CompraBocadillo(Bocadillo bocadillo, Compra compra, int cantidad)
        {
            Bocadillo = bocadillo;
            
            Compra = compra;
            
            Cantidad = cantidad;
            Precio = bocadillo.PVP;
            TipoPan = bocadillo.TipoPan.Nombre;
            
        }

        public Bocadillo Bocadillo { get; set; }

        public int BocadilloId { get; set; }


        public Compra Compra { get; set; }
        public int CompraId { get; set; }

        [Required]
        public int Cantidad {  get; set; }
        
        public float Precio { get; set; }

        public string TipoPan { get; set; }
        
        public string NombreBocadillo { get; set; }

        
    }
}
