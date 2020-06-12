using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class RenameJunctionSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Junction");

            migrationBuilder.RenameTable(
                name: "ProjectToUser",
                newName: "ProjectToUser",
                newSchema: "Junction");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ProjectToUser",
                schema: "Junction",
                newName: "ProjectToUser");
        }
    }
}
