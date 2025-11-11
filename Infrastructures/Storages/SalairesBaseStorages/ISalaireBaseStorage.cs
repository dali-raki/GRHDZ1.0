using GrhDz.Domains.Models.SalairesBase;

namespace Infrastructures.Storages.SalairesBaseStorages
{
    public interface ISalaireBaseStorage
    {
        Task<List<SalairesBase>> GetAll();
        Task<SalairesBase> GetById(int idSalaireBase);

        Task<List<SalairesBase>> GetByEmployeeId(int employeeId);

        Task<int> Add(SalairesBase salairesBase);

        Task Update(SalairesBase salairesBase);

        Task Delete(int idSalaireBase);



    }
}