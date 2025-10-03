namespace AppForSEII2526.API.Models
{
    public class Compra_Producto
    {
        // Atributos
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        [Key]
        public string CompraID { get; set; }
        public string Direccion_Envio { get; set; }
        public DateTime FechaCompra { get; set; }
        public string Metodo_Pago { get; set; }
        public string Nombre { get; set; }
        public float PrecioFinal { get; set; }
        //Relaciones
        public List<Producto_Compra> Productos_Compras { get; set; }
    }
}
