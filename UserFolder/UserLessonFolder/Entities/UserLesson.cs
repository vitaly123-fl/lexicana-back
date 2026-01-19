using lexicana.Common.Entities;
using lexicana.LessonFolder.Entites;
using lexicana.UserFolder.Entities;
using lexicana.UserFolder.UserLessonFolder.Enums;

namespace lexicana.UserFolder.UserLessonFolder.Entities;

public class UserLesson: BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid LessonId { get; set; }
    public Lesson Lesson { get; set; }
    public UserLessonStatus Status { get; set; }
    public List<Guid> CompleatedWordsIds { get; set; } = new();
}