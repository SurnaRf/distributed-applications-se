using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amigurumemi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToYarns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Yarns",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Yarns");
        }
    }
}
