using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class F01_DocumentoFiscal_Entidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Documento_TipoDocumentoId_NumeroDocumento",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.AlterColumn<string>(
                name: "NomeCliente",
                schema: "Documentos",
                table: "Documento",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MoradaCliente",
                schema: "Documentos",
                table: "Documento",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnoFiscal",
                schema: "Documentos",
                table: "Documento",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Anulado",
                schema: "Documentos",
                table: "Documento",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "BancoId",
                schema: "Documentos",
                table: "Documento",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BeneficiarioFaturaGlobal",
                schema: "Documentos",
                table: "Documento",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CaixaId",
                schema: "Documentos",
                table: "Documento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ClinicaId",
                schema: "Documentos",
                table: "Documento",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "CodigoAtcud",
                schema: "Documentos",
                table: "Documento",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoValidacaoTransporte",
                schema: "Documentos",
                table: "Documento",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAnulacao",
                schema: "Documentos",
                table: "Documento",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataDocumentoOrigem",
                schema: "Documentos",
                table: "Documento",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataTransporte",
                schema: "Documentos",
                table: "Documento",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataVencimentoPagamento",
                schema: "Documentos",
                table: "Documento",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DescontoPagamento",
                schema: "Documentos",
                table: "Documento",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentoOrigemId",
                schema: "Documentos",
                table: "Documento",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EstaEmitido",
                schema: "Documentos",
                table: "Documento",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "EstadoDocumento",
                schema: "Documentos",
                table: "Documento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FaturaGlobalDataFim",
                schema: "Documentos",
                table: "Documento",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FaturaGlobalDataInicio",
                schema: "Documentos",
                table: "Documento",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HashDocumentoOrigem",
                schema: "Documentos",
                table: "Documento",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoraTransporte",
                schema: "Documentos",
                table: "Documento",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorUnicoDocumentoOrigem",
                schema: "Documentos",
                table: "Documento",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IvaCaixa",
                schema: "Documentos",
                table: "Documento",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ModuloOrigem",
                schema: "Documentos",
                table: "Documento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MoedaId",
                schema: "Documentos",
                table: "Documento",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoAnulacao",
                schema: "Documentos",
                table: "Documento",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroExibicao",
                schema: "Documentos",
                table: "Documento",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecoUnitarioMercadorias",
                schema: "Documentos",
                table: "Documento",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QrCodeRecibo",
                schema: "Documentos",
                table: "Documento",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RetencaoImposto",
                schema: "Documentos",
                table: "Documento",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RetencaoTaxa",
                schema: "Documentos",
                table: "Documento",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RetencaoValor",
                schema: "Documentos",
                table: "Documento",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxaCambio",
                schema: "Documentos",
                table: "Documento",
                type: "decimal(18,6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoCambio",
                schema: "Documentos",
                table: "Documento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalBruto",
                schema: "Documentos",
                table: "Documento",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DocumentoLinha",
                schema: "Documentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroLinha = table.Column<int>(type: "int", nullable: false),
                    CodigoArtigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ServicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AdmissaoServicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PercentagemDesconto = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    ValorDesconto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DescontoTipo1 = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    DescontoTipo2 = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    DescontoTipo3 = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    TotalLinha = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxaIvaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaxaIvaPercentagem = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorImposto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ModuloOrigemLinha = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoLinha", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentoLinha_AdmissaoServico_AdmissaoServicoId",
                        column: x => x.AdmissaoServicoId,
                        principalSchema: "Consultas",
                        principalTable: "AdmissaoServico",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DocumentoLinha_Documento_DocumentoId",
                        column: x => x.DocumentoId,
                        principalSchema: "Documentos",
                        principalTable: "Documento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentoLinha_Servico_ServicoId",
                        column: x => x.ServicoId,
                        principalSchema: "Servicos",
                        principalTable: "Servico",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DocumentoLinha_TaxaIva_TaxaIvaId",
                        column: x => x.TaxaIvaId,
                        principalSchema: "Utility",
                        principalTable: "TaxaIva",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DocumentoOrigemClinica",
                schema: "Documentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuloOrigem = table.Column<int>(type: "int", nullable: false),
                    AdmissaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConsultaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NotaLiquidacao = table.Column<int>(type: "int", nullable: true),
                    FiltroOrigem = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoOrigemClinica", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentoOrigemClinica_Admissao_AdmissaoId",
                        column: x => x.AdmissaoId,
                        principalSchema: "Consultas",
                        principalTable: "Admissao",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DocumentoOrigemClinica_Consulta_ConsultaId",
                        column: x => x.ConsultaId,
                        principalSchema: "Consultas",
                        principalTable: "Consulta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DocumentoOrigemClinica_Documento_DocumentoId",
                        column: x => x.DocumentoId,
                        principalSchema: "Documentos",
                        principalTable: "Documento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documento_BancoId",
                schema: "Documentos",
                table: "Documento",
                column: "BancoId");

            migrationBuilder.CreateIndex(
                name: "IX_Documento_ClinicaId",
                schema: "Documentos",
                table: "Documento",
                column: "ClinicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Documento_ClinicaId_TipoDocumentoId_AnoFiscal_NumeroDocumento",
                schema: "Documentos",
                table: "Documento",
                columns: new[] { "ClinicaId", "TipoDocumentoId", "AnoFiscal", "NumeroDocumento" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documento_DocumentoOrigemId",
                schema: "Documentos",
                table: "Documento",
                column: "DocumentoOrigemId");

            migrationBuilder.CreateIndex(
                name: "IX_Documento_EstadoDocumento",
                schema: "Documentos",
                table: "Documento",
                column: "EstadoDocumento");

            migrationBuilder.CreateIndex(
                name: "IX_Documento_MoedaId",
                schema: "Documentos",
                table: "Documento",
                column: "MoedaId");

            migrationBuilder.CreateIndex(
                name: "IX_Documento_NumeroExibicao",
                schema: "Documentos",
                table: "Documento",
                column: "NumeroExibicao");

            migrationBuilder.CreateIndex(
                name: "IX_Documento_TipoDocumentoId",
                schema: "Documentos",
                table: "Documento",
                column: "TipoDocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoLinha_AdmissaoServicoId",
                schema: "Documentos",
                table: "DocumentoLinha",
                column: "AdmissaoServicoId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoLinha_DocumentoId_NumeroLinha",
                schema: "Documentos",
                table: "DocumentoLinha",
                columns: new[] { "DocumentoId", "NumeroLinha" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoLinha_ServicoId",
                schema: "Documentos",
                table: "DocumentoLinha",
                column: "ServicoId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoLinha_TaxaIvaId",
                schema: "Documentos",
                table: "DocumentoLinha",
                column: "TaxaIvaId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoOrigemClinica_AdmissaoId",
                schema: "Documentos",
                table: "DocumentoOrigemClinica",
                column: "AdmissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoOrigemClinica_ConsultaId",
                schema: "Documentos",
                table: "DocumentoOrigemClinica",
                column: "ConsultaId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoOrigemClinica_DocumentoId",
                schema: "Documentos",
                table: "DocumentoOrigemClinica",
                column: "DocumentoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Documento_Banco_BancoId",
                schema: "Documentos",
                table: "Documento",
                column: "BancoId",
                principalSchema: "Bancos",
                principalTable: "Banco",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Documento_Clinica_ClinicaId",
                schema: "Documentos",
                table: "Documento",
                column: "ClinicaId",
                principalSchema: "Core",
                principalTable: "Clinica",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Documento_Documento_DocumentoOrigemId",
                schema: "Documentos",
                table: "Documento",
                column: "DocumentoOrigemId",
                principalSchema: "Documentos",
                principalTable: "Documento",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Documento_Moeda_MoedaId",
                schema: "Documentos",
                table: "Documento",
                column: "MoedaId",
                principalSchema: "Utility",
                principalTable: "Moeda",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documento_Banco_BancoId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropForeignKey(
                name: "FK_Documento_Clinica_ClinicaId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropForeignKey(
                name: "FK_Documento_Documento_DocumentoOrigemId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropForeignKey(
                name: "FK_Documento_Moeda_MoedaId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropTable(
                name: "DocumentoLinha",
                schema: "Documentos");

            migrationBuilder.DropTable(
                name: "DocumentoOrigemClinica",
                schema: "Documentos");

            migrationBuilder.DropIndex(
                name: "IX_Documento_BancoId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropIndex(
                name: "IX_Documento_ClinicaId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropIndex(
                name: "IX_Documento_ClinicaId_TipoDocumentoId_AnoFiscal_NumeroDocumento",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropIndex(
                name: "IX_Documento_DocumentoOrigemId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropIndex(
                name: "IX_Documento_EstadoDocumento",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropIndex(
                name: "IX_Documento_MoedaId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropIndex(
                name: "IX_Documento_NumeroExibicao",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropIndex(
                name: "IX_Documento_TipoDocumentoId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "AnoFiscal",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "Anulado",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "BancoId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "BeneficiarioFaturaGlobal",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "CaixaId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "ClinicaId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "CodigoAtcud",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "CodigoValidacaoTransporte",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "DataAnulacao",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "DataDocumentoOrigem",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "DataTransporte",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "DataVencimentoPagamento",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "DescontoPagamento",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "DocumentoOrigemId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "EstaEmitido",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "EstadoDocumento",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "FaturaGlobalDataFim",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "FaturaGlobalDataInicio",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "HashDocumentoOrigem",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "HoraTransporte",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnicoDocumentoOrigem",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "IvaCaixa",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "ModuloOrigem",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "MoedaId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "MotivoAnulacao",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "NumeroExibicao",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "PrecoUnitarioMercadorias",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "QrCodeRecibo",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "RetencaoImposto",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "RetencaoTaxa",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "RetencaoValor",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "TaxaCambio",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "TipoCambio",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "TotalBruto",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.AlterColumn<string>(
                name: "NomeCliente",
                schema: "Documentos",
                table: "Documento",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MoradaCliente",
                schema: "Documentos",
                table: "Documento",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documento_TipoDocumentoId_NumeroDocumento",
                schema: "Documentos",
                table: "Documento",
                columns: new[] { "TipoDocumentoId", "NumeroDocumento" },
                unique: true);
        }
    }
}
