
using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Employees;
using GrhDz.Domains.Models.EmplyeeEquipe;

namespace Implementation.Services.EquipeEmploye
{
    public interface IEmployeeEquipeService
    {
        Task<Result<List<EmployeeEquipe>>> GetAllEmployeeEquipesAsync();
        Task<Result<EmployeeEquipe?>> GetEmployeeEquipeByIdAsync(int id);
        Task<Result<int>> AddEmployeeEquipeAsync(EmployeeEquipe employeeEquipe);
        Task<Result<bool>> AddEmployeesToEquipeAsync(int equipeId, List<int> employeeIds);
        Task<Result<bool>> UpdateEmployeeEquipeAsync(EmployeeEquipe employeeEquipe);
        Task<Result<bool>> DeleteEmployeeEquipeAsync(int id);
        Task<Result<List<Employe>>> GetEmployeesByEquipeIdAsync(int equipeId);
    }
}
