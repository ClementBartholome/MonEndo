using Microsoft.EntityFrameworkCore;
using MonEndoVue.Server.Models;

namespace MonEndoVue.Server.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<DonneesDouleur> DonneesDouleurs { get; set; }
    public DbSet<DonneesActivitePhysique> DonneesActivitePhysique { get; set; }
    public DbSet<CarnetSante> CarnetSantes { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<DonneesMedicament> DonneesMedicaments { get; set; }

}