using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetNET.Migrations
{
    /// <inheritdoc />
    public partial class testtttt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Blames");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreation",
                table: "Blames",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreation",
                table: "Blames");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Blames",
                type: "datetime",
                nullable: true);
        }
    }
}
