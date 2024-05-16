using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OasisAPI.Migrations
{
    /// <inheritdoc />
    public partial class adjust3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_oasis_messages_oasis_chats_OasisChatId",
                table: "oasis_messages");

            migrationBuilder.AlterColumn<int>(
                name: "OasisChatId",
                table: "oasis_messages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_oasis_messages_oasis_chats_OasisChatId",
                table: "oasis_messages",
                column: "OasisChatId",
                principalTable: "oasis_chats",
                principalColumn: "OasisChatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_oasis_messages_oasis_chats_OasisChatId",
                table: "oasis_messages");

            migrationBuilder.AlterColumn<int>(
                name: "OasisChatId",
                table: "oasis_messages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_oasis_messages_oasis_chats_OasisChatId",
                table: "oasis_messages",
                column: "OasisChatId",
                principalTable: "oasis_chats",
                principalColumn: "OasisChatId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
