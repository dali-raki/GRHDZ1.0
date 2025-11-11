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

    }
}
