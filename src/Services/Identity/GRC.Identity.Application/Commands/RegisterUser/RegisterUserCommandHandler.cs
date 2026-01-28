using GRC.Identity.Domain.Aggregates.RoleAggregate;
using GRC.Identity.Domain.Aggregates.UserAggregate;
using GRC.Identity.Infrastructure.Services;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Identity.Application.Commands.RegisterUser;
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;
    //private readonly IEventBus _eventBus;
    public RegisterUserCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }
    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }
        var email=Email.Create(request.Email);
        var password=Password.Create(request.Password,_passwordHasher);
        var user = new User(request.Email, request.Password, request.FullName);

        // Buscar cada rol por nombre y agregarlo al usuario
        foreach (var roleName in request.Roles)
        {
            var role = await _roleRepository.GetByNameAsync(roleName);
            if (role == null)
            {
                throw new InvalidOperationException($"Role '{roleName}' not found");
            }

            user.AddRole(role.Id);
        }

        await _userRepository.AddAsync(user);
        await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);        

        return user.Id;
    }
}