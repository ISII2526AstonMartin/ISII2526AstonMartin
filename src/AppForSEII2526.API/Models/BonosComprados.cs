using NuGet.Versioning;

namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(BonoId), nameof(CompraBonoId))]
    public class  BonosComprados
    {
        public BonosComprados()
        {
        }

        public BonosComprados(BonoBocadillo bonoBocadillo, CompraBono comprabono, int cantidad, float precioBono)
        {
            BonoBocadillo = bonoBocadillo;
            Comprabono = comprabono;
            Cantidad = cantidad;
            PrecioBono = precioBono;
            this.BonoId = BonoBocadillo.BonoID;
            this.CompraBonoId = comprabono.CompraBonoId;
        }

        public BonoBocadillo BonoBocadillo { get; set; }

        public string BonoId { get; set; }

        public CompraBono Comprabono { get; set; }

        public string CompraBonoId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "No se puede tener cantidad 0")]
        public int Cantidad { get; set; }

        public float PrecioBono { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is BonosComprados comprados &&
                   EqualityComparer<BonoBocadillo>.Default.Equals(BonoBocadillo, comprados.BonoBocadillo) &&
                   BonoId == comprados.BonoId &&
                   EqualityComparer<CompraBono>.Default.Equals(Comprabono, comprados.Comprabono) &&
                   CompraBonoId == comprados.CompraBonoId &&
                   Cantidad == comprados.Cantidad &&
                   PrecioBono == comprados.PrecioBono;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BonoBocadillo, BonoId, Comprabono, CompraBonoId, Cantidad, PrecioBono);
        }
    }
}
