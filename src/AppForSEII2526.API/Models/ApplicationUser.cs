using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {
    [Required]
    public string Nombre { get; set; }
    [Required]
    public string Apellido1 { get; set; }
    public string? Apellido2 { get; set; }
    public string? NombreUsuario { get; set; }


    public ApplicationUser() { }
    public ApplicationUser(string nombre, string apellido1, string? apellido2, string? nombreUsuario)
    {
        Nombre = nombre;
        Apellido1 = apellido1;
        Apellido2 = apellido2;
        NombreUsuario = nombreUsuario;
    }
    
}