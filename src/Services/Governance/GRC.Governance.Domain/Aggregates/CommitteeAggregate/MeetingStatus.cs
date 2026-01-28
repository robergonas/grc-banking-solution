using GRC.BuildingBlocks.Domain.SeedWork;

public class MeetingStatus : Enumeration
{
    public static MeetingStatus Scheduled = new(1, nameof(Scheduled));
    public static MeetingStatus InProgress = new(2, nameof(InProgress));
    public static MeetingStatus Completed = new(3, nameof(Completed));
    public static MeetingStatus Cancelled = new(4, nameof(Cancelled));

    public MeetingStatus(int id, string name) : base(id, name) { }
}