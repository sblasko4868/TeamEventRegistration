using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TeamEventRegistration.Core.Enumerations;

namespace TeamEventRegistration.Core.Models
{
    public class TeamEventRegistrationDbContext : IdentityDbContext<Member>
    {
        public TeamEventRegistrationDbContext(DbContextOptions<TeamEventRegistrationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<RegistrationRequirement> RegistrationRequirements { get; set; }
        public DbSet<EventRegistrationRequirement> EventRegistrationRequirements { get; set; }
        public DbSet<EventManager> EventManagers { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<EventActivity> EventActivities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<Event>().ToTable(nameof(Event));
            modelBuilder.Entity<Member>().ToTable(nameof(Member));
            modelBuilder.Entity<Registration>().ToTable(nameof(Registration));
            modelBuilder.Entity<Team>().ToTable(nameof(Team));
            modelBuilder.Entity<RegistrationRequirement>().ToTable(nameof(RegistrationRequirement));
            modelBuilder.Entity<EventRegistrationRequirement>().ToTable(nameof(EventRegistrationRequirement));
            modelBuilder.Entity<EventManager>().ToTable(nameof(EventManager));
            modelBuilder.Entity<Activity>().ToTable(nameof(Activity));
            modelBuilder.Entity<EventActivity>().ToTable(nameof(EventActivity));
            
            #region Seed Data
            modelBuilder.Entity<Member>().HasData(
                new Member()
                {
                    Id = "4fc18efa-7010-4fe1-8d4d-c296246e790a",
                    UserName = "sblasko4868@gmail.com",
                    Email = "sblasko4868@gmail.com",
                    FirstName = "Sean",
                    LastName = "Blasko",
                    PhoneNumber = "330-958-4868",
                    StreetAddress = "4325 Conestoga Trail",
                    City = "Copley",
                    State = "Ohio",
                    ZipCode = "44321",
                    NormalizedEmail = "sblasko4868@gmail.com".ToUpper(),
                    NormalizedUserName = "sblasko4868@gmail.com".ToUpper(),
                    SelfCreation = true
                }
            );
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "4d59867d-9af5-4bc0-9704-27a39ce9db2c",
                    Name = AuthorizationEnumerations.Roles.SystemAdministrator.ToString(),
                    NormalizedName = AuthorizationEnumerations.Roles.SystemAdministrator.ToString().ToUpper()
                },
                new IdentityRole
                {
                    Id = "b465c7e3-1096-4007-a8f3-21cc0adbb8da",
                    Name = AuthorizationEnumerations.Roles.EventAdministrator.ToString(),
                    NormalizedName = AuthorizationEnumerations.Roles.EventAdministrator.ToString().ToUpper()
                }
            );
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                // sblasko4868@gmail.com = System Administrator
                new IdentityUserRole<string>
                {
                    RoleId = "4d59867d-9af5-4bc0-9704-27a39ce9db2c",
                    UserId = "4fc18efa-7010-4fe1-8d4d-c296246e790a"
                }
            );
            modelBuilder.Entity<RegistrationRequirement>().HasData(new RegistrationRequirement()
            {
                Id = 1,
                Name = RegistrationRequirementEnumerations.RegistrationRequirementNames.BeerOlympicsRegistrationSignature.ToString(),
                Description = "Beer Olympics Registration Zoho form Signature"
            });
            #endregion
            
            modelBuilder.Entity<RegistrationRequirement>()
                .HasIndex(r => r.Name)
                    .IsUnique();

            modelBuilder.Entity<Registration>()
                .HasIndex(r => r.RegistrationExternalId)
                    .IsUnique();

            modelBuilder.Entity<EventManager>()
                .HasKey(x => new { x.MemberId, x.EventId });

            modelBuilder.Entity<EventRegistrationRequirement>()
                .HasKey(x => new { x.EventId, x.RegistrationRequirementId });

            modelBuilder.Entity<Team>()
                .HasIndex(t => new { t.EventId, t.TeamName })
                    .IsUnique();

            modelBuilder.Entity<EventActivity>()
                .HasKey(x => new { x.ActivityId, x.EventId });
        }
    }
}
