namespace AppForSEII2526.API.DTOs.BocadillosParaPedirDTOs
{
    public class ItemPedidoDTO
    {
        public int Id { get; set; }
        public string NombreBocadillo { get; set; }

        [Required]
        public int Cantidad { get; set; }
        public float PVP { get; set; }
        public string TipoPan { get; set; }


        public ItemPedidoDTO(int id, string nombreBocadillo, int cantidad, int pVP, string tipoPan)
        {
            Id = id;
            NombreBocadillo = nombreBocadillo;
            Cantidad = cantidad;
            PVP = pVP;
            TipoPan = tipoPan;
        }


        public override bool Equals(object? obj)
        {
            return obj is ItemPedidoDTO dTO &&
                    Id == dTO.Id &&
                   NombreBocadillo == dTO.NombreBocadillo &&
                   Cantidad == dTO.Cantidad &&
                   PVP == dTO.PVP &&
                   TipoPan == dTO.TipoPan;
                    
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, NombreBocadillo, Cantidad, PVP, TipoPan);
        }

    }
}
