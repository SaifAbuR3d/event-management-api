using EventManagement.Application.Identity.Login;
using EventManagement.Domain.Models;

namespace EventManagement.Application.Identity;

public interface IIdentityManager
{
    Task<string> AuthenticateCredentials(string email, string password);
    Task<Attendee> RegisterAttendee(string email, string password, string firstName, string lastName,
        string gender, DateTime dateOfBirth);
    Task<Organizer> RegisterOrganizer(string email, string password, string firstName, string lastName,
        string? companyName);
    Task<Admin> RegisterAdmin(string email, string password, string firstName, string lastName);
}
