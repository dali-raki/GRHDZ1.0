using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Avances;
using Implementation.Services.Avance;
using Infrastructures.Storages.AvancesStorages;

namespace Implementation.Services.Avances
{
    public class AvanceService(IAvanceStorage avanceStorage) : IAvanceService
    {
        public async Task<Result<List<AvanceModel>>> GetAllAsync()
        {
            try
            {
                var ravances = await avanceStorage.GetAll();
                return ravances;
            }
            catch (Exception exception)
            {
                return Error.Exception(exception);
            }
        }

        public async Task<Result<AvanceModel>> GetAvanceById(int avanceId)
        {
            try
            {
                var avance = await avanceStorage.GetById(avanceId);

                return avance is null ? Error.NullValue:avance;
            }
            catch (Exception exception)
            {
                return Error.Exception(exception);
            }
        }

        public async Task<Result<List<AvanceModel>>> GeAvanceByEmployeId(int employeId, DateTime date)
        {
            try
            {
                var avances = await avanceStorage.GetByEmployeIdInMonth(employeId, date);
                return avances;
            }
            catch (Exception exception)
            {
                return Error.Exception(exception);
            }
        }

        public async Task<Result<List<AvanceModel>>> GetByDateAsync(DateTime date)
        {
            try
            {
                var avances = await avanceStorage.GetByDate(date);
                return Result.Success(avances);
            }
            catch (Exception exception)
            {
                return Result.Failure<List<AvanceModel>>(Error.Exception(exception));
            }
        }

        public async Task<Result<int>> AddAsync(AvanceModel avance)
        {
            try
            {
                var id = await avanceStorage.Add(avance);
                return Result.Success(id);
            }
            catch (Exception exception)
            {
                return Result.Failure<int>(Error.Exception(exception));
            }
        }

        public async Task<Result<bool>> UpdateAsync(AvanceModel avance)
        {
            try
            {
                await avanceStorage.Update(avance);
                return Result.Success(true);
            }
            catch (Exception exception)
            {
                return Result.Failure<bool>(Error.Exception(exception));
            }
        }

        public async Task<Result<bool>> DeleteAsync(int avanceId)
        {
            try
            {
                await avanceStorage.Delete(avanceId);
                return Result.Success(true);
            }
            catch (Exception exception)
            {
                return Result.Failure<bool>(Error.Exception(exception));
            }
        }

        public async Task<Result<decimal>> GetTotaleAsync(DateTime date)
        {
            try
            {
                var total = await avanceStorage.GetTotale(date);
                return Result.Success(total);
            }
            catch (Exception exception)
            {
                return Result.Failure<decimal>(Error.Exception(exception));
            }
        }

        public async Task<Result<List<AvanceModel>>> GetAvancesWithEmployee(DateTime specificDate)
        {
            try
            {
                var avances = await avanceStorage.GetAvancesWithEmployee(specificDate);
                return Result.Success(avances);
            }
            catch (Exception exception)
            {
                return Result.Failure<List<AvanceModel>>(Error.Exception(exception));
            }
        }
    }
}
