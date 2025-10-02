namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(BocadilloId), nameof(ResenyaId))]
    public class ResenyaBocadillo
    {

        public Bocadillo Bocadillo { get; set; }
        public int BocadilloId{get; set; }

        [Range(1, 5)]
        public int Puntuacion { get; set; }

        public Resenya Resenya { get; set; }
        public int ResenyaId{get; set; }




    }
}
