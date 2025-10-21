
namespace AppForSEII2526.API.DTOs
{
    public class BonoBocadillosDTO
    {
        public BonoBocadillosDTO()
        {
        }

        public BonoBocadillosDTO(string bonoID, int cantidadDisponible, int nBocadillos, string nombreBono, float pVP, TipoBocadillo tipo)
        {
            BonoID = bonoID;
            CantidadDisponible = cantidadDisponible;
            NBocadillos = nBocadillos;
            NombreBono = nombreBono;
            PVP = pVP;
            Tipo = tipo;
        }

        public string BonoID { get; set; }

        public int CantidadDisponible { get; set; }

        public int NBocadillos { get; set; }

        public string NombreBono { get; set; }

        public float PVP { get; set; }

        public TipoBocadillo Tipo { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is BonoBocadillosDTO dTO &&
                   BonoID == dTO.BonoID &&
                   CantidadDisponible == dTO.CantidadDisponible &&
                   NBocadillos == dTO.NBocadillos &&
                   NombreBono == dTO.NombreBono &&
                   PVP == dTO.PVP &&
                   EqualityComparer<TipoBocadillo>.Default.Equals(Tipo, dTO.Tipo);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BonoID, CantidadDisponible, NBocadillos, NombreBono, PVP, Tipo);
        }
    }
}
