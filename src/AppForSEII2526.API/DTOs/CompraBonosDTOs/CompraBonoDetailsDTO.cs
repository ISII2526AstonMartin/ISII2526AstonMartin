namespace AppForSEII2526.API.DTOs.CompraBonosDTOs
{
    public class CompraBonoDetailsDTO : CompraBonoForCreateDTO
    {
        public CompraBonoDetailsDTO()
        {
        }

        public CompraBonoDetailsDTO(string Id, DateTime fecha, string nombreCliente, string apellido1, string? apellido2, MetodoPago metodoPago, List<CompraBonoItemDTO> compraItems) 
            : base(nombreCliente, apellido1, apellido2, metodoPago, compraItems)
        {
            id = Id;
            fechaCompra= fecha;
        }

        public string id { get; set; }
        public DateTime fechaCompra {  get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CompraBonoDetailsDTO dTO &&
                   base.Equals(obj) &&
                   nombreCliente == dTO.nombreCliente &&
                   apellido1 == dTO.apellido1 &&
                   apellido2 == dTO.apellido2 &&
                   metodoPago == dTO.metodoPago &&
                   EqualityComparer<List<CompraBonoItemDTO>>.Default.Equals(compraItems, dTO.compraItems) &&
                   id == dTO.id &&
                   fechaCompra == dTO.fechaCompra;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), nombreCliente, apellido1, apellido2, metodoPago, compraItems, id, fechaCompra);
        }
    }
}
