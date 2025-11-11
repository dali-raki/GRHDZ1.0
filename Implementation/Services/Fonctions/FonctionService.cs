using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Fonctions;
using Infrastructures.Storages.FonctionsStorages;

namespace Implementation.Services.Fonctions
{
    public class FonctionService(IFonctionStorage fonctionStorage) : IFonctionService
    {

        public async Task<Result<List<Fonction>>> GetAllAsync()
        {
            try
            {
                var fonctions = await fonctionStorage.GetAll();
                return Result.Success(fonctions);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<Fonction>>(Error.Exception(ex));
            }
        }

        public async Task<Result<Fonction?>> GetByIdAsync(int fonctionId)
        {
            try
            {
                var fonction = await fonctionStorage.GetById(fonctionId);
                return Result.Success(fonction);
            }
            catch (Exception ex)
            {
                return Result.Failure<Fonction?>(Error.Exception(ex));
            }
        }

        public async Task<Result<bool>> AddAsync(Fonction fonction)
        {
            try
            {
                if (fonction == null)
                    return Result.Failure<bool>(Error.Validation("Fonction cannot be null."));

                await fonctionStorage.Add(fonction);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<bool>> UpdateAsync(Fonction fonction)
        {
            try
            {
                if (fonction == null)
                    return Result.Failure<bool>(Error.Validation("Fonction cannot be null."));

                await fonctionStorage.Update(fonction);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<bool>> DeleteAsync(int fonctionId)
        {
            try
            {
                await fonctionStorage.Delete(fonctionId);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }
    }
}
