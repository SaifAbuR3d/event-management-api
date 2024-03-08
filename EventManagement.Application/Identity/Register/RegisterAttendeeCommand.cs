using EventManagement.Application.Common;
using EventManagement.Domain.Abstractions.Repositories;
using EventManagement.Domain.Enums;
using EventManagement.Domain.Models;
using FluentValidation;
using MediatR;

namespace EventManagement.Application.Identity.Register;


public record RegisterAttendeeCommand(string Email, string UserName, string Password,
    string FirstName, string LastName, Gender Gender, DateTime DateOfBirth) : IRequest<RegisterAttendeeResponse>;

public class RegisterAttendeeCommandValidator : AbstractValidator<RegisterAttendeeCommand>
{
    public RegisterAttendeeCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required")
            .Length(3, 20).WithMessage("Username must be between 2 and 20 characters");

        // password requirements which are specified in IdentityConfigurations.cs
        // are validated with Identity framework
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .ValidName(); 

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .ValidName();

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Gender is only Male or Female"); 

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required")
            .ValidDateOfBirth();
    }
}

public class RegisterAttendeeCommandHandler(IIdentityManager identityManager, 
    IAttendeeRepository attendeeRepository, IUnitOfWork unitOfWork) 
    : IRequestHandler<RegisterAttendeeCommand, RegisterAttendeeResponse>
{
    public async Task<RegisterAttendeeResponse> Handle(RegisterAttendeeCommand request,
        CancellationToken cancellationToken)
    {
        var userId = await identityManager.RegisterUser(request.Email, request.UserName, 
               request.Password, request.FirstName, request.LastName, Role.Attendee.ToString());

        var attendee = new Attendee
        {
            UserId = userId,
            DateOfBirth = DateOnly.FromDateTime(request.DateOfBirth),
            Gender = request.Gender
        }; 

        var attendeeEntity = await attendeeRepository.AddAttendeeAsync(attendee, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new RegisterAttendeeResponse("Registration successful", attendeeEntity.Id);
    }
}
public record RegisterAttendeeResponse(string Message, int AttendeeId);
