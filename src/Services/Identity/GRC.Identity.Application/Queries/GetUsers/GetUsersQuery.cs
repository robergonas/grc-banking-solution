using MediatR;
using GRC.Identity.Application.DTOs;

namespace GRC.Identity.Application.Queries.GetUsers;

public record GetUsersQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    string? Status = null
) : IRequest<PagedResult<UserDto>>;