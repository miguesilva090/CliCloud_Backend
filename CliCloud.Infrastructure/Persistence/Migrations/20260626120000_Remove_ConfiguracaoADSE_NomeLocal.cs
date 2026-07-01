using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Remove_ConfiguracaoADSE_NomeLocal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomeLocal",
                schema: "Faturacao",
                table: "ConfiguracaoADSE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NomeLocal",
                schema: "Faturacao",
                table: "ConfiguracaoADSE",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
