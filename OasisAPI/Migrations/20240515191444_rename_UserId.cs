using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OasisAPI.Migrations
{
    /// <inheritdoc />
    public partial class rename_UserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_oasis_chats_oasis_users_UserId",
                table: "oasis_chats");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "oasis_chats",
                newName: "OasisUserId");

            migrationBuilder.RenameIndex(
                name: "IX_oasis_chats_UserId",
                table: "oasis_chats",
                newName: "IX_oasis_chats_OasisUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_oasis_chats_oasis_users_OasisUserId",
                table: "oasis_chats",
                column: "OasisUserId",
                principalTable: "oasis_users",
                principalColumn: "OasisUserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_oasis_chats_oasis_users_OasisUserId",
                table: "oasis_chats");

            migrationBuilder.RenameColumn(
                name: "OasisUserId",
                table: "oasis_chats",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_oasis_chats_OasisUserId",
                table: "oasis_chats",
                newName: "IX_oasis_chats_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_oasis_chats_oasis_users_UserId",
                table: "oasis_chats",
                column: "UserId",
                principalTable: "oasis_users",
                principalColumn: "OasisUserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
