using Microsoft.AspNetCore.Components;
using GrhDz.Domains.Models.Avances;
using Implementation.Services.Dettes;
using GrhDz.Domains.Models.Salaires;
using GrhDz.Apps.Avances;
using GrhDz.Apps.Dettes;
using GrhDz.Domains.Models.Remboursements;
using GrhDz.Domains.Models.Primes;
using GrhDz.Domains.Models.Logs;
using GrhDz.Domains.Models.Dettes;
using Implementation.Services.Avance;
using Implementation.Services.LogsAction;
using Implementation.Services.Prime;
using Implementation.Services.Remboursement;

namespace Gestion_personal.Components.Layout.Paiements
{
    public partial class PaimentAvanceDettePopUp
    {
        [Parameter] public EventCallback OnSaved { get; set; }
        public SalaireDetail SalaireDetail { get; set; }
        [Inject] private ILogsActionService logsActionService { get; set; }
        [Inject] public IDetteService DetteService { get; set; }
        [Inject] IAvanceService AvanceService { get; set; }
        [Inject] IPrimeService primeService { get; set; }
        [Inject] IRemboursementService remboursement { get; set; }

        private bool display = false;
        private Dette newDette;
        private AvanceModel _newAvanceModel;
        private RemboursementType newRemboursement;
        private PrimeType newPrime;
        private int type;
     
        
        private string SelectedType { get; set; }

        public void Show(SalaireDetail s)
        {
            SalaireDetail = s;
            display = true;
            StateHasChanged();
        }

        public void Hide()
        {
            display = false;
            StateHasChanged();
        }
        private async Task ADDAvanceorDatte(SalaireDetail SalaireDetail)
        {
            if (type == 1)
            {

                _newAvanceModel = new AvanceModel
                {
                    EmployeID = SalaireDetail.EmployeId,
                    Montant = SalaireDetail.amount,
                    Date = DateTime.Now,
                    Description = SalaireDetail.Description,
                };
                await AvanceService.AddAsync(_newAvanceModel);
                var log = new LogAction
                {
                    ActionType = ActionType.Insert,
                    ActionDate = DateTime.Now,
                    Description = $"Donner une avanceModel ",
                    PerformedBy = UserSession.UserName,
                };
                await logsActionService.settLog(log);

            }
            if (type == 2)
            {
                newDette = new Dette
                {
                    EmployeID = SalaireDetail.EmployeId,
                    Montant = SalaireDetail.amount,
                    Date = DateTime.Now,
                    Description = SalaireDetail.Description,
                };
                await DetteService.AddAsync(newDette);

                var log = new LogAction
                {
                    ActionType = ActionType.Insert,
                    ActionDate = DateTime.Now,
                    Description = $"Donner une Dette ",
                    PerformedBy = UserSession.UserName,
                };
                await logsActionService.settLog(log);
            }
            if (type == 3)
            {
                newRemboursement = new RemboursementType
                {
                    EmployeID = SalaireDetail.EmployeId,
                    Montant = SalaireDetail.amount,
                    Date = DateTime.Now,
                    Description = SalaireDetail.Description,
                };
                await remboursement.AddAsync(newRemboursement);

                var log = new LogAction
                {
                    ActionType = ActionType.Insert,
                    ActionDate = DateTime.Now,
                    Description = $"Donner une Remboursement ",
                    PerformedBy = UserSession.UserName,
                };
                await logsActionService.settLog(log);
            }
            if (type == 4)
            {
                newPrime = new PrimeType
                {
                    EmployeID = SalaireDetail.EmployeId,
                    Montant = SalaireDetail.amount,
                    Date = DateTime.Now,
                    Description = SalaireDetail.Description,
                };
                await primeService.AddAsync(newPrime);

                var log = new LogAction
                {
                    ActionType = ActionType.Insert,
                    ActionDate = DateTime.Now,
                    Description = $"Donner une Prime ",
                    PerformedBy = UserSession.UserName,
                };
                await logsActionService.settLog(log);
            }

            display = false;
            if (OnSaved.HasDelegate)
            {
                await OnSaved.InvokeAsync();
            }
            StateHasChanged();


        }

       

        private string GetButtonText()
        {
            return type switch
            {
                1 => "Ajouter AvanceModel",
                2 => "Ajouter Dette",
                3 => "Ajouter Remboursement",
                4 => "Ajouter Prime",
                _ => "Ajouter",
            };
        }

        private string GetButtonClass()
        {
            return type switch
            {
                1 => "btn btn-primary",   // AvanceModel: Bleu
                2 => "btn btn-danger",    // Dette: Rouge
                3 => "btn btn-success",   // Remboursement: Vert
                4 => "btn btn-warning text-white",  // Prime: Jaune
                _ => "btn btn-primary",
            };
        }
    }
}