

namespace AppForSEII2526.API.Models
{
    public class Producto
    {
        public Producto()
        {

        }

        public Producto(string nombre, string productoID, float pVP, int stock, TipoProducto tipo_Producto, List<Producto_Compra> productos_Compras)
        {
            Nombre = nombre;
            ProductoID = productoID;
            PVP = pVP;
            Stock = stock;
            Tipo_Producto = tipo_Producto;
            Productos_Compras = productos_Compras;
        }


        // Atributos
        [Required]
        public string Nombre { get; set; }
        [Key]
        public string ProductoID { get; set; }
        [Required]
        public float PVP { get; set; }
        [Required]
        public int Stock { get; set; }
        //Relaciones
        public TipoProducto Tipo_Producto { get; set; }
        public List<Producto_Compra> Productos_Compras { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Producto producto &&
                   Nombre == producto.Nombre &&
                   ProductoID == producto.ProductoID &&
                   PVP == producto.PVP &&
                   Stock == producto.Stock &&
                   EqualityComparer<TipoProducto>.Default.Equals(Tipo_Producto, producto.Tipo_Producto) &&
                   EqualityComparer<List<Producto_Compra>>.Default.Equals(Productos_Compras, producto.Productos_Compras);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Nombre, ProductoID, PVP, Stock, Tipo_Producto, Productos_Compras);
        }
    }
}
