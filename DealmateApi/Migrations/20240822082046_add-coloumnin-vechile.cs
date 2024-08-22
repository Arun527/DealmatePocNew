using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealmateApi.Migrations
{
    /// <inheritdoc />
    public partial class addcoloumninvechile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "Active",
                table: "Vehicle",
                type: "interval",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Vehicle");
        }
    }
}
