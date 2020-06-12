using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectToUser_Project_ProjectId",
                schema: "Junction",
                table: "ProjectToUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignment_Assignment_OrderBeforeId",
                schema: "Project",
                table: "Assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUser_ApplicationUser_DeactivatedById",
                schema: "User",
                table: "ApplicationUser");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUser_DeactivatedById",
                schema: "User",
                table: "ApplicationUser");

            migrationBuilder.DropIndex(
                name: "IX_Assignment_OrderBeforeId",
                schema: "Project",
                table: "Assignment");

            migrationBuilder.DropColumn(
                name: "DeactivatedById",
                schema: "User",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "DeactivatedOn",
                schema: "User",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "Role",
                schema: "User",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "OrderBeforeId",
                schema: "Project",
                table: "Assignment");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectToUser_Project_ProjectId",
                schema: "Junction",
                table: "ProjectToUser",
                column: "ProjectId",
                principalSchema: "Project",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectToUser_Project_ProjectId",
                schema: "Junction",
                table: "ProjectToUser");

            migrationBuilder.AddColumn<int>(
                name: "DeactivatedById",
                schema: "User",
                table: "ApplicationUser",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivatedOn",
                schema: "User",
                table: "ApplicationUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                schema: "User",
                table: "ApplicationUser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderBeforeId",
                schema: "Project",
                table: "Assignment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_DeactivatedById",
                schema: "User",
                table: "ApplicationUser",
                column: "DeactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_OrderBeforeId",
                schema: "Project",
                table: "Assignment",
                column: "OrderBeforeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectToUser_Project_ProjectId",
                schema: "Junction",
                table: "ProjectToUser",
                column: "ProjectId",
                principalSchema: "Project",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignment_Assignment_OrderBeforeId",
                schema: "Project",
                table: "Assignment",
                column: "OrderBeforeId",
                principalSchema: "Project",
                principalTable: "Assignment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUser_ApplicationUser_DeactivatedById",
                schema: "User",
                table: "ApplicationUser",
                column: "DeactivatedById",
                principalSchema: "User",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
