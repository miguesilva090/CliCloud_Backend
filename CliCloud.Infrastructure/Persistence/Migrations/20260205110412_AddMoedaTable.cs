using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMoedaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Moeda",
                schema: "Utility",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Plural = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Cambio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Abreviatura = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Centesimos = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CentesimoPlural = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Simbolo = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moeda", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Moeda_Codigo",
                schema: "Utility",
                table: "Moeda",
                column: "Codigo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Moeda",
                schema: "Utility");
        }
    }
}
