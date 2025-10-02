using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {

    public DbSet<Bocadillo> Bocadillos { get; set; }
    public DbSet<TipoPan> TiposPan { get; set; }
    public DbSet<Resenya> Resenyas { get; set; }
    public DbSet<ResenyaBocadillo> ResenyaBocadillos { get; set; }





}
