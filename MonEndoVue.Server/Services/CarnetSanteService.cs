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
    
    public async Task<CarnetHomepageViewModel> GetCarnetSanteByUsername(string username)
    {
        var carnetSante = await _context.CarnetSantes
            .Include(c => c.User)
            .Include(c => c.DonneesDouleurs)
            .Include(c => c.DonneesActivitePhysique)
            .FirstOrDefaultAsync(c => c.User.UserName == username);
        
        if (carnetSante == null)
        {
            throw new Exception("Carnet de santé introuvable");
        }
        
        return new CarnetHomepageViewModel
        {
            UserName = carnetSante.User?.UserName,
            CarnetSanteId = carnetSante.Id,
            DonneesDouleur = carnetSante.DonneesDouleurs,
            DonneesActivitePhysique = carnetSante.DonneesActivitePhysique
        };
    }

    public async Task<CarnetHomepageViewModel> GetCarnetSanteById(int carnetSanteId)
    {
        var carnetSante = await _context.CarnetSantes
            .Include(c => c.User)
            .Include(c => c.DonneesDouleurs.OrderBy(d => d.Date))
            .Include(c => c.DonneesActivitePhysique.OrderBy(d => d.Date))
            .FirstOrDefaultAsync(c => c.Id == carnetSanteId);
    
        if (carnetSante == null)
        {
            throw new Exception("Carnet de santé introuvable");
        }
    
        return new CarnetHomepageViewModel
        {
            UserName = carnetSante.User?.UserName,
            CarnetSanteId = carnetSante.Id,
            DonneesDouleur = carnetSante.DonneesDouleurs,
            DonneesActivitePhysique = carnetSante.DonneesActivitePhysique
        };
    }
}