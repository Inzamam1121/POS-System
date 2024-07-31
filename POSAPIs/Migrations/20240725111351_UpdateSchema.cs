using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POSAPIs.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_Products_ProductId",
                table: "ProductItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_Purchases_ProductItemId",
                table: "ProductItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_Sales_ProductItemId",
                table: "ProductItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Users_CashierId",
                table: "Sales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductItems",
                table: "ProductItems");

            migrationBuilder.AddColumn<int>(
                name: "SaleId",
                table: "ProductItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ProductItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseId",
                table: "ProductItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductItems",
                table: "ProductItems",
                columns: new[] { "SaleId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_PurchaseId",
                table: "ProductItems",
                column: "PurchaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_Products_ProductId",
                table: "ProductItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_Purchases_PurchaseId",
                table: "ProductItems",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "PurchaseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_Sales_SaleId",
                table: "ProductItems",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "SaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Users_CashierId",
                table: "Sales",
                column: "CashierId",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_Products_ProductId",
                table: "ProductItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_Purchases_PurchaseId",
                table: "ProductItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_Sales_SaleId",
                table: "ProductItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Users_CashierId",
                table: "Sales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductItems",
                table: "ProductItems");

            migrationBuilder.DropIndex(
                name: "IX_ProductItems_PurchaseId",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "SaleId",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "PurchaseId",
                table: "ProductItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductItems",
                table: "ProductItems",
                column: "ProductItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_Products_ProductId",
                table: "ProductItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_Purchases_ProductItemId",
                table: "ProductItems",
                column: "ProductItemId",
                principalTable: "Purchases",
                principalColumn: "PurchaseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_Sales_ProductItemId",
                table: "ProductItems",
                column: "ProductItemId",
                principalTable: "Sales",
                principalColumn: "SaleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Users_CashierId",
                table: "Sales",
                column: "CashierId",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
