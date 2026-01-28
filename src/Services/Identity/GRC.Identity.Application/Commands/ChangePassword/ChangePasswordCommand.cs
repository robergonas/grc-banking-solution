using MediatR;

namespace GRC.Identity.Application.Commands.ChangePassword;

public record ChangePasswordCommand(
    Guid UserId,
    string CurrentPassword,
    string NewPassword
) : IRequest<ChangePasswordResult>;

public record ChangePasswordResult(
    bool Success,
    string Message
);