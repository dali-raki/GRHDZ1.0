using GrhDz.Domains.Models.Logs;
using Implementation.Services.LogsAction;
using Implementation.Services.Users;
using Microsoft.AspNetCore.Components;

namespace Gestion_personal.Components.Layout.Employes;

public partial class ConfirmationPopup
{
    [Parameter] public string Title { get; set; }
    [Parameter] public EventCallback OnConfirmation { get; set; }
    [Parameter] public bool IsVisibleConfirm { get; set; }
    [Parameter] public string objective { get; set; } = "Supprimer";
    
    [Inject] private ILogsActionService logsActionService { get; set; }
    [Inject] private UserSessionStateService UserSession { get; set; }
   
    private async Task confirm()
    {
        var log = new LogAction
        {

            ActionDate = DateTime.Now,
            PerformedBy = UserSession.UserName
        };

        if (objective == "Supprimer")
        {
            log.Description = "Supprimer Employe";
            log.ActionType = ActionType.Delete;
        }
        else if (objective == "Renvoyer")
        {
            log.Description = "Renvoyer l'employé";
            log.ActionType = ActionType.Insert;
        }

        await logsActionService.settLog(log);
        
        await OnConfirmation.InvokeAsync();
        close();

    }


    private void close() => IsVisibleConfirm = false;
    public void Show() => IsVisibleConfirm = true;
}