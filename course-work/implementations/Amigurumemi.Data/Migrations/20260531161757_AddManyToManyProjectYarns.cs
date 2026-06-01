using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amigurumemi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddManyToManyProjectYarns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Patterns_PatternId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Yarns_Projects_ProjectId",
                table: "Yarns");

            migrationBuilder.DropIndex(
                name: "IX_Yarns_ProjectId",
                table: "Yarns");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Yarns");

            migrationBuilder.CreateTable(
                name: "ProjectYarns",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    YarnId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectYarns", x => new { x.ProjectId, x.YarnId });
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Patterns_PatternId",
                table: "Projects",
                column: "PatternId",
                principalTable: "Patterns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Patterns_PatternId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "ProjectYarns");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Yarns",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Yarns_ProjectId",
                table: "Yarns",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Patterns_PatternId",
                table: "Projects",
                column: "PatternId",
                principalTable: "Patterns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Yarns_Projects_ProjectId",
                table: "Yarns",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
