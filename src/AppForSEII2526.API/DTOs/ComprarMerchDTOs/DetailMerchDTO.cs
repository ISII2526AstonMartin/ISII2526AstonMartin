namespace AppForSEII2526.API.DTOs.ComprarMerchDTOs
{
    public class DetailMerchDTO : CreateMerchDTO
    {
        public DetailMerchDTO(
            string nombreUsuario,
            string apellido1,
            string? apellido2,
            string direccionEnvio,
            MetodoPago metodoPago,
            IList<ItemMerchDTO> items,
            int compraID,
            DateTime fechaCompra,
            float precioFinal)
            : base(nombreUsuario, apellido1, apellido2, direccionEnvio, metodoPago, items)
        {
            CompraID = compraID;
            FechaCompra = fechaCompra;
            PrecioFinal = precioFinal;
        }

        public int CompraID { get; set; }

        public DateTime FechaCompra { get; set; }

        public float PrecioFinal { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is DetailMerchDTO dto &&
                   base.Equals(obj) &&
                   CompraID == dto.CompraID &&
                   FechaCompra == dto.FechaCompra &&
                   PrecioFinal == dto.PrecioFinal;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), CompraID, FechaCompra, PrecioFinal);
        }
    }
}
