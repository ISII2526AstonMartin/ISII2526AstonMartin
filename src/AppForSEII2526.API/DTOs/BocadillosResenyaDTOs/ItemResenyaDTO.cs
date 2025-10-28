
namespace AppForSEII2526.API.DTOs.BocadillosResenyaDTOs
{
    public class ItemResenyaDTO
    {
        public ItemResenyaDTO(int bocadilloID, string BocadilloNombre, int puntuacion, Tamanyo tamanyo, float pvp)
        {
            BocadilloId = bocadilloID;
            this.BocadilloNombre = BocadilloNombre;
            this.puntuacion = puntuacion;
            this.tamanyo = tamanyo;
            this.pvp = pvp;
        }
        public int BocadilloId { get; set; }
        public string BocadilloNombre { get; set; }
        [Required(ErrorMessage = "La puntuación es obligatoria")]
        [Range(1, 10, ErrorMessage = "La puntuación debe estar entre 1 y 10")]
        [Display(Name = "Puntuación")]
        public int puntuacion { get; set; }
        public Tamanyo tamanyo { get; set; }
        public float pvp { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ItemResenyaDTO dTO &&
                   BocadilloId == dTO.BocadilloId &&
                   BocadilloNombre == dTO.BocadilloNombre &&
                   puntuacion == dTO.puntuacion &&
                   tamanyo == dTO.tamanyo &&
                   pvp == dTO.pvp;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BocadilloId, BocadilloNombre, puntuacion, tamanyo, pvp);
        }
    }
}
