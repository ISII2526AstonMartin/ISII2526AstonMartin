
namespace AppForSEII2526.API.DTOs.BocadillosResenyaDTOs
{
    public class CreateResenyaDTO
    {
        
        public CreateResenyaDTO(string? nombreUsuario, string titulo, string descripcion, Valoracion_General valoracion_General, List<ItemResenyaDTO> items)
        {
            NombreUsuario = nombreUsuario;
            Titulo = titulo;
            this.descripcion = descripcion;
            this.valoracion_General = valoracion_General;
            this.items = items;
        }

        public CreateResenyaDTO()
        {
            items = new List<ItemResenyaDTO>();
        }
        
        
        [Display(Name = "Nombre de Usuario")]
        [StringLength(20, ErrorMessage = "El nombre de usuario no puede tener más de 20 caracteres.")]  
        public string? NombreUsuario { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "El titulo no puede tener más de 40 caracteres.")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "La descripción no puede tener más de 500 caracteres.")]
        [Display(Name = "Descripción")]
        public string descripcion { get; set; }

        [Required]
        [Display(Name = "Valoración General")]
        public Valoracion_General valoracion_General { get; set; }

        [Required(ErrorMessage = "Debe incluir al menos un bocadillo")]
        [MinLength(1, ErrorMessage = "Debe incluir al menos un bocadillo")]
        public List<ItemResenyaDTO> items { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CreateResenyaDTO dTO &&
                   NombreUsuario == dTO.NombreUsuario &&
                   Titulo == dTO.Titulo &&
                   descripcion == dTO.descripcion &&
                   valoracion_General == dTO.valoracion_General &&
                   EqualityComparer<List<ItemResenyaDTO>>.Default.Equals(items, dTO.items);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NombreUsuario, Titulo, descripcion, valoracion_General, items);
        }

        public enum Valoracion_General
        {
            Uno,
            Dos,
            Tres,
            Cuatro,
            Cinco
        }




    }
}
