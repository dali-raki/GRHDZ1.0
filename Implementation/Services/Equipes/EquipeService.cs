using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Employees;
using GrhDz.Domains.Models.Equipe;
using Infrastructures.Storages.EquipesStorages;

namespace Implementation.Services.Equipes
{
    public class EquipeService(IEquipeStorage equipeStorage) : IEquipeService
    {

        public async Task<Result<List<Equipe>>> GetAllEquipesAsync()
        {
            try
            {
                var equipes = await equipeStorage.GetAll();
                return Result.Success(equipes);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<Equipe>>(Error.Exception(ex));
            }
        }

        public async Task<Result<Equipe?>> GetEquipeByIdAsync(int equipeId)
        {
            try
            {
                var equipe = await equipeStorage.GetById(equipeId);
                return Result.Success(equipe);
            }
            catch (Exception ex)
            {
                return Result.Failure<Equipe?>(Error.Exception(ex));
            }
        }

        public async Task<Result<int>> Add(Equipe equipe)
        {
            try
            {
                if (equipe == null)
                    return Result.Failure<int>(Error.Validation("Equipe cannot be null."));

                var id = await equipeStorage.Add(equipe);
                return Result.Success(id);
            }
            catch (Exception ex)
            {
                return Result.Failure<int>(Error.Exception(ex));
            }
        }

        public async Task<Result<bool>> UpdateEquipeAsync(Equipe equipe)
        {
            try
            {
                if (equipe == null)
                    return Result.Failure<bool>(Error.Validation("Equipe cannot be null."));

                await equipeStorage.Update(equipe);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<bool>> UpdateChefEquipeByIdAsync(int equipeId, int chefEquipeId)
        {
            try
            {
                await equipeStorage.UpdateChefEquipeById(equipeId, chefEquipeId);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<bool>> DeleteEquipeAsync(int equipeId)
        {
            try
            {
                await equipeStorage.Delete(equipeId);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<List<EquipesInfos>>> GetEquipePostesInfoAsync(DateTime selectedDate)
        {
            try
            {
                var infos = await equipeStorage.GetEquipePostesInfoAsync(selectedDate);
                return Result.Success(infos);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<EquipesInfos>>(Error.Exception(ex));
            }
        }

        public async Task<Result<List<Employe>>> GetEmployeesByEquipeIdAsync(int equipeId)
        {
            try
            {
                var employees = await equipeStorage.GetEmployeesByEquipeIdAsync(equipeId);
                return Result.Success(employees);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<Employe>>(Error.Exception(ex));
            }
        }
    }
}
