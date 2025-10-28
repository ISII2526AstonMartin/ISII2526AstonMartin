using AppForSEII2526.API.DTOs.BocadillosParaPedirDTOs;
using AppForSEII2526.API.DTOs.BocadillosResenyaDTOs;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTOs.ComprarMerchDTOs
{
    public class CreateMerchDTO
    {

        public CreateMerchDTO()
        {
            items = new List<ItemMerchDTO>();
        }
        public CreateMerchDTO(string? nombreUsuario, string apellido1, string? apellido2, string direccionEnvio, int cantidad, MetodoPago metodoPago, IList<ItemMerchDTO> items)
        {
            NombreUsuario = nombreUsuario;
            Apellido1 = apellido1;
            Apellido2 = apellido2;
            DireccionEnvio = direccionEnvio;
            Cantidad = cantidad;
            this.metodoPago = metodoPago;
            this.items = items;
        } 



        [Display(Name = "Nombre de Usuario")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor introduzca su nombre")]
        [StringLength(20, ErrorMessage = "El nombre de usuario no puede tener más de 20 caracteres.")]
        public string? NombreUsuario { get; set; }

        [Display(Name = "Apellidos")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor introduzca su primer apellido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El apellido tiene que tener al menos 3 caracteres")]
        public string Apellido1 { get; set; }

        public string? Apellido2 { get; set; }

        [Display(Name = "Dirección de envio")]
        [StringLength(70, ErrorMessage = "La dirección tiene que ser como máximo de 70 caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor introduzca la dirección de envio")]
        public string DireccionEnvio { get; set; }
        [Display(Name = "Cantidad")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Introduzca la cantidad por favor")]
        public int Cantidad { get; set; }

        [Display(Name = "Método de pago")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor introduzca el metodo de pago que desea")]
        public MetodoPago metodoPago { get; set; }
        [Required(ErrorMessage = "Debe incluir al menos un producto")]
        [MinLength(1, ErrorMessage = "Debe incluir al menos un procucto")]
        public IList<ItemMerchDTO> items { get; set; }

        public
            override bool Equals(object? obj)
        {
            return obj is CreateMerchDTO dTO &&
                   NombreUsuario == dTO.NombreUsuario &&
                   Apellido1 == dTO.Apellido1 &&
                   Apellido2 == dTO.Apellido2 &&
                   DireccionEnvio == dTO.DireccionEnvio &&
                   Cantidad == dTO.Cantidad &&
                   metodoPago == dTO.metodoPago &&
                   items.SequenceEqual(dTO.items);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(NombreUsuario, Apellido1, Apellido2, DireccionEnvio, Cantidad, metodoPago, items);
        }

    }
}