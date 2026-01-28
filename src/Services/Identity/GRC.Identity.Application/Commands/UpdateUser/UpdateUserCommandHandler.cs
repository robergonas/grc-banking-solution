using GRC.Identity.Domain.Aggregates.UserAggregate;
using GRC.Identity.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GRC.Identity.Application.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserResult>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(
        IUserRepository userRepository,
        ILogger<UpdateUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UpdateUserResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Updating user: {UserId}", request.UserId);

            // Verificar si el usuario existe
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", request.UserId);
                return new UpdateUserResult(false, "Usuario no encontrado");
            }

            // Si se proporciona un nuevo email, verificar que no esté en uso
            if (!string.IsNullOrWhiteSpace(request.Email) &&
                request.Email != user.Email.Value)
            {
                var existingUser = await _userRepository.GetByEmailAsync(request.Email);
                if (existingUser != null && existingUser.Id != user.Id)
                {
                    _logger.LogWarning("Email already in use: {Email}", request.Email);
                    return new UpdateUserResult(false, "El email ya está en uso");
                }

                // Actualizar email
                user.UpdateEmail(Email.Create(request.Email));
            }

            // Actualizar nombre completo
            user.UpdateFullName(request.FullName);

            // Guardar cambios
            await _userRepository.UpdateAsync(user);
            await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            _logger.LogInformation("User updated successfully: {UserId}", user.Id);

            return new UpdateUserResult(
                true,
                "Usuario actualizado exitosamente",
                user.Id);
        }
        catch (IdentityDomainException ex)
        {
            _logger.LogError(ex, "Domain error updating user: {UserId}", request.UserId);
            return new UpdateUserResult(false, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user: {UserId}", request.UserId);
            return new UpdateUserResult(false, "Error al actualizar el usuario");
        }
    }
}