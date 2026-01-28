using MediatR;
using System;
using System.Collections.Generic;

namespace GRC.Identity.Application.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<Guid>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public List<string> Roles { get; set; }

    public RegisterUserCommand()
    {
        Roles = new List<string>();
    }

    public RegisterUserCommand(string email, string password, string fullName, List<string> roles)
    {
        Email = email;
        Password = password;
        FullName = fullName;
        Roles = roles != null ? roles : new List<string>();
    }
}