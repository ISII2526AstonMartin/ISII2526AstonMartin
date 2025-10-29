namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(BocadilloId), nameof(ResenyaId))]


    public class ResenyaBocadillo
    {
        public ResenyaBocadillo()
        {
        }
        public ResenyaBocadillo(Bocadillo bocadillo, int puntuacion, Resenya resenya)
        {
            Puntuacion = puntuacion;
            Resenya = resenya;
            ResenyaId = resenya.Id;
            Bocadillo = bocadillo;
            BocadilloId = bocadillo.Id;
        }

        public ResenyaBocadillo(int bocadilloId, int puntuacion, Resenya resenya)
        {
            this.BocadilloId = bocadilloId;
            Puntuacion = puntuacion;
            Resenya = resenya;
            ResenyaId = resenya.Id;
            Bocadillo = null!;

        }

        public Bocadillo Bocadillo { get; set; }
        public int BocadilloId { get; set; }

        [Required]
        [Range(1, 10)]
        public int Puntuacion { get; set; }

        public Resenya Resenya { get; set; }
        public int ResenyaId { get; set; }




    }
}
