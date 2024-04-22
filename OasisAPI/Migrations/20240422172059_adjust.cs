using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OasisAPI.Migrations
{
    /// <inheritdoc />
    public partial class adjust : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OasisChats_OasisUsers_UserId",
                table: "OasisChats");

            migrationBuilder.DropForeignKey(
                name: "FK_OasisMessages_OasisChats_ChatId",
                table: "OasisMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OasisUsers",
                table: "OasisUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OasisMessages",
                table: "OasisMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OasisChats",
                table: "OasisChats");

            migrationBuilder.RenameTable(
                name: "OasisUsers",
                newName: "oasis_users");

            migrationBuilder.RenameTable(
                name: "OasisMessages",
                newName: "oasis_messages");

            migrationBuilder.RenameTable(
                name: "OasisChats",
                newName: "oasis_chats");

            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "oasis_messages",
                newName: "OasisChatId");

            migrationBuilder.RenameIndex(
                name: "IX_OasisMessages_ChatId",
                table: "oasis_messages",
                newName: "IX_oasis_messages_OasisChatId");

            migrationBuilder.RenameIndex(
                name: "IX_OasisChats_UserId",
                table: "oasis_chats",
                newName: "IX_oasis_chats_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "oasis_users",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_oasis_users",
                table: "oasis_users",
                column: "OasisUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_oasis_messages",
                table: "oasis_messages",
                column: "OasisMessageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_oasis_chats",
                table: "oasis_chats",
                column: "OasisChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_oasis_chats_oasis_users_UserId",
                table: "oasis_chats",
                column: "UserId",
                principalTable: "oasis_users",
                principalColumn: "OasisUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_oasis_messages_oasis_chats_OasisChatId",
                table: "oasis_messages",
                column: "OasisChatId",
                principalTable: "oasis_chats",
                principalColumn: "OasisChatId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_oasis_chats_oasis_users_UserId",
                table: "oasis_chats");

            migrationBuilder.DropForeignKey(
                name: "FK_oasis_messages_oasis_chats_OasisChatId",
                table: "oasis_messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_oasis_users",
                table: "oasis_users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_oasis_messages",
                table: "oasis_messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_oasis_chats",
                table: "oasis_chats");

            migrationBuilder.RenameTable(
                name: "oasis_users",
                newName: "OasisUsers");

            migrationBuilder.RenameTable(
                name: "oasis_messages",
                newName: "OasisMessages");

            migrationBuilder.RenameTable(
                name: "oasis_chats",
                newName: "OasisChats");

            migrationBuilder.RenameColumn(
                name: "OasisChatId",
                table: "OasisMessages",
                newName: "ChatId");

            migrationBuilder.RenameIndex(
                name: "IX_oasis_messages_OasisChatId",
                table: "OasisMessages",
                newName: "IX_OasisMessages_ChatId");

            migrationBuilder.RenameIndex(
                name: "IX_oasis_chats_UserId",
                table: "OasisChats",
                newName: "IX_OasisChats_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "OasisUsers",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OasisUsers",
                table: "OasisUsers",
                column: "OasisUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OasisMessages",
                table: "OasisMessages",
                column: "OasisMessageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OasisChats",
                table: "OasisChats",
                column: "OasisChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_OasisChats_OasisUsers_UserId",
                table: "OasisChats",
                column: "UserId",
                principalTable: "OasisUsers",
                principalColumn: "OasisUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OasisMessages_OasisChats_ChatId",
                table: "OasisMessages",
                column: "ChatId",
                principalTable: "OasisChats",
                principalColumn: "OasisChatId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
