using lexicana.Common.Enums;
using lexicana.Common.Entities;

namespace lexicana.UserFolder.Entities;

public class User : BaseEntity
{
    public string Email { get; set; }
    public string DisplayName { get; set; }
    public string PhotoUrl { get; set; }
    public string Provider { get; set; }
    public string FirebaseId { get; set; }
    public Language? Language { get; set; } = null;
} 