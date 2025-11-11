using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Dashboards;
namespace Implementation.Services.Dashboard;

public interface IDashboardService
{
    Task<List<DashboardModel>> GetDashboard();
    Task<Result<List<DashboardPointage>>> GetPointageOfDashboardAsync(int year, int month);
    Task<DifferenceofPointage> GetPresenceComparisonAsync();
    Task<DifferenceofPointage> GetAbsenceComparisonAsync();
    Task<int> GetCountEquipesAsync();
}