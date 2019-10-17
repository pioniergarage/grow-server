using Microsoft.EntityFrameworkCore.Migrations;

namespace Grow.Data.Migrations
{
    public partial class AddedPartnerWebsiteUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WebsiteUrl",
                table: "Partners",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WebsiteUrl",
                table: "Partners");
        }
    }
}
