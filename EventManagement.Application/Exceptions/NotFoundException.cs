namespace EventManagement.Application.Exceptions;
public class NotFoundException : CustomException
{
    public NotFoundException(string name, object key)
        : base($"Entity '{name}' ({key}) was not found.")
    { }

    public NotFoundException(string name)
        : base($"Entity '{name}' was not found.")
    { }

    // keyName is the name of the key, e.g. "UserId", "OrganizerId", ...
    public NotFoundException(string name, string keyName, object key)
        : base($"Entity '{name}' with {keyName} ({key}) was not found.")
    { }
}
