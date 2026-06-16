using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class F18_Pagamentos_GuidReferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CondicaoPagamentoId",
                schema: "Organismos",
                table: "Organismo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModoPagamentoId",
                schema: "Organismos",
                table: "Organismo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CondicaoPagamentoId",
                schema: "Fornecedores",
                table: "Fornecedor",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModoPagamentoId",
                schema: "Fornecedores",
                table: "Fornecedor",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CondicaoPagamentoId",
                schema: "Empresas",
                table: "Empresa",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModoPagamentoId",
                schema: "Empresas",
                table: "Empresa",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CondicaoPagamentoId",
                schema: "Documentos",
                table: "Documento",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModoPagamentoId",
                schema: "Documentos",
                table: "Documento",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE d
                SET d.CondicaoPagamentoId = cp.Id
                FROM [Documentos].[Documento] d
                INNER JOIN [Pagamentos].[CondicaoPagamento] cp
                    ON cp.ClinicaId = d.ClinicaId
                   AND cp.Codigo = d.CondicaoPagamento
                WHERE d.CondicaoPagamento IS NOT NULL;

                UPDATE d
                SET d.ModoPagamentoId = mp.Id
                FROM [Documentos].[Documento] d
                INNER JOIN [Pagamentos].[ModoPagamento] mp
                    ON mp.ClinicaId = d.ClinicaId
                   AND mp.Codigo = d.TipoModoPagamento
                WHERE d.TipoModoPagamento IS NOT NULL;

                UPDATE o
                SET o.CondicaoPagamentoId = cp.Id
                FROM [Organismos].[Organismo] o
                OUTER APPLY (
                    SELECT TOP (1) x.Id
                    FROM [Pagamentos].[CondicaoPagamento] x
                    WHERE x.Codigo = o.CondicaoPagamento
                    ORDER BY x.CreatedOn
                ) cp
                WHERE o.CondicaoPagamento IS NOT NULL;

                UPDATE o
                SET o.ModoPagamentoId = mp.Id
                FROM [Organismos].[Organismo] o
                OUTER APPLY (
                    SELECT TOP (1) x.Id
                    FROM [Pagamentos].[ModoPagamento] x
                    WHERE x.Codigo = o.TipoModoPagamento
                    ORDER BY x.CreatedOn
                ) mp
                WHERE o.TipoModoPagamento IS NOT NULL;

                UPDATE f
                SET f.CondicaoPagamentoId = cp.Id
                FROM [Fornecedores].[Fornecedor] f
                OUTER APPLY (
                    SELECT TOP (1) x.Id
                    FROM [Pagamentos].[CondicaoPagamento] x
                    WHERE x.Codigo = f.CondicaoPagamento
                    ORDER BY x.CreatedOn
                ) cp
                WHERE f.CondicaoPagamento IS NOT NULL;

                UPDATE f
                SET f.ModoPagamentoId = mp.Id
                FROM [Fornecedores].[Fornecedor] f
                OUTER APPLY (
                    SELECT TOP (1) x.Id
                    FROM [Pagamentos].[ModoPagamento] x
                    WHERE x.Codigo = f.TipoModoPagamento
                    ORDER BY x.CreatedOn
                ) mp
                WHERE f.TipoModoPagamento IS NOT NULL;

                UPDATE e
                SET e.CondicaoPagamentoId = cp.Id
                FROM [Empresas].[Empresa] e
                OUTER APPLY (
                    SELECT TOP (1) x.Id
                    FROM [Pagamentos].[CondicaoPagamento] x
                    WHERE x.Codigo = e.CondicaoPagamento
                    ORDER BY x.CreatedOn
                ) cp
                WHERE e.CondicaoPagamento IS NOT NULL;

                UPDATE e
                SET e.ModoPagamentoId = mp.Id
                FROM [Empresas].[Empresa] e
                OUTER APPLY (
                    SELECT TOP (1) x.Id
                    FROM [Pagamentos].[ModoPagamento] x
                    WHERE x.Codigo = e.TipoModoPagamento
                    ORDER BY x.CreatedOn
                ) mp
                WHERE e.TipoModoPagamento IS NOT NULL;
                """);

            migrationBuilder.DropColumn(
                name: "CondicaoPagamento",
                schema: "Organismos",
                table: "Organismo");

            migrationBuilder.DropColumn(
                name: "TipoModoPagamento",
                schema: "Organismos",
                table: "Organismo");

            migrationBuilder.DropColumn(
                name: "CondicaoPagamento",
                schema: "Fornecedores",
                table: "Fornecedor");

            migrationBuilder.DropColumn(
                name: "TipoModoPagamento",
                schema: "Fornecedores",
                table: "Fornecedor");

            migrationBuilder.DropColumn(
                name: "CondicaoPagamento",
                schema: "Empresas",
                table: "Empresa");

            migrationBuilder.DropColumn(
                name: "TipoModoPagamento",
                schema: "Empresas",
                table: "Empresa");

            migrationBuilder.DropColumn(
                name: "CondicaoPagamento",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "TipoModoPagamento",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.CreateIndex(
                name: "IX_Organismo_CondicaoPagamentoId",
                schema: "Organismos",
                table: "Organismo",
                column: "CondicaoPagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Organismo_ModoPagamentoId",
                schema: "Organismos",
                table: "Organismo",
                column: "ModoPagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedor_CondicaoPagamentoId",
                schema: "Fornecedores",
                table: "Fornecedor",
                column: "CondicaoPagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedor_ModoPagamentoId",
                schema: "Fornecedores",
                table: "Fornecedor",
                column: "ModoPagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_CondicaoPagamentoId",
                schema: "Empresas",
                table: "Empresa",
                column: "CondicaoPagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_ModoPagamentoId",
                schema: "Empresas",
                table: "Empresa",
                column: "ModoPagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Documento_CondicaoPagamentoId",
                schema: "Documentos",
                table: "Documento",
                column: "CondicaoPagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Documento_ModoPagamentoId",
                schema: "Documentos",
                table: "Documento",
                column: "ModoPagamentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documento_CondicaoPagamento_CondicaoPagamentoId",
                schema: "Documentos",
                table: "Documento",
                column: "CondicaoPagamentoId",
                principalSchema: "Pagamentos",
                principalTable: "CondicaoPagamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Documento_ModoPagamento_ModoPagamentoId",
                schema: "Documentos",
                table: "Documento",
                column: "ModoPagamentoId",
                principalSchema: "Pagamentos",
                principalTable: "ModoPagamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Empresa_CondicaoPagamento_CondicaoPagamentoId",
                schema: "Empresas",
                table: "Empresa",
                column: "CondicaoPagamentoId",
                principalSchema: "Pagamentos",
                principalTable: "CondicaoPagamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Empresa_ModoPagamento_ModoPagamentoId",
                schema: "Empresas",
                table: "Empresa",
                column: "ModoPagamentoId",
                principalSchema: "Pagamentos",
                principalTable: "ModoPagamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Fornecedor_CondicaoPagamento_CondicaoPagamentoId",
                schema: "Fornecedores",
                table: "Fornecedor",
                column: "CondicaoPagamentoId",
                principalSchema: "Pagamentos",
                principalTable: "CondicaoPagamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Fornecedor_ModoPagamento_ModoPagamentoId",
                schema: "Fornecedores",
                table: "Fornecedor",
                column: "ModoPagamentoId",
                principalSchema: "Pagamentos",
                principalTable: "ModoPagamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Organismo_CondicaoPagamento_CondicaoPagamentoId",
                schema: "Organismos",
                table: "Organismo",
                column: "CondicaoPagamentoId",
                principalSchema: "Pagamentos",
                principalTable: "CondicaoPagamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Organismo_ModoPagamento_ModoPagamentoId",
                schema: "Organismos",
                table: "Organismo",
                column: "ModoPagamentoId",
                principalSchema: "Pagamentos",
                principalTable: "ModoPagamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documento_CondicaoPagamento_CondicaoPagamentoId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropForeignKey(
                name: "FK_Documento_ModoPagamento_ModoPagamentoId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropForeignKey(
                name: "FK_Empresa_CondicaoPagamento_CondicaoPagamentoId",
                schema: "Empresas",
                table: "Empresa");

            migrationBuilder.DropForeignKey(
                name: "FK_Empresa_ModoPagamento_ModoPagamentoId",
                schema: "Empresas",
                table: "Empresa");

            migrationBuilder.DropForeignKey(
                name: "FK_Fornecedor_CondicaoPagamento_CondicaoPagamentoId",
                schema: "Fornecedores",
                table: "Fornecedor");

            migrationBuilder.DropForeignKey(
                name: "FK_Fornecedor_ModoPagamento_ModoPagamentoId",
                schema: "Fornecedores",
                table: "Fornecedor");

            migrationBuilder.DropForeignKey(
                name: "FK_Organismo_CondicaoPagamento_CondicaoPagamentoId",
                schema: "Organismos",
                table: "Organismo");

            migrationBuilder.DropForeignKey(
                name: "FK_Organismo_ModoPagamento_ModoPagamentoId",
                schema: "Organismos",
                table: "Organismo");

            migrationBuilder.DropIndex(
                name: "IX_Organismo_CondicaoPagamentoId",
                schema: "Organismos",
                table: "Organismo");

            migrationBuilder.DropIndex(
                name: "IX_Organismo_ModoPagamentoId",
                schema: "Organismos",
                table: "Organismo");

            migrationBuilder.DropIndex(
                name: "IX_Fornecedor_CondicaoPagamentoId",
                schema: "Fornecedores",
                table: "Fornecedor");

            migrationBuilder.DropIndex(
                name: "IX_Fornecedor_ModoPagamentoId",
                schema: "Fornecedores",
                table: "Fornecedor");

            migrationBuilder.DropIndex(
                name: "IX_Empresa_CondicaoPagamentoId",
                schema: "Empresas",
                table: "Empresa");

            migrationBuilder.DropIndex(
                name: "IX_Empresa_ModoPagamentoId",
                schema: "Empresas",
                table: "Empresa");

            migrationBuilder.DropIndex(
                name: "IX_Documento_CondicaoPagamentoId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropIndex(
                name: "IX_Documento_ModoPagamentoId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "CondicaoPagamentoId",
                schema: "Organismos",
                table: "Organismo");

            migrationBuilder.DropColumn(
                name: "ModoPagamentoId",
                schema: "Organismos",
                table: "Organismo");

            migrationBuilder.DropColumn(
                name: "CondicaoPagamentoId",
                schema: "Fornecedores",
                table: "Fornecedor");

            migrationBuilder.DropColumn(
                name: "ModoPagamentoId",
                schema: "Fornecedores",
                table: "Fornecedor");

            migrationBuilder.DropColumn(
                name: "CondicaoPagamentoId",
                schema: "Empresas",
                table: "Empresa");

            migrationBuilder.DropColumn(
                name: "ModoPagamentoId",
                schema: "Empresas",
                table: "Empresa");

            migrationBuilder.DropColumn(
                name: "CondicaoPagamentoId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "ModoPagamentoId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.AddColumn<int>(
                name: "CondicaoPagamento",
                schema: "Organismos",
                table: "Organismo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoModoPagamento",
                schema: "Organismos",
                table: "Organismo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CondicaoPagamento",
                schema: "Fornecedores",
                table: "Fornecedor",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoModoPagamento",
                schema: "Fornecedores",
                table: "Fornecedor",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CondicaoPagamento",
                schema: "Empresas",
                table: "Empresa",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoModoPagamento",
                schema: "Empresas",
                table: "Empresa",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CondicaoPagamento",
                schema: "Documentos",
                table: "Documento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoModoPagamento",
                schema: "Documentos",
                table: "Documento",
                type: "int",
                nullable: true);
        }
    }
}
