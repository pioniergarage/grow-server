using Microsoft.EntityFrameworkCore.Migrations;

namespace Grow.Data.Migrations
{
    public partial class GeneralizeFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Images_ImageId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Judges_Images_ImageId",
                table: "Judges");

            migrationBuilder.DropForeignKey(
                name: "FK_Mentors_Images_ImageId",
                table: "Mentors");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizers_Images_ImageId",
                table: "Organizers");

            migrationBuilder.DropForeignKey(
                name: "FK_Partners_Images_ImageId",
                table: "Partners");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Images_LogoImageId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Images_TeamPhotoId",
                table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.RenameTable(
                name: "Images",
                newName: "Files");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Files",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Files",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "Files",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Files",
                table: "Files",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Files_ImageId",
                table: "Events",
                column: "ImageId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Judges_Files_ImageId",
                table: "Judges",
                column: "ImageId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mentors_Files_ImageId",
                table: "Mentors",
                column: "ImageId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizers_Files_ImageId",
                table: "Organizers",
                column: "ImageId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_Files_ImageId",
                table: "Partners",
                column: "ImageId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Files_LogoImageId",
                table: "Teams",
                column: "LogoImageId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Files_TeamPhotoId",
                table: "Teams",
                column: "TeamPhotoId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Files_ImageId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Judges_Files_ImageId",
                table: "Judges");

            migrationBuilder.DropForeignKey(
                name: "FK_Mentors_Files_ImageId",
                table: "Mentors");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizers_Files_ImageId",
                table: "Organizers");

            migrationBuilder.DropForeignKey(
                name: "FK_Partners_Files_ImageId",
                table: "Partners");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Files_LogoImageId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Files_TeamPhotoId",
                table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Files",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "Files");

            migrationBuilder.RenameTable(
                name: "Files",
                newName: "Images");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Images",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Images_ImageId",
                table: "Events",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Judges_Images_ImageId",
                table: "Judges",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mentors_Images_ImageId",
                table: "Mentors",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizers_Images_ImageId",
                table: "Organizers",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_Images_ImageId",
                table: "Partners",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Images_LogoImageId",
                table: "Teams",
                column: "LogoImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Images_TeamPhotoId",
                table: "Teams",
                column: "TeamPhotoId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
