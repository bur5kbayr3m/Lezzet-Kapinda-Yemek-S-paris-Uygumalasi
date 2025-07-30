using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YemekSiparis.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldsToStore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Stores",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderCount",
                table: "Stores",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "OrderCount",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Products");
        }
    }
}
