using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Dashboards;
using GrhDz.Domains.Models.Employees;

namespace Implementation.Services.EmployeModel
{
    public interface IEmployeService
	{
        Task<Result<List<Employe>>> GetEmployeesByStatus(EmployeeStatus status);
        Task<Result<Employe?>> GetEmployeeByIdAsync(int id);
        Task<Result<bool>> AddEmployeAsync(Employe employee);
        Task<Result<bool>> UpdateEmployeAsync(Employe employee);
        Task<Result<bool>> ReturnEmployeAsync(int id);
        Task<Result<int>> GetTotaleNumberOfEmployeAsync();
        Task<Result<decimal>> GetTotaleSalaryForMonthAsync(DateTime month);
        Task<Result<List<Employe>>> GetEmployeByFunctionIdAsync(int fonctionId);
        Task<Result<int?>> GetEmployeIdByNameAsync(string nom, string prenom, string nomfunction);
        Task<Result<List<CountFunction>>> GetEmployeesCountByFunction();
        Task<Result<bool>> SetEmployeAsync(int id, EmployeeStatus status);


    }
}
