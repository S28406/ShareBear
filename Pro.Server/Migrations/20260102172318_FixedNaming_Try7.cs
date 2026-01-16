using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRO_.Migrations
{
    /// <inheritdoc />
    public partial class FixedNaming_Try7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBorrows_Borrows_OrderId",
                table: "ProductBorrows");

            migrationBuilder.DropIndex(
                name: "IX_ProductBorrows_OrderId",
                table: "ProductBorrows");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ProductBorrows");

            migrationBuilder.DropColumn(
                name: "OrdersId",
                table: "ProductBorrows");

            migrationBuilder.RenameColumn(
                name: "ToolsId",
                table: "ProductBorrows",
                newName: "BorrowId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBorrows_BorrowId",
                table: "ProductBorrows",
                column: "BorrowId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBorrows_Borrows_BorrowId",
                table: "ProductBorrows",
                column: "BorrowId",
                principalTable: "Borrows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBorrows_Borrows_BorrowId",
                table: "ProductBorrows");

            migrationBuilder.DropIndex(
                name: "IX_ProductBorrows_BorrowId",
                table: "ProductBorrows");

            migrationBuilder.RenameColumn(
                name: "BorrowId",
                table: "ProductBorrows",
                newName: "ToolsId");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "ProductBorrows",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OrdersId",
                table: "ProductBorrows",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductBorrows_OrderId",
                table: "ProductBorrows",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBorrows_Borrows_OrderId",
                table: "ProductBorrows",
                column: "OrderId",
                principalTable: "Borrows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
