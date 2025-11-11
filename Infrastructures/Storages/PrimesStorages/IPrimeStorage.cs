using GrhDz.Domains.Models.Primes;

namespace Infrastructures.Storages.PrimesStorages
{
    public interface IPrimeStorage
    {
        Task Add(PrimeType prime);
    }
}
