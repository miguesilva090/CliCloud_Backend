using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class F09_SinistradoLinhaServico_ServicoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CodigoServico",
                schema: "Sinistros",
                table: "SinistradoLinhaServico",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(36)",
                oldMaxLength: 36);

            migrationBuilder.AddColumn<Guid>(
                name: "ServicoId",
                schema: "Sinistros",
                table: "SinistradoLinhaServico",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SinistradoLinhaServico_ServicoId",
                schema: "Sinistros",
                table: "SinistradoLinhaServico",
                column: "ServicoId");

            // CodigoServico com GUID completo (36) ou truncado (F08) → ServicoId
            migrationBuilder.Sql("""
                UPDATE s
                SET s.ServicoId = TRY_CONVERT(uniqueidentifier, s.CodigoServico)
                FROM [Sinistros].[SinistradoLinhaServico] s
                WHERE s.ServicoId IS NULL
                  AND TRY_CONVERT(uniqueidentifier, s.CodigoServico) IS NOT NULL;
                """);

            // GUID truncado nos primeiros 36 caracteres
            migrationBuilder.Sql("""
                UPDATE s
                SET s.ServicoId = sv.Id
                FROM [Sinistros].[SinistradoLinhaServico] s
                INNER JOIN [Servicos].[Servico] sv
                    ON REPLACE(CAST(sv.Id AS nvarchar(36)), '-', '')
                       LIKE REPLACE(s.CodigoServico, '-', '') + '%'
                WHERE s.ServicoId IS NULL
                  AND LEN(REPLACE(s.CodigoServico, '-', '')) >= 24
                  AND s.CodigoServico NOT LIKE 'CONS-%'
                  AND s.CodigoServico NOT LIKE 'TRAT-%';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SinistradoLinhaServico_ServicoId",
                schema: "Sinistros",
                table: "SinistradoLinhaServico");

            migrationBuilder.DropColumn(
                name: "ServicoId",
                schema: "Sinistros",
                table: "SinistradoLinhaServico");

            migrationBuilder.AlterColumn<string>(
                name: "CodigoServico",
                schema: "Sinistros",
                table: "SinistradoLinhaServico",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40);
        }
    }
}
