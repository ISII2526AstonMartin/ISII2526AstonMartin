using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.Models
{
    public class TipoProducto
    {
        public TipoProducto()
        {
        }

        public TipoProducto(string nombre, int productoID, List<Producto> productos)
        {
            Nombre = nombre;
            ProductoID = productoID;
            Productos = productos;
        }

        // Atributos
        public string Nombre { get; set; }
        [Key]
        public int ProductoID { get; set; }
        //Relaciones
        public List<Producto> Productos { get; set; }

        // Comparar solo por claves lógicas; evitar comparar colecciones por referencia.
        public override bool Equals(object? obj)
        {
            return obj is TipoProducto producto &&
                   ProductoID == producto.ProductoID &&
                   string.Equals(Nombre, producto.Nombre, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ProductoID, Nombre);
        }
    }
}