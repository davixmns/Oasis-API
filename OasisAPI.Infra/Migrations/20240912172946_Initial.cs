using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OasisAPI.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "oasis_users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RefreshToken = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RefreshTokenExpiryDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_oasis_users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OasisChats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OasisUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OasisChats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OasisChats_oasis_users_OasisUserId",
                        column: x => x.OasisUserId,
                        principalTable: "oasis_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OasisChatBotInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OasisChatId = table.Column<int>(type: "int", nullable: false),
                    ChatBotEnum = table.Column<int>(type: "int", nullable: false),
                    IsSelected = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ThreadId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OasisChatBotInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OasisChatBotInfos_OasisChats_OasisChatId",
                        column: x => x.OasisChatId,
                        principalTable: "OasisChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OasisMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OasisChatId = table.Column<int>(type: "int", nullable: true),
                    From = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsSaved = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OasisMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OasisMessages_OasisChats_OasisChatId",
                        column: x => x.OasisChatId,
                        principalTable: "OasisChats",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_OasisChatBotInfos_OasisChatId",
                table: "OasisChatBotInfos",
                column: "OasisChatId");

            migrationBuilder.CreateIndex(
                name: "IX_OasisChats_OasisUserId",
                table: "OasisChats",
                column: "OasisUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OasisMessages_OasisChatId",
                table: "OasisMessages",
                column: "OasisChatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OasisChatBotInfos");

            migrationBuilder.DropTable(
                name: "OasisMessages");

            migrationBuilder.DropTable(
                name: "OasisChats");

            migrationBuilder.DropTable(
                name: "oasis_users");
        }
    }
}
