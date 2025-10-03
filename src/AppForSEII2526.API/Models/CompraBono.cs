using NuGet.Versioning;
using System.Diagnostics.CodeAnalysis;

namespace AppForSEII2526.API.Models
{
    public class  CompraBono
    {
        /*
        public CompraBono(string compraBonoId, string nombreCliente, string apellido1Cliente, string? apellido2Cliente, DateTime fechaCompraBono, int nBono, float precioTotalBono)
        {
            this.compraBonoId = compraBonoId;
            this.nombreCliente = nombreCliente;
            this.apellido1Cliente = apellido1Cliente;
            this.apellido2Cliente = apellido2Cliente;
            this.fechaCompraBono = fechaCompraBono;
            this.nBono = nBono;
            this.precioTotalBono = precioTotalBono;
        }
        */

        [Key]
        public string CompraBonoId { get; set; }

        [Required]
        public string NombreCliente { get; set; }

        [Required]
        public string Apellido1Cliente { get; set; }

        [AllowNull]
        public string Apellido2Cliente { get; set; }

        [Required]
        public DateTime FechaCompraBono { get; set; }

        [Required]
        public int NBono { get; set; }

        [Required]
        public float PrecioTotalBono { get; set; } 

        public MetodoPago MetodoPagoUsuario { get; set; }

        public List<BonosComprados> ListaBonosComprados { get; set; }

        /*
        public override bool Equals(object? obj)
        {
            return obj is CompraBono bono && compraBonoId == bono.compraBonoId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(compraBonoId, nombreCliente, apellido1Cliente, apellido2Cliente, fechaCompraBono, nBono, precioTotalBono);
        }
        */
        public enum MetodoPago
        {
            Paypal,
            Bizum,
            GooglePay
        }
    }
}
