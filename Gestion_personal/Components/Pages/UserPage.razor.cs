
using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Users;
using Implementation.Services.LogsAction;
using Implementation.Services.Users;
using Microsoft.AspNetCore.Components;

namespace Gestion_personal.Components.Pages
{
    public partial class UserPage
    {
        private Result<List<User>> activeUsers ;
        private List<User> filteredUsers = new();
        private string userSearch = string.Empty;
        private bool isSuccessPopupVisible = false;
        public string datapop;
        private void HideSuccessPopup() => isSuccessPopupVisible = false;
        private void ShowSuccessPopup() => isSuccessPopupVisible = true;
        private User selectedUser = new();
        private bool isUpdateModalVisible = false;
        private bool isDeleteModalVisible = false;
        private bool isAddModalVisible = false;
        private User newUser = new();
        [Inject] NavigationManager Nav { get; set; }
        [Inject] private ILogsActionService logsActionService { get; set; }
        [Inject] IUserService UserService { get; set; }
        [Inject] UserSessionStateService UserSession { get; set; }
        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(UserSession.UserId.ToString()))
            {
                Nav.NavigateTo("/", forceLoad: true);
            }
            await RefreshUsers();

        }
       
        private void SearchUsers(ChangeEventArgs e)
        {
            userSearch = e.Value?.ToString() ?? "";
            filteredUsers = activeUsers.Value
                .Where(u =>
                    (!string.IsNullOrEmpty(u.Username) && u.Username.Contains(userSearch, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(u.Role) && u.Role.Contains(userSearch, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        private void OpenUpdateModal(User user)
        {
            selectedUser = new User
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                Password = user.Password,
                State = user.State
            };
            isUpdateModalVisible = true;
        }

        private void CloseUpdateModal() => isUpdateModalVisible = false;

        private async Task UpdateUser()
        {
            await UserService.SetUserAsync(selectedUser);
            isUpdateModalVisible = false;
            ShowSuccessPopup();
            await RefreshUsers();
        }

        private void OpenDeleteModal(User user)
        {
            selectedUser = user;
            isDeleteModalVisible = true;
        }

        private void CloseDeleteModal() => isDeleteModalVisible = false;

        private async Task DeleteUser()
        {
            await UserService.ChangeUserStateAsync(selectedUser.Id, UserState.Deleted);
            isDeleteModalVisible = false;
            ShowSuccessPopup();
            await RefreshUsers();
        }

        private async Task RefreshUsers()
        {
            activeUsers = await UserService.GetAllActiveUsersAsync();
            filteredUsers = activeUsers.Value
                .Where(u => string.IsNullOrEmpty(userSearch) ||
                    (!string.IsNullOrEmpty(u.Username) && u.Username.Contains(userSearch, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(u.Role) && u.Role.Contains(userSearch, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        private void OpenAddModal()
        {
            newUser = new User();
            isAddModalVisible = true;
        }

        private void CloseAddModal() => isAddModalVisible = false;

        private async Task AddUser()
        {
            newUser.Id = Guid.NewGuid();
            newUser.State = UserState.Active;
            await UserService.AddUserAsync(newUser); // ? CORRECTED
            isAddModalVisible = false;
            ShowSuccessPopup();
            await RefreshUsers();
        }
    }
}