
namespace AppForSEII2526.API.DTOs.CompraBonosDTOs
{
    public class CompraBonoForCreateDTO
    {
        public CompraBonoForCreateDTO() 
        {
            compraItems= new List<CompraBonoItemDTO>();
        }

        public CompraBonoForCreateDTO(string nombreCliente, string apellido1, string? apellido2, MetodoPago metodoPago, List<CompraBonoItemDTO> compraItems)
        {
            this.nombreCliente = nombreCliente;
            this.apellido1 = apellido1;
            this.apellido2 = apellido2;
            this.metodoPago = metodoPago;
            this.compraItems = compraItems;
        }

        //[Required]
        public string nombreCliente {  get; set; }
        //[Required]
        public string apellido1 { get; set; }
        public string? apellido2 { get; set; }
        //[Required]
        public MetodoPago metodoPago { get; set; }
        public List<CompraBonoItemDTO> compraItems { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CompraBonoForCreateDTO dTO &&
                   nombreCliente == dTO.nombreCliente &&
                   apellido1 == dTO.apellido1 &&
                   apellido2 == dTO.apellido2 &&
                   metodoPago == dTO.metodoPago &&
                   compraItems.SequenceEqual(dTO.compraItems);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(nombreCliente, apellido1, apellido2, metodoPago, compraItems);
        }
    }
}
