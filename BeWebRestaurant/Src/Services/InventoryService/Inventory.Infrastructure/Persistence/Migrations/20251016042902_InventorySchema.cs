using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InventorySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    IdIngredients = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IngredientsName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.IdIngredients);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    IdStock = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StockName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.IdStock);
                });

            migrationBuilder.CreateTable(
                name: "FoodRecipe",
                columns: table => new
                {
                    IdFoodRecipe = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FoodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IngredientsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Measurement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodRecipe", x => x.IdFoodRecipe);
                    table.ForeignKey(
                        name: "FK_FoodRecipe_Ingredients_IngredientsId",
                        column: x => x.IngredientsId,
                        principalTable: "Ingredients",
                        principalColumn: "IdIngredients",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockItems",
                columns: table => new
                {
                    IdStockItems = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StockId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IngredientsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Measurement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockItemsStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockItems", x => x.IdStockItems);
                    table.ForeignKey(
                        name: "FK_StockItems_Ingredients_IngredientsId",
                        column: x => x.IngredientsId,
                        principalTable: "Ingredients",
                        principalColumn: "IdIngredients",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockItems_Stock_StockId",
                        column: x => x.StockId,
                        principalTable: "Stock",
                        principalColumn: "IdStock",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodRecipe_FoodId",
                table: "FoodRecipe",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodRecipe_FoodId_IngredientsId",
                table: "FoodRecipe",
                columns: new[] { "FoodId", "IngredientsId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FoodRecipe_IngredientsId",
                table: "FoodRecipe",
                column: "IngredientsId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_IngredientsName",
                table: "Ingredients",
                column: "IngredientsName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stock_StockName",
                table: "Stock",
                column: "StockName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockItems_IngredientsId",
                table: "StockItems",
                column: "IngredientsId");

            migrationBuilder.CreateIndex(
                name: "IX_StockItems_StockId",
                table: "StockItems",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockItems_StockId_IngredientsId",
                table: "StockItems",
                columns: new[] { "StockId", "IngredientsId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodRecipe");

            migrationBuilder.DropTable(
                name: "StockItems");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Stock");
        }
    }
}
