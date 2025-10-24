using lexicana.Common.Enums;
using lexicana.Common.Entities;
using lexicana.TopicFolder.WordFolder.Entities;
using lexicana.UserFolder.UserTopicFolder.Entities;

namespace lexicana.UserFolder.Entities;

public class User : BaseEntity
{
    public string Email { get; set; }
    public string DisplayName { get; set; }
    public string PhotoUrl { get; set; }
    public string Provider { get; set; }
    public string FirebaseId { get; set; }
    public Language? Language { get; set; }
    public string? ResetCode { get; set; }
    public List<Word> FavoriteWords { get; set; } = new();
    public List<UserTopic> UserTopics { get; set; } = new();
} 