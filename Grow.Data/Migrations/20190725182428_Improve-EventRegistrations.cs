using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Grow.Data.Migrations
{
    public partial class ImproveEventRegistrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationOptions_Type",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "RegistrationOptions_Until",
                table: "Events",
                newName: "TeamRegistrationOptions_Until");

            migrationBuilder.RenameColumn(
                name: "RegistrationOptions_From",
                table: "Events",
                newName: "TeamRegistrationOptions_From");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "EventResponses");

            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "EventResponses",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Teams",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Prizes",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Partners",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Organizers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Mentors",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Judges",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Files",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Events",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CanTeamsRegister",
                table: "Events",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanVisitorsRegister",
                table: "Events",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TeamRegistrationOptions_AcceptFileUploads",
                table: "Events",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TeamRegistrationOptions_AllowedFileExtensionsString",
                table: "Events",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "EventResponses",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "EventResponses",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "EventResponses",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Contests",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CommonQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    ContestId = table.Column<int>(nullable: false),
                    Question = table.Column<string>(nullable: true),
                    Answer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommonQuestion_Contests_ContestId",
                        column: x => x.ContestId,
                        principalTable: "Contests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventResponses_TeamId",
                table: "EventResponses",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_CommonQuestion_ContestId",
                table: "CommonQuestion",
                column: "ContestId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventResponses_Teams_TeamId",
                table: "EventResponses",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventResponses_Teams_TeamId",
                table: "EventResponses");

            migrationBuilder.DropTable(
                name: "CommonQuestion");

            migrationBuilder.DropIndex(
                name: "IX_EventResponses_TeamId",
                table: "EventResponses");

            migrationBuilder.DropColumn(
                name: "CanTeamsRegister",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CanVisitorsRegister",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TeamRegistrationOptions_AcceptFileUploads",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TeamRegistrationOptions_AllowedFileExtensionsString",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "EventResponses");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "EventResponses");

            migrationBuilder.RenameColumn(
                name: "TeamRegistrationOptions_Until",
                table: "Events",
                newName: "RegistrationOptions_Until");

            migrationBuilder.RenameColumn(
                name: "TeamRegistrationOptions_From",
                table: "Events",
                newName: "RegistrationOptions_From");

            migrationBuilder.RenameColumn(
                name: "FileUrl",
                table: "EventResponses",
                newName: "Email");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Teams",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Prizes",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Partners",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Organizers",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Mentors",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Judges",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Files",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Events",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "RegistrationOptions_Type",
                table: "Events",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "EventResponses",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Contests",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
