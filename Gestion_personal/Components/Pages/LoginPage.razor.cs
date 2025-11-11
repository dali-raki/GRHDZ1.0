using Gestion_personal.Components.Models.Login;

using GrhDz.Domains.Models.Logs;
using Implementation.Services.LogsAction;
using Implementation.Services.Users;
using Microsoft.AspNetCore.Components;

namespace Gestion_personal.Components.Pages
{
    public partial class LoginPage
    {
        [CascadingParameter]
        public HttpContext? HttpContext { get; set; }
        private LoginModel loginModel = new LoginModel();
        private bool showLoginFailed = false;
        private bool showLoginSuccess = false;
        [Inject]
        public AppDbContext AppDbContext { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject]
        HttpClient Http { get; set; }
        [Inject] IHttpContextAccessor HttpContextAccessor { get; set; }
        [Inject] private UserSessionStateService UserSessionStateService { get; set; } = default!;
        [Inject] private ILogsActionService logsActionService { get; set; }

        private async Task Login()
        {
            var response = await Http.PostAsJsonAsync("api/auth/login", loginModel);

            if (response.IsSuccessStatusCode)
            {
                var userInfo = await response.Content.ReadFromJsonAsync<UserAccount>();

                if (userInfo != null)
                {
                    UserSessionStateService.SetUser(userInfo.UserName!,userInfo.Role!, userInfo.Id.ToString());
                }
                var log = new LogAction
                {
                    ActionType = ActionType.Login,
                    ActionDate = DateTime.Now,
                    Description = $"Connecté en tant que {userInfo.Role}",

                };
                await logsActionService.settLog(log);
                NavigationManager.NavigateTo("/Dashboard", true);
                
            }
            else
            {
                var log = new LogAction
                {
                    ActionType = ActionType.Login,
                    ActionDate = DateTime.Now,
                    Description = $"Échec de la connexion",
                    

                };
                await logsActionService.settLog(log);
                showLoginFailed = true;
            }
        }





    }
}