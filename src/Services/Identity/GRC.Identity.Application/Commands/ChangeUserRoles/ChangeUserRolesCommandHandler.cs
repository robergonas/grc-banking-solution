using GRC.Identity.Domain.Aggregates.UserAggregate;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GRC.Identity.Application.Commands.ChangeUserRoles;

public class ChangeUserRolesCommandHandler : IRequestHandler<ChangeUserRolesCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public ChangeUserRolesCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(ChangeUserRolesCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);

        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        user.ChangeRoles(request.RolesIds);

        await _userRepository.UpdateAsync(user);
        await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return true;
    }
}