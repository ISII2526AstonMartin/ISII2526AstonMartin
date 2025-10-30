using NuGet.Versioning;

namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(BonoId), nameof(CompraBonoId))]
    public class  BonosComprados
    {
        public BonosComprados()
        {
        }
        public BonosComprados(int bonoid, CompraBono cb, int cantidad)
        {
            BonoId = bonoid;
            Comprabono = cb;
            CompraBonoId = cb.CompraBonoId;
            Cantidad = cantidad;
        }

        public BonosComprados(BonoBocadillo bb, CompraBono cb, int cantidad, float precioBono)
        {
            Comprabono = cb;
            BonoBocadillo = bb;
            Cantidad = cantidad;
            PrecioBono = precioBono;
            BonoId = bb.BonoID;
            CompraBonoId = cb.CompraBonoId;
        }

        public BonosComprados(int bonoid, int comprabonoid, int cantidad, float precioBono)
        {
            Cantidad = cantidad;
            PrecioBono = precioBono;
            BonoId = bonoid;
            CompraBonoId = comprabonoid;
        }

        public BonosComprados(int bonoId, CompraBono comprabono, int cantidad, float precioBono)
        {
            BonoId = bonoId;
            Comprabono = comprabono;
            CompraBonoId = comprabono.CompraBonoId;
            Cantidad = cantidad;
            PrecioBono = precioBono;
        }

        public BonoBocadillo BonoBocadillo { get; set; }

        public int BonoId { get; set; }

        public CompraBono Comprabono { get; set; }

        public int CompraBonoId { get; set; }

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
