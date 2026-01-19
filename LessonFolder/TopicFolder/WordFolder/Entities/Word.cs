using lexicana.Common.Enums;
using lexicana.Common.Entities;
using lexicana.UserFolder.Entities;
using lexicana.LessonFolder.TopicFolder.Entities;

namespace lexicana.LessonFolder.TopicFolder.WordFolder.Entities;

public class Word: BaseEntity
{
    public string Value { get; set; }
    public Language Language { get; set; }
    public string Translation { get; set; }
    public Guid TopicId { get; set; }
    public Topic Topic { get; set; } = new();
    public List<User> UsersForFavorite { get; set; } = new();
}