using GRC.Identity.Domain.Aggregates.UserAggregate;
using System;
using System.Collections.Generic;

namespace GRC.Identity.Application.Commands.LoginUser;
public interface ITokenGenerator
{
    string GenerateToken(User user);
}