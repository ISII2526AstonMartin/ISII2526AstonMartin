namespace AppForSEII2526.API.DTOs.BocadillosResenyaDTOs
{
    public class DetailResenyaDTO : CreateResenyaDTO
    {
        public DetailResenyaDTO(int id,string nombreUsuario, string titulo, string descripcion, DateTime fechaPublicacion,
            Valoracion_General valoracion_General, List<ItemResenyaDTO> itemResenyas) : base(nombreUsuario, titulo, descripcion, valoracion_General, itemResenyas)
        {
            Id = id;
            FechaPublicacion = fechaPublicacion;
        }

        public DetailResenyaDTO(int id, string titulo, string descripcion, DateTime fechaPublicacion,
            Valoracion_General valoracion_General, List<ItemResenyaDTO> itemResenyas) : base(null, titulo, descripcion, valoracion_General, itemResenyas)
        {
            Id = id;
            FechaPublicacion = fechaPublicacion;
        }

        public int Id { get; set; }
        public DateTime FechaPublicacion { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is DetailResenyaDTO dTO &&
               base.Equals(obj) &&
               NombreUsuario == dTO.NombreUsuario &&
               Titulo == dTO.Titulo &&
               descripcion == dTO.descripcion &&
               valoracion_General == dTO.valoracion_General &&
               items.SequenceEqual(dTO.items) &&
               Id == dTO.Id &&
               FechaPublicacion == dTO.FechaPublicacion;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), NombreUsuario, Titulo, descripcion, valoracion_General, items, Id, FechaPublicacion);
        }
    }
}
