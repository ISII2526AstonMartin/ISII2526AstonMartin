
namespace AppForSEII2526.API.DTOs
{
    public class ResenyaDTO
    {
        public ResenyaDTO()
        {
            bocadillos = new List<BocadillosPuntuacion>();
        }

        public ResenyaDTO(int id, string? nombreUsuario, string titulo, string descripcion, ValoracionGeneral valoracionGeneral, List<BocadillosPuntuacion> bocadillos)
        {
            Id = id;
            this.nombreUsuario = nombreUsuario;
            this.titulo = titulo;
            this.descripcion = descripcion;
            this.valoracionGeneral = valoracionGeneral;
            this.bocadillos = bocadillos;
        }

        public int Id { get; set; }
        public string? nombreUsuario { get; set; }
        [Required]
        public string titulo { get; set; }
        [Required]
        public string descripcion { get; set; }
        [Required]
        public ValoracionGeneral valoracionGeneral { get; set; }


        public enum ValoracionGeneral
        {
            Uno,
            Dos,
            Tres,
            Cuatro,
            Cinco
        }


        public List<BocadillosPuntuacion> bocadillos { get; set; }
        public class BocadillosPuntuacion
        {
            public BocadillosPuntuacion(int idBocadillo, int puntuacion)
            {
                this.idBocadillo = idBocadillo;
                this.puntuacion = puntuacion;
            }
            public int idBocadillo { get; set; }
            public int puntuacion { get; set; }
            
        }

        public override bool Equals(object? obj)
        {
            return obj is ResenyaDTO dTO &&
                   Id == dTO.Id &&
                   nombreUsuario == dTO.nombreUsuario &&
                   titulo == dTO.titulo &&
                   descripcion == dTO.descripcion &&
                   valoracionGeneral == dTO.valoracionGeneral &&
                   EqualityComparer<List<BocadillosPuntuacion>>.Default.Equals(bocadillos, dTO.bocadillos);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, nombreUsuario, titulo, descripcion, valoracionGeneral, bocadillos);
        }
    }
}
