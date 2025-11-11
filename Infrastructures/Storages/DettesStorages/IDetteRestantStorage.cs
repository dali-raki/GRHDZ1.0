using GrhDz.Domains.Models.Dettes;

namespace Infrastructures.Storages.DettesStorages
{
    public interface IDetteRestantStorage
    {
        Task<bool> ExisteDettePourEmploye(int employeId);
        Task<List<DetteRestant>> GetByEmployeIdAsync(int employeId);
        Task<List<DetteRestant>> GetAll();
        Task<DetteRestant?> GetById(int id);
        Task<DetteRestant?> GetById2(int id);

        Task Add(DetteRestant detteRestant);

        Task Update(DetteRestant detteRestant);

        Task Delete(int id);
        Task MontantRetirer(int employeid, decimal montant);
    }
}