using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class InvitationNonPersistent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitation_ApplicationUser_DeletedById",
                schema: "Project",
                table: "Invitation");

            migrationBuilder.DropIndex(
                name: "IX_Invitation_DeletedById",
                schema: "Project",
                table: "Invitation");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                schema: "Project",
                table: "Invitation");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Project",
                table: "Invitation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeletedById",
                schema: "Project",
                table: "Invitation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Project",
                table: "Invitation",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invitation_DeletedById",
                schema: "Project",
                table: "Invitation",
                column: "DeletedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitation_ApplicationUser_DeletedById",
                schema: "Project",
                table: "Invitation",
                column: "DeletedById",
                principalSchema: "User",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
