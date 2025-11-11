using GrhDz.Domains.Models.Remboursements;

namespace GrhDz.Apps.Remboursements
{
    public interface IRemboursementService
    {
        Task AddAsync(RemboursementType remboursement);
        Task<List<RemboursementType>> SelectByEmployeIdInMonthasync(int employeId, DateTime selectedMonth);
    }
}