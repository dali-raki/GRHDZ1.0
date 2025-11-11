using GrhDz.Domains.Models.Salaire;
using GrhDz.Domains.Models.Salaires;

namespace Infrastructures.Storages.SalairesStorages
{
    public interface ISalaireStorage
    {
        Task<List<SalaireModel>> GetAll();
        Task<SalaireModel?> GetById(int id);
        Task Add(SalaireModel salaire);
        Task Update(SalaireModel salaire);
        Task Delete(int id);
        Task<List<SalaireDetail>> SelectSalariesByMonth(DateTime mois);
        Task UpdateDette(int employeeid, decimal dette, DateTime mois);

        Task<int> InsertMonthlySalaries();

    }
}