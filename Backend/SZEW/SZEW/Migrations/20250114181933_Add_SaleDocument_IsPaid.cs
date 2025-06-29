﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SZEW.Migrations
{
    /// <inheritdoc />
    public partial class Add_SaleDocument_IsPaid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "SaleDocuments",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "SaleDocuments");
        }
    }
}
