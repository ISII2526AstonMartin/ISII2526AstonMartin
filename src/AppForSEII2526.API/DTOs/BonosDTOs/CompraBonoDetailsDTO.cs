
namespace AppForSEII2526.API.DTOs.BonosDTOs
{
    public class CompraBonoDetailsDTO : CompraBonoForCreateDTO
    {
        public CompraBonoDetailsDTO(string Id, string nombreCliente, string apellido1, string apellido2, MetodoPago metodoPago, DateTime fechaCompraBono, List<CompraBonoItemDTO> compraitems) 
            : base(nombreCliente, apellido1, apellido2, metodoPago, fechaCompraBono, compraitems)
        {
            id= Id;
        }

        public string id {  get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CompraBonoDetailsDTO dTO &&
                   base.Equals(obj) &&
                   nombreCliente == dTO.nombreCliente &&
                   apellido1 == dTO.apellido1 &&
                   apellido2 == dTO.apellido2 &&
                   metodoPago == dTO.metodoPago &&
                   FechaCompraBono == dTO.FechaCompraBono &&
                   EqualityComparer<List<CompraBonoItemDTO>>.Default.Equals(compraItems, dTO.compraItems) &&
                   id == dTO.id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), nombreCliente, apellido1, apellido2, metodoPago, FechaCompraBono, compraItems, id);
        }
    }
}
