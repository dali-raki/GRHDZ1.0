using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Fonctions;
using GrhDz.Domains.Models.Logs;
using Implementation.Services.Fonctions;
using Implementation.Services.LogsAction;
using Implementation.Services.Users;
using Microsoft.AspNetCore.Components;

namespace Gestion_personal.Components.Layout.Employes
{
    public partial class ModifierFonctionPopup
    {
        [Parameter] public bool IsVisibleUpdateFunction { get; set; }
        [Parameter] public EventCallback OnClose { get; set; }
        [Parameter] public EventCallback OnDelete { get; set; }
        [Parameter] public EventCallback<Fonction> OnSave { get; set; }
        [Inject] private ILogsActionService logsActionService { get; set; }
        [Inject] private IFonctionService fonctionService { get; set; }
        [Inject] private UserSessionStateService userSession{ get; set; }
        private List<Fonction> fonctions = new ();
        private Result<List<Fonction>>  fonctionsr;
        private int selectedFonctionId;
        private string newFonctionName;

        protected override async Task OnInitializedAsync()
        {
            await LoadFonctions();
        }

        protected override async Task OnParametersSetAsync()
        {

            if (IsVisibleUpdateFunction)
            {
                await LoadFonctions();
            }
        }

        private async Task LoadFonctions()
        {
            try
            {
                fonctionsr = await fonctionService.GetAllAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading fonctions: " + ex.Message);
            }
        }

        private async Task SaveFonction()
        {
            if (selectedFonctionId > 0 && !string.IsNullOrEmpty(newFonctionName))
            {
                var fonctionToUpdate = fonctions.FirstOrDefault(f => f.FonctionID == selectedFonctionId);
                if (fonctionToUpdate != null)
                {
                    fonctionToUpdate.NomFonction = newFonctionName;

                    try
                    {
                        await fonctionService.UpdateAsync(fonctionToUpdate);
                        await OnSave.InvokeAsync(fonctionToUpdate);
                        await OnClose.InvokeAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error updating fonction: " + ex.Message);
                    }
                }
                var log = new LogAction
                {
                    ActionType = ActionType.Update,
                    ActionDate = DateTime.Now,
                    Description = $"modifier fonction",
                    PerformedBy = userSession.UserName
                };
                await logsActionService.settLog(log);
            }
        }

        private async Task DeleteFonction()
        {
            if (selectedFonctionId > 0)
            {
                var fonctionToDelete = fonctions.FirstOrDefault(f => f.FonctionID == selectedFonctionId);
                if (fonctionToDelete != null)
                {
                    try
                    {
                        await fonctionService.DeleteAsync(fonctionToDelete.FonctionID);
                        selectedFonctionId = 0;
                        await OnDelete.InvokeAsync();
                       
                        await LoadFonctions();
                        var log = new LogAction
                        {
                            ActionType = ActionType.Delete,
                            ActionDate = DateTime.Now,
                            Description = $"supprimer fonction",
                            PerformedBy = userSession.UserName
                            
                        };
                        await logsActionService.settLog(log);
                        await OnClose.InvokeAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error deleting fonction: " + ex.Message);
                    }
                
                }
            }
        }

        private async Task Cancel()
        {
            selectedFonctionId = 0;
            newFonctionName = string.Empty;
            await OnClose.InvokeAsync();
        }

        public async Task RefreshFonctions(Fonction newFonction)
        {
            await LoadFonctions();
            selectedFonctionId = newFonction.FonctionID;
        }

    }
}