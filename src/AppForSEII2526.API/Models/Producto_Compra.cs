

namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(CompraID),nameof(ProductoID))]
    public class Producto_Compra
    {
        public Producto_Compra()
        {
        }

        public Producto_Compra(int cantidad, string compraID, string productoID, float pVP, Producto producto, Compra_Producto compra)
        {
            Cantidad = cantidad;
            CompraID = compraID;
            ProductoID = productoID;
            PVP = pVP;
            Producto = producto;
            Compra = compra;
        }



        // Atributos
        [Required]
        public int Cantidad { get; set; }
        public string CompraID { get; set; }
        public string ProductoID { get; set; }
        [Required]
        public float PVP { get; set; }
        //Relaciones
        public Producto Producto { get; set; }
        public Compra_Producto Compra { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Producto_Compra compra &&
                   Cantidad == compra.Cantidad &&
                   CompraID == compra.CompraID &&
                   ProductoID == compra.ProductoID &&
                   PVP == compra.PVP &&
                   EqualityComparer<Producto>.Default.Equals(Producto, compra.Producto) &&
                   EqualityComparer<Compra_Producto>.Default.Equals(Compra, compra.Compra);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Cantidad, CompraID, ProductoID, PVP, Producto, Compra);
        }
    }
}
