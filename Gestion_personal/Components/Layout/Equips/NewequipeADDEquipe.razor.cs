using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Employees;
using GrhDz.Domains.Models.Equipe;
using GrhDz.Domains.Models.Fonctions;
using GrhDz.Domains.Models.Logs;
using Implementation.Services.EmployeModel;
using Implementation.Services.EquipeEmploye;
using Implementation.Services.Equipes;
using Implementation.Services.Fonctions;
using Implementation.Services.LogsAction;
using Implementation.Services.Users;
using Microsoft.AspNetCore.Components;

namespace Gestion_personal.Components.Layout.Equips
{
    public partial class NewequipeADDEquipe
    {
        [Inject] private IEmployeService EmployeService { get; set; }
        [Inject] private IFonctionService FonctionService { get; set; }
        [Inject] private IEquipeService EquipeService { get; set; }
        [Inject] private IEmployeeEquipeService EmployeeEquipeService { get; set; }
        [Inject] private NavigationManager Navigation { get; set; }
        [Inject] private ILogsActionService logsActionService { get; set; }

        [Inject] private UserSessionStateService userSession { get; set; }
        [Parameter] public bool IsVisibleAddEquipe { get; set; }
        [Parameter] public EventCallback OnClose { get; set; }
        [Parameter] public EventCallback OnAddEquipe { get; set; }
        

        private Result<List<Employe>> employes ;
        private Result<List<Fonction>> fonctionsr;
        private List<Fonction> fonctions;
        private List<Employe> filteredEmployes = new();
        private Dictionary<int, bool> employeeSelection = new();
        private string equipeName;
        private int? selectedFonctionId;
        private int selectedChefId = 0;
        private string searchTerm = "";

        protected override async Task OnInitializedAsync()
        {
            employes = await EmployeService.GetEmployeesByStatus(EmployeeStatus.Active);
            fonctionsr = await FonctionService.GetAllAsync();
            fonctions = fonctionsr.Value;
            filteredEmployes = employes.Value;

            employeeSelection = employes.Value.ToDictionary(emp => emp.EmployeID, emp => false);
        }

        private async Task OnFonctionChange(object value)
        {
            selectedFonctionId = value as int?;

            if (selectedFonctionId.HasValue)
            {
                filteredEmployes = employes.Value
                    .Where(emp => emp.FonctionID == selectedFonctionId.Value)
                    .ToList();
            }
            else
            {
                filteredEmployes = employes.Value;
            }

            employeeSelection = filteredEmployes.ToDictionary(emp => emp.EmployeID, emp => false);
        }

        private IEnumerable<Employe> FilteredEmployes =>
            string.IsNullOrWhiteSpace(searchTerm)
                ? filteredEmployes
                : filteredEmployes.Where(e =>
                    (!string.IsNullOrEmpty(e.Nom) && e.Nom.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(e.Prenom) && e.Prenom.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));

        private async Task HandleSubmit()
        {
            try
            {
                if (string.IsNullOrEmpty(equipeName) || selectedChefId == 0)
                    return;

                var newEquipe = new Equipe
                {
                    NomEquipe = equipeName,
                    ChefEquipeID = selectedChefId,
                    Status = 1
                };

                Result<int> equipeId = await EquipeService.Add(newEquipe);

                var selectedIds = employeeSelection
                    .Where(e => e.Value)
                    .Select(e => e.Key)
                    .ToList();

                if (selectedIds.Any())
                {
                    await EmployeeEquipeService.AddEmployeesToEquipeAsync(equipeId.Value, selectedIds);
                }

                var log = new LogAction
                {
                    ActionType = ActionType.Insert,
                    ActionDate = DateTime.Now,
                    Description = $"Ajouter Equipe",
                    PerformedBy = userSession.UserName
                };

                await logsActionService.settLog(log);

                ResetForm();
                Hide_Popup_AddEquipe();
                await OnAddEquipe.InvokeAsync();
                await Task.Delay(1300);
                Navigation.NavigateTo("/equipe", forceLoad: true);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erreur lors de l'ajout: {ex.Message}");
            }
        }

        private void ResetForm()
        {
            equipeName = string.Empty;
            selectedChefId = 0;
            selectedFonctionId = null;
            employeeSelection = employes.Value.ToDictionary(emp => emp.EmployeID, emp => false);
            filteredEmployes = employes.Value;
            searchTerm = "";
        }

        private void Hide_Popup_AddEquipe()
        {
            IsVisibleAddEquipe = false;
            OnClose.InvokeAsync();
        }
    }
}
