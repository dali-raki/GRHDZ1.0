using GrhDz.Domains.Models.Dettes;

namespace Infrastructures.Storages.DettesStorages
{
    public interface IDetteStorage
    {
        Task<List<Dette>> GetAll();
        Task<List<Dette>> GetByEmployeIdInMonth(int employeId, DateTime selectedMonth);
        Task<int> Add(Dette dette);
        Task Update(Dette dette);

        Task Delete(int detteId);
        Task<List<PaimentsInfo>> GetEmployeeDebtDetails();
        Task<decimal> GetTotalDettes();
        Task SetMonthlySalaries();
    }
}