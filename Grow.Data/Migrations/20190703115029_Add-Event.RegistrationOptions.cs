using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Grow.Data.Migrations
{
    public partial class AddEventRegistrationOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Registration",
                newName: "RegistrationOptions_Type",
                table: "Events");

            migrationBuilder.AddColumn<int>(
                name: "SlidesId",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationOptions_From",
                table: "Events",
                nullable: true);
            
            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationOptions_Until",
                table: "Events",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_SlidesId",
                table: "Events",
                column: "SlidesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Files_SlidesId",
                table: "Events",
                column: "SlidesId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Files_SlidesId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_SlidesId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "SlidesId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "RegistrationOptions_From",
                table: "Events");
            
            migrationBuilder.DropColumn(
                name: "RegistrationOptions_Until",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "RegistrationOptions_Type",
                newName: "Registration",
                table: "Events");
        }
    }
}
