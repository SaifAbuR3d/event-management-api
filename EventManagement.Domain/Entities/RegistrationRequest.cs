﻿namespace EventManagement.Domain.Entities;

public enum RegistrationStatus
{
    Pending,
    Approved,
    Rejected
}
public class RegistrationRequest : Entity
{
    public int AttendeeId { get; set; }
    public Attendee Attendee { get; set; } = default!;
    public int EventId { get; set; }
    public Event Event { get; set; } = default!;
    public RegistrationStatus Status { get; set; }
}
