namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(CompraID),nameof(ProductoID))]
    public class Producto_Compra
    {
        // Atributos
        public int Cantidad { get; set; }
        public string CompraID { get; set; }
        public string ProductoID { get; set; }
        public float PVP { get; set; }
        //Relaciones
        public Producto Producto { get; set; }
        public Compra_Producto Compra { get; set; }
    }
}
