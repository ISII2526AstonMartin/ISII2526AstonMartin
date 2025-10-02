using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
    public DbSet <Bocadillo> Bocadillo { get; set; }
    public DbSet<TipoPan> TipoPan { get; set; }
    public DbSet<Compra> Compra { get; set; }
    public DbSet<CompraBocadillo> CompraBocadillo { get; set; }


    
}
