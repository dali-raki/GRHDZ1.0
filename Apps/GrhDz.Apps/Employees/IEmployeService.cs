using GrhDz.Domains.Models.Dashboards;
using GrhDz.Domains.Models.Employees;

namespace GrhDz.Apps.Employees;

    public interface IEmployeService
{
	Task<List<Employee>> GetEmployeesByStatus(EmployeeStatus status);

    Task<Employee?> GetEmployeeByIdAsync(int id);

	Task AddEmployeAsync(Employee employee);

	Task UpdateEmployeAsync(Employee employee);

	Task SetEmployeAsync(int id, EmployeeStatus status);
	Task ReturnEmployeAsync(int id);

        Task<int> GetTotaleNumberOfEmployeAsync();

	Task<decimal> GetTotaleSalaryForMonthAsync(DateTime month);

	Task<List<Employee>> GetEmployeByFunctionIdAsync (int fonctionId);

	Task<int?> GetEmployeIdByNameAsync(string nom, string prenom, string nomfunction);

	Task<List<CountFunction>> GetEmployeesCountByFunction();



    }
