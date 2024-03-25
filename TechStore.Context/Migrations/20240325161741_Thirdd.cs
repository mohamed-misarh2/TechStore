using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechStore.Context.Migrations
{
    /// <inheritdoc />
    public partial class Thirdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quntity",
                table: "Products",
                newName: "Quantity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Products",
                newName: "Quntity");
        }
    }
}
