using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFase0Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Core");

            migrationBuilder.EnsureSchema(
                name: "Seguradoras");

            migrationBuilder.CreateTable(
                name: "Clinica",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeComercial = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Abreviatura = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Ano = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinica", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clinica_Entidade_Id",
                        column: x => x.Id,
                        principalSchema: "Utility",
                        principalTable: "Entidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exame",
                schema: "Consultas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exame", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recibo",
                schema: "Documentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recibo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recibo_Documento_Id",
                        column: x => x.Id,
                        principalSchema: "Documentos",
                        principalTable: "Documento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seguradora",
                schema: "Seguradoras",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Apolice = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Avenca = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DataInicioContrato = table.Column<DateOnly>(type: "date", nullable: true),
                    DataFimContrato = table.Column<DateOnly>(type: "date", nullable: true),
                    NumeroPagamentos = table.Column<int>(type: "int", nullable: true),
                    PrazoPagamento = table.Column<int>(type: "int", nullable: true),
                    Desconto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DescontoUtente = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Faltas = table.Column<int>(type: "int", nullable: true),
                    Contacto = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Categoria = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NumeroIdentificacaoBancaria = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: true),
                    BancoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CodigoClinica = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Ars = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Subregiao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Regiao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Abreviatura = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seguradora", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seguradora_Banco_BancoId",
                        column: x => x.BancoId,
                        principalSchema: "Bancos",
                        principalTable: "Banco",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Seguradora_Entidade_Id",
                        column: x => x.Id,
                        principalSchema: "Utility",
                        principalTable: "Entidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TipoAparelho",
                schema: "Tratamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Designacao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoAparelho", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Aparelho",
                schema: "Tratamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoAparelhoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoSerie = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CodigoInventario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Local = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Ocupado = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aparelho", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aparelho_TipoAparelho_TipoAparelhoId",
                        column: x => x.TipoAparelhoId,
                        principalSchema: "Tratamentos",
                        principalTable: "TipoAparelho",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tratamento_ReciboId",
                schema: "Tratamentos",
                table: "Tratamento",
                column: "ReciboId");

            migrationBuilder.CreateIndex(
                name: "IX_Tratamento_SeguradoraId",
                schema: "Tratamentos",
                table: "Tratamento",
                column: "SeguradoraId");

            migrationBuilder.CreateIndex(
                name: "IX_SessaoTratamento_ReciboId",
                schema: "Tratamentos",
                table: "SessaoTratamento",
                column: "ReciboId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicoSessao_AparelhoId",
                schema: "Tratamentos",
                table: "ServicoSessao",
                column: "AparelhoId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicoConsulta_ExameId",
                schema: "Consultas",
                table: "ServicoConsulta",
                column: "ExameId");

            migrationBuilder.CreateIndex(
                name: "IX_Servico_TipoAparelhoId",
                schema: "Servicos",
                table: "Servico",
                column: "TipoAparelhoId");

            migrationBuilder.CreateIndex(
                name: "IX_Consulta_ReciboId",
                schema: "Consultas",
                table: "Consulta",
                column: "ReciboId");

            migrationBuilder.CreateIndex(
                name: "IX_Consulta_SeguradoraId",
                schema: "Consultas",
                table: "Consulta",
                column: "SeguradoraId");

            migrationBuilder.CreateIndex(
                name: "IX_Aparelho_TipoAparelhoId",
                schema: "Tratamentos",
                table: "Aparelho",
                column: "TipoAparelhoId");

            migrationBuilder.CreateIndex(
                name: "IX_Seguradora_BancoId",
                schema: "Seguradoras",
                table: "Seguradora",
                column: "BancoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consulta_Recibo_ReciboId",
                schema: "Consultas",
                table: "Consulta",
                column: "ReciboId",
                principalSchema: "Documentos",
                principalTable: "Recibo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Consulta_Seguradora_SeguradoraId",
                schema: "Consultas",
                table: "Consulta",
                column: "SeguradoraId",
                principalSchema: "Seguradoras",
                principalTable: "Seguradora",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Servico_TipoAparelho_TipoAparelhoId",
                schema: "Servicos",
                table: "Servico",
                column: "TipoAparelhoId",
                principalSchema: "Tratamentos",
                principalTable: "TipoAparelho",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServicoConsulta_Exame_ExameId",
                schema: "Consultas",
                table: "ServicoConsulta",
                column: "ExameId",
                principalSchema: "Consultas",
                principalTable: "Exame",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServicoSessao_Aparelho_AparelhoId",
                schema: "Tratamentos",
                table: "ServicoSessao",
                column: "AparelhoId",
                principalSchema: "Tratamentos",
                principalTable: "Aparelho",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SessaoTratamento_Recibo_ReciboId",
                schema: "Tratamentos",
                table: "SessaoTratamento",
                column: "ReciboId",
                principalSchema: "Documentos",
                principalTable: "Recibo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tratamento_Recibo_ReciboId",
                schema: "Tratamentos",
                table: "Tratamento",
                column: "ReciboId",
                principalSchema: "Documentos",
                principalTable: "Recibo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tratamento_Seguradora_SeguradoraId",
                schema: "Tratamentos",
                table: "Tratamento",
                column: "SeguradoraId",
                principalSchema: "Seguradoras",
                principalTable: "Seguradora",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consulta_Recibo_ReciboId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropForeignKey(
                name: "FK_Consulta_Seguradora_SeguradoraId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropForeignKey(
                name: "FK_Servico_TipoAparelho_TipoAparelhoId",
                schema: "Servicos",
                table: "Servico");

            migrationBuilder.DropForeignKey(
                name: "FK_ServicoConsulta_Exame_ExameId",
                schema: "Consultas",
                table: "ServicoConsulta");

            migrationBuilder.DropForeignKey(
                name: "FK_ServicoSessao_Aparelho_AparelhoId",
                schema: "Tratamentos",
                table: "ServicoSessao");

            migrationBuilder.DropForeignKey(
                name: "FK_SessaoTratamento_Recibo_ReciboId",
                schema: "Tratamentos",
                table: "SessaoTratamento");

            migrationBuilder.DropForeignKey(
                name: "FK_Tratamento_Recibo_ReciboId",
                schema: "Tratamentos",
                table: "Tratamento");

            migrationBuilder.DropForeignKey(
                name: "FK_Tratamento_Seguradora_SeguradoraId",
                schema: "Tratamentos",
                table: "Tratamento");

            migrationBuilder.DropTable(
                name: "Aparelho",
                schema: "Tratamentos");

            migrationBuilder.DropTable(
                name: "Clinica",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "Exame",
                schema: "Consultas");

            migrationBuilder.DropTable(
                name: "Recibo",
                schema: "Documentos");

            migrationBuilder.DropTable(
                name: "Seguradora",
                schema: "Seguradoras");

            migrationBuilder.DropTable(
                name: "TipoAparelho",
                schema: "Tratamentos");

            migrationBuilder.DropIndex(
                name: "IX_Tratamento_ReciboId",
                schema: "Tratamentos",
                table: "Tratamento");

            migrationBuilder.DropIndex(
                name: "IX_Tratamento_SeguradoraId",
                schema: "Tratamentos",
                table: "Tratamento");

            migrationBuilder.DropIndex(
                name: "IX_SessaoTratamento_ReciboId",
                schema: "Tratamentos",
                table: "SessaoTratamento");

            migrationBuilder.DropIndex(
                name: "IX_ServicoSessao_AparelhoId",
                schema: "Tratamentos",
                table: "ServicoSessao");

            migrationBuilder.DropIndex(
                name: "IX_ServicoConsulta_ExameId",
                schema: "Consultas",
                table: "ServicoConsulta");

            migrationBuilder.DropIndex(
                name: "IX_Servico_TipoAparelhoId",
                schema: "Servicos",
                table: "Servico");

            migrationBuilder.DropIndex(
                name: "IX_Consulta_ReciboId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropIndex(
                name: "IX_Consulta_SeguradoraId",
                schema: "Consultas",
                table: "Consulta");
        }
    }
}
