using Microsoft.EntityFrameworkCore.Migrations;

namespace MasteryAPI.DataAccess.Migrations
{
    public partial class AddOneManyTaskRecords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaskId",
                table: "Records",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Records_TaskId",
                table: "Records",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Tasks_TaskId",
                table: "Records",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_Tasks_TaskId",
                table: "Records");

            migrationBuilder.DropIndex(
                name: "IX_Records_TaskId",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Records");
        }
    }
}
