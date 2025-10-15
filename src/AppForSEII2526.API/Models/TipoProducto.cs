namespace AppForSEII2526.API.Models
{
    public class TipoProducto
    {
        // Atributos
        public string Nombre { get; set; }
        [Key]
        public string ProductoID { get; set; }
        //Relaciones
        public List<Producto> Productos { get; set; }

    }
}
