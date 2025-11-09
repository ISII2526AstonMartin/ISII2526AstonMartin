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

        [Display(Name = "Nombre de Usuario")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor introduzca su nombre")]
        [StringLength(20, ErrorMessage = "El nombre de usuario no puede tener más de 20 caracteres.")]
        public string? NombreUsuario { get; set; }

        [Display(Name = "Primer Apellido")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor introduzca su primer apellido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El apellido tiene que tener al menos 3 caracteres")]
        public string Apellido1 { get; set; }

        public string? Apellido2 { get; set; }

        [Display(Name = "Dirección de envío")]
        [StringLength(70, ErrorMessage = "La dirección tiene que ser como máximo de 70 caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor introduzca la dirección de envío")]
        public string DireccionEnvio { get; set; }

        [Display(Name = "Método de pago")]
        [Required(ErrorMessage = "Por favor introduzca el método de pago que desea")]
        public MetodoPago MetodoPago { get; set; }

        [Required(ErrorMessage = "Debe incluir al menos un producto")]
        [MinLength(1, ErrorMessage = "Debe incluir al menos un producto")]
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
