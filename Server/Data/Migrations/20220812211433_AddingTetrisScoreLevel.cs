using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gejms.Server.Data.Migrations
{
    public partial class AddingTetrisScoreLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "TetrisScores",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "TetrisScores");
        }
    }
}
