using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class AddDiscussions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_ParentId",
                schema: "Interactivity",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_ParentId",
                schema: "Interactivity",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "ParentId",
                schema: "Interactivity",
                table: "Comment");

            migrationBuilder.CreateTable(
                name: "DiscussionMessage",
                schema: "Interactivity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedById = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscussionMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscussionMessage_ApplicationUser_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscussionMessage_ApplicationUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscussionMessage_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "Project",
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionMessage_CreatedById",
                schema: "Interactivity",
                table: "DiscussionMessage",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionMessage_ModifiedById",
                schema: "Interactivity",
                table: "DiscussionMessage",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionMessage_ProjectId",
                schema: "Interactivity",
                table: "DiscussionMessage",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscussionMessage",
                schema: "Interactivity");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                schema: "Interactivity",
                table: "Comment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ParentId",
                schema: "Interactivity",
                table: "Comment",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_ParentId",
                schema: "Interactivity",
                table: "Comment",
                column: "ParentId",
                principalSchema: "Interactivity",
                principalTable: "Comment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
