namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(bocadilloId), nameof(resenyaId))]
    public class ResenyaBocadillo
    {

        public Bocadillo bocadillo { get; set; }
        public int bocadilloId{get; set; }

        [Range(1, 5)]
        public int puntuacion { get; set; }

        public Resenya resenya { get; set; }
        public int resenyaId{get; set; }




    }
}
