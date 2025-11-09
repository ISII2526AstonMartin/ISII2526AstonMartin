namespace AppForSEII2526.API.Models
{
    public class Compra_Producto
    {
        public Compra_Producto()
        {
        }

        public Compra_Producto(ApplicationUser usuario, string direccion_Envio, DateTime fechaCompra, MetodoPago metodo_Pago, List<Producto_Compra> productos_Compras)
        {
            Usuario = usuario;
            Direccion_Envio = direccion_Envio;
            FechaCompra = fechaCompra;
            Metodo_Pago = metodo_Pago;
            PrecioFinal = 0;
            Productos_Compras = productos_Compras;
        }


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

        public override bool Equals(object? obj)
        {
            return obj is Compra_Producto producto &&
                   EqualityComparer<ApplicationUser>.Default.Equals(Usuario, producto.Usuario) &&
                   CompraID == producto.CompraID &&
                   Direccion_Envio == producto.Direccion_Envio &&
                   FechaCompra == producto.FechaCompra &&
                   Metodo_Pago == producto.Metodo_Pago &&
                   PrecioFinal == producto.PrecioFinal &&
                   EqualityComparer<List<Producto_Compra>>.Default.Equals(Productos_Compras, producto.Productos_Compras);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Usuario, CompraID, Direccion_Envio, FechaCompra, Metodo_Pago, PrecioFinal, Productos_Compras);
        }
    }
}