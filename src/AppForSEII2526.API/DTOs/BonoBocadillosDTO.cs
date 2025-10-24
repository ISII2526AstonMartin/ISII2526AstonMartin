

namespace AppForSEII2526.API.DTOs
{
    public class BonoBocadillosDTO
    {
        public BonoBocadillosDTO()
        {
        }

        public BonoBocadillosDTO(string id, int cantidadDisponible, int nBocadillos, string nombreBono, float pVP, string tipo)
        {
            Id = id;
            CantidadDisponible = cantidadDisponible;
            NBocadillos = nBocadillos;
            NombreBono = nombreBono;
            PVP = pVP;
            TipoBocadillo = tipo;
        }

        public string Id { get; set; }

        public int CantidadDisponible { get; set; }

        public int NBocadillos { get; set; }

        public string NombreBono { get; set; }

        public float PVP { get; set; }

        public string TipoBocadillo { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is BonoBocadillosDTO dTO &&
                   Id == dTO.Id &&
                   CantidadDisponible == dTO.CantidadDisponible &&
                   NBocadillos == dTO.NBocadillos &&
                   NombreBono == dTO.NombreBono &&
                   PVP == dTO.PVP &&
                   TipoBocadillo == dTO.TipoBocadillo;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, CantidadDisponible, NBocadillos, NombreBono, PVP, TipoBocadillo);
        }
    }
}
