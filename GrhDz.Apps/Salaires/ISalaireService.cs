using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Salaire;
using GrhDz.Domains.Models.Salaires;

namespace Implementation.Services.Salaire
{
    public interface ISalaireService
    {
        Task<Result<List<SalaireModel>>> GetAllAsync();
        Task<Result<List<SalaireDetail>>> GetSalaireDetails();
        Task<Result<SalaireModel?>> GetByIdAsync(int id);
        Task<Result> AddAsync(SalaireModel salaire);
        Task<Result> UpdateAsync(SalaireModel salaire);
        Task<Result> DeleteAsync(int id);
        Task<Result<List<SalaireDetail>>> GetSalariesByMonthAsync(DateTime mois);
        Task<Result> UpdateDetteAsync(int employeeId, decimal dette, DateTime mois);
        Task<Result<int>> SetMonthlySalariesAsync();
    }
}
