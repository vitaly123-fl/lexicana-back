using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lexicana.Database.Migrations
{
    /// <inheritdoc />
    public partial class add_is_grouped : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGrouped",
                table: "Topics",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGrouped",
                table: "Topics");
        }
    }
}
