using NuGet.Versioning;

namespace AppForSEII2526.API.Models
{
    public class  BonoBocadillo
    {
        public BonoBocadillo(string bonoID, TipoBocadillo tipo, int cantidad, int nBoc, string name, float precio)
        {
            this.bonoID = bonoID;
            this.TipoBocadillo = tipo;
            this.cantidadDisponible = cantidad;
            this.nBocadillos = nBoc;
            this.nombre = name;
            this.pvp = precio;
        }

        [Key]
        public string bonoID { get; set; }
        [Required, ForeignKey("idTipo")]
        public TipoBocadillo TipoBocadillo { get; set; }
        [Required]
        public int cantidadDisponible { get; set; }
        [Required]
        public int nBocadillos { get; set; }
        [Required]
        public string nombre { get; set; }
        [Required]
        public float pvp { get; set; } //ver si puedo cambiarlo a int (float no es preciso por la coma flotante)

        public override bool Equals(object? other)
        {
            return other is BonoBocadillo bono && bonoID == bono.bonoID;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(bonoID, TipoBocadillo, cantidadDisponible, nBocadillos, nombre, pvp);
        }
    }
}
