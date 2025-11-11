using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Remboursements;
namespace Implementation.Services.Remboursement
{
    public interface IRemboursementService
    {
        Task<Result<bool>> AddAsync(RemboursementType remboursement);
        Task<Result<List<RemboursementType>>> GetByEmployeIdInMonthAsync(int employeId, DateTime selectedMonth);
    }
}