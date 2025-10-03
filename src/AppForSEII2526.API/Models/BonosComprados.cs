using NuGet.Versioning;

namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(BonoId), nameof(CompraId))]
    public class  BonosComprados
    {
        /*
        public BonosComprados(BonoBocadillo bonoBocadillo, CompraBono compraBono , int cantidad, float precioBono)
        {
            this.cantidad = cantidad;
            this.precioBono = precioBono;
            this.bonoBocadillo = bonoBocadillo;
            this.compraBono = compraBono;
            this.bonoId = bonoBocadillo.bonoID;
            this.compraId = compraBono.compraBonoId;
        }
        */
        public BonoBocadillo BonoBocadillo { get; set; }

        public string BonoId { get; set; }

        public CompraBono Comprabono { get; set; }

        public string CompraId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "No se puede tener cantidad 0")]
        public int Cantidad { get; set; }

        public float PrecioBono { get; set; }
        /*
        public override bool Equals(object? other)
        {
            return other is BonosComprados bono && bonoId == bono.bonoId && compraId == bono.compraId;
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(bonoId, compraId, cantidad, precioBono, bonoBocadillo, compraBono);
        }
        */
    }
}
