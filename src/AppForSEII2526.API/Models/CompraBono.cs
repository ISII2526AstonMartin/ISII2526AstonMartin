using NuGet.Versioning;
using System.Diagnostics.CodeAnalysis;

namespace AppForSEII2526.API.Models
{
    public class CompraBono
    {
        public CompraBono()
        {
        }
        public CompraBono(DateTime fechaCompraBono, int nBono, float precioTotalBono, MetodoPago metodoPagoUsuario, List<BonosComprados> listaBonosComprados, ApplicationUser applicationuser)
        {
            FechaCompraBono = fechaCompraBono;
            NBono = nBono;
            PrecioTotalBono = precioTotalBono;
            MetodoPagoUsuario = metodoPagoUsuario;
            ListaBonosComprados = listaBonosComprados;
            this.applicationuser = applicationuser;
        }

        public CompraBono(int id, DateTime fechaCompraBono, int nBono, float precioTotalBono, MetodoPago metodoPagoUsuario, List<BonosComprados> listaBonosComprados, ApplicationUser applicationuser)
        {
            CompraBonoId= id;
            FechaCompraBono = fechaCompraBono;
            NBono = nBono;
            PrecioTotalBono = precioTotalBono;
            MetodoPagoUsuario = metodoPagoUsuario;
            ListaBonosComprados = listaBonosComprados;
            this.applicationuser = applicationuser;
        }
        /*
        public CompraBono(string compraBonoId, DateTime fechaCompraBono, int nBono, float precioTotalBono, MetodoPago metodoPagoUsuario, List<BonosComprados> listaBonosComprados, ApplicationUser applicationuser)
        {
            CompraBonoId = compraBonoId;
            FechaCompraBono = fechaCompraBono;
            NBono = nBono;
            PrecioTotalBono = precioTotalBono;
            MetodoPagoUsuario = metodoPagoUsuario;
            ListaBonosComprados = listaBonosComprados;
            this.applicationuser = applicationuser;
        }
        */

        [Key]
        public int CompraBonoId { get; set; }

        public DateTime FechaCompraBono { get; set; }

        public int NBono { get; set; }

        public float PrecioTotalBono { get; set; }

        [Required]
        public MetodoPago MetodoPagoUsuario { get; set; }

        public List<BonosComprados> ListaBonosComprados { get; set; }

        public ApplicationUser applicationuser { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CompraBono bono &&
                   CompraBonoId == bono.CompraBonoId &&
                   FechaCompraBono == bono.FechaCompraBono &&
                   NBono == bono.NBono &&
                   PrecioTotalBono == bono.PrecioTotalBono &&
                   MetodoPagoUsuario == bono.MetodoPagoUsuario &&
                   EqualityComparer<List<BonosComprados>>.Default.Equals(ListaBonosComprados, bono.ListaBonosComprados) &&
                   EqualityComparer<ApplicationUser>.Default.Equals(applicationuser, bono.applicationuser);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CompraBonoId, FechaCompraBono, NBono, PrecioTotalBono, MetodoPagoUsuario, ListaBonosComprados, applicationuser);
        }
    }
}
