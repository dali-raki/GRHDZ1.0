using GrhDz.Domains.Models.Logs;
using Implementation.Services.LogsAction;
using Implementation.Services.Users;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace Gestion_personal.Components.Pages
{
    public partial class LogsPage
    {
        [Inject] UserSessionStateService UserSession { get; set; }
        [Inject] private ILogsActionService logsActionService { get; set; }
        private string searchTerm = string.Empty;
        private List<LogAction> logs = new();
        private RadzenDataGrid<LogAction> grid;
        private IEnumerable<LogAction> FilteredLogs => logs
            .Where(log =>
                string.IsNullOrWhiteSpace(searchTerm) ||
                log.PerformedBy.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                log.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                log.ActionType.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(log => log.ActionDate);

        protected override async Task OnInitializedAsync()
        {
            logs = await logsActionService.GetAllLogs();
        }
    }
}
