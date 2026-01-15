using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialLedger.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueKeyInAccountBalanceAndIdempotencyRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IdempotencyRecords_AccountId",
                table: "IdempotencyRecords");

            migrationBuilder.DropIndex(
                name: "IX_AccountBalance_AccountId",
                table: "AccountBalance");

            migrationBuilder.AlterColumn<long>(
                name: "LedgerEntryId",
                table: "IdempotencyRecords",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountBalance",
                table: "AccountBalance",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_IdempotencyRecords_AccountId_IdempotencyKey",
                table: "IdempotencyRecords",
                columns: new[] { "AccountId", "IdempotencyKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdempotencyRecords_LedgerEntryId",
                table: "IdempotencyRecords",
                column: "LedgerEntryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_IdempotencyRecords_LedgerEntries_LedgerEntryId",
                table: "IdempotencyRecords",
                column: "LedgerEntryId",
                principalTable: "LedgerEntries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdempotencyRecords_LedgerEntries_LedgerEntryId",
                table: "IdempotencyRecords");

            migrationBuilder.DropIndex(
                name: "IX_IdempotencyRecords_AccountId_IdempotencyKey",
                table: "IdempotencyRecords");

            migrationBuilder.DropIndex(
                name: "IX_IdempotencyRecords_LedgerEntryId",
                table: "IdempotencyRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountBalance",
                table: "AccountBalance");

            migrationBuilder.AlterColumn<long>(
                name: "LedgerEntryId",
                table: "IdempotencyRecords",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdempotencyRecords_AccountId",
                table: "IdempotencyRecords",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountBalance_AccountId",
                table: "AccountBalance",
                column: "AccountId");
        }
    }
}
