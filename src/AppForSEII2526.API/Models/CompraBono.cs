using NuGet.Versioning;

namespace AppForSEII2526.API.Models
{
    public class  CompraBono
    {
        public CompraBono(string compraBonoId, string nombreCliente, string apellido1Cliente, string? apellido2Cliente, DateTime fechaCompraBono, int nBono, string precioTotalBono, MetodosDePago metodoPago)
        {
            this.compraBonoId = compraBonoId;
            this.nombreCliente = nombreCliente;
            this.apellido1Cliente = apellido1Cliente;
            this.apellido2Cliente = apellido2Cliente;
            this.fechaCompraBono = fechaCompraBono;
            this.nBono = nBono;
            this.precioTotalBono = precioTotalBono;
            this.metodoPago = metodoPago;
        }
        [Key]
        public string compraBonoId { get; set; }

        [Required]
        public string nombreCliente { get; set; }

        [Required]
        public string apellido1Cliente { get; set; }

        public string apellido2Cliente { get; set; }

        [Required]
        public DateTime fechaCompraBono { get; set; }

        [Required]
        public int nBono { get; set; }

        [Required]
        public string precioTotalBono { get; set; }

        [Required]
        public MetodosDePago metodoPago { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CompraBono bono && compraBonoId == bono.compraBonoId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(compraBonoId, nombreCliente, apellido1Cliente, apellido2Cliente, fechaCompraBono, nBono, precioTotalBono, metodoPago);
        }

        public enum MetodosDePago
        {
            Tarjeta,
            Paypal,
            GooglePay
        }
    }
}
