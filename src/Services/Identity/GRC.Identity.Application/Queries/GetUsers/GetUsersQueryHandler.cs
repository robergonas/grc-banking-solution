using AutoMapper;
using GRC.Identity.Application.DTOs;
using GRC.Identity.Domain.Aggregates.UserAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GRC.Identity.Application.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PagedResult<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetUsersQueryHandler> _logger;

    public GetUsersQueryHandler(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<GetUsersQueryHandler> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResult<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting users - Page: {PageNumber}, PageSize: {PageSize}, SearchTerm: {SearchTerm}",
            request.PageNumber, request.PageSize, request.SearchTerm);

        var (users, totalCount) = await _userRepository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            request.SearchTerm,
            request.Status);

        var userDtos = _mapper.Map<List<UserDto>>(users);

        var result = new PagedResult<UserDto>
        {
            Items = userDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };

        _logger.LogInformation("Retrieved {Count} users out of {TotalCount}", userDtos.Count, totalCount);

        return result;
    }
}