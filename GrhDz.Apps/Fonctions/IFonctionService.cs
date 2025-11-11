using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Fonctions;

namespace Implementation.Services.Fonctions
{
    public interface IFonctionService
    {
        Task<Result<List<Fonction>>> GetAllAsync();
        Task<Result<Fonction?>> GetByIdAsync(int fonctionId);
        Task<Result<bool>> AddAsync(Fonction fonction);
        Task<Result<bool>> UpdateAsync(Fonction fonction);
        Task<Result<bool>> DeleteAsync(int fonctionId);
    }
}
