using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Project");

            migrationBuilder.EnsureSchema(
                name: "Interactivity");

            migrationBuilder.EnsureSchema(
                name: "User");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    AccessKey = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Role = table.Column<int>(nullable: false),
                    DeactivatedOn = table.Column<DateTime>(nullable: true),
                    DeactivatedById = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUser_ApplicationUser_DeactivatedById",
                        column: x => x.DeactivatedById,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project",
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
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_ApplicationUser_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_ApplicationUser_DeletedById",
                        column: x => x.DeletedById,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_ApplicationUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectToUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedById = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    CollaboratorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectToUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectToUser_ApplicationUser_CollaboratorId",
                        column: x => x.CollaboratorId,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectToUser_ApplicationUser_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectToUser_ApplicationUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectToUser_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "Project",
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Epic",
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
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Epic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Epic_ApplicationUser_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Epic_ApplicationUser_DeletedById",
                        column: x => x.DeletedById,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Epic_ApplicationUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Epic_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "Project",
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Assignment",
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
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    EpicId = table.Column<int>(nullable: true),
                    AssigneeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignment_ApplicationUser_AssigneeId",
                        column: x => x.AssigneeId,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignment_ApplicationUser_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignment_ApplicationUser_DeletedById",
                        column: x => x.DeletedById,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignment_Epic_EpicId",
                        column: x => x.EpicId,
                        principalSchema: "Project",
                        principalTable: "Epic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignment_ApplicationUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                schema: "Interactivity",
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
                    Content = table.Column<string>(nullable: true),
                    AssignmentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_Assignment_AssignmentId",
                        column: x => x.AssignmentId,
                        principalSchema: "Project",
                        principalTable: "Assignment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comment_ApplicationUser_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comment_ApplicationUser_DeletedById",
                        column: x => x.DeletedById,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comment_ApplicationUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "User",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectToUser_CollaboratorId",
                table: "ProjectToUser",
                column: "CollaboratorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectToUser_CreatedById",
                table: "ProjectToUser",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectToUser_ModifiedById",
                table: "ProjectToUser",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectToUser_ProjectId",
                table: "ProjectToUser",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_AssignmentId",
                schema: "Interactivity",
                table: "Comment",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_CreatedById",
                schema: "Interactivity",
                table: "Comment",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_DeletedById",
                schema: "Interactivity",
                table: "Comment",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ModifiedById",
                schema: "Interactivity",
                table: "Comment",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_AssigneeId",
                schema: "Project",
                table: "Assignment",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_CreatedById",
                schema: "Project",
                table: "Assignment",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_DeletedById",
                schema: "Project",
                table: "Assignment",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_EpicId",
                schema: "Project",
                table: "Assignment",
                column: "EpicId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_ModifiedById",
                schema: "Project",
                table: "Assignment",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Epic_CreatedById",
                schema: "Project",
                table: "Epic",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Epic_DeletedById",
                schema: "Project",
                table: "Epic",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Epic_ModifiedById",
                schema: "Project",
                table: "Epic",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Epic_ProjectId",
                schema: "Project",
                table: "Epic",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_CreatedById",
                schema: "Project",
                table: "Project",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Project_DeletedById",
                schema: "Project",
                table: "Project",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Project_ModifiedById",
                schema: "Project",
                table: "Project",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_DeactivatedById",
                schema: "User",
                table: "ApplicationUser",
                column: "DeactivatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ProjectToUser");

            migrationBuilder.DropTable(
                name: "Comment",
                schema: "Interactivity");

            migrationBuilder.DropTable(
                name: "Assignment",
                schema: "Project");

            migrationBuilder.DropTable(
                name: "Epic",
                schema: "Project");

            migrationBuilder.DropTable(
                name: "Project",
                schema: "Project");

            migrationBuilder.DropTable(
                name: "ApplicationUser",
                schema: "User");
        }
    }
}
