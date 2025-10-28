namespace AppForSEII2526.API.DTOs.BocadillosParaPedirDTOs
{
    public class ItemPedidoDTO
    {

        public string nombreBocadillo { get; set; }

        [Required]
        public int Cantidad { get; set; }
        public int PVP { get; set; }
        public string TipoPan { get; set; }


        public ItemPedidoDTO(string nombreBocadillo, int cantidad, int pVP, string tipoPan)
        {
            this.nombreBocadillo = nombreBocadillo;
            Cantidad = cantidad;
            PVP = pVP;
            TipoPan = tipoPan;
        }


        public override bool Equals(object? obj)
        {
            return obj is ItemPedidoDTO dTO &&
                   nombreBocadillo == dTO.nombreBocadillo &&
                   Cantidad == dTO.Cantidad &&
                   PVP == dTO.PVP &&
                   TipoPan == dTO.TipoPan;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(nombreBocadillo, Cantidad, PVP, TipoPan);
        }

    }
}
