using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Redde.Migrations
{
    /// <inheritdoc />
    public partial class phoneFieldRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Companies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Companies",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
