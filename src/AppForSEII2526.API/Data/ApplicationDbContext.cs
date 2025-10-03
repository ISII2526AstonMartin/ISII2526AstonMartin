using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
    public DbSet<Producto> Productos { get; set; }
    public DbSet<TipoProducto> TiposProductos { get; set; }
    public DbSet<Compra_Producto> Compras { get; set; }
    public DbSet<Producto_Compra> Productos_Compras { get; set; }
    
    
    public DbSet<BonoBocadillo> BonoBocadillos { get; set; }
    public DbSet<BonosComprados> BonosComprados { get; set; }
    public DbSet<CompraBono> ComprasBono { get; set; }
    public DbSet<TipoBocadillo> TiposBocadillos { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<BonosComprados>().HasKey(bc => new { bc.BonoId, bc.CompraId });
    }
}
