using lexicana.Common.Enums;

namespace lexicana.LessonFolder.TopicFolder.WordFolder.DTOs;

public class WordModel
{
    public Guid Id { get; set; }
    public string Word { get; set; }
    public Language Language { get; set; }
    public string Translation { get; set; }
}