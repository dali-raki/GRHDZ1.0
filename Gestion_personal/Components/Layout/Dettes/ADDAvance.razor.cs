
using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Avances;
using GrhDz.Domains.Models.Employees;
using Implementation.Services.Avance;
using Implementation.Services.EmployeModel;
using Microsoft.AspNetCore.Components;

namespace Gestion_personal.Components.Layout.Dettes;

public partial class ADDAvance
{
    [Parameter] public bool IsVisibleADDAvance { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public EventCallback OnLoad { get; set; }

    [Inject] public IEmployeService EmployeService { get; set; }
    [Inject] public IAvanceService AvanceService { get; set; }
    private Result<List<Employe>> employes;
    private List<Employe> listememployes;
    private int? selectedEmployeId;
    private decimal avanceMontant;
    private string media;

    protected override async Task OnInitializedAsync()
    {
        employes = await EmployeService.GetEmployeesByStatus(EmployeeStatus.Active);
        listememployes = employes.Value;
    }

    private void Hide_Popup_AddAvance()
    {
        OnClose.InvokeAsync();
    }

    private async Task HandleSubmit()
    {
        if (selectedEmployeId.HasValue)
        {
            // Create a new AvanceModel instance
            var newAvance = new AvanceModel
            {
                EmployeID = selectedEmployeId.Value,
                Montant = avanceMontant,
                Date = DateTime.Now // Set the current date for the advance
            };

            // Add the new avance using the service
            await AvanceService.AddAsync(newAvance);

            // Optionally, clear the form
            selectedEmployeId = null;
            avanceMontant = 0;
            await OnLoad.InvokeAsync();
            // Close the modal after adding
            await OnClose.InvokeAsync();
            StateHasChanged();
        }
    }
}