using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRO_.Migrations
{
    /// <inheritdoc />
    public partial class Added_Navigation_For_Review : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_Users_UsersId",
                table: "Borrows");

            migrationBuilder.DropIndex(
                name: "IX_Borrows_UsersId",
                table: "Borrows");

            migrationBuilder.AddColumn<Guid>(
                name: "BorrowId",
                table: "Reviews",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Reviews",
                type: "text",
                nullable: false,
                defaultValue: "Pending");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Borrows",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BorrowId_ToolId_UserId",
                table: "Reviews",
                columns: new[] { "BorrowId", "ToolId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_UserId",
                table: "Borrows",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_Users_UserId",
                table: "Borrows",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Borrows_BorrowId",
                table: "Reviews",
                column: "BorrowId",
                principalTable: "Borrows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_Users_UserId",
                table: "Borrows");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Borrows_BorrowId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_BorrowId_ToolId_UserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Borrows_UserId",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "BorrowId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Borrows");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_UsersId",
                table: "Borrows",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_Users_UsersId",
                table: "Borrows",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
