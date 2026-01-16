using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRO_.Migrations
{
    /// <inheritdoc />
    public partial class AddedPagination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tools_Location",
                table: "Tools",
                column: "Location");

            migrationBuilder.CreateIndex(
                name: "IX_Tools_Price",
                table: "Tools",
                column: "Price");
            
            migrationBuilder.Sql(@"CREATE EXTENSION IF NOT EXISTS pg_trgm;");

            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ix_tools_name_trgm ON ""Tools"" USING gin (""Name"" gin_trgm_ops);");

            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ix_tools_desc_trgm ON ""Tools"" USING gin (""Description"" gin_trgm_ops);");

            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ix_tools_location_trgm ON ""Tools"" USING gin (""Location"" gin_trgm_ops);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tools_Location",
                table: "Tools");

            migrationBuilder.DropIndex(
                name: "IX_Tools_Price",
                table: "Tools");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ix_tools_name_trgm;");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ix_tools_desc_trgm;");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ix_tools_location_trgm;");

        }
    }
}
