namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(BocadilloId),nameof(CompraId))]
    public class CompraBocadillo
    {
        public CompraBocadillo()
        {
            NombreBocadillo = string.Empty;
            TipoPan = string.Empty;
        }

        public CompraBocadillo(int bocadilloId,Compra compra, int cantidad)
        {
            BocadilloId = bocadilloId;
            Compra = compra;
            
            Cantidad = cantidad;
            CompraId = compra.CompraID;
            

        }

        public CompraBocadillo(int bocadilloId, Compra compra, int compraId, int cantidad, float precio,string tipoPan, string nombreBocadillo)
        {
            BocadilloId = bocadilloId;
            Compra = compra;
            CompraId = compraId;
            Cantidad = cantidad;
            Precio = precio;
            TipoPan = tipoPan;
            NombreBocadillo = nombreBocadillo;
        }

        public CompraBocadillo(Bocadillo bocadillo, Compra compra, int cantidad)
        {
            Bocadillo = bocadillo;
            BocadilloId = bocadillo.Id;
            Compra = compra;
            
            Cantidad = cantidad;
            Precio = bocadillo.PVP;
            TipoPan = bocadillo.TipoPan.Nombre;
            NombreBocadillo=bocadillo.Nombre;
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
