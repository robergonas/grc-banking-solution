using GRC.Identity.Domain.Aggregates.RoleAggregate;
using GRC.Identity.Domain.Aggregates.UserAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Identity.Application.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ITokenGenerator _tokenGenerator;

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        ITokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }
        
        if (!user.IsActive())
        {
            throw new UnauthorizedAccessException("User account is not active");
        }

        if (user.IsLocked())
        {
            throw new UnauthorizedAccessException("User account is locked due to multiple failed login attempts");
        }

        if (!user.ValidatePassword(request.Password))
        {
            // Registrar intento fallido
            user.RecordFailedLogin();
            await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            throw new UnauthorizedAccessException("Invalid email or password");
        }

        user.RecordSuccessfulLogin();
        await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        // 6. Obtener roles y permisos del usuario
        var userRoles = new List<string>();
        var userPermissions = new List<string>();

        foreach (var userRole in user.UserRoles)
        {
            var role = await _roleRepository.GetByIdAsync(userRole.RoleId);
            if (role != null && role.IsActive)
            {
                userRoles.Add(role.Name);

                // Agregar permisos del rol
                foreach (var permission in role.Permissions)
                {
                    if (!userPermissions.Contains(permission.Name))
                    {
                        userPermissions.Add(permission.Name);
                    }
                }
            }
        }

        var token = _tokenGenerator.GenerateToken(user);

        var refreshToken = Guid.NewGuid().ToString();

        return new LoginUserResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(1440),
            User = new UserInfo
            {
                Id = user.Id,
                Email = user.Email.Value,
                FullName = user.FullName,
                Roles = userRoles,
                Permissions = userPermissions
            }
        };
    }
}