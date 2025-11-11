using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Pointages;

namespace Implementation.Services.PointageService
{
    public interface IPointageService
    {
        Task<Result<List<Pointage>>> GetAll();
        Task<Result<List<Pointage>>> GetByDate(DateTime date);
        Task<Result<Pointage?>> GetByIdAndDate(int id, DateOnly date);
        Task<Result<bool>> Add(Pointage pointage);
        Task<Result<bool>> Update(Pointage pointage);
        Task<Result<bool>> Delete(int id);
        Task<Result<List<Pointage>>> GetAll_Pointage();
    }
}
