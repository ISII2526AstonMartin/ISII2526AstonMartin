
namespace AppForSEII2526.API.DTOs
{
    public class BonoBocadillosDTO
    {
        public BonoBocadillosDTO()
        {
        }

        public BonoBocadillosDTO(int cantidadDisponible, int nBocadillos, string nombreBono, float pVP, TipoBocadillo tipo)
        {
            CantidadDisponible = cantidadDisponible;
            NBocadillos = nBocadillos;
            NombreBono = nombreBono;
            PVP = pVP;
            Tipo = tipo;
        }

        public int CantidadDisponible { get; set; }

        public int NBocadillos { get; set; }

        public string NombreBono { get; set; }

        public float PVP { get; set; }

        public TipoBocadillo Tipo { get; set; }
    }
}
