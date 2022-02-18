using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamEventRegistration.Core.Migrations
{
    public partial class EventActivities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DisplayEventActivities",
                schema: "dbo",
                table: "Event",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Activity",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityName = table.Column<string>(nullable: true),
                    ActivityDescription = table.Column<string>(nullable: true),
                    ActivityRules = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventActivity",
                schema: "dbo",
                columns: table => new
                {
                    EventId = table.Column<int>(nullable: false),
                    ActivityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventActivity", x => new { x.ActivityId, x.EventId });
                    table.ForeignKey(
                        name: "FK_EventActivity_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalSchema: "dbo",
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventActivity_Event_EventId",
                        column: x => x.EventId,
                        principalSchema: "dbo",
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d59867d-9af5-4bc0-9704-27a39ce9db2c",
                column: "ConcurrencyStamp",
                value: "44433bf4-7e07-4901-bd92-2def36edb69e");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b465c7e3-1096-4007-a8f3-21cc0adbb8da",
                column: "ConcurrencyStamp",
                value: "0ed5931d-e628-476e-aab4-2657dc64db19");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Member",
                keyColumn: "Id",
                keyValue: "4fc18efa-7010-4fe1-8d4d-c296246e790a",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "3b3979b3-bfc3-4814-80d9-c0f573135ab4", "fd22e75d-8ae5-4e8a-be3a-83c8157bc8bb" });

            migrationBuilder.CreateIndex(
                name: "IX_EventActivity_EventId",
                schema: "dbo",
                table: "EventActivity",
                column: "EventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventActivity",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Activity",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "DisplayEventActivities",
                schema: "dbo",
                table: "Event");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d59867d-9af5-4bc0-9704-27a39ce9db2c",
                column: "ConcurrencyStamp",
                value: "28812bc0-f95c-4415-9c9e-abdfa3e63dc5");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b465c7e3-1096-4007-a8f3-21cc0adbb8da",
                column: "ConcurrencyStamp",
                value: "a024dfe4-1faa-477d-a9a7-62900beb5202");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Member",
                keyColumn: "Id",
                keyValue: "4fc18efa-7010-4fe1-8d4d-c296246e790a",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "3bfacdfe-34c0-44f4-b3d5-14bdda9cb003", "11710be4-4f5d-4ef3-bfbf-c91c84bbf9ef" });
        }
    }
}
