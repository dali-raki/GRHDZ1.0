
using GrhDz.Domains.Models.Users;
using Implementation.Services.Users;
using Microsoft.AspNetCore.Components;

namespace Gestion_personal.Components.Layout.Users;

public partial class UsersUpdate
{
    [Parameter] public User selectedUser { get; set; } = new();
    [Inject] IUserService UserService { get; set; }

    private bool isUpdateModalVisible = false;

    private async Task UpdateUser()
    {
        await UserService.SetUserAsync(selectedUser);
        isUpdateModalVisible = false;
        //await RefreshUsers();
    }
    private void CloseUpdateModal()
    {
        isUpdateModalVisible = false;
        StateHasChanged();
    }

    public void OpenUpdateModal()
    {
        isUpdateModalVisible = true;
        StateHasChanged();
    }
}