

using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Dettes;

namespace GrhDz.Apps.Dettes
{
    public interface IDetteService
    {
        Task<Result<List<Dette>>> GetAllAsync();
        Task<Result<List<Dette>>> GetByEmployeIdAsync(int employeId, DateTime date);
        Task<Result<int>> AddAsync(Dette dette);
        Task<Result<bool>> UpdateAsync(Dette dette);
        Task<Result<bool>> DeleteAsync(int detteId);
        Task<Result<List<PaimentsInfo>>> GetEmployeeDebtDetailsAsync();
        Task<Result<decimal>> GetTotalDettesAsync();
        Task<Result<bool>> UpdateMonthlySalariesAsync();
    }
}
