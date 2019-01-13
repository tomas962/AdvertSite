using Microsoft.EntityFrameworkCore.Migrations;

namespace AdvertSite.Migrations
{
    public partial class OnDeleteCascadeComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_Comments_Listings11",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "fk_Comments_Users11",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "fk_Comments_Listings11",
                table: "Comments",
                column: "listingid",
                principalTable: "Listings",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_Comments_Users11",
                table: "Comments",
                column: "userid",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_Comments_Listings11",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "fk_Comments_Users11",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "fk_Comments_Listings11",
                table: "Comments",
                column: "listingid",
                principalTable: "Listings",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_Comments_Users11",
                table: "Comments",
                column: "userid",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
