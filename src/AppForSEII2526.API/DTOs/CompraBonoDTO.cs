
namespace AppForSEII2526.API.DTOs
{
    public class CompraBonoDTO
    {
        public CompraBonoDTO() { }

        public string CompraBonoId { get; set; }

        public DateTime FechaCompraBono { get; set; }

        public int NBono { get; set; }

        public float PrecioTotalBono { get; set; }

        public MetodoPago MetodoPagoUsuario { get; set; }

        public ApplicationUser applicationuser { get; set; }

        
    }
}
