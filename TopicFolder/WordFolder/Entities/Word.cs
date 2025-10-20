using lexicana.Common.Enums;
using lexicana.Common.Entities;
using lexicana.TopicFolder.TopicWordFolder.Entities;
using lexicana.UserFolder.Entities;

namespace lexicana.TopicFolder.WordFolder.Entities;

public class Word: BaseEntity
{
    public string Value { get; set; }
    public Language Language { get; set; }
    public string Translation { get; set; }
    public List<TopicWord> Topics { get; set; } = new();
    public List<User> UsersForFavorite { get; set; } = new();
}