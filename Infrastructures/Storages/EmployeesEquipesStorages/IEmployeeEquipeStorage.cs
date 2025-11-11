using GrhDz.Domains.Models.Employees;
using GrhDz.Domains.Models.EmplyeeEquipe;

namespace Infrastructures.Storages.EmployeesEquipesStorages
{
    public interface IEmployeeEquipeStorage
    {
        Task<List<EmployeeEquipe>> GetAll();
        Task<EmployeeEquipe> GetById(int employeeEquipeId);


        Task<int> Add(EmployeeEquipe employeeEquipe);

        Task AddEmpolyeesEquipe(int equipeId, List<int> employeeIds);
        Task Update(EmployeeEquipe employeeEquipe);
        Task Delete(int employeeEquipeId);
        Task<List<Employe>> GetEmployeesByEquipeId(int equipeId);
    }
}