using lexicana.Common.Enums;
using lexicana.Common.Entities;
using lexicana.LessonFolder.Entites;
using lexicana.TopicFolder.WordFolder.Entities;

namespace lexicana.TopicFolder.Entities;

public class Topic: BaseEntity
{
    public Language Language { get; set; }
    public List<Lesson> Lessons { get; set; } = new();
    public List<Word> Words { get; set; } = new();
}
