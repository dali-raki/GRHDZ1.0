using GrhDz.Domains.Models.EquipePost;

namespace Infrastructures.Storages.PostesStorages
{
    public interface IPosteStorage
    {
        Task InsererDonneesPoste(string idPoste, int idEquipe, DateTime date, List<int> idEmployes);

        Task<(List<EmployePosts> EmployePosts, EquipeSalaires EquipeSalaires)> SelectEquipeSalairesAndPostes(int equipeId, DateTime date);
    }
}