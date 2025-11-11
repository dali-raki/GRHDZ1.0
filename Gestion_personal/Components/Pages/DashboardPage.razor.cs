using Gestion_personal.Components.Layout.Employes;
using GrhDz.Apps.Avances;
using GrhDz.Apps.Dettes;
using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Dashboards;
using GrhDz.Domains.Models.Employees;
using GrhDz.Domains.Models.Logs;
using Implementation.Services.Avance;
using Implementation.Services.Dashboard;
using Implementation.Services.EmployeModel;
using Implementation.Services.LogsAction;
using Implementation.Services.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using Radzen.Blazor;


namespace Gestion_personal.Components.Pages
{
    public partial class DashboardPage
    {
        [Inject] UserSessionStateService UserSession { get; set; } 
        [Inject] NavigationManager Nav { get; set; }
        [Inject] public IEmployeService EmployeService { get; set; }
        [Inject] public IDetteService DetteService { get; set; }
        [Inject] public IAvanceService AvanceService { get; set; }
        [Inject] public IDashboardService DashboardService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] private ProtectedSessionStorage SessionStorage { get; set; }
        [Inject] private ILogsActionService logsActionService { get; set; }
        private int SelectedYear => Dashboards?.FirstOrDefault()?.Year ?? DateTime.Now.Year;
        public int Total_Number_Employe;
        private Result<int> Total_Number_Employe_r;
        public Result<decimal> Totale_Dargent;
        public decimal Total_Dette;
        public decimal Total_Avance;
        private string searchTerm = string.Empty;
        private DateTime selectedDate = DateTime.Today;
        private RadzenDataGrid<DashboardPointage> grid;
        DifferenceofPointage presenceComparison;
        DifferenceofPointage absenceComparison;
        double presencePercentage;
        double absencePercentage;
        int countEquipe;
        private int employeeToReturn;
        public List<DashboardModel> Dashboards { get; set; }
        public Result<List<CountFunction>> countfunctionr { get; set; }
        public List<CountFunction> countfunction;
        private Result<List<Employe>> employees;
        private List<Employe> filteredEmployees;

        private RadzenDataGrid<LogAction> grid2;
        private List<LogAction> logs = new();
        private string searchTerm3 = string.Empty;
        int selectedIndex = 0;



        private List<DashboardPointage> ListPointage;
        public string datapop;
        private bool isSuccessPopupVisible = false;


        private SuccessPopup successTost;
        public class TotalItem
        {
            public string Label { get; set; }
            public decimal Value { get; set; }
        }
        private bool showDataLabels { get; set; } = true;
        List<TotalItem> totals => new()
        {
            new TotalItem { Label = "Dette", Value = Total_Dette },
            new TotalItem { Label = "AvanceModel", Value = Total_Avance }
        };
      

    
        private IEnumerable<DashboardPointage> filteredPointage;
        public IEnumerable<DashboardPointage> FilteredPointage =>
            string.IsNullOrWhiteSpace(searchTerm)
                ? (filteredPointage ?? ListPointage)
                : (filteredPointage ?? ListPointage).Where(p =>
                    (p.NomComplet?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    p.EmployeID.ToString().Contains(searchTerm)
                );
        

        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(UserSession.UserId.ToString()))
            {
                Nav.NavigateTo("/", forceLoad: true);
            }
            Total_Number_Employe_r = await EmployeService.GetTotaleNumberOfEmployeAsync();
            Total_Number_Employe = Total_Number_Employe_r.Value;
            Totale_Dargent = await EmployeService.GetTotaleSalaryForMonthAsync(DateTime.Now);
            //Total_Dette = await DetteService.GetTotalDettesAsync();
            //Total_Avance = await AvanceService.GetTotaleAsync(DateTime.Now);
            //Dashboards = await DashboardService.GetDashboard();
            countfunctionr = await EmployeService.GetEmployeesCountByFunction();
            countfunction = countfunctionr.Value;
            presenceComparison = await DashboardService.GetPresenceComparisonAsync();
            absenceComparison = await DashboardService.GetAbsenceComparisonAsync();
            var result = await DashboardService.GetPointageOfDashboardAsync(selectedDate.Year, selectedDate.Month);
            filteredPointage = result.Value;
            if (presenceComparison != null && Total_Number_Employe > 0)
            {
                presencePercentage = (presenceComparison.Difference * 100) / (Total_Number_Employe * 26);
            }
            else
            {
                presencePercentage = 0;
            }
            if (absenceComparison != null && Total_Number_Employe > 0)
            {
                absencePercentage = (absenceComparison.Difference * 100) / (Total_Number_Employe * 26);
            }
            else
            {
                absencePercentage = 0;
            }

            var countEquipeResult = await DashboardService.GetCountEquipesAsync();
            countEquipe = countEquipeResult;
            employees = await EmployeService.GetEmployeesByStatus(EmployeeStatus.Active);
            filteredEmployees = employees.Value;

            logs = await logsActionService.GetAllLogs();


        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var avanceDetteList = Dashboards?.Select(item => new object[] { item.Dette, item.Avance })?.ToArray() ??
                                      Array.Empty<object[]>();

                var countfunctionEmployesList =
                    countfunction?.Select(item => new object[] { item.Name, item.Total }).ToArray() ??
                    Array.Empty<object[]>();

                // await JSRuntime.InvokeVoidAsync("renderCharts", Total_Number_Employe, avanceDetteList, countfunctionEmployesList);
            }
        }
        public class DashboardChartItem
        {
            public string Label { get; set; } // e.g., "2025-07"
            public decimal Dette { get; set; }
            public decimal Avance { get; set; }
        }

        public List<DashboardChartItem> DashboardChartData =>
            Dashboards?.Select(d => new DashboardChartItem
            {
                Label = $"{d.Month:D2}",
                Dette = d.Dette,
                Avance = d.Avance
            }).ToList() ?? new List<DashboardChartItem>();

        private async Task GetPointageByDate(int year, int month)
        {
            var filteredPointageResult = await DashboardService.GetPointageOfDashboardAsync(year, month);
            filteredPointage = filteredPointageResult.Value;
            await grid.Reload();
        }
        private IEnumerable<LogAction> FilteredLogs => logs
          .Where(log =>
              string.IsNullOrWhiteSpace(searchTerm) ||
              log.PerformedBy.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
              log.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
              log.ActionType.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
          .OrderByDescending(log => log.ActionDate);


    }


}