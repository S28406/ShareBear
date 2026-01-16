using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRO_.Migrations
{
    /// <inheritdoc />
    public partial class AddedPaymentCollectionToBorrow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Borrows_BorrowID",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_BorrowID",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "BorrowID",
                table: "Payments");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Orders_ID",
                table: "Payments",
                column: "Orders_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Borrows_Orders_ID",
                table: "Payments",
                column: "Orders_ID",
                principalTable: "Borrows",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Borrows_Orders_ID",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_Orders_ID",
                table: "Payments");

            migrationBuilder.AddColumn<Guid>(
                name: "BorrowID",
                table: "Payments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BorrowID",
                table: "Payments",
                column: "BorrowID");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Borrows_BorrowID",
                table: "Payments",
                column: "BorrowID",
                principalTable: "Borrows",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
