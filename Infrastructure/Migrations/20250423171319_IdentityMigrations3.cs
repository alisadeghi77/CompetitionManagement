using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IdentityMigrations3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserIdentityUserRole<string>");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "UserRoles",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_ApplicationUserId",
                table: "UserRoles",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_ApplicationUserId",
                table: "UserRoles",
                column: "ApplicationUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_ApplicationUserId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_ApplicationUserId",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "UserRoles");

            migrationBuilder.CreateTable(
                name: "ApplicationUserIdentityUserRole<string>",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(type: "text", nullable: false),
                    RolesUserId = table.Column<string>(type: "text", nullable: false),
                    RolesRoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserIdentityUserRole<string>", x => new { x.ApplicationUserId, x.RolesUserId, x.RolesRoleId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserIdentityUserRole<string>_UserRoles_RolesUser~",
                        columns: x => new { x.RolesUserId, x.RolesRoleId },
                        principalTable: "UserRoles",
                        principalColumns: new[] { "UserId", "RoleId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserIdentityUserRole<string>_Users_ApplicationUs~",
                        column: x => x.ApplicationUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserIdentityUserRole<string>_RolesUserId_RolesRo~",
                table: "ApplicationUserIdentityUserRole<string>",
                columns: new[] { "RolesUserId", "RolesRoleId" });
        }
    }
}
