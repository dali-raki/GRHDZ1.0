using GrhDz.Domains.Models.Primes;

namespace GrhDz.Apps.Primes
{
    public interface IPrimeService
    {
        Task AddAsync(PrimeType prime);
    }
}
