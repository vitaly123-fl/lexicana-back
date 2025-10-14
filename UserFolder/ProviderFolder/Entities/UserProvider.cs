using lexicana.Common.Entities;
using lexicana.UserFolder.Entities;

namespace lexicana.UserFolder.ProviderFolder.Entities;

public class UserProvider : BaseEntity
{
    public string FirebaseId { get; set; }
    public string Provider { get; set; }
    public User User { get; set; }
}