using lexicana.Common.Enums;
using lexicana.Common.Entities;
using lexicana.LessonFolder.TopicFolder.Entities;

namespace lexicana.LessonFolder.Entites;

public class Lesson : BaseEntity
{
    public int Order { get; set; }
    public string Title { get; set; }
    public bool IsPremium { get; set; }
    public Language Language { get; set; }
    public List<Topic> Topics { get; set; } = new();
}