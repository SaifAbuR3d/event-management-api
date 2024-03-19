using Microsoft.AspNetCore.Identity;

namespace EventManagement.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<int>
{
    public ApplicationUser(string firstName, string lastName, 
        string email, string userName) : base(userName)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}
