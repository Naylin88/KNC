using Microsoft.EntityFrameworkCore.Migrations;

namespace InitCMS.Migrations
{
    public partial class ModelChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductCategory_ProductCategoryId",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "ProductCategoryId",
                table: "Product",
                newName: "ProductCategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Product_ProductCategoryId",
                table: "Product",
                newName: "IX_Product_ProductCategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductCategory_ProductCategoryID",
                table: "Product",
                column: "ProductCategoryID",
                principalTable: "ProductCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductCategory_ProductCategoryID",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "ProductCategoryID",
                table: "Product",
                newName: "ProductCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_ProductCategoryID",
                table: "Product",
                newName: "IX_Product_ProductCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductCategory_ProductCategoryId",
                table: "Product",
                column: "ProductCategoryId",
                principalTable: "ProductCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
