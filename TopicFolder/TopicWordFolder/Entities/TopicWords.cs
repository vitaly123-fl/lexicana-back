using lexicana.Common.Entities;
using lexicana.TopicFolder.Entities;
using lexicana.TopicFolder.WordFolder.Entities;

namespace lexicana.TopicFolder.TopicWordFolder.Entities;

public class TopicWord: BaseEntity
{
    public int Order { get; set; }
    public Guid TopicId { get; set; }
    public Topic Topic { get; set; }
    public Guid WordId { get; set; }
    public Word Word { get; set; }
}