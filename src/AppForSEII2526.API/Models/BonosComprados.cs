using NuGet.Versioning;

namespace AppForSEII2526.API.Models
{
    public class  BonosComprados
    {
        
        public BonosComprados(BonoBocadillo bonoBocadillo, CompraBono compraBono , int cantidad, float precioBono)
        {
            this.cantidad = cantidad;
            this.precioBono = precioBono;
            this.bonoBocadillo = bonoBocadillo;
            this.compraBono = compraBono;
            this.bonoId = bonoBocadillo.bonoID;
            this.compraId = compraBono.compraBonoId;
        }

        public string bonoId { get; set; }
        public string compraId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "No se puede tener cantidad 0")]
        public int cantidad { get; set; }
        [Required]
        public float precioBono { get; set; } //ver si puedo cambiarlo a int (float no es preciso por la coma flotante)
        [Required, ForeignKey("bonoId")]
        public BonoBocadillo bonoBocadillo { get; set; }

        [Required, ForeignKey("compraBonoId")]
        public CompraBono compraBono { get; set; }

        public override bool Equals(object? other)
        {
            return other is BonosComprados bono && bonoId == bono.bonoId && compraId == bono.compraId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(bonoId, compraId, cantidad, precioBono, bonoBocadillo, compraBono);
        }
    }
}
