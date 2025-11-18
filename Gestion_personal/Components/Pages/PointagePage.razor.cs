using Gestion_personal.Components.Models.Toast;
using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Logs;
using GrhDz.Domains.Models.Pointages;
using Implementation.Services.LogsAction;
using Implementation.Services.PointageService;
using Implementation.Services.ReadUSB;
using Implementation.Services.Users;
using Infrastructures.Storages.TransferData;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen.Blazor;

namespace Gestion_personal.Components.Pages;

public partial class PointagePage
{
    [Inject] NavigationManager Nav { get; set; }

    [Inject] UserSessionStateService UserSession { get; set; }
    [Inject] public ITransferDataStorage transferDataStorage { get; set; }
    private string searchTerm = string.Empty;
    private DateTime selectedDate = DateTime.Now.Date;
    private List<Pointage> pointages;
    private List<Pointage> filteredPointages;
    private bool isVisiblePointage = false;
    private Pointage selectedPointage;
    private RadzenDataGrid<Pointage> grid;
    private bool showFileInput = false;
    private EditContext editContext = default!;
    private IBrowserFile? selectedFile;
    private ToastType toastType = ToastType.Success;
    private string toastTitle = string.Empty;
    private string toastMessage = string.Empty;
    private bool isToastVisible = false;
    private bool isSuccessPopupVisible = false;
    public string datapop;
    private void HideSuccessPopup() => isSuccessPopupVisible = false;
    private void ShowSuccessPopup() => isSuccessPopupVisible = true;
    [Inject]
    private IFileProcessingService FileProcessingService { get; set; } = default!;
    [Inject] private ILogsActionService logsActionService { get; set; }
    [Inject] private IPointageService    pointageService { get; set; }
    private void CloseModal()
    {
        showFileInput = false;
    }
    private void hendelSubmit()
    {
        ShowSuccessPopup();
    }
    private void OnInputFileChange(InputFileChangeEventArgs e)
    {
        selectedFile = e.File;
    }
    private async Task UploadRecord()
    {
        if (selectedFile == null)
        {
            ShowToast("Avertissement", "Aucun fichier sélectionné.", ToastType.Warning);
            showFileInput = false;
            return;
        }

        try
        {
            using var stream = selectedFile.OpenReadStream();
            using var reader = new StreamReader(stream);
            var fileContent = await reader.ReadToEndAsync();

            await FileProcessingService.ProcessFile(fileContent);
            var log = new LogAction
            {
                ActionType = ActionType.Insert,
                ActionDate = DateTime.Now,
                Description = $"Ajouter list pointage ",
                PerformedBy = UserSession.UserName,
            };
            await logsActionService.settLog(log);
            ShowSuccessPopup();
            ShowToast("Succès", "Fichier téléchargé avec succès!", ToastType.Success);
            selectedFile = null;
            showFileInput = false;

        }
        catch (Exception ex)
        {
            ShowSuccessPopup();
            ShowToast("Erreur", $"Il y a une erreur de fichier.", ToastType.Danger);
            selectedFile = null;
            showFileInput = false;

        }
    }

    private void ShowToast(string title, string message, ToastType type)
    {
        toastTitle = title;
        toastMessage = message;
        toastType = type;
        isToastVisible = true;
    }

    private void CloseToast()
    {
        isToastVisible = false;
    }
    
    private void ShowFileInput()
    {
        showFileInput = true;
    }
    private void Show_Popup_UpdatePointage(Pointage pointage)
    {
        if (pointage != null)
        {
            selectedPointage = pointage;
            isVisiblePointage = true;
        }
    }

    private async Task Hide_Popup_UpdatePointage()
    {
        isVisiblePointage = false;
        await GetPointageByDate();
        await grid.Reload(); 
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        if (string.IsNullOrEmpty(UserSession.UserId.ToString()))
        {
            Nav.NavigateTo("/", forceLoad: true);
        }
        editContext = new EditContext(new object());
        var pointages = await pointageService.GetByDate(selectedDate);
        filteredPointages = pointages.Value.ToList() ?? new List<Pointage>();
    }

    private async Task GetPointageByDate()
    {
        Result<List<Pointage>> result = await pointageService.GetByDate(selectedDate);
        pointages = [.. result.Value];
        FilterPointages();
    }

    private void Searchpointage(ChangeEventArgs changeEvent)
    {
        searchTerm = changeEvent.Value.ToString();
        FilterPointages();
    }

    private void FilterPointages()
    {
        if (pointages == null)
        {
            filteredPointages = new List<Pointage>();
            return;
        }

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            filteredPointages = pointages;
        }
        else
        {
/*            filteredPointages = pointages.Where(p =>
       (!string.IsNullOrEmpty(p.NomEmploye) &&
        p.NomEmploye.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
       (!string.IsNullOrEmpty(p.PrenomEmploye) &&
        p.PrenomEmploye.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
       (!string.IsNullOrEmpty(p.NomFonction) &&
        p.NomFonction.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
       p.EmployeID.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
   ).ToList();*/
        }
    }

    /*public async Task TransferPointage()
    {
        await transferDataStorage.TransfererPointages();
        await transferDataStorage.CalculeCofficient();
    }*/
}