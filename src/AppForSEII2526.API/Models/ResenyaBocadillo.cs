namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(BocadilloId), nameof(ResenyaId))]


    public class ResenyaBocadillo
    {
        /*
        public ResenyaBocadillo(Bocadillo bocadillo, int puntuacion, Resenya resenya)
        {
            Puntuacion = puntuacion;
            Resenya = resenya;
            BocadilloId = bocadilloId;
            ResenyaId = resenya.Id;
        }
        */
        public Bocadillo Bocadillo { get; set; }
        public int BocadilloId { get; set; }

        [Required]
        [Range(1, 10)]
        public int Puntuacion { get; set; }

        public Resenya Resenya { get; set; }
        public int ResenyaId { get; set; }




    }
}
