namespace GrhDz.Domains.Models.Avances;

public class AvanceModel
{
    public int AvanceID { get; set; }
    public DateTime Date { get; set; }
    public int EmployeID { get; set; }
    public string Description { get; set; }
    public decimal Montant { get; set; }
    public string NomEmployee { get; set; }
    public string PrenomEmployee { get; set; }
    
    public AvanceStatus Status { get; set; } = AvanceStatus.Draft;
}
