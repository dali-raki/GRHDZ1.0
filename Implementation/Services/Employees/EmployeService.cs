using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Dashboards;
using GrhDz.Domains.Models.Employees;
using Implementation.Services.EmployeModel;
using Infrastructures.Storages.EmployeesStorages;

namespace Implementation.Services.Employees
{
    public class EmployeService(IEmployeStorage employeStorage) : IEmployeService
    {
        public async Task<Result<List<Employe>>> GetEmployeesByStatus(EmployeeStatus status)
        {
            try
            {
                var employees = await employeStorage.SelectEmployeesByStatus(status);
                return Result.Success(employees);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<Employe>>(Error.Exception(ex));
            }
        }

        public async Task<Result<List<CountFunction>>> GetEmployeesCountByFunction()
        {
            try
            {
                var counts = await employeStorage.SelectEmployeesCountByFunction();
                return Result.Success(counts);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<CountFunction>>(Error.Exception(ex));
            }
        }

        public async Task<Result<Employe?>> GetEmployeeByIdAsync(int id)
        {
            try
            {
                var employee = await employeStorage.GetById(id);
                return Result.Success(employee);
            }
            catch (Exception ex)
            {
                return Result.Failure<Employe?>(Error.Exception(ex));
            }
        }

        public async Task<Result<bool>> AddEmployeAsync(Employe employee)
        {
            try
            {
                await employeStorage.Add(employee);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<bool>> UpdateEmployeAsync(Employe employee)
        {
            try
            {
                await employeStorage.Update(employee);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<bool>> SetEmployeAsync(int id, EmployeeStatus status)
        {
            try
            {
                await employeStorage.UpdateStatusofEmploye(id, status);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<bool>> ReturnEmployeAsync(int id)
        {
            try
            {
                var exists = await employeStorage.GetById(id) != null;
                return Result.Success(exists);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<int>> GetTotaleNumberOfEmployeAsync()
        {
            try
            {
                var total = await employeStorage.GetTotalNumberOfEmployees();
                return Result.Success(total);
            }
            catch (Exception ex)
            {
                return Result.Failure<int>(Error.Exception(ex));
            }
        }

        public async Task<Result<decimal>> GetTotaleSalaryForMonthAsync(DateTime month)
        {
            try
            {
                var total = await employeStorage.GetTotalSalaryForMonth(month);
                return Result.Success(total);
            }
            catch (Exception ex)
            {
                return Result.Failure<decimal>(Error.Exception(ex));
            }
        }

        public async Task<Result<List<Employe>>> GetEmployeByFunctionIdAsync(int fonctionId)
        {
            try
            {
                var employees = await employeStorage.GetEmployeesByFunctionId(fonctionId);
                return Result.Success(employees);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<Employe>>(Error.Exception(ex));
            }
        }

        public async Task<Result<int?>> GetEmployeIdByNameAsync(string nom, string prenom, string nomFonction)
        {
            try
            {
                var id = await employeStorage.GetEmployeeIdByName(nom, prenom, nomFonction);
                return Result.Success(id);
            }
            catch (Exception ex)
            {
                return Result.Failure<int?>(Error.Exception(ex));
            }
        }
    }
}
