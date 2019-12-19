using Microsoft.EntityFrameworkCore.Migrations;

namespace Saitynai_BeerReview.Migrations
{
    public partial class initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BeerId",
                table: "Comments",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeerId",
                table: "Comments");
        }
    }
}
