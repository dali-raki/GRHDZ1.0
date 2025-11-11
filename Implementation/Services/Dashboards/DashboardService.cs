using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Dashboards;
using Implementation.Services.Dashboard;
using Infrastructures.Storages.DashboardStorages;

namespace Implementation.Services.Dashboards;

public class DashboardService(IDashboardStorage dashboardStorage) : IDashboardService
{

    public async Task<List<DashboardModel>> GetDashboard()
    {
        try
        {
            return await dashboardStorage.GetDashboardDataAsync();
        }
        catch (Exception exception)
        {
            Console.WriteLine("error", exception);
            throw;
        }
    }

    /*
    public async Task<int> GetCountEquipesAsync()
    {
        try
        {
            int result = await dashboardStorage.SelectCountEquipes();
            return result is 0 
                ? Error.NotFound("countequipe.notfound","number equipe 0") 
                : Result.Success(result);
        }
        catch (Exception ex)
        {
            return Error.Exception(ex);
        }
        return await dashboardStorage.SelectCountEquipes();
    }*/

    public async Task<Result<List<DashboardPointage>>> GetPointageOfDashboardAsync(int year, int month)
    {
        try
        {
            return await dashboardStorage.SelectPointageOfDashboard(year, month);
        }
        catch (Exception exception)
        {
            return Error.Exception(exception);
        }
    }


    public async Task<DifferenceofPointage> GetAbsenceComparisonAsync()
    {
        try
        {
            return  await dashboardStorage.SelectAbsenceComparison();
             
        }
        catch (Exception exception)
        {
            Console.WriteLine("error", exception);
            throw;
        }
    }



    public async Task<DifferenceofPointage> GetPresenceComparisonAsync()
    {
        try
        {
            return  await dashboardStorage.SelectPresenceComparison();
          
        }
        catch (Exception exception)
        {
            Console.WriteLine("error", exception);
            throw;
        }
    }

           public async Task<int> GetCountEquipesAsync()
    {
        try
        {
            return await dashboardStorage.SelectCountEquipes();

        }
        catch (Exception exception)
        {
            Console.WriteLine("error", exception);
            throw;
        }
    }

}