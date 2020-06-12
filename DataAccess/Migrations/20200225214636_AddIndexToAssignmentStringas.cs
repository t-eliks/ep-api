using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class AddIndexToAssignmentStringas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Index",
                schema: "Project",
                table: "Assignment");

            migrationBuilder.AddColumn<int>(
                name: "OrderBeforeId",
                schema: "Project",
                table: "Assignment",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_OrderBeforeId",
                schema: "Project",
                table: "Assignment",
                column: "OrderBeforeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignment_Assignment_OrderBeforeId",
                schema: "Project",
                table: "Assignment",
                column: "OrderBeforeId",
                principalSchema: "Project",
                principalTable: "Assignment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignment_Assignment_OrderBeforeId",
                schema: "Project",
                table: "Assignment");

            migrationBuilder.DropIndex(
                name: "IX_Assignment_OrderBeforeId",
                schema: "Project",
                table: "Assignment");

            migrationBuilder.DropColumn(
                name: "OrderBeforeId",
                schema: "Project",
                table: "Assignment");

            migrationBuilder.AddColumn<string>(
                name: "Index",
                schema: "Project",
                table: "Assignment",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
