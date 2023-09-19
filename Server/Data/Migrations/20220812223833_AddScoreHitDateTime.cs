using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gejms.Server.Data.Migrations
{
    public partial class AddScoreHitDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ScoreDate",
                table: "TetrisScores",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScoreDate",
                table: "TetrisScores");
        }
    }
}
