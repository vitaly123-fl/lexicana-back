using lexicana.Common.Entities;
using lexicana.TopicFolder.Entities;
using lexicana.UserFolder.Entities;
using lexicana.UserFolder.UserTopicFolder.Enums;

namespace lexicana.UserFolder.UserTopicFolder.Entities;

public class UserTopic: BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid TopicId { get; set; }
    public Topic Topic { get; set; }
    public UserTopicStatus Status { get; set; }
    public List<Guid> CompleatedWordsIds { get; set; } = new();
}