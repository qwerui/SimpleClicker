﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClickerBackend.Migrations
{
    /// <inheritdoc />
    public partial class DeleteClickDamage2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
