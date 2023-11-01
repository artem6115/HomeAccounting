using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class Table_Inventory_ForeingKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Inventories_AccountId",
                table: "Inventories",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Accounts_AccountId",
                table: "Inventories",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Accounts_AccountId",
                table: "Inventories");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_AccountId",
                table: "Inventories");
        }
    }
}
