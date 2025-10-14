using lexicana.Common.Enums;
using lexicana.Common.Entities;
using lexicana.UserFolder.ProviderFolder.Entities;

namespace lexicana.UserFolder.Entities;

public class User : BaseEntity
{
    public string Email { get; set; }
    public string DisplayName { get; set; }
    public string PhotoUrl { get; set; }
    public List<UserProvider> Providers { get; set; } = new();
    public Language? Language { get; set; } = null;
} 