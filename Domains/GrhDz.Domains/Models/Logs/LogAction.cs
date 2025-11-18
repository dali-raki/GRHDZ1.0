namespace GrhDz.Domains.Models.Logs;

public class LogAction
{
    public int Id { get; set; }
    public ActionType ActionType { get; set; }
    public string PerformedBy { get; set; }
    public string Description { get; set; }
    public DateTime ActionDate { get; set; }
}
