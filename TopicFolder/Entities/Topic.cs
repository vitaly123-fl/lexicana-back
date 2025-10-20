using lexicana.Common.Enums;
using lexicana.Common.Entities;
using lexicana.TopicFolder.TopicWordFolder.Entities;

namespace lexicana.TopicFolder.Entities;

public class Topic: BaseEntity
{
    public int Order { get; set; }
    public string Title { get; set; }
    public bool IsPremium { get; set; } = true;
    public bool IsGrouped { get; set; } = false;
    public Language Language { get; set; }
    public List<TopicWord> Words { get; set; } = new();
}