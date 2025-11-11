using GrhDz.Domains.Models.Employees;
using GrhDz.Domains.Models.Equipe;

namespace GrhDz.Apps.Equipes
{
    public interface IEquipeService
    {
        Task<List<Equipe>> GetAllEquipesAsync();
        Task<Equipe> GetEquipeByIdAsync(int equipeId);
        Task<int> Add(Equipe equipe);
        Task UpdateEquipeAsync(Equipe equipe);
        Task UpdateChefEquipeByIdAsync(int equipeId, int chefEquipeId);
        Task DeleteEquipeAsync(int equipeId);
        Task<List<EquipesInfos>> GetEquipePostesInfoAsync(DateTime selectedDate);
        Task<List<Employee>> GetEmployeesByEquipeIdAsync(int equipeId);
    
    }
}
