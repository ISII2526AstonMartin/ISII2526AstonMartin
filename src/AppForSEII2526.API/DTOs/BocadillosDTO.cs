
namespace AppForSEII2526.API.DTOs
{
    public class BocadillosDTO
    {

        public string Nombre { get; set; }
        public Tamanyo Tamanyo { get; set; }
        public string TipoPan { get; set; }
        public float PVP { get; set; }
        public int Id { get; set; }

        public BocadillosDTO()
        {

        }  

        public BocadillosDTO(int id, string nombre, Tamanyo tamanyo, string tipoPan, float PVP)
        {
            Id = id;
            Nombre = nombre;
            Tamanyo = tamanyo;
            TipoPan = tipoPan;
            this.PVP = PVP;

        }

        public override bool Equals(object? obj)
        {
            return obj is BocadillosDTO dTO &&
                   Id == dTO.Id &&
                   Nombre == dTO.Nombre &&
                   Tamanyo == dTO.Tamanyo &&
                   TipoPan == dTO.TipoPan &&
                   PVP == dTO.PVP;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, Tamanyo, TipoPan, PVP);
        }
    }
}
