using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Employees;
using GrhDz.Domains.Models.Fonctions;
using GrhDz.Domains.Models.Logs;
using Implementation.Services.EmployeModel;
using Implementation.Services.Fonctions;
using Implementation.Services.LogsAction;
using Implementation.Services.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Gestion_personal.Components.Layout.Employes;

public partial class UpdateEmployeePopup
{
    [Parameter] public bool IsVisibleUpdateEmploye { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public EventCallback<Employe> OnSave { get; set; }
    [Parameter] public Employe Employee { get; set; } = new ();
    [Parameter] public bool showbtn { get; set; } = true;
    [Parameter] public bool IsDisabled { get; set; } = false;
    [Inject] private ILogsActionService logsActionService { get; set; }
    [Inject] private IFonctionService fonctionService { get; set; }
    [Inject] private UserSessionStateService userSession { get; set; }
    [Inject] private IEmployeService    employeService{ get; set; }
    private Result<List<Fonction>> fonctionsr;
    private List<Fonction> fonctions;
    private bool isSubmitting;
    private string errorMessage;

    protected override async Task OnInitializedAsync()
    {
        LoadFonction();
    }
    decimal SalaireMensuel
    {
        get => Employee.Journee * 26;
        set => Employee.Journee = (int)Math.Floor(value / 26);
    }

    protected override async Task OnParametersSetAsync()
    {
        if (IsVisibleUpdateEmploye)
        {
            await LoadFonction();
        }
    }

    public async Task LoadFonction()
    {
        fonctionsr = await fonctionService.GetAllAsync();
        fonctions = fonctionsr.Value;
    }

    private async Task HandleSubmit()
    {
        try
        {
            if (isSubmitting) return;
            isSubmitting = true;
            var log = new LogAction
            {
                ActionType = ActionType.Update,
                ActionDate = DateTime.Now,
                Description = "modifier Employe",
                PerformedBy = userSession.UserName,        
            };
           
            await employeService.UpdateEmployeAsync(Employee);
             await logsActionService.settLog(log);
            await OnSave.InvokeAsync(Employee);
            await OnClose.InvokeAsync();
            Console.WriteLine("Update photo");
        }
        catch (Exception ex)
        {
            errorMessage = "An error occurred while saving the employee: " + ex.Message;
        }
        finally
        {
            isSubmitting = false;
        }
    }

    private async Task HandleFileSelected(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file != null)
        {
            using (var stream = new MemoryStream())
            {
                await file.OpenReadStream().CopyToAsync(stream);
                Employee.Photo = stream.ToArray();
            }


            StateHasChanged();
        }
    }

    private async Task Hide_Popup_UpdateEmploye()
    {
        
        OnClose.InvokeAsync();
    }
}