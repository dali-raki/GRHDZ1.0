using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.SalairesBase;
using GrhDz.Domains.Models.TypeDePaiment;

namespace Implementation.Services.TypeDePaiment
{
    public interface ITypeDePaiementService
    {
        Task<Result<List<TypeDePaiement>>> GetAllAsync();
        Task<Result<TypeDePaiement>> GetByIdAsync(int id);

        Task<Result<int>> AddAsync(TypeDePaiement typeDePaiement);

        Task<Result> UpdateAsync(TypeDePaiement typeDePaiement);

        Task<Result> DeleteAsync(int id);
    }
}
