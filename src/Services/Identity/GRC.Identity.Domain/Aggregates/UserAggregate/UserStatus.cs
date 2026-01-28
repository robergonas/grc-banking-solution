using GRC.BuildingBlocks.Domain.SeedWork;

namespace GRC.Identity.Domain.Aggregates.UserAggregate;

public class UserStatus : Enumeration
{
    public static UserStatus Active = new UserStatus(1, nameof(Active));
    public static UserStatus Inactive = new UserStatus(2, nameof(Inactive));
    public static UserStatus Locked = new UserStatus(3, nameof(Locked));
    public static UserStatus PendingActivation = new UserStatus(4, nameof(PendingActivation));

    public UserStatus(int id, string name) : base(id, name)
    {
    }
    public static UserStatus FromValue(int value)
    {
        return FromValue<UserStatus>(value);
    }
    public static UserStatus FromName(string name)
    {
        return FromDisplayName<UserStatus>(name);
    }
}