
namespace AppForSEII2526.API.DTOs
{
    public class BocadillosParaResenyaDTO
    {
        public BocadillosParaResenyaDTO(int id, string nombre, float PVP, Tamanyo tamanyo, TipoPan tipoPan)
        {
            Id = id;
            Nombre = nombre;
            this.PVP = PVP;
            Tamanyo = tamanyo;
            TipoPan = tipoPan;

        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public float PVP { get; set; }
        public Tamanyo Tamanyo { get; set; }
        public TipoPan TipoPan { get; set; }


        public override bool Equals(object? obj)
        {
            return obj is BocadillosParaResenyaDTO dTO &&
                   Id == dTO.Id &&
                   Nombre == dTO.Nombre &&
                   PVP == dTO.PVP &&
                   Tamanyo == dTO.Tamanyo &&
                   EqualityComparer<TipoPan>.Default.Equals(TipoPan, dTO.TipoPan);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, PVP, Tamanyo, TipoPan);
        }
    }
}
