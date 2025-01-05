using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClickerBackend.Migrations
{
    /// <inheritdoc />
    public partial class DeleteClickDamage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClickDamage",
                table: "Game");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClickDamage",
                table: "Game",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
