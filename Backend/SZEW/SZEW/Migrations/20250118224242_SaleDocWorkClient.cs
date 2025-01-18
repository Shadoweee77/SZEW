using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SZEW.Migrations
{
    /// <inheritdoc />
    public partial class SaleDocWorkClient : Migration
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

            migrationBuilder.RenameColumn(
                name: "WorkshopIndividualClient_Name",
                table: "Clients",
                newName: "BusinessName");
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

            migrationBuilder.RenameColumn(
                name: "BusinessName",
                table: "Clients",
                newName: "WorkshopIndividualClient_Name");
        }
    }
}
