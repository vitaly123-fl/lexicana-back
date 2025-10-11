namespace lexicana.Common.Entities;

public class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
}