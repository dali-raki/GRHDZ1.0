using GrhDz.Domains.Models.EquipePost;

namespace GrhDz.Apps.Posts
{
	public interface IPosteService
	{
		
		Task InsererDonneesPoste(string numeroPoste, int idEquipe, DateTime date, List<int> idEmployes);
		Task<(List<EmployePosts> EmployePosts, EquipeSalaires EquipeSalaires)>GetEquipeSalairesAndPostes(int equipeId, DateTime date);
	}
}
