using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class F23_Faturacao_AdseComunicacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdseCoPagamento",
                schema: "Faturacao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoPreFatura = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    NumOrdemPreFatura = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    DataComunicacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorTotalUtente = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorTotalAdse = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PdfFicheiro = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: true),
                    PdfRelatorioFicheiro = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: true),
                    Erros = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataDevolucao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NumDevolucoes = table.Column<int>(type: "int", nullable: false),
                    OrigemClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroDevolucao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdseCoPagamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdseCoPagamento_Documento_DocumentoId",
                        column: x => x.DocumentoId,
                        principalSchema: "Documentos",
                        principalTable: "Documento",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdsePreFatura",
                schema: "Faturacao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoPreFatura = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    NumOrdem = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    DataAbertura = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFecho = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumDocumentos = table.Column<int>(type: "int", nullable: false),
                    PdfFicheiro = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: true),
                    DocumentoFechoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferenciaSerie = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReferenciaNumeroDocumento = table.Column<int>(type: "int", nullable: true),
                    ReferenciaData = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReferenciaValor = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdsePreFatura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdsePreFatura_Documento_DocumentoFechoId",
                        column: x => x.DocumentoFechoId,
                        principalSchema: "Documentos",
                        principalTable: "Documento",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdseCoPagamento_DocumentoId",
                schema: "Faturacao",
                table: "AdseCoPagamento",
                column: "DocumentoId",
                unique: true,
                filter: "[DeletedOn] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AdsePreFatura_ClinicaId_TipoPreFatura_NumOrdem",
                schema: "Faturacao",
                table: "AdsePreFatura",
                columns: new[] { "ClinicaId", "TipoPreFatura", "NumOrdem" },
                unique: true,
                filter: "[DeletedOn] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AdsePreFatura_DocumentoFechoId",
                schema: "Faturacao",
                table: "AdsePreFatura",
                column: "DocumentoFechoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdseCoPagamento",
                schema: "Faturacao");

            migrationBuilder.DropTable(
                name: "AdsePreFatura",
                schema: "Faturacao");
        }
    }
}
