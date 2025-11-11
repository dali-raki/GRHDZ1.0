using Gestion_personal.Components.Layout.Employes;
using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Employees;
using GrhDz.Domains.Models.Fonctions;
using Implementation.Services.EmployeModel;
using Implementation.Services.Fonctions;
using Implementation.Services.Users;
using Infrastructures.Storages.TransferData;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
namespace Gestion_personal.Components.Pages;

public partial class EmployePage
{
    [Inject] NavigationManager Nav { get; set; }
    [Inject] public ITransferDataStorage transferDataStorage { get; set; }

    [Inject]
    public IEmployeService EmployeService { get; set; }
    [Inject] public IFonctionService FonctionService { get; set; }
    [Inject] public UserSessionStateService UserSession { get; set; }
    public string datapop;
    private ModifierFonctionPopup modifierPopupRef;
    private Result<List<Employe>> employeesStatus1;
    private Result<List<Employe>> employeesStatus0;
    private List<Employe> filteredEmployeesStatus1;
    private List<Employe> filteredEmployeesStatus0;
    private Result<List<Fonction>> fonctions ;
    private Employe selectedEmployee = new ();
    private bool isEditPopupVisible = false;
    private bool isDisplayPopupVisible = false;
    private bool isPopupVisible = false;
    private bool isVisibleAddFunction = false;
    private bool isVisibleUpdFunction = false;
    private bool isSuccessPopupVisible = false;
    private bool isConfirmVisible = false;
    private bool isConfirmVisible2 = false;
    private int employeeToDelete;
    private string searchTerm = string.Empty;
    int selectedIndex = 0;
    private ConfirmationPopup confirmationPopup, confirmationPopup2;
    private int employeeToReturn;
    private SuccessPopup successTost;
    private string searchTerm2 = string.Empty;
    private void Show_Popup_AddEmploye() => isPopupVisible = true;
    private void Hide_Popup_AddEmploye() => isPopupVisible = false;
    private void Show_Popup_AddFunction() => isVisibleAddFunction = true;
    private void Hide_Popup_AddFunction() => isVisibleAddFunction = false;
    private void Show_Popup_UpdateFunction() => isVisibleUpdFunction = true;
    private void ShowSuccessPopup() => isSuccessPopupVisible = true;
    private void HideSuccessPopup() => isSuccessPopupVisible = false;
    private void Hide_Popup_UpdateEmploye() => isEditPopupVisible = false;
    private void Hide_Popup_ShowEmploye() => isEditPopupVisible = false;

    private void Hide_Popup_DisplayEmploye() => isDisplayPopupVisible = false;


    private void Hide_Popup_UpdateFunction() { 
        isVisibleUpdFunction = false;
        
    }
    protected override async Task OnInitializedAsync()
    {
        if (string.IsNullOrEmpty(UserSession.UserId.ToString()))
        {
            Nav.NavigateTo("/", forceLoad: true);
        }
        await LoadEmployees();
    }
    RadzenDataGrid<IDictionary<string, object>> dataGrid;
    IEnumerable<IDictionary<string, object>> data;

    protected override void OnParametersSet()
    {
        if (filteredEmployeesStatus1 == null)
        {
            data = null;
            return;
        }

        data = filteredEmployeesStatus1.Select(e => new Dictionary<string, object>
        {
            { "NomPrenom", $"{e.Nom} {e.Prenom}" },
            { "NSecuriteSocial", e.NSecuriteSocial },
            { "FonctionName", e.FonctionName },
            { "EmployeID", e.EmployeID },
            { "EmployeeObject", e } // For passing the full object to your popup
        }).ToList();
    }
    private async Task LoadEmployees()
    {
        try
        {
            employeesStatus1 = await EmployeService.GetEmployeesByStatus(EmployeeStatus.Active);
            filteredEmployeesStatus1 = employeesStatus1.Value;

            employeesStatus0 = await EmployeService.GetEmployeesByStatus(EmployeeStatus.Blocked);
            filteredEmployeesStatus0 = employeesStatus0.Value;
        }
        catch (Exception ex)
        {
            Console.WriteLine("employees not loaded: " + ex.Message);
        }
    }

    private async Task LoadFonction()
    {
        try
        {
            fonctions = await FonctionService.GetAllAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("fonctions not loaded: " + ex.Message);
        }
    }

    private async Task HandleSave(Employe newEmployee)
    {
        try
        {
            await EmployeService.AddEmployeAsync(newEmployee);
            await LoadEmployees();
            ShowSuccessPopup();
            Show_Popup_AddEmploye();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving employee: {ex.Message}");
        }
    }

    private async Task HandleFonctionSave()
    {
        ShowSuccessPopup();
        Hide_Popup_AddFunction();
        await LoadFonction();
    }

    private async Task DeleteEmployee(int employeID)
    {
        try
        {
            await EmployeService.SetEmployeAsync(employeID,EmployeeStatus.Blocked);
            await LoadEmployees();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error deleting employee: {ex.Message}");
        }
    }

    private async Task HandleModifyFonctionSave(Fonction newFonction)
    {
        await LoadFonction();
        if (modifierPopupRef != null)
        {
            await modifierPopupRef.RefreshFonctions(newFonction);
        }

        await LoadFonction();
        await LoadEmployees();
        ShowSuccessPopup();
    }

    private async Task HandleDeleteFonction()
    {
        await LoadFonction();
        await LoadEmployees();
        ShowSuccessPopup();
    }

    private async Task HandelConfirmation()
    {
        await LoadFonction();
        await LoadEmployees();
        ShowSuccessPopup();
    }

    private void Show_Popup_UpdateEmploye(Employe employee)
    {
        selectedEmployee = new Employe
        {
            EmployeID = employee.EmployeID,
            Nom = employee.Nom,
            Prenom = employee.Prenom,
            NSecuriteSocial = employee.NSecuriteSocial,
            FonctionID = employee.FonctionID,
            DateDeNaissance = employee.DateDeNaissance,
            DateEntree = employee.DateEntree,
            GroupSanguin = employee.GroupSanguin,
            Adresse = employee.Adresse,
            NTelephone = employee.NTelephone,
            SituationFamiliale = employee.SituationFamiliale,
            Photo = employee.Photo,
            Journee = employee.Journee
        };
        isEditPopupVisible = true;
    }

    private void Show_Popup_DisplayEmploye(Employe employee)
    {
        selectedEmployee = new Employe
        {
            EmployeID = employee.EmployeID,
            Nom = employee.Nom,
            Prenom = employee.Prenom,
            NSecuriteSocial = employee.NSecuriteSocial,
            FonctionID = employee.FonctionID,
            DateDeNaissance = employee.DateDeNaissance,
            DateEntree = employee.DateEntree,
            GroupSanguin = employee.GroupSanguin,
            Adresse = employee.Adresse,
            NTelephone = employee.NTelephone,
            SituationFamiliale = employee.SituationFamiliale,
            Photo = employee.Photo,
            Journee = employee.Journee
        };
        isDisplayPopupVisible = true;
    }

    private async Task HandleEditSave(Employe updatedEmployee)
    {
        try
        {
            await EmployeService.UpdateEmployeAsync(updatedEmployee);
            await LoadEmployees();
            ShowSuccessPopup();
            Hide_Popup_UpdateEmploye();
            datapop = "Votre opération s'est terminée avec succès.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error updating employee: {ex.Message}");
        }
    }

   

    private async Task HandlePopupResponse()
    {
        confirmationPopup.Show();
        await EmployeService.SetEmployeAsync(employeeToDelete,EmployeeStatus.Blocked);
        LoadEmployees();
        ShowSuccessPopup();
    }

    private void SearchEmployeesByStatus1(ChangeEventArgs e)
    {
        searchTerm = e.Value.ToString();
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            filteredEmployeesStatus1 = employeesStatus1.Value;
        }
        else
        {
            filteredEmployeesStatus1 = employeesStatus1.Value.Where(emp =>
                emp.Nom.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                emp.Prenom.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                emp.NSecuriteSocial.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                emp.EmployeID.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                emp.FonctionName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }

    private void SearchEmployeesByStatus0(ChangeEventArgs e)
    {
        searchTerm2 = e.Value.ToString();
        if (string.IsNullOrWhiteSpace(searchTerm2))
        {
            filteredEmployeesStatus0 = employeesStatus0.Value;
        }
        else
        {
            filteredEmployeesStatus0 = employeesStatus0.Value.Where(emp =>
                emp.Nom.Contains(searchTerm2, StringComparison.OrdinalIgnoreCase) ||
                emp.Prenom.Contains(searchTerm2, StringComparison.OrdinalIgnoreCase) ||
                emp.NSecuriteSocial.Contains(searchTerm2, StringComparison.OrdinalIgnoreCase) ||
                emp.EmployeID.ToString().Contains(searchTerm2, StringComparison.OrdinalIgnoreCase) ||
                emp.FonctionName.Contains(searchTerm2, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }

    private async Task TransferEmploye()
    {
        await transferDataStorage.TransfererEmployees();
        await LoadEmployees();
    }
    private void ConfirmDelete(int employeID)
    {
        confirmationPopup.Show();
        employeeToDelete = employeID;
    }
    private void ConfirmReturn(int employeID)
    {
        confirmationPopup.Show();
        employeeToReturn = employeID;
    }



    private async Task HandlePopupResponse2()
    {
        confirmationPopup2.Show();
        await EmployeService.SetEmployeAsync(employeeToReturn,EmployeeStatus.Active);
        successTost.Show();
        LoadEmployees();
    }

}