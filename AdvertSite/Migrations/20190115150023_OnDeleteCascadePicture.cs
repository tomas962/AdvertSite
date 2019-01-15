using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AdvertSite.Migrations
{
    public partial class OnDeleteCascadePicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_Listing_pictures_Listings1",
                table: "Listing_pictures");

            //migrationBuilder.AlterColumn<int>(
            //    name: "picture_id",
            //    table: "Listing_pictures",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddForeignKey(
                name: "fk_Listing_pictures_Listings1",
                table: "Listing_pictures",
                column: "Listing_id",
                principalTable: "Listings",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_Listing_pictures_Listings1",
                table: "Listing_pictures");

            //migrationBuilder.AlterColumn<int>(
            //    name: "picture_id",
            //    table: "Listing_pictures",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddForeignKey(
                name: "fk_Listing_pictures_Listings1",
                table: "Listing_pictures",
                column: "Listing_id",
                principalTable: "Listings",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
