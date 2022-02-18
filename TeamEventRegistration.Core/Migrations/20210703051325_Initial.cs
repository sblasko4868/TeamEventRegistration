using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamEventRegistration.Core.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(nullable: true),
                    RegistrationStart = table.Column<DateTime>(nullable: false),
                    RegistrationEnd = table.Column<DateTime>(nullable: false),
                    EventDisplayDateTime = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    EventPassword = table.Column<string>(nullable: true),
                    MaxTeamMembers = table.Column<int>(nullable: true),
                    EmbedHTML = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Member",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Email = table.Column<string>(maxLength: 256, nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    NickName = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    StreetAddress = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    SelfCreation = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationRequirement",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationRequirement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamName = table.Column<string>(nullable: false),
                    EventId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Team_Event_EventId",
                        column: x => x.EventId,
                        principalSchema: "dbo",
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Member_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "dbo",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_Member_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Member_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Member_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventManager",
                schema: "dbo",
                columns: table => new
                {
                    MemberId = table.Column<string>(nullable: false),
                    EventId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventManager", x => new { x.MemberId, x.EventId });
                    table.ForeignKey(
                        name: "FK_EventManager_Event_EventId",
                        column: x => x.EventId,
                        principalSchema: "dbo",
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventManager_Member_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "dbo",
                        principalTable: "Member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventRegistrationRequirement",
                schema: "dbo",
                columns: table => new
                {
                    RegistrationRequirementId = table.Column<int>(nullable: false),
                    EventId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventRegistrationRequirement", x => new { x.EventId, x.RegistrationRequirementId });
                    table.ForeignKey(
                        name: "FK_EventRegistrationRequirement_Event_EventId",
                        column: x => x.EventId,
                        principalSchema: "dbo",
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventRegistrationRequirement_RegistrationRequirement_RegistrationRequirementId",
                        column: x => x.RegistrationRequirementId,
                        principalSchema: "dbo",
                        principalTable: "RegistrationRequirement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Registration",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<string>(nullable: false),
                    EventId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: true),
                    RegistrationComplete = table.Column<bool>(nullable: false),
                    RegistrationMessage = table.Column<string>(nullable: true),
                    RegistrationData = table.Column<string>(nullable: true),
                    RegistrationExternalId = table.Column<string>(nullable: true),
                    IsCaptain = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Registration_Event_EventId",
                        column: x => x.EventId,
                        principalSchema: "dbo",
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registration_Member_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "dbo",
                        principalTable: "Member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registration_Team_TeamId",
                        column: x => x.TeamId,
                        principalSchema: "dbo",
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4d59867d-9af5-4bc0-9704-27a39ce9db2c", "28812bc0-f95c-4415-9c9e-abdfa3e63dc5", "SystemAdministrator", "SYSTEMADMINISTRATOR" },
                    { "b465c7e3-1096-4007-a8f3-21cc0adbb8da", "a024dfe4-1faa-477d-a9a7-62900beb5202", "EventAdministrator", "EVENTADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Member",
                columns: new[] { "Id", "AccessFailedCount", "City", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NickName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "SelfCreation", "State", "StreetAddress", "TwoFactorEnabled", "UserName", "ZipCode" },
                values: new object[] { "4fc18efa-7010-4fe1-8d4d-c296246e790a", 0, "Copley", "3bfacdfe-34c0-44f4-b3d5-14bdda9cb003", "sblasko4868@gmail.com", false, "Sean", "Blasko", false, null, null, "SBLASKO4868@GMAIL.COM", "SBLASKO4868@GMAIL.COM", null, "330-958-4868", false, "11710be4-4f5d-4ef3-bfbf-c91c84bbf9ef", true, "Ohio", "4325 Conestoga Trail", false, "sblasko4868@gmail.com", "44321" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "RegistrationRequirement",
                columns: new[] { "Id", "Description", "DisplayName", "Name" },
                values: new object[] { 1, "Beer Olympics Registration Zoho form Signature", null, "BeerOlympicsRegistrationSignature" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "4fc18efa-7010-4fe1-8d4d-c296246e790a", "4d59867d-9af5-4bc0-9704-27a39ce9db2c" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "dbo",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "dbo",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "dbo",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "dbo",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "dbo",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_EventManager_EventId",
                schema: "dbo",
                table: "EventManager",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistrationRequirement_RegistrationRequirementId",
                schema: "dbo",
                table: "EventRegistrationRequirement",
                column: "RegistrationRequirementId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "dbo",
                table: "Member",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "dbo",
                table: "Member",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_EventId",
                schema: "dbo",
                table: "Registration",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_MemberId",
                schema: "dbo",
                table: "Registration",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_RegistrationExternalId",
                schema: "dbo",
                table: "Registration",
                column: "RegistrationExternalId",
                unique: true,
                filter: "[RegistrationExternalId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_TeamId",
                schema: "dbo",
                table: "Registration",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationRequirement_Name",
                schema: "dbo",
                table: "RegistrationRequirement",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Team_EventId_TeamName",
                schema: "dbo",
                table: "Team",
                columns: new[] { "EventId", "TeamName" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EventManager",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EventRegistrationRequirement",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Registration",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RegistrationRequirement",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Member",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Team",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Event",
                schema: "dbo");
        }
    }
}
