﻿using MonEndoVue.Server.Data;

namespace MonEndoVue.Server.Models;

public class CarnetSante
{
    public int Id { get; set; }
    public string UserId { get; set; }
    
    public ApplicationUser? User { get; set; }
    public List<DonneesDouleur> DonneesDouleurs { get; set; }
    public List<DonneesActivitePhysique> DonneesActivitePhysique { get; set; }
}