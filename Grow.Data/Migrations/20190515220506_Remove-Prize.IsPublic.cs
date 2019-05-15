using Microsoft.EntityFrameworkCore.Migrations;

namespace Grow.Data.Migrations
{
    public partial class RemovePrizeIsPublic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Prizes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Prizes",
                nullable: false,
                defaultValue: false);
        }
    }
}
