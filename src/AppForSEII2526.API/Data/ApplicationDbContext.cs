using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
    public DbSet<Producto> Productos { get; set; }
    public DbSet<TipoProducto> TiposProductos { get; set; }
    public DbSet<Compra_Producto> Compras { get; set; }
    public DbSet<Producto_Compra> Productos_Compras { get; set; }
    
}
