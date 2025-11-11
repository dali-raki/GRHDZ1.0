using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Employees;
using GrhDz.Domains.Models.Fonctions;
using GrhDz.Domains.Models.Logs;
using Implementation.Services.EmployeModel;
using Implementation.Services.Fonctions;
using Implementation.Services.LogsActions;
using Implementation.Services.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace GrhDz.EmployeeUI;

public partial class EmployeeForm
{
    [Parameter] public bool IsVisibleUpdateEmploye { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public EventCallback<Employe> OnSave { get; set; }
    [Parameter] public Employe Employee { get; set; } = new ();
    [Parameter] public bool showbtn { get; set; } = true;
    [Parameter] public bool IsDisabled { get; set; } = false;
    [Inject] private LogsActionService logsActionService { get; set; }
    [Inject] private IFonctionService fonctionService { get; set; }
    [Inject] private IEmployeService EmployeService{ get; set; }
    [Inject] private UserSessionStateService UserSession{ get; set; }
    private List<Fonction> fonctions;
    private Result<List<Fonction>> fonctionsr;
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
                PerformedBy = UserSession.UserName,
            };
            await logsActionService.settLog(log);
            await EmployeService.UpdateEmployeAsync(Employee);
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