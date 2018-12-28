using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace AdvertSite.Migrations
{
    public partial class MessagesColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "dateSent",
                table: "Messages",
                type: "datetime2(0)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dateSent",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "alreadyRead",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Messages");
        }
    }
}
