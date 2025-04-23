using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IdentityMigrations2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserIdentityRole");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserIdentityUserRole<string>");

            migrationBuilder.CreateTable(
                name: "ApplicationUserIdentityRole",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(type: "text", nullable: false),
                    RolesId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserIdentityRole", x => new { x.ApplicationUserId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserIdentityRole_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserIdentityRole_Users_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserIdentityRole_RolesId",
                table: "ApplicationUserIdentityRole",
                column: "RolesId");
        }
    }
}
