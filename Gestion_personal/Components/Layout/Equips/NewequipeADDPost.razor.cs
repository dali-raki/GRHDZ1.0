using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Employees;
using GrhDz.Domains.Models.EmplyeeEquipe;
using GrhDz.Domains.Models.Equipe;
using GrhDz.Domains.Models.Logs;
using Implementation.Services.EmployeModel;
using Implementation.Services.Equipes;
using Implementation.Services.LogsAction;
using Implementation.Services.Post;
using Implementation.Services.Users;
using Microsoft.AspNetCore.Components;

namespace Gestion_personal.Components.Layout.Equips;

public partial class NewequipeADDPost
{
    [Parameter] public bool IsVisibleAddPost { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public EventCallback<EquipesInfos> OnSave { get; set; }
    [Inject] private ILogsActionService logsActionService { get; set; }


    [Inject]private IPosteService PosteService { get; set; }
    [Inject] private IEquipeService EquipeService { get; set; }
    [Inject] private IEmployeService EmployeService { get; set; }
    [Inject] private UserSessionStateService UserSession { get; set; }

    private Result<List<Employe>> employees;
    private string searchTerm = "";
    private Result<List<Equipe>> equipesr;
    private List<Equipe> equipes;
    private List<EmployeeEquipe> employeeequipe;
    private List<int> SelectedEmployeeIds { get; set; } = new List<int>();
    private IEnumerable<Employe> FilteredEmployees => string.IsNullOrWhiteSpace(searchTerm)
    ? employees.Value
    : employees.Value.Where(e =>
        (!string.IsNullOrEmpty(e.Nom) && e.Nom.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
        (!string.IsNullOrEmpty(e.Prenom) && e.Prenom.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));

    private void Hide_Popup_AddPost()
    {
        IsVisibleAddPost = false;
        OnClose.InvokeAsync();
    }

    private DateTime? DateFin = DateTime.Now;
    private string numeroPoste;
    private int? SelectedEquipeId;

    protected override async Task OnInitializedAsync()
    {
        equipesr = await EquipeService.GetAllEquipesAsync();
        equipes = equipesr.Value;
    }

    private async Task OnEquipeChanged(object value)
    {
        SelectedEquipeId = value as int?;

        if (SelectedEquipeId.HasValue)
        {
            employees = await EquipeService.GetEmployeesByEquipeIdAsync(SelectedEquipeId.Value);
        }
        else
        {
            employees = new List<Employe>(); // or keep previous value
        }
    }

    private void ToggleEmployeeSelection(int employeeId)
    {
        if (SelectedEmployeeIds.Contains(employeeId))
        {
            SelectedEmployeeIds.Remove(employeeId);
        }
        else
        {
            SelectedEmployeeIds.Add(employeeId);
        }

        Console.WriteLine(employeeId);
    }

    private async Task HandleSubmit()
    {
        if (!string.IsNullOrEmpty(numeroPoste) && SelectedEquipeId.HasValue && SelectedEmployeeIds.Any())
        {
            Console.WriteLine(SelectedEmployeeIds.Count);
            // Ensure DateFin is not null before calling the service
            await PosteService.InsererDonneesPoste(numeroPoste, SelectedEquipeId.Value, DateFin ?? DateTime.MinValue,
                SelectedEmployeeIds);
            numeroPoste = "";
            DateFin = DateTime.Now;
            SelectedEquipeId = null;
            SelectedEmployeeIds = new List<int>();


            var log = new LogAction
            {
                ActionType = ActionType.Insert,
                ActionDate = DateTime.Now,
                Description = $"Ajouter Post",
                PerformedBy = UserSession.UserName,
            };
            await logsActionService.settLog(log);

            await OnSave.InvokeAsync();
            Hide_Popup_AddPost();
        }
        else
        {
            // Handle form validation if necessary
            Console.WriteLine("Please fill in all the fields.");
            Hide_Popup_AddPost();
        }
    }
}