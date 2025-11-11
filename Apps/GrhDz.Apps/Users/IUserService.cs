using GrhDz.Domains.Models.Users;

namespace GrhDz.Apps.Users
{
    public interface IUserService
    {
     //   User GetUserAuth(String Username, String Password);
        Task<LoginStatus> CanLogin(LoginCredentials credentials);
        Task AddUserAsync(User user);
        Task SetUserAsync(User user);
        Task ChangeUserStateAsync(Guid userId, UserState newState);
        Task<List<User>> GetAllActiveUsers();
    }
}