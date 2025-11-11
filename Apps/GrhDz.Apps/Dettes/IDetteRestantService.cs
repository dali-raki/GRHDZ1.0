using GrhDz.Domains.Models.Dettes;

namespace GrhDz.Apps.Dettes
{
    public interface IDetteRestantService
    {
        Task<bool> ExisteDettePourEmployeAsync(int employeId);
        Task<List<DetteRestant>> GetDettesRestantesParEmployeAsync(int employeId);
        Task<List<DetteRestant>> GetToutesDettesRestantesAsync();
        Task<DetteRestant?> GetDetteRestanteParIdAsync(int id);
        Task AjouterDetteRestanteAsync(DetteRestant detteRestant);
        Task ModifierDetteRestanteAsync(DetteRestant detteRestant);
        Task SupprimerDetteRestanteAsync(int id);
        Task MontantRetirer(int employeid, decimal montant);
    }
}