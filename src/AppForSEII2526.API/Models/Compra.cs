namespace AppForSEII2526.API.Models
{
    public class Compra
    {
        public Compra( DateTime fechaCompra,  List<CompraBocadillo> compraBocadillos, MetodoPago metodoPago, ApplicationUser usuario)
        {
          
            FechaCompra = fechaCompra;
            
            CompraBocadillos = compraBocadillos;
          
            MetodoPago = metodoPago;
            this.usuario = usuario;
        }

        public int CompraID { get; set; }
        [Required]
        public DateTime FechaCompra {  get; set; }
        [Required]
        public int nBocadillo { get; set; }

        public List<CompraBocadillo> CompraBocadillos { get; set; }
        
        
        [Required]
        public float PrecioTotal { get; set; }
        
        [Required]
        public MetodoPago MetodoPago { get; set; }

        [Required]
        public ApplicationUser usuario { get; set; }
    }


    public enum MetodoPago
    {
        Tarjeta,
        Paypal,
        Gpay

    }

}
