using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBrandImageUrlToItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "ItemTable",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ItemTable",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "ItemTable");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ItemTable");
        }
    }
}
