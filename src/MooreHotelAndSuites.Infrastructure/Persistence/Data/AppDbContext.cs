using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MooreHotelAndSuites.Domain.Entities;
using MooreHotelAndSuites.Infrastructure.Identity;

namespace MooreHotelAndSuites.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

    
        public DbSet<Hotel> Hotels => Set<Hotel>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<Guest> Guests => Set<Guest>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Amenity> Amenities => Set<Amenity>();
        public DbSet<RoomAmenity> RoomAmenities => Set<RoomAmenity>();
        public DbSet<RoomImage> RoomImages => Set<RoomImage>();
        public DbSet<RoomReview> RoomReviews => Set<RoomReview>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();


  protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);


    builder.Entity<RoomAmenity>()
        .HasKey(ra => new { ra.RoomId, ra.AmenityId });

    builder.Entity<RoomAmenity>()
        .HasOne(ra => ra.Room)
        .WithMany(r => r.RoomAmenities)
        .HasForeignKey(ra => ra.RoomId);

    builder.Entity<RoomAmenity>()
        .HasOne(ra => ra.Amenity)
        .WithMany(a => a.RoomAmenities)
        .HasForeignKey(ra => ra.AmenityId);

   
    builder.Entity<RoomImage>()
        .HasIndex(i => new { i.RoomId, i.DisplayOrder })
        .IsUnique();

  builder.Entity<RoomImage>()
    .HasOne(i => i.Room)
    .WithMany(r => r.Images)
    .HasForeignKey(i => i.RoomId)
    .OnDelete(DeleteBehavior.Cascade);

builder.Entity<Booking>()
    .HasOne(b => b.Room)
    .WithMany()
    .HasForeignKey(b => b.RoomId)
    .OnDelete(DeleteBehavior.Restrict);


  
builder.Entity<RoomReview>()
    .HasOne(r => r.Room)
    .WithMany(room => room.Reviews)
    .HasForeignKey(r => r.RoomId)
    .OnDelete(DeleteBehavior.Cascade);

}

    }
}
