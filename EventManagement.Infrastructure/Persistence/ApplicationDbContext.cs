using Microsoft.EntityFrameworkCore;
using EventManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EventManagement.Domain.Entities;


namespace EventManagement.Infrastructure.Persistence;

public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>

{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Admin> Admins { get; set; } = default!;
    public DbSet<Organizer> Organizers { get; set; } = default!;
    public DbSet<Attendee> Attendees { get; set; } = default!;

    public DbSet<EventImage> EventImages { get; set; } = default!;
    public DbSet<ProfileImage> ProfileImages { get; set; } = default!;
    public DbSet<UserImage> UserImages { get; set; } = default!;
    public DbSet<CityImage> CityImages { get; set; } = default!;
    public DbSet<Document> Documents { get; set; } = default!;

    public DbSet<City> Cities { get; set; } = default!;
    public DbSet<Profile> Profiles { get; set; } = default!;
    public DbSet<Event> Events { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<Ticket> Tickets { get; set; } = default!;

    public DbSet<RegistrationRequest> RegistrationRequests { get; set; } = default!;
    public DbSet<IdentityVerificationRequest> IdentityVerificationRequests { get; set; } = default!;

    public DbSet<Following> Followings { get; set; } = default!;

    public DbSet<Booking> Bookings { get; set; } = default!;
    public DbSet<Review> Reviews { get; set; } = default!;
    public DbSet<Report> Reports { get; set; } = default!;



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureApplicationUser(modelBuilder);

        SetPrecisionForFloatingPointTypes(modelBuilder);

        ConfigureDeleteBehavior(modelBuilder);

       //ConfigureEnumToString(modelBuilder); if needed
    }

    private static void ConfigureEnumToString(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attendee>()
            .Property(a => a.Gender)
            .HasConversion<string>();

        modelBuilder.Entity<Document>()
            .Property(d => d.DocumentType)
            .HasConversion<string>();

        modelBuilder.Entity<IdentityVerificationRequest>()
            .Property(ivr => ivr.Status)
            .HasConversion<string>();

        modelBuilder.Entity<RegistrationRequest>()
            .Property(rr => rr.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Report>()
            .Property(r => r.Status)
            .HasConversion<string>();
    }

    private void ConfigureApplicationUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>()
            .HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<Admin>(a => a.UserId)
            .IsRequired();

        modelBuilder.Entity<Organizer>()
            .HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<Organizer>(o => o.UserId)
            .IsRequired();

        modelBuilder.Entity<Attendee>()
            .HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<Attendee>(a => a.UserId)
            .IsRequired();

        modelBuilder.Entity<UserImage>()
            .HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<UserImage>(ui => ui.UserId)
            .IsRequired();

        modelBuilder.Entity<IdentityVerificationRequest>()
            .HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<IdentityVerificationRequest>(ivr => ivr.UserId)
            .IsRequired();
    }

    private void SetPrecisionForFloatingPointTypes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>()
                    .Property(t => t.Price)
                    .HasPrecision(18, 2);

        // set precision for location properties
        modelBuilder.Entity<Event>()
                    .Property(h => h.Latitude)
                    .HasPrecision(8, 6);
        modelBuilder.Entity<Event>()
                    .Property(h => h.Longitude)
                    .HasPrecision(9, 6);
    }

    private void ConfigureDeleteBehavior(ModelBuilder modelBuilder)
    {
        // Set default delete behavior to Restrict

        var cascadeDeleteFKs = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior != DeleteBehavior.Restrict);

        foreach (var fk in cascadeDeleteFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

    }
}
