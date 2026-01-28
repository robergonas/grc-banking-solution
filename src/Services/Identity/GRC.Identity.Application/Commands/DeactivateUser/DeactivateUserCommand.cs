using MediatR;
using System;

namespace GRC.Identity.Application.Commands.DeactivateUser;

public class DeactivateUserCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    public string Reason { get; set; }

    public DeactivateUserCommand() { }

    public DeactivateUserCommand(Guid userId, string reason)
    {
        UserId = userId;
        Reason = reason;
    }
}