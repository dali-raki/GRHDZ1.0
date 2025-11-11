
using GrhDz.Domains.Models.Salaires;

namespace Implementation.Services.Salaire
{
    public interface IPDFService
    {
        Task<byte[]> GenerateSalairePDFAsync(SalaireDetail salaireDetail, DateOnly selectedDate);
    }
}
