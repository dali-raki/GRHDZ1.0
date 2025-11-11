using GrhDz.Domains.Models.TypeDePaiment;

namespace GrhDz.Apps.TypeDePaiments
{
    public interface ITypeDePaiementService
    {
        Task<List<TypeDePaiement>> GetAllAsync();
      
    }
}
