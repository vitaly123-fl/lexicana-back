using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lexicana.Database.Migrations
{
    /// <inheritdoc />
    public partial class add_user_additional_info : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetCode",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetCode",
                table: "Users");
        }
    }
}
