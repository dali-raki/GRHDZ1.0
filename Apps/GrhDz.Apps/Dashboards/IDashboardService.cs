using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Dashboards;

namespace GrhDz.Apps.Dashboards;

public interface IDashboardService
{
    Task<List<DashboardModel>> GetDashboard();
    ValueTask<Result<List<DashboardPointage>>> GetPointageOfDashboardAsync(int year, int month);
    Task<DifferenceofPointage> GetPresenceComparisonAsync();
    Task<DifferenceofPointage> GetAbsenceComparisonAsync();
    Task<int> GetCountEquipesAsync();
    ValueTask<Result<int>> GetCountEquipes();
}