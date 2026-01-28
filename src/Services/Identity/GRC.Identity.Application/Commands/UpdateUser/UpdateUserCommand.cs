using MediatR;

namespace GRC.Identity.Application.Commands.UpdateUser;

public record UpdateUserCommand(
    Guid UserId,
    string FullName,
    string? Email = null
) : IRequest<UpdateUserResult>;

public record UpdateUserResult(
    bool Success,
    string Message,
    Guid? UserId = null
);