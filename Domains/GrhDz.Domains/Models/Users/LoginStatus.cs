namespace GrhDz.Domains.Models.Users;


public enum LoginStatus
{
    CanLogin,
    InvalidCredentials,
    UserNotFound,
    UserNotActive
}