namespace AppForSEII2526.API.Models
{
    public class Compra
    {
        
        public int CompraID { get; set; }
        [Required]
        public DateTime FechaCompra {  get; set; }
        [Required]
        public int nBocadillo { get; set; }

        public List<CompraBocadillo> CompraBocadillos { get; set; }
        [Required]
        public string NombreCliente { get; set; }
        [Required]
        public float PrecioTotal { get; set; }
        [Required]
        public string Apellido_1Cliente { get; set; }
        public string Apellido_2Cliente { get; set; }
        [Required]
        public MetodoPago MetodoPago { get; set; }
    }


    public enum MetodoPago
    {
        Tarjeta,
        Paypal,
        Gpay

    }

}
