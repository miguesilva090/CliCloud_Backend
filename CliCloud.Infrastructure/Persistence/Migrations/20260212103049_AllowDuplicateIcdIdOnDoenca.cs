using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AllowDuplicateIcdIdOnDoenca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Doenca_IcdId",
                schema: "Doencas",
                table: "Doenca");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Doenca_IcdId",
                schema: "Doencas",
                table: "Doenca",
                column: "IcdId",
                unique: true);
        }
    }
}
