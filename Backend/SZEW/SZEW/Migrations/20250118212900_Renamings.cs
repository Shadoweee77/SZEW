using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SZEW.Migrations
{
    /// <inheritdoc />
    public partial class Renamings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Verified",
                table: "ToolsRequests",
                newName: "Accepted");

            migrationBuilder.RenameColumn(
                name: "Complete",
                table: "Jobs",
                newName: "IsComplete");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Accepted",
                table: "ToolsRequests",
                newName: "Verified");

            migrationBuilder.RenameColumn(
                name: "IsComplete",
                table: "Jobs",
                newName: "Complete");
        }
    }
}
