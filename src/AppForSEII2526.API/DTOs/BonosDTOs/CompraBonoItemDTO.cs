using System.Security.Policy;

namespace AppForSEII2526.API.DTOs.BonosDTOs
{
    public class CompraBonoItemDTO
    {
        public CompraBonoItemDTO() { }

        public CompraBonoItemDTO(string compraBonoId, string nombreBono, float precio, int nBocadillos, string tipoBocadillo, int cantidad)
        {
            this.compraBonoId = compraBonoId;
            this.nombreBono = nombreBono;
            this.precio = precio;
            this.nBocadillos = nBocadillos;
            this.tipoBocadillo = tipoBocadillo;
            this.cantidad = cantidad;
        }

        public string compraBonoId {  get; set; }
        public string nombreBono {  get; set; }
        public float precio { get; set; }
        public int nBocadillos {  get; set; }
        public string tipoBocadillo {  get; set; }
        [Required]
        public int cantidad {  get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CompraBonoItemDTO dTO &&
                   compraBonoId == dTO.compraBonoId &&
                   nombreBono == dTO.nombreBono &&
                   precio == dTO.precio &&
                   nBocadillos == dTO.nBocadillos &&
                   tipoBocadillo == dTO.tipoBocadillo &&
                   cantidad == dTO.cantidad;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(compraBonoId, nombreBono, precio, nBocadillos, tipoBocadillo, cantidad);
        }
    }
}
