using GrhDz.Apps.Dettes;
using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Dettes;
using Infrastructures.Storages.DettesStorages;

namespace Implementation.Services.Dettes
{
    public class DetteRestantService(IDetteRestantStorage detteRestantStorage) : IDetteRestantService
    {
        public async Task<Result<bool>> ExisteDettePourEmployeAsync(int employeId)
        {
            try
            {
                var exists = await detteRestantStorage.ExisteDettePourEmploye(employeId);
                return Result.Success(exists);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<List<DetteRestant>>> GetDettesRestantesParEmployeAsync(int employeId)
        {
            try
            {
                var dettes = await detteRestantStorage.GetByEmployeIdAsync(employeId);
                return Result.Success(dettes);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<DetteRestant>>(Error.Exception(ex));
            }
        }

        public async Task<Result<List<DetteRestant>>> GetToutesDettesRestantesAsync()
        {
            try
            {
                var dettes = await detteRestantStorage.GetAll();
                return Result.Success(dettes);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<DetteRestant>>(Error.Exception(ex));
            }
        }

        public async Task<Result<DetteRestant?>> GetDetteRestanteParIdAsync(int id)
        {
            try
            {
                var dette = await detteRestantStorage.GetById(id);
                return Result.Success(dette);
            }
            catch (Exception ex)
            {
                return Result.Failure<DetteRestant?>(Error.Exception(ex));
            }
        }

        public async Task<Result<bool>> AjouterDetteRestanteAsync(DetteRestant detteRestant)
        {
            try
            {
                await detteRestantStorage.Add(detteRestant);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<bool>> ModifierDetteRestanteAsync(DetteRestant detteRestant)
        {
            try
            {
                await detteRestantStorage.Update(detteRestant);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<bool>> SupprimerDetteRestanteAsync(int id)
        {
            try
            {
                await detteRestantStorage.Delete(id);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<bool>> MontantRetirer(int employeId, decimal montant)
        {
            try
            {
                await detteRestantStorage.MontantRetirer(employeId, montant);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }
    }
}
