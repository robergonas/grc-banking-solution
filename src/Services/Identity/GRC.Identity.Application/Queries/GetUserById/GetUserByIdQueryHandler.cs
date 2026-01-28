using AutoMapper;
using GRC.Identity.Application.DTOs;
using GRC.Identity.Domain.Aggregates.UserAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GRC.Identity.Application.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;

    public GetUserByIdQueryHandler(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<GetUserByIdQueryHandler> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting user by ID: {UserId}", request.UserId);

        var user = await _userRepository.GetByIdAsync(request.UserId);

        if (user == null)
        {
            _logger.LogWarning("User not found with ID: {UserId}", request.UserId);
            return null;
        }

        var userDto = _mapper.Map<UserDto>(user);

        _logger.LogInformation("User found: {Email}", user.Email.Value);

        return userDto;
    }
}