using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.EquipePost;
using Implementation.Services.Post;
using Infrastructures.Storages.PostesStorages;

namespace Implementation.Services.Posts
{
	public class PosteService(PosteStorage posteStorage) : IPosteService
	{

        public async Task<Result<bool>> InsererDonneesPoste(string idPoste, int idEquipe, DateTime date, List<int> idEmployes)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(idPoste))
                    return Result.Failure<bool>(Error.Validation("idPoste cannot be null or empty."));

                if (idEmployes == null || idEmployes.Count == 0)
                    return Result.Failure<bool>(Error.Validation("The list of employees cannot be empty."));

                await posteStorage.InsererDonneesPoste(idPoste, idEquipe, date, idEmployes);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(Error.Exception(ex));
            }
        }

        public async Task<Result<(List<EmployePosts> EmployePosts, EquipeSalaires EquipeSalaires)>> GetEquipeSalairesAndPostes(int equipeId, DateTime date)
        {
            try
            {
                var result = await posteStorage.SelectEquipeSalairesAndPostes(equipeId, date);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure<(List<EmployePosts>, EquipeSalaires)>(Error.Exception(ex));
            }
        }

    }
}
