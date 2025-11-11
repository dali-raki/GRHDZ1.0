using GrhDz.Domains.Models.Primes;

namespace Infrastructures.Storages.PrimesStorages
{
    public interface IPrimeStorage
    {
        Task Add(PrimeType prime);

        Task<List<PrimeType>> SelectByEmployeIdInMonth(int employeId, DateTime selectedMonth);
    }
}
