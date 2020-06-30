using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MasteryAPI.DataAccess.Migrations
{
    public partial class AddColoroAndTotalDurationToCategoryAndTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "TotalDuration",
                table: "Tasks",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TotalDuration",
                table: "Categories",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalDuration",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "TotalDuration",
                table: "Categories");
        }
    }
}
