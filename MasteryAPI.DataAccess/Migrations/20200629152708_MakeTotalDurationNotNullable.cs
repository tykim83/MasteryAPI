using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MasteryAPI.DataAccess.Migrations
{
    public partial class MakeTotalDurationNotNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "TotalDuration",
                table: "Records",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "TotalDuration",
                table: "Records",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeSpan));
        }
    }
}
