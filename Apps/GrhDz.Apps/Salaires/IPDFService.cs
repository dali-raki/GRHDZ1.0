using GrhDz.Domains.Models.Salaires;

namespace GrhDz.Apps.Salaires
{
    public interface IPDFService
    {
        Task<byte[]> GenerateSalairePDFAsync(SalaireDetail salaireDetail, DateOnly selectedDate);
    }
}
