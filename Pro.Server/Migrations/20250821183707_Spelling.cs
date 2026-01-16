using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRO_.Migrations
{
    /// <inheritdoc />
    public partial class Spelling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Tools_Tools_ID",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_Tools_ID",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Tools_ID",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "Users_ID",
                table: "Reviews",
                newName: "ToolID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ToolID",
                table: "Reviews",
                column: "ToolID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Tools_ToolID",
                table: "Reviews",
                column: "ToolID",
                principalTable: "Tools",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Tools_ToolID",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ToolID",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "ToolID",
                table: "Reviews",
                newName: "Users_ID");

            migrationBuilder.AddColumn<Guid>(
                name: "Tools_ID",
                table: "Reviews",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_Tools_ID",
                table: "Reviews",
                column: "Tools_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Tools_Tools_ID",
                table: "Reviews",
                column: "Tools_ID",
                principalTable: "Tools",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
