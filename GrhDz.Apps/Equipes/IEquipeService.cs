using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Employees;
using GrhDz.Domains.Models.Equipe;


namespace Implementation.Services.Equipes
{
    public interface IEquipeService
    {
        Task<Result<List<Equipe>>> GetAllEquipesAsync();
        Task<Result<Equipe?>> GetEquipeByIdAsync(int equipeId);
        Task<Result<int>> Add(Equipe equipe);
        Task<Result<bool>> UpdateEquipeAsync(Equipe equipe);
        Task<Result<bool>> UpdateChefEquipeByIdAsync(int equipeId, int chefEquipeId);
        Task<Result<bool>> DeleteEquipeAsync(int equipeId);
        Task<Result<List<EquipesInfos>>> GetEquipePostesInfoAsync(DateTime selectedDate);
        Task<Result<List<Employe>>> GetEmployeesByEquipeIdAsync(int equipeId);

    }
}
