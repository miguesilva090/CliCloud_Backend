using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class F15_MotivoRetencao : Migration
    {
        private const string SeedUserId = "00000000-0000-0000-0000-000000000001";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MotivoRetencao",
                schema: "Utility",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TipoImposto = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotivoRetencao", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MotivoRetencao_Codigo",
                schema: "Utility",
                table: "MotivoRetencao",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MotivoRetencao_TipoImposto",
                schema: "Utility",
                table: "MotivoRetencao",
                column: "TipoImposto");

            migrationBuilder.Sql($"""
                DECLARE @CreatedBy uniqueidentifier = '{SeedUserId}';
                DECLARE @Now datetime2 = SYSUTCDATETIME();

                INSERT INTO [Utility].[MotivoRetencao] (Id, Codigo, Descricao, TipoImposto, CreatedBy, CreatedOn)
                SELECT v.Id, v.Codigo, v.Descricao, v.TipoImposto, @CreatedBy, @Now
                FROM (VALUES
                    ('A1510000-0000-4000-8000-000000000001', 1, N'Artigo 98º, Alínea 1', N'IRS'),
                    ('A1510000-0000-4000-8000-000000000002', 2, N'Artigo 98º, Alínea 2', N'IRS'),
                    ('A1510000-0000-4000-8000-000000000003', 3, N'Artigo 98º, Alínea 3', N'IRS'),
                    ('A1510000-0000-4000-8000-000000000004', 4, N'Artigo 98º, Alínea 4', N'IRS'),
                    ('A1510000-0000-4000-8000-000000000005', 5, N'Artigo 98º, Alínea 6', N'IRS'),
                    ('A1510000-0000-4000-8000-000000000006', 6, N'Artigo 98º, Alínea 7', N'IRS'),
                    ('A1510000-0000-4000-8000-000000000007', 7, N'Artigo 96º, Alínea 3', N'IRC'),
                    ('A1510000-0000-4000-8000-000000000008', 8, N'Artigo 96º, Alínea 4', N'IRC'),
                    ('A1510000-0000-4000-8000-000000000009', 9, N'Artigo 96º, Alínea 5', N'IRC'),
                    ('A1510000-0000-4000-8000-00000000000A', 10, N'Artigo 96º, Alínea 6', N'IRC')
                ) v(Id, Codigo, Descricao, TipoImposto)
                WHERE NOT EXISTS (
                    SELECT 1 FROM [Utility].[MotivoRetencao] m
                    WHERE m.Codigo = v.Codigo AND m.DeletedOn IS NULL
                );
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MotivoRetencao",
                schema: "Utility");
        }
    }
}
