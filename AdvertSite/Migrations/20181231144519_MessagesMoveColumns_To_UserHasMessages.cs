using Microsoft.EntityFrameworkCore.Migrations;

namespace AdvertSite.Migrations
{
    public partial class MessagesMoveColumns_To_UserHasMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "alreadyRead",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Messages");

            migrationBuilder.AddColumn<short>(
                name: "isAdminMessage",
                table: "Users_has_Messages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<short>(
                name: "alreadyRead",
                table: "Users_has_Messages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<short>(
                name: "isDeleted",
                table: "Users_has_Messages",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAdminMessage",
                table: "Users_has_Messages");

            migrationBuilder.DropColumn(
                name: "alreadyRead",
                table: "Users_has_Messages");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Users_has_Messages");

            migrationBuilder.AddColumn<short>(
                name: "alreadyRead",
                table: "Messages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<short>(
                name: "isDeleted",
                table: "Messages",
                nullable: false,
                defaultValue: 0);
        }
    }
}
