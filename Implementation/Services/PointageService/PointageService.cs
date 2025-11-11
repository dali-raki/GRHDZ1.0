using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Pointages;
using Infrastructures.Storages.PointagesStorages;

namespace Implementation.Services.PointageService
{
    public class PointageService(PointageStorage pointageStorage) : IPointageService
    {


        public async Task<Result<List<Pointage>>> GetAll()
        {
            try
            {
                var pointages = await pointageStorage.GetAll();
                return Result.Success(pointages);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<Pointage>>(Error.Exception(ex));
            }
        }

        public async Task<Result<List<Pointage>>> GetByDate(DateTime date)
        {
            try
            {
                var pointages = await pointageStorage.GetPointagesByDateAsync(date);
                return Result.Success(pointages);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<Pointage>>(Error.Exception(ex));
            }
        }

        public async Task<Result<Pointage?>> GetByIdAndDate(int id, DateOnly date)
        {
            try
            {
                var pointage = await pointageStorage.GetByIdAndDate(id, date);
                return Result.Success(pointage);
            }
            catch (Exception ex)
            {
                return Result.Failure<Pointage?>(Error.Exception(ex));
            }
        }

        public async Task<Result<bool>> Add(Pointage pointage)
        {
            try
            {
                if (pointage == null)
                    return Result.Failure<bool>(Error.Validation("Pointage cannot be null."));

                await pointageStorage.Add(pointage);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<bool>> Update(Pointage pointage)
        {
            try
            {
                if (pointage == null)
                    return Result.Failure<bool>(Error.Validation("Pointage cannot be null."));

                await pointageStorage.Update(pointage);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            try
            {
                await pointageStorage.Delete(id);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<List<Pointage>>> GetAll_Pointage()
        {
            try
            {
                var pointages = await pointageStorage.GetAllWithCoefficients();
                return Result.Success(pointages);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<Pointage>>(Error.Exception(ex));
            }
        }
    }
}
