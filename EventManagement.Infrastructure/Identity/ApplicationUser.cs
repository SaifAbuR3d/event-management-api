using EventManagement.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace EventManagement.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<int>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}
