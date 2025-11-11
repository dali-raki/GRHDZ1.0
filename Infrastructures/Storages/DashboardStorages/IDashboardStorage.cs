using GrhDz.Domains.Models.Dashboards;

namespace Infrastructures.Storages.DashboardStorages
{
    public interface IDashboardStorage
    {
        Task<List<DashboardModel>> GetDashboardDataAsync();
       Task<List<DashboardPointage>> SelectPointageOfDashboard(int year, int month);
        Task<DifferenceofPointage> SelectAbsenceComparison();
        Task<DifferenceofPointage> SelectPresenceComparison();
        Task<int> SelectCountEquipes();
    }
}