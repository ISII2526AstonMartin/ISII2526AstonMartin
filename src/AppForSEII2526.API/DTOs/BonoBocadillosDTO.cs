namespace AppForSEII2526.API.DTOs
{
    public class BonoBocadillosDTO
    {
        public BonoBocadillosDTO()
        {
        }

        public BonoBocadillosDTO(int id, int nBocadillos, string nombreBono, float pVP, string tipo)
        {
            Id = id;
            NBocadillos = nBocadillos;
            NombreBono = nombreBono;
            PVP = pVP;
            TipoBocadillo = tipo;
        }

        public int Id { get; set; }

        public int NBocadillos { get; set; }

        public string NombreBono { get; set; }

        public float PVP { get; set; }

        public string TipoBocadillo { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is BonoBocadillosDTO dTO &&
                   Id == dTO.Id &&
                   NBocadillos == dTO.NBocadillos &&
                   NombreBono == dTO.NombreBono &&
                   PVP == dTO.PVP &&
                   TipoBocadillo == dTO.TipoBocadillo;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, NBocadillos, NombreBono, PVP, TipoBocadillo);
        }
    }
}
