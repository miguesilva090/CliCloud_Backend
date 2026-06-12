using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class F11_NaturezaDocumento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NaturezaDocumento",
                schema: "Documentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sigla = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaturezaDocumento", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NaturezaDocumento_Sigla",
                schema: "Documentos",
                table: "NaturezaDocumento",
                column: "Sigla",
                unique: true);

            migrationBuilder.Sql(@"
INSERT INTO Documentos.NaturezaDocumento (Id, Sigla, Descricao, CreatedBy, CreatedOn)
VALUES
    ('11111111-1111-1111-1111-000000000001', N'C', N'Crédito', '00000000-0000-0000-0000-000000000001', SYSUTCDATETIME()),
    ('11111111-1111-1111-1111-000000000002', N'D', N'Débito',  '00000000-0000-0000-0000-000000000001', SYSUTCDATETIME()),
    ('11111111-1111-1111-1111-000000000003', N'V', N'Venda',   '00000000-0000-0000-0000-000000000001', SYSUTCDATETIME());
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NaturezaDocumento",
                schema: "Documentos");
        }
    }
}
