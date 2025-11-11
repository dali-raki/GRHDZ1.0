using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Employees;
using GrhDz.Domains.Models.SalairesBase;
using GrhDz.Domains.Models.TypeDePaiment;
using Implementation.Services.EmployeModel;
using Implementation.Services.SalaireBase;
using Implementation.Services.TypeDePaiment;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Gestion_personal.Components.Layout.Paiements;

public partial class NewpaiementPopUp
{
    [Parameter] public bool IsVisible { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }

    private Result<List<Employe>> employes;
    private List<Employe> listememployes;
    private Result<List<TypeDePaiement>> typeDePaiementsr;
    private List<SalairesBase> typeDePaiements;
    private int? selectedEmployeId;
    private int? selectedTypePaiementId;
    private string salaire;
    private string fonction;
    private Employe selectedEmploye;

    [Inject] private IEmployeService EmployeService { get; set; }
    [Inject] private ITypeDePaiementService TypeDePaiementService { get; set; }
    [Inject] private ISalaireBaseService SalairesBaseService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        employes = await EmployeService.GetEmployeesByStatus(EmployeeStatus.Active);
        listememployes = employes.Value;
        typeDePaiementsr = await TypeDePaiementService.GetAllAsync();
    }

    private async Task HandleFormSubmit()
    {
        if (selectedEmployeId.HasValue && selectedTypePaiementId.HasValue && !string.IsNullOrEmpty(salaire))
        {
            var salairesBase = new SalairesBase
            {
                EmplyeId = selectedEmployeId.Value,
                TypePaiementID = selectedTypePaiementId.Value,
                SalaireBase = decimal.Parse(salaire),
            };

            await SalairesBaseService.Add(salairesBase);
            ClosePopup();
        }
    }

    private async Task OnEmployeeChange(ChangeEventArgs e)
    {
        if (e.Value != null && int.TryParse(e.Value.ToString(), out var id))
        {
            selectedEmployeId = id;
            selectedEmploye = employes.Value.FirstOrDefault(emp => emp.EmployeID == id);

            if (selectedEmploye != null)
            {
                fonction = selectedEmploye.FonctionName;
            }
        }
        else
        {
            fonction = string.Empty;
        }
    }

    private void ClosePopup()
    {
        OnClose.InvokeAsync();
    }
}