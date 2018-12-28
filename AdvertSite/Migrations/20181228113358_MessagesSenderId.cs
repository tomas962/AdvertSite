using Microsoft.EntityFrameworkCore.Migrations;

namespace AdvertSite.Migrations
{
    public partial class MessagesSenderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.DropForeignKey(
                name: "fk_Users_has_Messages_Users1",
                table: "Users_has_Messages"
                );

            migrationBuilder.DropColumn(
                name: "Messages_sender_id",
                table: "Users_has_Messages"
                );

            migrationBuilder.DropPrimaryKey(
                name: "sender_id",
                table: "Messages"
                );

            migrationBuilder.DropColumn(
                name: "sender_id",
                table: "Messages"
                );

            migrationBuilder.AlterColumn<string>(
                name: "sender_id",
                table: "Users_has_Messages"
                );

            migrationBuilder.AddForeignKey(
                name: "fk_Users_has_Messages_Users1",
                table: "Users_has_Messages",
                column: "sender_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict
                );
            */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.AlterColumn<string>(
                 name: "sender_id",
                 table: "Messages"
                 );

            migrationBuilder.AlterColumn<string>(
                 name: "Messages_sender_id",
                 table: "Users_has_Messages"
                 );
            migrationBuilder.AddForeignKey(
                name: "fk_Users_has_Messages_Users1",
                table: "Users_has_Messages",
                column: "sender_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict
                );
                */
        }
    }
}
