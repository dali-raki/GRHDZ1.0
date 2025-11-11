using GrhDz.Domains.Models.Avances;

namespace Infrastructures.Storages.AvancesStorages
{
    public interface IAvanceStorage
    {

        Task<List<AvanceModel>> GetByEmployeIdInMonth(int employeId, DateTime selectedMonth);

        Task<List<AvanceModel>> GetAll();

        Task<AvanceModel?> GetById(int avanceId);
        Task<int> Add(AvanceModel avanceModel);

        Task Update(AvanceModel avanceModel);
        Task Delete(int avanceId);
        Task<List<AvanceModel>> GetByDate(DateTime date);
        Task<decimal> GetTotale(DateTime date);
        Task<List<AvanceModel>> GetAvancesWithEmployee(DateTime specificDate);
    }
}