using GrhDz.Domains.Models.Dashboards;
using GrhDz.Domains.Models.Employees;

namespace Infrastructures.Storages.EmployeesStorages
{
    public interface IEmployeStorage
    {
        Task<int> SelectCountEquipes();
        Task<Employe?> GetById(int id);

        Task Add(Employe employe);
        Task Update(Employe employe);
        Task UpdateStatusofEmploye(int id , EmployeeStatus status);
        Task<int> GetTotalNumberOfEmployees();
        Task<decimal> GetTotalSalaryForMonth(DateTime month);
        Task<List<Employe>> GetEmployeesByFunctionId(int fonctionId);
        Task<int?> GetEmployeeIdByName(string nom, string prenom, string nomFonction);
        //Task<List<Employee>> GetEmployeesBystatus(EmployeeStatus status);
        Task<List<CountFunction>> SelectEmployeesCountByFunction();

        Task<List<Employe>> SelectEmployeesByStatus(EmployeeStatus status);
    }
}