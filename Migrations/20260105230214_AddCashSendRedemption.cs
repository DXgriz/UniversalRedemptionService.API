using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversalRedemptionService.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCashSendRedemption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashSendRedemptions_Wallets_WalletId",
                table: "CashSendRedemptions");

            migrationBuilder.AlterColumn<int>(
                name: "WalletId",
                table: "CashSendRedemptions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ReferenceNumber",
                table: "CashSendRedemptions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_CashSendRedemptions_ReferenceNumber",
                table: "CashSendRedemptions",
                column: "ReferenceNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CashSendRedemptions_Wallets_WalletId",
                table: "CashSendRedemptions",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashSendRedemptions_Wallets_WalletId",
                table: "CashSendRedemptions");

            migrationBuilder.DropIndex(
                name: "IX_CashSendRedemptions_ReferenceNumber",
                table: "CashSendRedemptions");

            migrationBuilder.AlterColumn<int>(
                name: "WalletId",
                table: "CashSendRedemptions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReferenceNumber",
                table: "CashSendRedemptions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_CashSendRedemptions_Wallets_WalletId",
                table: "CashSendRedemptions",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
