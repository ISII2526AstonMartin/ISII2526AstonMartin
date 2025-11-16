using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTOs.ComprarMerchDTOs
{
    public class CreateMerchDTO
    {
        public CreateMerchDTO()
        {
            Items = new List<ItemMerchDTO>();
        }

        public CreateMerchDTO(string? nombreUsuario, string apellido1, string? apellido2, string direccionEnvio, MetodoPago metodoPago, IList<ItemMerchDTO> items)
        {
            NombreUsuario = nombreUsuario;
            Apellido1 = apellido1;
            Apellido2 = apellido2;
            DireccionEnvio = direccionEnvio;
            MetodoPago = metodoPago;
            Items = items;
        }
        public string? NombreUsuario { get; set; }
        public string Apellido1 { get; set; }
        public string? Apellido2 { get; set; }
        public string DireccionEnvio { get; set; }
        public MetodoPago MetodoPago { get; set; }
        public IList<ItemMerchDTO> Items { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CreateMerchDTO dto &&
                   NombreUsuario == dto.NombreUsuario &&
                   Apellido1 == dto.Apellido1 &&
                   Apellido2 == dto.Apellido2 &&
                   DireccionEnvio == dto.DireccionEnvio &&
                   MetodoPago == dto.MetodoPago &&
                   Items.SequenceEqual(dto.Items);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NombreUsuario, Apellido1, Apellido2, DireccionEnvio, MetodoPago, Items);
        }
    }
}
