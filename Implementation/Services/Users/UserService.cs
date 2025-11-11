using GrhDz.Apps.Shared;
using GrhDz.Domains.Models.Users;
using Infrastructures.Storages.UserStorages;

namespace Implementation.Services.Users
{
    public class UserService(IUserStorage userStorage) : IUserService
    {
   

        

        public async Task<LoginStatus> CanLogin(LoginCredentials credentials)
        {
            var user = userStorage.SelectUserByUsername(credentials.Username);

            if (user is null)
                return LoginStatus.UserNotFound;

            if (user.State == UserState.Inactive)
                return LoginStatus.UserNotActive;
            if (user.State == UserState.Deleted)
                return LoginStatus.UserNotFound;

            bool isPassVerifyed = BCrypt.Net.BCrypt.Verify(credentials.Password, user.Password);
            if (isPassVerifyed == false)
                return LoginStatus.InvalidCredentials;

            return LoginStatus.CanLogin;
        }

        public async Task<Result<List<User>>> GetAllActiveUsersAsync()
        {
            try
            {
                var users = await userStorage.SelectAllActiveUsers();
                return Result<List<User>>.Success(users);
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }

        public async Task<Result> AddUserAsync(User user)
        {
            try
            {
                await userStorage.InsertUser(user);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }

        public async Task<Result> SetUserAsync(User user)
        {
            try
            {
                await userStorage.UpdateUser(user);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }

        public async Task<Result> ChangeUserStateAsync(Guid userId, UserState newState)
        {
            try
            {
                await userStorage.ChangeUserState(userId, newState);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }
    }
}