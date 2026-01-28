using MediatR;
using System.Collections.Generic;

namespace GRC.Identity.Application.Commands.LoginUser;
public class LoginUserCommand : IRequest<LoginUserResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }

    public LoginUserCommand() { }

    public LoginUserCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }
}
public class LoginUserResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public UserInfo User { get; set; }
}
public class UserInfo
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public List<string> Roles { get; set; }
    public List<string> Permissions { get; set; }
}