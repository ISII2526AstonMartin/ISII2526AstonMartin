using NuGet.Versioning;

namespace AppForSEII2526.API.Models
{
    public class BonoBocadillo
    {
        public BonoBocadillo()
        {
        }
        public BonoBocadillo(int cantidadDisponible, int nBocadillos, string nombreBono, float pVP, TipoBocadillo tipo, List<BonosComprados> listaBonosComprados)
        {
            CantidadDisponible = cantidadDisponible;
            NBocadillos = nBocadillos;
            NombreBono = nombreBono;
            PVP = pVP;
            Tipo = tipo;
            ListaBonosComprados = listaBonosComprados;
        }

        public BonoBocadillo(int bonoID, int cantidadDisponible, int nBocadillos, string nombreBono, float pVP, TipoBocadillo tipo, List<BonosComprados> listaBonosComprados)
        {
            BonoID = bonoID;
            CantidadDisponible = cantidadDisponible;
            NBocadillos = nBocadillos;
            NombreBono = nombreBono;
            PVP = pVP;
            Tipo = tipo;
            ListaBonosComprados = listaBonosComprados;
        }

        [Key]
        public int BonoID { get; set; }
        
        public int CantidadDisponible { get; set; }
        
        public int NBocadillos { get; set; }
        
        public string NombreBono { get; set; }
        
        public float PVP { get; set; }

        public TipoBocadillo Tipo { get; set; }
        
        public List<BonosComprados> ListaBonosComprados { get; set; }


        public override bool Equals(object? obj)
        {
            return obj is BonoBocadillo bocadillo &&
                   BonoID == bocadillo.BonoID &&
                   CantidadDisponible == bocadillo.CantidadDisponible &&
                   NBocadillos == bocadillo.NBocadillos &&
                   NombreBono == bocadillo.NombreBono &&
                   PVP == bocadillo.PVP &&
                   EqualityComparer<TipoBocadillo>.Default.Equals(Tipo, bocadillo.Tipo) &&
                   EqualityComparer<List<BonosComprados>>.Default.Equals(ListaBonosComprados, bocadillo.ListaBonosComprados);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BonoID, CantidadDisponible, NBocadillos, NombreBono, PVP, Tipo, ListaBonosComprados);
        }
    }
}
