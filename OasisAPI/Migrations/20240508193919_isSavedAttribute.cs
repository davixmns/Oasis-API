using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OasisAPI.Migrations
{
    /// <inheritdoc />
    public partial class isSavedAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSaved",
                table: "oasis_messages",
                type: "tinyint(1)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSaved",
                table: "oasis_messages");
        }
    }
}
