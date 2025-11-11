using GrhDz.Domains.Models.Fonctions;
using GrhDz.Domains.Models.Logs;
using Implementation.Services.Fonctions;
using Implementation.Services.LogsAction;
using Implementation.Services.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Gestion_personal.Components.Layout.Employes;

public partial class AddFonctionPopup
{
    [Parameter] public bool IsVisibleAddFunction { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public EventCallback OnSave { get; set; }
    [Inject] private ILogsActionService logsActionService { get; set; }
    private string FonctionName;
    [Inject] private IFonctionService FonctionService { get; set; }
    [Inject] private UserSessionStateService UserSession { get; set; }



    private void Hide_Popup_AddFunction()
    {
        FonctionName = string.Empty;
        OnClose.InvokeAsync();
    }

    private async Task HandleSave()
    {
        var newFonction = new Fonction
        {
            NomFonction = FonctionName
        };

        try
        {
            await FonctionService.AddAsync(newFonction);
            await OnSave.InvokeAsync(newFonction);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving fonction: " + ex.Message);
        }
        var log = new LogAction
        {
            ActionType = ActionType.Insert,
            ActionDate = DateTime.Now,
            Description = $"Ajouter fonction",
            PerformedBy = UserSession.UserName,

        };
        await logsActionService.settLog(log);

        Hide_Popup_AddFunction();
    }
}