using Microsoft.EntityFrameworkCore;
using MonEndoVue.Server.Data;
using MonEndoVue.Server.Models;
using MonEndoVue.Server.ViewModels;

namespace MonEndoVue.Server.Services;

public class CarnetSanteService
{
    private readonly AppDbContext _context;

    public CarnetSanteService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CarnetSante> GetCarnetSanteId(string userId)
    {
        var carnetSante = await _context.CarnetSantes
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (carnetSante == null)
        {
            throw new Exception("Carnet de santé introuvable");
        }

        return carnetSante;
    }

    public async Task<CarnetViewModel> GetCarnetSanteByUsername(string username)
    {
        var carnetSante = await _context.CarnetSantes
            .Include(c => c.User)
            .Include(c => c.DonneesDouleurs)
            .Include(c => c.DonneesActivitePhysique)
            .Include(c => c.Medicaments)
            .Include(c => c.DonneesMedicaments)
            .FirstOrDefaultAsync(c => c.User.UserName == username);

        if (carnetSante == null)
        {
            throw new Exception("Carnet de santé introuvable");
        }

        return new CarnetViewModel
        {
            UserName = carnetSante.User?.UserName,
            CarnetSanteId = carnetSante.Id,
            DonneesDouleur = carnetSante.DonneesDouleurs,
            DonneesActivitePhysique = carnetSante.DonneesActivitePhysique
        };
    }

    public async Task<CarnetViewModel> GetCarnetSanteById(int carnetSanteId)
    {
        var carnetSante = await _context.CarnetSantes
            .Include(c => c.User)
            .Include(c => c.DonneesDouleurs.OrderBy(d => d.Date))
            .Include(c => c.DonneesActivitePhysique.OrderBy(d => d.Date))
            .Include(c => c.Medicaments)
            .Include(c => c.DonneesMedicaments.OrderBy(d => d.Date))
            .Include(c => c.DonneesTransit.OrderBy(d => d.Date))
            .FirstOrDefaultAsync(c => c.Id == carnetSanteId);

        if (carnetSante == null)
        {
            throw new Exception("Carnet de santé introuvable");
        }

        var donneesMedicamentViewModel = carnetSante.DonneesMedicaments.Select(dm => new DonneesMedicamentViewModel
        {
            Id = dm.Id,
            NomMedicament = carnetSante.Medicaments.FirstOrDefault(m => m.Id == dm.MedicamentId)?.Nom!,
            Date = dm.Date,
            Commentaire = dm.Commentaire
        });

        return new CarnetViewModel
        {
            UserName = carnetSante.User?.UserName,
            CarnetSanteId = carnetSante.Id,
            DonneesDouleur = carnetSante.DonneesDouleurs,
            DonneesActivitePhysique = carnetSante.DonneesActivitePhysique,
            DonneesTransit = carnetSante.DonneesTransit,
            Medicaments = carnetSante.Medicaments,
            DonneesMedicament = donneesMedicamentViewModel
        };
    }


    public async Task<CarnetHomepageViewModel> GetLastEntries(int carnetSanteId)
    {
        var carnetSante = await _context.CarnetSantes
            .Include(c => c.User)
            .Include(c => c.DonneesDouleurs.OrderBy(d => d.Date))
            .Include(c => c.DonneesActivitePhysique.OrderBy(d => d.Date))
            .Include(c => c.DonneesMedicaments.OrderBy(d => d.Date))
            .Include(c => c.DonneesTransit.OrderBy(d => d.Date))
            .FirstOrDefaultAsync(c => c.Id == carnetSanteId);

        if (carnetSante == null)
        {
            throw new Exception("Carnet de santé introuvable");
        }

        var derniereDonneesDouleur = await _context.DonneesDouleurs
            .Where(d => d.CarnetSanteId == carnetSanteId)
            .OrderByDescending(d => d.Date)
            .FirstOrDefaultAsync();

        var derniereDonneesActivitePhysique = await _context.DonneesActivitePhysique
            .Where(d => d.CarnetSanteId == carnetSanteId)
            .OrderByDescending(d => d.Date)
            .FirstOrDefaultAsync();
        
        var derniereDonneesMedicaments = await _context.DonneesMedicaments
            .Where(d => d.CarnetSanteId == carnetSanteId)
            .OrderByDescending(d => d.Date)
            .Include(d => d.Medicament)
            .FirstOrDefaultAsync();
        
        var derniereDonneesTransit = await _context.DonneesTransit
            .Where(d => d.CarnetSanteId == carnetSanteId)
            .OrderByDescending(d => d.Date)
            .FirstOrDefaultAsync();

        return new CarnetHomepageViewModel
        {
            UserName = carnetSante.User?.UserName,
            CarnetSanteId = carnetSante.Id,
            DonneesDouleur = derniereDonneesDouleur,
            DonneesActivitePhysique = derniereDonneesActivitePhysique,
            DonneesMedicament = derniereDonneesMedicaments,
            DonneesTransit = derniereDonneesTransit,
            NomMedicament = derniereDonneesMedicaments?.Medicament?.Nom
        };
    }
}