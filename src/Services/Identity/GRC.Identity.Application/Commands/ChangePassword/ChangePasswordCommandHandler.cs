using GRC.Identity.Domain.Aggregates.UserAggregate;
using GRC.Identity.Domain.Exceptions;
using GRC.Identity.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GRC.Identity.Application.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ChangePasswordResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<ChangePasswordCommandHandler> _logger;

    public ChangePasswordCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ILogger<ChangePasswordCommandHandler> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<ChangePasswordResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Changing password for user: {UserId}", request.UserId);

            // Obtener usuario
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", request.UserId);
                return new ChangePasswordResult(false, "Usuario no encontrado");
            }

            // Verificar que el usuario esté activo
            if (!user.IsActive())
            {
                _logger.LogWarning("User is not active: {UserId}", request.UserId);
                return new ChangePasswordResult(false, "El usuario no está activo");
            }

            // Verificar contraseña actual
            if (!_passwordHasher.VerifyPassword(request.CurrentPassword, user._passwordHash))
            {
                _logger.LogWarning("Invalid current password for user: {UserId}", request.UserId);
                return new ChangePasswordResult(false, "La contraseña actual es incorrecta");
            }

            // Hash de la nueva contraseña
            var newPasswordHash = _passwordHasher.HashPassword(request.NewPassword);

            // Actualizar contraseña
            user.ChangePassword(newPasswordHash);

            // Guardar cambios
            await _userRepository.UpdateAsync(user);
            await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            _logger.LogInformation("Password changed successfully for user: {UserId}", user.Id);

            return new ChangePasswordResult(true, "Contraseña actualizada exitosamente");
        }
        catch (IdentityDomainException ex)
        {
            _logger.LogError(ex, "Domain error changing password: {UserId}", request.UserId);
            return new ChangePasswordResult(false, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password: {UserId}", request.UserId);
            return new ChangePasswordResult(false, "Error al cambiar la contraseña");
        }
    }
}