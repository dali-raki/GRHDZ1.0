
using GrhDz.Domains.Models.Logs;
using Implementation.Services.LogsAction;
using Implementation.Services.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Gestion_personal.Components.Layout.Users;

public partial class ProfileDropdown
{
    [Inject] private IJSRuntime JSRuntime { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private UserSessionStateService UserSession { get; set; }
    [Inject] private HttpClient Http { get; set; }
    [Inject] private ILogsActionService logsActionService { get; set; }
    private async Task Logout()
    {
        var log = new LogAction
        {
            ActionType = ActionType.Logout,
            ActionDate = DateTime.Now,
            Description = $"Déconnexion en tant que {UserSession.UserName}",
        };

        await logsActionService.settLog(log);
        await Http.PostAsync("api/auth/logout", null);
        UserSession.ClearUser();
        NavigationManager.NavigateTo("/");
    }
}