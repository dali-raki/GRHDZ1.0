using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.SalairesBase;

namespace Implementation.Services.SalaireBase
{
    public interface ISalaireBaseService
    {
        Task<Result<List<SalairesBase>>> GetAll();
        Task<Result<SalairesBase>> GetById(int idSalaireBase);
        Task<Result<List<SalairesBase>>> GetByEmployeeId(int employeeId);
        Task<Result<int>> Add(SalairesBase salairesBase);
        Task<Result> Update(SalairesBase salairesBase);
        Task<Result> Delete(int idSalaireBase);
    }
}
