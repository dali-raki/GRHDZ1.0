using GrhDz.Domains.Models.Employees;
using GrhDz.Domains.Models.Equipe;
using GrhDz.Domains.Models.EquipePaiment;

namespace Infrastructures.Storages.EquipesStorages
{
    public interface IEquipeStorage
    {
        Task<List<Equipe>> GetAll();
        Task<Equipe> GetById(int equipeId);
        Task<int> Add(Equipe equipe);
        Task Update(Equipe equipe);
        Task UpdateChefEquipeById(int equipeId, int chefEquipeId);
        Task Delete(int equipeId);
        Task<List<EquipesInfos>> GetEquipePostesInfoAsync(DateTime selectedDate);
        Task<List<Employe>> GetEmployeesByEquipeIdAsync(int equipeId);
        Task<List<EquipePaiment>> SelectEquipePaimentandEquipe(int equipeID, DateTime date);
    }
}