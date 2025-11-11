
using GrhDz.Domains.Models.Logs;
using GrhDz.Domains.Models.Pointages;
using Implementation.Services.LogsAction;
using Implementation.Services.PointageService;
using Implementation.Services.Users;
using Microsoft.AspNetCore.Components;

namespace Gestion_personal.Components.Layout.Pointages
{
    public partial class NewPointagePopUp
    {
        [Parameter]
        public bool IsVisiblePointage { get; set; }

        [Parameter]
        public EventCallback OnClose { get; set; }
        [Parameter]
        public EventCallback Onsubmit { get; set; }


        [Parameter]
        public Pointage Pointage { get; set; }
        [Inject] private ILogsActionService logsActionService { get; set; }
        [Inject] private UserSessionStateService UserSession { get; set; }
        [Inject] private IPointageService pointageService { get; set; }



        private decimal tempHeuresTravaillees;
        private string tempRemarque;

        protected override void OnParametersSet()
        {
            if (Pointage != null)
            {

                tempHeuresTravaillees = Pointage.HeuresTravaillees;
                tempRemarque = Pointage.Remarque;
            }
        }

        private void CancelChanges()
        {

            tempHeuresTravaillees = Pointage.HeuresTravaillees;
            tempRemarque = Pointage.Remarque;

            Hide_Popup_UpdatePointage();
        }

        private async Task Hide_Popup_UpdatePointage()
        {
            await OnClose.InvokeAsync();
        }

        private async Task SaveChanges()
        {
            var log = new LogAction
            {
                ActionType = ActionType.Update,
                ActionDate = DateTime.Now,
                Description = $"modifier poinatge",
                PerformedBy = UserSession.UserName,
            };
            await logsActionService.settLog(log);
            Pointage.HeuresTravaillees = tempHeuresTravaillees;
            Pointage.Remarque = tempRemarque;
            pointageService.Update(Pointage);
            Hide_Popup_UpdatePointage();
            await Onsubmit.InvokeAsync();
        }
    }
}