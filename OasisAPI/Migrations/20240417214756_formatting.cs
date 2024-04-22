using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OasisAPI.Migrations
{
    /// <inheritdoc />
    public partial class formatting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "OasisUsers",
                newName: "OasisUserId");

            migrationBuilder.RenameColumn(
                name: "MessageId",
                table: "OasisMessages",
                newName: "OasisMessageId");

            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "OasisChats",
                newName: "OasisChatId");

            migrationBuilder.AddColumn<string>(
                name: "FromThreadId",
                table: "OasisMessages",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromThreadId",
                table: "OasisMessages");

            migrationBuilder.RenameColumn(
                name: "OasisUserId",
                table: "OasisUsers",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "OasisMessageId",
                table: "OasisMessages",
                newName: "MessageId");

            migrationBuilder.RenameColumn(
                name: "OasisChatId",
                table: "OasisChats",
                newName: "ChatId");
        }
    }
}
