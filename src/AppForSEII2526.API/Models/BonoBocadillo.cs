using NuGet.Versioning;

namespace AppForSEII2526.API.Models
{
    public class BonoBocadillo
    {
        /*
        public BonoBocadillo(string bonoID, int cantidad, int nBoc, string name, float precio)
        {
            this.bonoID = bonoID;
            //this.TipoBocadillo = tipo;
            this.cantidadDisponible = cantidad;
            this.nBocadillos = nBoc;
            this.nombre = name;
            this.pvp = precio;
        }
        */

        [Key]
        public string BonoID { get; set; }
        /*
        [Required, ForeignKey("idTipo")]
        public TipoBocadillo TipoBocadillo { get; set; }
        */
        
        [Required]
        public int CantidadDisponible { get; set; }
        
        [Required]
        public int NBocadillos { get; set; }
        
        [Required]
        public string NombreBocadillo { get; set; }
        
        [Required]
        public float PVP { get; set; }

        public TipoBocadillo Tipo { get; set; }
        
        public List<BonosComprados> ListaBonosComprados { get; set; }
        /*
        public override bool Equals(object? other)
        {
            return other is BonoBocadillo bono && bonoID == bono.bonoID;
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(bonoID, TipoBocadillo, cantidadDisponible, nBocadillos, nombre, pvp);
        }
        */
    }
}
