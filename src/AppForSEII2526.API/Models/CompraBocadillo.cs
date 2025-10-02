namespace AppForSEII2526.API.Models
{

    [PrimaryKey(nameof(BocadilloId),
nameof(CompraId))]
    public class CompraBocadillo
    {
        public Bocadillo Bocadillo { get; set; }
        public int BocadilloId { get; set; }


        public Compra Compra { get; set; }
        public int CompraId { get; set; }


        public int Cantidad {  get; set; }
        public float Precio { get; set; }

        public TipoPan TipoPan { get; set; }
        public string NombreBocadillo { get; set; }
    }
}
