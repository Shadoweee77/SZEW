using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SZEW.Migrations
{
    /// <inheritdoc />
    public partial class Add_Missing_Models_And_Relationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Jobs_WorkshopJobId",
                table: "Tasks");

            migrationBuilder.AlterColumn<int>(
                name: "WorkshopJobId",
                table: "Tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SaleDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DocumentType = table.Column<int>(type: "integer", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DocumentIssuerId = table.Column<int>(type: "integer", nullable: false),
                    RelatedJobId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleDocuments_Jobs_RelatedJobId",
                        column: x => x.RelatedJobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleDocuments_Users_DocumentIssuerId",
                        column: x => x.DocumentIssuerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SparePartsOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrdererId = table.Column<int>(type: "integer", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SparePartsOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SparePartsOrders_Users_OrdererId",
                        column: x => x.OrdererId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ToolsOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrdererId = table.Column<int>(type: "integer", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolsOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToolsOrders_Users_OrdererId",
                        column: x => x.OrdererId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ToolsRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RequesterId = table.Column<int>(type: "integer", nullable: false),
                    VerifierId = table.Column<int>(type: "integer", nullable: true),
                    Verified = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolsRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToolsRequests_Users_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ToolsRequests_Users_VerifierId",
                        column: x => x.VerifierId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SpareParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpareParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpareParts_SparePartsOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "SparePartsOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tools_ToolsOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "ToolsOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaleDocuments_DocumentIssuerId",
                table: "SaleDocuments",
                column: "DocumentIssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDocuments_RelatedJobId",
                table: "SaleDocuments",
                column: "RelatedJobId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpareParts_OrderId",
                table: "SpareParts",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SparePartsOrders_OrdererId",
                table: "SparePartsOrders",
                column: "OrdererId");

            migrationBuilder.CreateIndex(
                name: "IX_Tools_OrderId",
                table: "Tools",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ToolsOrders_OrdererId",
                table: "ToolsOrders",
                column: "OrdererId");

            migrationBuilder.CreateIndex(
                name: "IX_ToolsRequests_RequesterId",
                table: "ToolsRequests",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_ToolsRequests_VerifierId",
                table: "ToolsRequests",
                column: "VerifierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Jobs_WorkshopJobId",
                table: "Tasks",
                column: "WorkshopJobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Jobs_WorkshopJobId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "SaleDocuments");

            migrationBuilder.DropTable(
                name: "SpareParts");

            migrationBuilder.DropTable(
                name: "Tools");

            migrationBuilder.DropTable(
                name: "ToolsRequests");

            migrationBuilder.DropTable(
                name: "SparePartsOrders");

            migrationBuilder.DropTable(
                name: "ToolsOrders");

            migrationBuilder.AlterColumn<int>(
                name: "WorkshopJobId",
                table: "Tasks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Jobs_WorkshopJobId",
                table: "Tasks",
                column: "WorkshopJobId",
                principalTable: "Jobs",
                principalColumn: "Id");
        }
    }
}
