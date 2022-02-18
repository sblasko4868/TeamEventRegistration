using TeamEventRegistration.Core.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace TeamEventRegistration.Core.Infrastructure
{
    public class DbInitializer
    {
        private readonly TeamEventRegistrationDbContext _context;
        private readonly UserManager<Member> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public DbInitializer(TeamEventRegistrationDbContext context, 
            UserManager<Member> userManager, 
            RoleManager<IdentityRole> roleManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _env = webHostEnvironment;
        }

        public async Task SetIdentityInsert(string table, bool on)
        {
            _context.Database.OpenConnection();
            if (!on)
                await _context.SaveChangesAsync();
            await _context.Database.ExecuteSqlRawAsync(@"SET IDENTITY_INSERT [" + table + "] " + (on ? "ON" : "OFF"));
            _context.Database.CloseConnection();
        }

        public async Task InitializeAsync()
        {
            if (_env.IsEnvironment("Development-Local"))
            {
                _context.Database.EnsureCreated();

                #region Member Seed Data
                var seedMembers = new List<Member>()
                {
                    // Add members who will not have passwords
                };

                var seedMembersWithPasswords = new List<Member>()
                {
                    new Member()
                    {
                        Id = "b1ece4ec-eefb-43d4-becf-a90b48c22cb0",
                        FirstName = "Initialization",
                        LastName = "TeamMember",
                        Email = "test1@email.com",
                        StreetAddress = "123 Initialization",
                        City = "InitializationCity",
                        State = "InitializationState",
                        ZipCode = "InitZ",
                        PhoneNumber = "123-456-7890",
                        UserName = "test1@email.com",
                        EmailConfirmed = true,
                        NormalizedEmail = "test1@email.com".ToUpper(),
                        NormalizedUserName = "test1@email.com".ToUpper(),
                        SelfCreation = true
                    },
                    new Member()
                    {
                        Id = "40358a8d-5375-4448-8e30-35a9a5224f3d",
                        FirstName = "Initialization",
                        LastName = "EventRegistrant",
                        Email = "test2@email.com",
                        StreetAddress = "123 Initialization",
                        City = "InitializationCity",
                        State = "InitializationState",
                        ZipCode = "InitZ",
                        PhoneNumber = "123-456-7890",
                        UserName = "test2@email.com",
                        EmailConfirmed = true,
                        NormalizedEmail = "test2@email.com".ToUpper(),
                        NormalizedUserName = "test2@email.com".ToUpper(),
                        SelfCreation = false
                    },
                    new Member()
                    {
                        Id = "2b66b956-907a-4c7a-8da0-5799764a7c0f",
                        FirstName = "Initialization",
                        LastName = "Unregistered",
                        Email = "test3@email.com",
                        StreetAddress = "123 Initialization",
                        City = "InitializationCity",
                        State = "InitializationState",
                        ZipCode = "InitZ",
                        PhoneNumber = "123-456-7890",
                        UserName = "test3@email.com",
                        EmailConfirmed = true,
                        NormalizedEmail = "test3@email.com".ToUpper(),
                        NormalizedUserName = "test3@email.com".ToUpper(),
                        SelfCreation = true
                    },
                    // Team Captain
                    new Member()
                    {
                        Id = "ef2982b8-c44b-4884-9c06-feb71a981ed5",
                        UserName = "test4@gmail.com",
                        Email = "test4@gmail.com",
                        FirstName = "Team",
                        LastName = "Captain-User",
                        PhoneNumber = "123-456-7890",
                        StreetAddress = "1234 Main St",
                        City = "Akron",
                        State = "Ohio",
                        ZipCode = "12345",
                        EmailConfirmed = true,
                        NormalizedEmail = "test4@gmail.com".ToUpper(),
                        NormalizedUserName = "test4@gmail.com".ToUpper(),
                        SelfCreation = true
                    },
                    // Event Manager
                    new Member()
                    {
                        Id = "8082cd1d-04de-4f0f-8521-9a7e0fa6ff3f",
                        UserName = "test5@gmail.com",
                        Email = "test5@gmail.com",
                        FirstName = "Event",
                        LastName = "Manager-User",
                        PhoneNumber = "123-456-7890",
                        StreetAddress = "1234 Main St",
                        City = "Akron",
                        State = "Ohio",
                        ZipCode = "12345",
                        EmailConfirmed = true,
                        NormalizedEmail = "test5@gmail.com".ToUpper(),
                        NormalizedUserName = "test5@gmail.com".ToUpper(),
                        SelfCreation = true
                    },
                    // Registered with team
                    new Member()
                    {
                        Id = "a1e7528b-92c6-4c98-a3f1-5280b1d6ccb8",
                        UserName = "test6@gmail.com",
                        Email = "test6@gmail.com",
                        FirstName = "Registered",
                        LastName = "Team-User",
                        PhoneNumber = "123-456-7890",
                        StreetAddress = "1234 Main St",
                        City = "Akron",
                        State = "Ohio",
                        ZipCode = "12345",
                        EmailConfirmed = true,
                        NormalizedEmail = "test6@gmail.com".ToUpper(),
                        NormalizedUserName = "test6@gmail.com".ToUpper(),
                        SelfCreation = true
                    },
                    // Not registered with team
                    new Member()
                    {
                        Id = "6aac8124-b8e3-4aee-a8d5-b264e4197a83",
                        UserName = "test7@gmail.com",
                        Email = "test7@gmail.com",
                        FirstName = "Not-Registered",
                        LastName = "Team-User",
                        PhoneNumber = "123-456-7890",
                        StreetAddress = "1234 Main St",
                        City = "Akron",
                        State = "Ohio",
                        ZipCode = "12345",
                        EmailConfirmed = true,
                        NormalizedEmail = "test7@gmail.com".ToUpper(),
                        NormalizedUserName = "test7@gmail.com".ToUpper(),
                        SelfCreation = true
                    },
                    // Event Administrator
                    new Member()
                    {
                        Id = "51b15f95-4b86-41df-b2e8-bb5cfac0556e",
                        UserName = "test8@gmail.com",
                        Email = "test8@gmail.com",
                        FirstName = "Event",
                        LastName = "Administrator",
                        PhoneNumber = "123-456-7890",
                        StreetAddress = "1234 Main St",
                        City = "Akron",
                        State = "Ohio",
                        ZipCode = "12345",
                        EmailConfirmed = true,
                        NormalizedEmail = "test8@gmail.com".ToUpper(),
                        NormalizedUserName = "test8@gmail.com".ToUpper(),
                        SelfCreation = true
                    }
                };

                PasswordHasher<Member> passwordHasher = new PasswordHasher<Member>();
                var generalPassword = "P@ssword1";
                foreach (var seedMemberWithPassword in seedMembersWithPasswords)
                {
                    seedMemberWithPassword.PasswordHash = passwordHasher.HashPassword(seedMemberWithPassword, generalPassword);
                }
                var developerAccount = _context.Members.Find("4fc18efa-7010-4fe1-8d4d-c296246e790a");
                developerAccount.PasswordHash = passwordHasher.HashPassword(developerAccount, "b57avS!E.DqBHhL");
                developerAccount.EmailConfirmed = true;
                _context.Members.Update(developerAccount);

                foreach (var member in seedMembers.Concat(seedMembersWithPasswords)) {
                    var memberEntity = await _context.Members.FirstOrDefaultAsync(m => m.Id == member.Id);
                    if (memberEntity == null)
                        await _context.Members.AddAsync(member);
                    else
                        _context.Entry(memberEntity).CurrentValues.SetValues(member);
                }
                #endregion
                await _context.SaveChangesAsync();

                #region UserRole Seed Data
                var userRoles = new List<IdentityUserRole<string>>()
                {
                    // Event Administrator
                    new IdentityUserRole<string>
                    {
                        UserId = "51b15f95-4b86-41df-b2e8-bb5cfac0556e",
                        RoleId = "b465c7e3-1096-4007-a8f3-21cc0adbb8da"
                    }
                };
                foreach (var userRole in userRoles)
                {
                    var userRoleEntity = _context.UserRoles
                        .AsNoTracking()
                        .Where(ur => ur.RoleId == userRole.RoleId && ur.UserId == userRole.UserId)
                        .FirstOrDefault();
                    if (userRoleEntity == null)
                        await _context.UserRoles.AddAsync(userRole);
                    else
                        _context.Entry(userRoleEntity).CurrentValues.SetValues(userRole);
                }
                #endregion
                await _context.SaveChangesAsync();

                _context.Database.OpenConnection();
                await _context.Database.ExecuteSqlRawAsync(@"SET IDENTITY_INSERT [dbo].[Event] ON");
                #region Event Seed Data
                var events = new List<Event>()
                {
                    new Event()
                    {
                        Id = 1,
                        Active = true,
                        EventName = "Initialization Event",
                        EventPassword = "password",
                        RegistrationStart = new DateTime(2021, 1, 1),
                        RegistrationEnd = new DateTime(2021, 12, 31),
                        EventDisplayDateTime = new DateTime(2021, 7, 1, 12, 0, 0),
                        MaxTeamMembers = 3
                    }
                };
                foreach (var _event in events)
                {
                    var eventEntity = await _context.Events.AsNoTracking().FirstOrDefaultAsync(e => e.Id == _event.Id);
                    if (eventEntity == null)
                        await _context.Events.AddAsync(_event);
                    else
                        _context.Entry(eventEntity).CurrentValues.SetValues(_event);
                }
                #endregion
                await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlRawAsync(@"SET IDENTITY_INSERT [dbo].[Event] OFF");
                _context.Database.CloseConnection();

                #region EventManager Seed Data
                var eventManagers = new List<EventManager>() {
                    new EventManager
                    {
                        EventId = 1,
                        MemberId = "8082cd1d-04de-4f0f-8521-9a7e0fa6ff3f"
                    } 
                };
                foreach (var eventManager in eventManagers)
                {
                    var eventManagerEntity = _context.EventManagers
                        .AsNoTracking()
                        .Where(em => em.EventId == eventManager.EventId && em.MemberId == eventManager.MemberId)
                        .FirstOrDefault();
                    if (eventManagerEntity == null)
                        await _context.EventManagers.AddAsync(eventManager);
                    else
                        _context.Entry(eventManagerEntity).CurrentValues.SetValues(eventManager);
                }
                #endregion
                await _context.SaveChangesAsync();

                _context.Database.OpenConnection();
                await _context.Database.ExecuteSqlRawAsync(@"SET IDENTITY_INSERT [dbo].[Team] ON");
                #region Team Seed Data
                var teams = new List<Team>()
                {
                    new Team()
                    {
                        Id = 1,
                        EventId = 1,
                        TeamName = "Initialization Team"
                    },
                    new Team()
                    {
                        Id = 2,
                        EventId = 1,
                        TeamName = "Team with Captain"
                    }
                };
                foreach (var team in teams)
                {
                    var teamEntity = await _context.Teams.AsNoTracking().FirstOrDefaultAsync(t => t.Id == team.Id);
                    if (teamEntity == null)
                        await _context.Teams.AddAsync(team);
                    else
                        _context.Entry(teamEntity).CurrentValues.SetValues(team);
                }
                #endregion
                await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlRawAsync(@"SET IDENTITY_INSERT [dbo].[Team] OFF");
                _context.Database.CloseConnection();

                _context.Database.OpenConnection();
                await _context.Database.ExecuteSqlRawAsync(@"SET IDENTITY_INSERT [dbo].[Registration] ON");
                #region Registration Seed Data
                var registrations = new List<Registration>()
                {
                    new Registration()
                    {
                        Id = 1,
                        TeamId = 1,
                        EventId = 1,
                        MemberId = "b1ece4ec-eefb-43d4-becf-a90b48c22cb0"
                    },
                    new Registration()
                    {
                        Id = 2,
                        EventId = 1,
                        MemberId = "40358a8d-5375-4448-8e30-35a9a5224f3d"
                    },
                    new Registration()
                    {
                        Id = 3,
                        EventId = 1,
                        TeamId = 2,
                        MemberId = "ef2982b8-c44b-4884-9c06-feb71a981ed5",
                        IsCaptain = true
                    },
                    new Registration()
                    {
                        Id = 4,
                        EventId = 1,
                        MemberId = "6aac8124-b8e3-4aee-a8d5-b264e4197a83",
                        IsCaptain = false
                    },
                    new Registration()
                    {
                        Id = 5,
                        EventId = 1,
                        TeamId = 2,
                        MemberId = "a1e7528b-92c6-4c98-a3f1-5280b1d6ccb8",
                        IsCaptain = false
                    }
                };
                foreach (var registration in registrations)
                {
                    var registrationEntity = await _context.Registrations.AsNoTracking().FirstOrDefaultAsync(r => r.Id == registration.Id);
                    if (registrationEntity == null)
                        await _context.Registrations.AddAsync(registration);
                    else
                        _context.Entry(registrationEntity).CurrentValues.SetValues(registration);
                }
                #endregion
                await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlRawAsync(@"SET IDENTITY_INSERT [dbo].[Registration] OFF");
                _context.Database.CloseConnection();

                #region EventRegistrationRequirements Seed Data
                var eventRegistrationRequirements = new List<EventRegistrationRequirement>()
                {
                    new EventRegistrationRequirement()
                    {
                        EventId = 1,
                        RegistrationRequirementId = 1
                    }
                };
                foreach (var eventRegistrationRequirement in eventRegistrationRequirements)
                {
                    var eventRegistrationRequirementEntity = _context.EventRegistrationRequirements
                        .AsNoTracking()
                        .Where(err => err.EventId == eventRegistrationRequirement.EventId && 
                                err.RegistrationRequirementId == eventRegistrationRequirement.RegistrationRequirementId)
                        .FirstOrDefault();
                    if (eventRegistrationRequirementEntity == null)
                        await _context.EventRegistrationRequirements.AddAsync(eventRegistrationRequirement);
                    else
                        _context.Entry(eventRegistrationRequirementEntity).CurrentValues.SetValues(eventRegistrationRequirement);
                }
                #endregion
                await _context.SaveChangesAsync();
            }
        }
    }
}
