using MonEndoVue.Server.Models;

namespace MonEndoVue.Server.ViewModels;

public class CarnetHomepageViewModel
{
    public string? UserName { get; set; }
    public int CarnetSanteId { get; set; }
    public List<DonneesDouleur> DonneesDouleur { get; set; }
    public List<DonneesActivitePhysique> DonneesActivitePhysique { get; set; }
}