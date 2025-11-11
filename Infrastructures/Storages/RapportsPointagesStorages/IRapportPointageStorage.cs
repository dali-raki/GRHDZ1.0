using GrhDz.Domains.Models.RapportPointage;

namespace Infrastructures.Storages.RapportsPointagesStorages
{
    public interface IRapportPointageStorage
    {
        Task<List<RapportPointage>> GetAll();
        Task<RapportPointage> GetById(int rapportId);

        Task<int> Add(RapportPointage rapportPointage);

        Task Update(RapportPointage rapportPointage);

        Task Delete(int rapportId);

        Task<int> GetTotalDaysByMonth(int employeId, DateTime monthYear);


    }
}