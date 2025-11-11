using GrhDz.Domains.Models.TypeDePaiment;

namespace Infrastructures.Storages.TypeDePaimentStorages
{
    public interface ITypeDePaiementStorage
    {
        Task<List<TypeDePaiement>> GetAll();

        Task<TypeDePaiement?> GetById(int id);

        Task Add(TypeDePaiement typeDePaiement);

        Task Update(TypeDePaiement typeDePaiement);

        Task Delete(int id);

    }
}