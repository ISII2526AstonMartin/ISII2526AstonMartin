namespace AppForSEII2526.API.Models
{
    public class Compra
    {
        public int compraID { get; set; }
        public DateTime fechaCompra {  get; set; }
        public int nBocadillo { get; set; }

        public IList<CompraBocadillo> compraBocadillos { get; set; }

        public string nombreCliente { get; set; }
        public float precioTotal { get; set; }

    }

}
