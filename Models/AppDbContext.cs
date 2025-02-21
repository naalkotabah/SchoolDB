using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;

namespace CREDAJAX.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<studint> Studints { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {


        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging(); // Enable sensitive data logging for debugging
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

      
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.id);
                entity.Property(u => u.name).IsRequired().HasMaxLength(100);
                entity.Property(u => u.password).IsRequired().HasMaxLength(10);

                entity.HasOne(u => u.Roles)
                      .WithMany(r => r.Users)
                      .HasForeignKey(u => u.RoleId)
                      .OnDelete(DeleteBehavior.Restrict);


            });





            modelBuilder.Entity<Roles>().HasData(
            new Roles { Id = 1, Name = "User" },
            new Roles { Id = 2, Name = "Admin" }
      
        );


            modelBuilder.Entity<User>().HasData(
                   new User { id = 1, name = "User", password = "12345", RoleId = 1 },
                new User { id = 2, name = "Admin", password = "12345", RoleId = 2 }


            );


        }
    }
}

   

