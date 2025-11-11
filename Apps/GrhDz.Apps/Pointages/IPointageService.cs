using GrhDz.Domains.Models.Pointages;

namespace GrhDz.Apps.Pointages;

public interface IPointageService
{
    Task<List<Pointage>> GetAll();
    Task<IEnumerable<Pointage>> GetByDate(DateTime date);
    Task<Pointage?> GetByIdAndDate(int id, DateOnly date);
    Task Add(Pointage pointage);
    Task Update(Pointage pointage);
    Task Delete(int id);
    Task <List<Pointage>> GetAll_Pointage();
}
