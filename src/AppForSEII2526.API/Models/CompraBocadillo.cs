namespace AppForSEII2526.API.Models
{

    [PrimaryKey(nameof(bocadilloId),
nameof(compraId))]
    public class CompraBocadillo
    {
        public Bocadillo bocadillo { get; set; }
        public int bocadilloId { get; set; }


        public Compra compra { get; set; }
        public int compraId { get; set; }


        public int cantidad {  get; set; }
        public float precio { get; set; }

        public TipoPan tipoPan { get; set; }
        public string nombreBocadillo { get; set; }
    }
}
