

using GrhDz.Domains.Models.Dettes;

namespace GrhDz.Apps.Dettes
{
    public interface IDetteService
    {
        Task<List<Dette>> GetAllAsync();
        Task<List<Dette>> GetByEmployeIdAsync(int employeId,DateTime date);
        Task<int> AddAsync(Dette dette);
        Task UpdateAsync(Dette dette);
        Task DeleteAsync(int detteId);
        Task<List<PaimentsInfo>> GetEmployeeDebtDetailsAsync();
        Task<decimal> GetTotalDettesAsync();

        Task UpdateMonthlySalariesAsync();
    }
}
