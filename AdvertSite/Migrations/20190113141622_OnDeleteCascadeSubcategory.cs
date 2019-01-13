using Microsoft.EntityFrameworkCore.Migrations;

namespace AdvertSite.Migrations
{
    public partial class OnDeleteCascadeSubcategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_Subcategory_Category1",
                table: "Subcategory");

            migrationBuilder.AddForeignKey(
                name: "fk_Subcategory_Category1",
                table: "Subcategory",
                column: "categoryid",
                principalTable: "Category",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_Subcategory_Category1",
                table: "Subcategory");

            migrationBuilder.AddForeignKey(
                name: "fk_Subcategory_Category1",
                table: "Subcategory",
                column: "categoryid",
                principalTable: "Category",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
