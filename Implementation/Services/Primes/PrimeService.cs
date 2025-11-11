using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Primes;
using Implementation.Services.Prime;
using Infrastructures.Storages.PrimesStorages;
namespace Implementation.Services.Primes
{
    public class PrimeService(PrimeStorage primeStorage) : IPrimeService
    {


        public async Task<Result<bool>> AddAsync(PrimeType prime)
        {
            try
            {
                if (prime == null)
                    return Result.Failure<bool>(Error.Validation("Prime cannot be null."));

                await primeStorage.Add(prime);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<List<PrimeType>>> GetByEmployeIdInMonth(int employeId, DateTime selectedMonth)
        {
            try
            {
                var Primes = await primeStorage.SelectByEmployeIdInMonth(employeId, selectedMonth);
                return Primes;
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }
    }
}
