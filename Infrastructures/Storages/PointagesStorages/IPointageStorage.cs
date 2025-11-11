using GrhDz.Domains.Models.Pointages;

namespace Infrastructures.Storages.PointagesStorages
{
    public interface IPointageStorage
    {
        Task<List<Pointage>> GetAll();
        Task<List<Pointage>> GetPointagesByDateAsync(DateTime date);
        Task<Pointage?> GetByIdAndDate(int id, DateOnly date);
        Task Add(Pointage pointage);
        Task Update(Pointage pointage);
        Task Delete(int id);
        Task<List<Pointage>> GetAllWithCoefficients();
    }
}