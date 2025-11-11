using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Users;
namespace Implementation.Services.Users
{
    public interface IUserService
    {
        Task<LoginStatus> CanLogin(LoginCredentials credentials);
        Task<Result<List<User>>> GetAllActiveUsersAsync();
        Task<Result> AddUserAsync(User user);
        Task<Result> SetUserAsync(User user);
        Task<Result> ChangeUserStateAsync(Guid userId, UserState newState);
    }
}