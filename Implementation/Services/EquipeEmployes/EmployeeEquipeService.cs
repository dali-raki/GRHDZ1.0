using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Employees;
using GrhDz.Domains.Models.EmplyeeEquipe;
using Infrastructures.Storages.EmployeesEquipesStorages;

namespace Implementation.Services.EquipeEmploye
{
    public class EmployeeEquipeService(IEmployeeEquipeStorage employeeEquipeStorage) : IEmployeeEquipeService
    {

        public async Task<Result<List<EmployeeEquipe>>> GetAllEmployeeEquipesAsync()
        {
            try
            {
                var equipes = await employeeEquipeStorage.GetAll();
                return Result.Success(equipes);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<EmployeeEquipe>>(Error.Exception(ex));
            }
        }

        public async Task<Result<EmployeeEquipe?>> GetEmployeeEquipeByIdAsync(int id)
        {
            try
            {
                var equipe = await employeeEquipeStorage.GetById(id);
                return Result.Success(equipe);
            }
            catch (Exception ex)
            {
                return Result.Failure<EmployeeEquipe?>(Error.Exception(ex));
            }
        }

        public async Task<Result<int>> AddEmployeeEquipeAsync(EmployeeEquipe employeeEquipe)
        {
            try
            {
                var id = await employeeEquipeStorage.Add(employeeEquipe);
                return Result.Success(id);
            }
            catch (Exception ex)
            {
                return Result.Failure<int>(Error.Exception(ex));
            }
        }

        public async Task<Result<bool>> AddEmployeesToEquipeAsync(int equipeId, List<int> employeeIds)
        {
            try
            {
                await employeeEquipeStorage.AddEmpolyeesEquipe(equipeId, employeeIds);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<bool>> UpdateEmployeeEquipeAsync(EmployeeEquipe employeeEquipe)
        {
            try
            {
                await employeeEquipeStorage.Update(employeeEquipe);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<bool>> DeleteEmployeeEquipeAsync(int id)
        {
            try
            {
                await employeeEquipeStorage.Delete(id);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<List<Employe>>> GetEmployeesByEquipeIdAsync(int equipeId)
        {
            try
            {
                var employees = await employeeEquipeStorage.GetEmployeesByEquipeId(equipeId);
                return Result.Success(employees);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<Employe>>(Error.Exception(ex));
            }
        }
    }
}
