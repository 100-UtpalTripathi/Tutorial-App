using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorialApp.Migrations
{
    /// <inheritdoc />
    public partial class Minorchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "UserCredentials",
                newName: "PasswordHashKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHashKey",
                table: "UserCredentials",
                newName: "PasswordHash");
        }
    }
}
