using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Governance.Domain.Aggregates.CommitteeAggregate;

namespace GRC.Governance.Domain.Aggregates.CommitteeAggregate;

public interface ICommitteeRepository : IRepository<Committee>
{
    Task<Committee> GetByIdAsync(Guid id);
    Task<Committee> GetByIdWithMembersAsync(Guid id);
    Task<Committee> GetByIdWithMeetingsAsync(Guid id);
    Task<IEnumerable<Committee>> GetAllAsync();
    Task<IEnumerable<Committee>> GetByTypeAsync(CommitteeType type);
    Task<IEnumerable<Committee>> GetByStatusAsync(CommitteeStatus status);
    Task<IEnumerable<Committee>> GetByMemberAsync(Guid userId);
    Task<CommitteeMeeting> GetMeetingByIdAsync(Guid meetingId);
    Task<IEnumerable<CommitteeMeeting>> GetUpcomingMeetingsAsync(int days = 30);

    Committee Add(Committee committee);
    void Update(Committee committee);
    void Remove(Committee committee);
}
