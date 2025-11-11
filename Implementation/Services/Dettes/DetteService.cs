using GrhDz.Apps.Dettes;
using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Dettes;
using Infrastructures.Storages.DettesStorages;

namespace Implementation.Services.Dettes
{
    public class DetteService(IDetteStorage detteStorage) : IDetteService
    {
        public async Task<Result<List<Dette>>> GetAllAsync()
        {
            try
            {
                var dettes = await detteStorage.GetAll();
                return Result.Success(dettes);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<Dette>>(Error.Exception(ex));
            }
        }

        public async Task<Result<List<Dette>>> GetByEmployeIdAsync(int employeId, DateTime date)
        {
            try
            {
                var dettes = await detteStorage.GetByEmployeIdInMonth(employeId, date);
                return Result.Success(dettes);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<Dette>>(Error.Exception(ex));
            }
        }

        public async Task<Result<int>> AddAsync(Dette dette)
        {
            try
            {
                var id = await detteStorage.Add(dette);
                return Result.Success(id);
            }
            catch (Exception ex)
            {
                return Result.Failure<int>(Error.Exception(ex));
            }
        }

        public async Task<Result<bool>> UpdateAsync(Dette dette)
        {
            try
            {
                await detteStorage.Update(dette);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<bool>> DeleteAsync(int detteId)
        {
            try
            {
                await detteStorage.Delete(detteId);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<List<PaimentsInfo>>> GetEmployeeDebtDetailsAsync()
        {
            try
            {
                var details = await detteStorage.GetEmployeeDebtDetails();
                return Result.Success(details);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<PaimentsInfo>>(Error.Exception(ex));
            }
        }

        public async Task<Result<decimal>> GetTotalDettesAsync()
        {
            try
            {
                var total = await detteStorage.GetTotalDettes();
                return Result.Success(total);
            }
            catch (Exception ex)
            {
                return Result.Failure<decimal>(Error.Exception(ex));
            }
        }

        public async Task<Result<bool>> UpdateMonthlySalariesAsync()
        {
            try
            {
                await detteStorage.SetMonthlySalaries();
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }
    }
}
