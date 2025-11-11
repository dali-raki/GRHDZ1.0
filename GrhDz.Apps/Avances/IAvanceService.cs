using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Avances;


namespace Implementation.Services.Avance
{
    public interface IAvanceService
    {
        Task<Result<List<AvanceModel>>> GetAllAsync();
        Task<Result<AvanceModel>> GetAvanceById(int avanceId);
        Task<Result<List<AvanceModel>>> GeAvanceByEmployeId(int employeId, DateTime date);
        Task<Result<List<AvanceModel>>> GetByDateAsync(DateTime date);
        Task<Result<int>> AddAsync(AvanceModel avance);
        Task<Result<bool>> UpdateAsync(AvanceModel avance);
        Task<Result<bool>> DeleteAsync(int avanceId);
        Task<Result<decimal>> GetTotaleAsync(DateTime date);
        Task<Result<List<AvanceModel>>> GetAvancesWithEmployee(DateTime specificDate);
    }
}
