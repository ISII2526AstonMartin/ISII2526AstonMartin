namespace AppForSEII2526.API.Models
{
    public class Compra
    {
        public int CompraID { get; set; }
        public DateTime FechaCompra {  get; set; }
        public int nBocadillo { get; set; }

        public IList<CompraBocadillo> CompraBocadillos { get; set; }

        public string NombreCliente { get; set; }
        public float PrecioTotal { get; set; }

        public string Apellido_1Cliente { get; set; }
        public string Apellido_2Cliente { get; set; }

        public MetodoPago MetodoPago { get; set; }
    }


    public enum MetodoPago
    {
        Tarjeta,
        Paypal,
        Gpay

    }

}
