using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRO_.Migrations
{
    /// <inheritdoc />
    public partial class FixedReturns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "SecurityDeposits");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "SecurityDeposits",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SecurityDeposits_UserId1",
                table: "SecurityDeposits",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityDeposits_Users_UserId1",
                table: "SecurityDeposits",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SecurityDeposits_Users_UserId1",
                table: "SecurityDeposits");

            migrationBuilder.DropIndex(
                name: "IX_SecurityDeposits_UserId1",
                table: "SecurityDeposits");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "SecurityDeposits");

            migrationBuilder.AddColumn<Guid>(
                name: "UsersId",
                table: "SecurityDeposits",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
