using Microsoft.EntityFrameworkCore.Migrations;

namespace AdvertSite.Migrations
{
    public partial class MoveSenderIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_Messages_Users1",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "fk_Users_has_Messages_Messages1",
                table: "Users_has_Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");


            migrationBuilder.DropColumn(
                name: "sender_id",
                table: "Messages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_has_Messages_Messages_id",
                table: "Users_has_Messages",
                column: "Messages_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_has_Messages_Messages_sender_id",
                table: "Users_has_Messages",
                column: "Messages_sender_id");

            migrationBuilder.AddForeignKey(
                name: "fk_Users_has_Messages_Messages1",
                table: "Users_has_Messages",
                column: "Messages_id",
                principalTable: "Messages",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_Users_has_Messages_Users2",
                table: "Users_has_Messages",
                column: "Messages_sender_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_Users_has_Messages_Messages1",
                table: "Users_has_Messages");

            migrationBuilder.DropForeignKey(
                name: "fk_Users_has_Messages_Users2",
                table: "Users_has_Messages");

            migrationBuilder.DropIndex(
                name: "IX_Users_has_Messages_Messages_id",
                table: "Users_has_Messages");

            migrationBuilder.DropIndex(
                name: "IX_Users_has_Messages_Messages_sender_id",
                table: "Users_has_Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "sender_id",
                table: "Messages",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                columns: new[] { "id", "sender_id" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_has_Messages_Messages_id_Messages_sender_id",
                table: "Users_has_Messages",
                columns: new[] { "Messages_id", "Messages_sender_id" });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_sender_id",
                table: "Messages",
                column: "sender_id");

            migrationBuilder.AddForeignKey(
                name: "fk_Messages_Users1",
                table: "Messages",
                column: "sender_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_Users_has_Messages_Messages1",
                table: "Users_has_Messages",
                columns: new[] { "Messages_id", "Messages_sender_id" },
                principalTable: "Messages",
                principalColumns: new[] { "id", "sender_id" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
