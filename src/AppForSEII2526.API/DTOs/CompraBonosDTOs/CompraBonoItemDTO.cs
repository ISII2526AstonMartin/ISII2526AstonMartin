using System.Security.Policy;

namespace AppForSEII2526.API.DTOs.CompraBonosDTOs
{
    public class CompraBonoItemDTO
    {
        public CompraBonoItemDTO()
        {
        }

        public CompraBonoItemDTO(string id, float pvp, int nBocadillos, string nombreBono, string tipoBocadillo, int cantidad)
        {
            this.id=id;
            this.pvp = pvp;
            this.nBocadillos = nBocadillos;
            this.nombreBono = nombreBono;
            this.tipoBocadillo = tipoBocadillo;
            this.cantidad = cantidad;
        }

        public string id { get; set; }
        public float pvp { get; set; }
        public int nBocadillos { get; set; }
        public string nombreBono { get; set; }
        public string tipoBocadillo { get; set; }
        [Required]
        public int cantidad { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CompraBonoItemDTO dTO &&
                   id == dTO.id &&
                   pvp == dTO.pvp &&
                   nBocadillos == dTO.nBocadillos &&
                   nombreBono == dTO.nombreBono &&
                   tipoBocadillo == dTO.tipoBocadillo &&
                   cantidad == dTO.cantidad;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id, pvp, nBocadillos, nombreBono, tipoBocadillo, cantidad);
        }
    }
}
