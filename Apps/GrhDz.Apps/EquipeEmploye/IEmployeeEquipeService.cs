using GrhDz.Domains.Models.Employees;
using GrhDz.Domains.Models.EmplyeeEquipe;

namespace GrhDz.Apps.EquipeEmploye;


public interface IEmployeeEquipeService
{
    Task<List<EmployeeEquipe>> GetAllEmployeeEquipesAsync();
    Task<EmployeeEquipe> GetEmployeeEquipeByIdAsync(int id);
    Task<int> AddEmployeeEquipeAsync(EmployeeEquipe employeeEquipe);
    Task AddEmployeesToEquipeAsync(int equipeId, List<int> employeeIds);
    Task UpdateEmployeeEquipeAsync(EmployeeEquipe employeeEquipe);
    Task DeleteEmployeeEquipeAsync(int id);
    Task<List<Employee>> GetEmployeesByEquipeIdAsync(int equipeId);
}
