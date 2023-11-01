using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class Table_Inventory_Rebuild_Sheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Accounts_AccountId",
                table: "Inventories");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Accounts_TempId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "TempId",
                table: "Accounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TempId",
                table: "Accounts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Accounts_TempId",
                table: "Accounts",
                column: "TempId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Accounts_AccountId",
                table: "Inventories",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "TempId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
