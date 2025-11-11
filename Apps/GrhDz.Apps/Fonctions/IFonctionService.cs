using GrhDz.Domains.Models.Fonctions;

namespace GrhDz.Apps.Fonctions
{
    public interface IFonctionService
    {
        Task<List<Fonction>> GetAllAsync();
        Task<Fonction> GetByIdAsync(int fonctionId);
        Task AddAsync(Fonction fonction);
        Task UpdateAsync(Fonction fonction);
        Task DeleteAsync(int fonctionId);
    }
}
