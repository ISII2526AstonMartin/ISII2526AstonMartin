using NuGet.Versioning;

namespace AppForSEII2526.API.Models
{
    public class  BonosComprados
    {
        
        public BonosComprados(string bonoId, string compraId, int cantidad, float precioBono, BonoBocadillo bonoBocadillo, CompraBono compraBono)
        {
            this.bonoId = bonoId;
            this.compraId = compraId;
            this.cantidad = cantidad;
            this.precioBono = precioBono;
            this.bonoBocadillo = bonoBocadillo;
            this.compraBono = compraBono;
        }

        [Key]
        public string bonoId { get; set; }
        [Key]
        public string compraId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "No se puede tener cantidad 0")]
        public int cantidad { get; set; }
        [Required]
        public float precioBono { get; set; }
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
