namespace EventManagement.Domain.Models;

public abstract class Entity
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastModified { get; set; }
}
