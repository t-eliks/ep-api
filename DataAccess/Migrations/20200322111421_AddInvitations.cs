using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class AddInvitations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invitation",
                schema: "Project",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedById = table.Column<int>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invitation_ApplicationUser_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invitation_ApplicationUser_DeletedById",
                        column: x => x.DeletedById,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invitation_ApplicationUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invitation_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "Project",
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invitation_CreatedById",
                schema: "Project",
                table: "Invitation",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Invitation_DeletedById",
                schema: "Project",
                table: "Invitation",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Invitation_ModifiedById",
                schema: "Project",
                table: "Invitation",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Invitation_ProjectId",
                schema: "Project",
                table: "Invitation",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invitation",
                schema: "Project");
        }
    }
}
