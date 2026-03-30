using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTaxaIvaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaxaIva",
                schema: "Utility",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Taxa = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxaIva", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaxaIva_Codigo",
                schema: "Utility",
                table: "TaxaIva",
                column: "Codigo",
                unique: true);

            // Seed Taxas IVA: Isento, IVA 6%, IVA 13%, IVA 23% (conforme imagem de referência)
            migrationBuilder.Sql(@"
                INSERT INTO Utility.TaxaIva (Id, Codigo, Descricao, Taxa, CreatedBy, CreatedOn)
                VALUES
                    (NEWID(), '1', 'Isento', 0, '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
                    (NEWID(), '2', 'IVA 6%', 6, '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
                    (NEWID(), '3', 'IVA 13%', 13, '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
                    (NEWID(), '4', 'IVA 23%', 23, '00000000-0000-0000-0000-000000000000', GETUTCDATE());
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxaIva",
                schema: "Utility");
        }
    }
}
