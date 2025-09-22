using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eyewear.Shop.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ToThePoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ProductVariants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MainImagesUrls",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailImageUrl",
                table: "Products",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "MainImagesUrls",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ThumbnailImageUrl",
                table: "Products");
        }
    }
}
