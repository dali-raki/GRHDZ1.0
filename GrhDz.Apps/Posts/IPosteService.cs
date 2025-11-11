using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.EquipePost;

namespace Implementation.Services.Post
{
	public interface IPosteService
	{

        Task<Result<bool>> InsererDonneesPoste(string idPoste, int idEquipe, DateTime date, List<int> idEmployes);
        Task<Result<(List<EmployePosts> EmployePosts, EquipeSalaires EquipeSalaires)>> GetEquipeSalairesAndPostes(int equipeId, DateTime date);
	}
}
