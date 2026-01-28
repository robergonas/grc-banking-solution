using MediatR;
using GRC.Identity.Application.DTOs;

namespace GRC.Identity.Application.Queries.GetUserById;

public record GetUserByIdQuery(Guid UserId) : IRequest<UserDto?>;