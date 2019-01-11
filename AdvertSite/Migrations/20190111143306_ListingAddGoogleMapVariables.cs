using Microsoft.EntityFrameworkCore.Migrations;

namespace AdvertSite.Migrations
{
    public partial class ListingAddGoogleMapVariables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<double>(
                name: "googleLatitude",
                table: "Listings",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "googleLongitude",
                table: "Listings",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "googleRadius",
                table: "Listings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "googleLatitude",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "googleLongitude",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "googleRadius",
                table: "Listings");
        }
    }
}
