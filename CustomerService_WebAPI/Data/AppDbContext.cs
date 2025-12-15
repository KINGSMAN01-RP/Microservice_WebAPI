
using CustomerService_WebAPI.Models;
using CustomerService_WebAPI.Models.Customer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CustomerService_WebAPI.Data
{
    
    public class AppDbContext : DbContext
    {
        // DbSet properties map to your database tables
        public DbSet<Customers> Customers { get; set; }

        // Add other entities here (e.g., public DbSet<Ticket> Tickets { get; set; })

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API Configuration

            // Configure the Customer entity
            modelBuilder.Entity<Customers>(entity =>
            {
                // Set the primary key
                entity.HasKey(c => c.Id);

                // Configure required fields and maximum lengths
                entity.Property(c => c.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(c => c.LastName).IsRequired().HasMaxLength(50);

                // Configure unique constraint for Email
                entity.HasIndex(c => c.Email).IsUnique();
                entity.Property(c => c.Email).IsRequired().HasMaxLength(100);

                // Default values for dates
                entity.Property(c => c.DateCreated)
                      .HasDefaultValueSql("GETUTCDATE()"); // Uses DB function for UTC time
            });

        }
    }
}