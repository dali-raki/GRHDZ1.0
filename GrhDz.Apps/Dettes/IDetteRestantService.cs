using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Dettes;

namespace GrhDz.Apps.Dettes
{
    public interface IDetteRestantService
    {
        Task<Result<bool>> ExisteDettePourEmployeAsync(int employeId);
        Task<Result<List<DetteRestant>>> GetDettesRestantesParEmployeAsync(int employeId);
        Task<Result<List<DetteRestant>>> GetToutesDettesRestantesAsync();
        Task<Result<DetteRestant?>> GetDetteRestanteParIdAsync(int id);
        Task<Result<bool>> AjouterDetteRestanteAsync(DetteRestant detteRestant);
        Task<Result<bool>> ModifierDetteRestanteAsync(DetteRestant detteRestant);
        Task<Result<bool>> SupprimerDetteRestanteAsync(int id);
        Task<Result<bool>> MontantRetirer(int employeId, decimal montant);
    }
}