using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialLedger.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LedgerEntrys_Accounts_AccountId",
                table: "LedgerEntrys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LedgerEntrys",
                table: "LedgerEntrys");

            migrationBuilder.RenameTable(
                name: "LedgerEntrys",
                newName: "LedgerEntries");

            migrationBuilder.RenameIndex(
                name: "IX_LedgerEntrys_AccountId",
                table: "LedgerEntries",
                newName: "IX_LedgerEntries_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LedgerEntries",
                table: "LedgerEntries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerEntries_Accounts_AccountId",
                table: "LedgerEntries",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LedgerEntries_Accounts_AccountId",
                table: "LedgerEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LedgerEntries",
                table: "LedgerEntries");

            migrationBuilder.RenameTable(
                name: "LedgerEntries",
                newName: "LedgerEntrys");

            migrationBuilder.RenameIndex(
                name: "IX_LedgerEntries_AccountId",
                table: "LedgerEntrys",
                newName: "IX_LedgerEntrys_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LedgerEntrys",
                table: "LedgerEntrys",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerEntrys_Accounts_AccountId",
                table: "LedgerEntrys",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
