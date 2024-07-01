namespace MonEndoVue.Server.Models;

public class DonneesMedicament
{
    public int Id { get; set; }
    public int CarnetSanteId { get; set; }
    public int MedicamentId { get; set; }
    public DateTime Date { get; set; }
    public string? Commentaire { get; set; }
}