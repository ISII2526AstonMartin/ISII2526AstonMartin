namespace AppForSEII2526.API.Models
{
    public class Compra_Producto
    {
        // Atributos
        [Required]
        public ApplicationUser Usuario { get; set; }
        [Key]
        public string CompraID { get; set; }
        [Required]
        public string Direccion_Envio { get; set; }
        public DateTime FechaCompra { get; set; }
        [Required]
        public MetodoPago Metodo_Pago { get; set; }
        public float PrecioFinal { get; set; }
        //Relaciones
        public List<Producto_Compra> Productos_Compras { get; set; }
    }
    

}
