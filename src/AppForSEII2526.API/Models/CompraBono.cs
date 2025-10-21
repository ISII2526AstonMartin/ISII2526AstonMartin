using NuGet.Versioning;
using System.Diagnostics.CodeAnalysis;

namespace AppForSEII2526.API.Models
{
    public class CompraBono
    {
        public CompraBono(string compraBonoId, DateTime fechaCompraBono, int nBono, float precioTotalBono, MetodoPago metodoPagoUsuario)
        {
            CompraBonoId = compraBonoId;
            FechaCompraBono = fechaCompraBono;
            NBono = nBono;
            PrecioTotalBono = precioTotalBono;
            MetodoPagoUsuario = metodoPagoUsuario;
        }

        [Key]
        public string CompraBonoId { get; set; }

        public DateTime FechaCompraBono { get; set; }

        public int NBono { get; set; }

        public float PrecioTotalBono { get; set; }

        [Required]
        public MetodoPago MetodoPagoUsuario { get; set; }

        public List<BonosComprados> ListaBonosComprados { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CompraBono bono &&
                   CompraBonoId == bono.CompraBonoId &&
                   FechaCompraBono == bono.FechaCompraBono &&
                   NBono == bono.NBono &&
                   PrecioTotalBono == bono.PrecioTotalBono &&
                   MetodoPagoUsuario == bono.MetodoPagoUsuario &&
                   EqualityComparer<List<BonosComprados>>.Default.Equals(ListaBonosComprados, bono.ListaBonosComprados);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CompraBonoId, FechaCompraBono, NBono, PrecioTotalBono, MetodoPagoUsuario, ListaBonosComprados);
        }
    }
}
