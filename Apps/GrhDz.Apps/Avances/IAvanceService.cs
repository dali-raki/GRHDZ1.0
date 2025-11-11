using GrhDz.Domains.Models.Avances;

namespace GrhDz.Apps.Avances;


public interface IAvanceService
{
    Task<List<Avance>> GetAllAsync();
    Task<Avance> GetByIdAsync(int avanceId);
    Task<List<Avance>> GetByEmployeIdAsync(int employeId, DateTime date);
    Task<List<Avance>> GetByDateAsync(DateTime date);
    Task<int> AddAsync(Avance avance);
    Task UpdateAsync(Avance avance);
    Task DeleteAsync(int avanceId);
    Task<decimal> GetTotaleAsync(DateTime date);
    Task<List<Avance>> GetAvancesWithEmployee(DateTime specificDate);
}
