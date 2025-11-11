using GrhDz.Domains.Models.Fonctions;

namespace Infrastructures.Storages.FonctionsStorages
{
    public interface IFonctionStorage
    {
        Task<List<Fonction>> GetAll();
        Task Add(Fonction fonction);
        Task Update(Fonction fonction);
        Task Delete(int fonctionId);
        Task<Fonction> GetById(int fonctionId);
    }
}