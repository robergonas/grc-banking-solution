using GRC.BuildingBlocks.Domain.SeedWork;
public class MeetingFrequency : Enumeration
{
    public static MeetingFrequency Weekly = new(1, nameof(Weekly));
    public static MeetingFrequency Biweekly = new(2, nameof(Biweekly));
    public static MeetingFrequency Monthly = new(3, nameof(Monthly));
    public static MeetingFrequency Quarterly = new(4, nameof(Quarterly));
    public static MeetingFrequency Semiannually = new(5, nameof(Semiannually));
    public static MeetingFrequency Annually = new(6, nameof(Annually));
    public static MeetingFrequency AsNeeded = new(7, nameof(AsNeeded));

    public MeetingFrequency(int id, string name) : base(id, name) { }
}