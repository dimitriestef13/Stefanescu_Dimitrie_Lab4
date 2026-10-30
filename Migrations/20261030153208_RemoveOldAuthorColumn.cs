using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stefanescu_Dimitrie_Lab2.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOldAuthorColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
    name: "Author",
    table: "Book");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
    name: "Author",
    table: "Book",
    type: "nvarchar(max)",
    nullable: false,
    defaultValue: "");
        }
    }
}
