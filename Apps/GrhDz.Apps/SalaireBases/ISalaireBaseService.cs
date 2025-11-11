using GrhDz.Domains.Models.SalairesBase;

namespace GrhDz.Apps.SalaireBases
{
    public interface ISalaireBaseService
    {
        Task<List<SalairesBase>> GetAll();
        Task<SalairesBase> GetById(int idSalaireBase);
        Task<List<SalairesBase>> GetByEmployeeId(int employeeId);
        Task<int> Add(SalairesBase salairesBase);
        Task Update(SalairesBase salairesBase);
        Task Delete(int idSalaireBase);
    }
}
