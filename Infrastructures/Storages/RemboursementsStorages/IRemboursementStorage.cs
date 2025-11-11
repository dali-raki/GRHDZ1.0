using GrhDz.Domains.Models.Remboursements;

namespace Infrastructures.Storages.RemboursementsStorages
{
    public interface IRemboursementStorage
    {
         Task Add(RemboursementType remboursement);
        Task<List<RemboursementType>> GetByEmployeIdInMonth(int employeId, DateTime selectedMonth);
    }
}
