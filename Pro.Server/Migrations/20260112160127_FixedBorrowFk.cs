using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRO_.Migrations
{
    /// <inheritdoc />
    public partial class FixedBorrowFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_Users_UserId",
                table: "Borrows");

            migrationBuilder.DropIndex(
                name: "IX_Borrows_UserId",
                table: "Borrows");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_Users_UsersId",
                table: "Borrows");

            migrationBuilder.DropIndex(
                name: "IX_Borrows_UsersId",
                table: "Borrows");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Borrows",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
        }
    }
}
