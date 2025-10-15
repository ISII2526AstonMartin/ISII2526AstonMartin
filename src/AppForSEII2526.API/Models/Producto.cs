namespace AppForSEII2526.API.Models
{
    public class Producto
    {
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
    }
}
