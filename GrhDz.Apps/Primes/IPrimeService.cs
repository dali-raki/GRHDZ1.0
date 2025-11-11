using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Primes;

namespace Implementation.Services.Prime
{
    public interface IPrimeService
    {
        Task<Result<bool>> AddAsync(PrimeType prime);
        Task<Result<List<PrimeType>>> GetByEmployeIdInMonth(int employeId, DateTime selectedMonth);

    }
}
