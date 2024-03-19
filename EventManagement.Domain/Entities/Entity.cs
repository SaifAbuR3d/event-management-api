namespace EventManagement.Domain.Entities;

public abstract class Entity
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastModified { get; set; }
}
