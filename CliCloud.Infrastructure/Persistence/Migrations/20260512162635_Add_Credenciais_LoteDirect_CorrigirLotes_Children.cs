using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Credenciais_LoteDirect_CorrigirLotes_Children : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoteDirectAgregado",
                schema: "Credenciais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Indice = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroLote = table.Column<int>(type: "int", nullable: false),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false),
                    CodigoOrganismo = table.Column<int>(type: "int", nullable: false),
                    TipoLote = table.Column<int>(type: "int", nullable: false),
                    TipoServico = table.Column<int>(type: "int", nullable: false),
                    DataLote = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorTaxa = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Isencao = table.Column<int>(type: "int", nullable: true),
                    NumeroRequisicoes = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoteDirectAgregado", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoteDirectLinha",
                schema: "Credenciais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoteDirectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorUtenteOriginal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorInstituicaoOriginal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorUtente = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorInstituicao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoteDirectLinha", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoteDirectLinha_LoteDirect_LoteDirectId",
                        column: x => x.LoteDirectId,
                        principalSchema: "Credenciais",
                        principalTable: "LoteDirect",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoteDirectLinha_Servico_ServicoId",
                        column: x => x.ServicoId,
                        principalSchema: "Servicos",
                        principalTable: "Servico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoteDirectLinha789",
                schema: "Credenciais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoteDirectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorUtenteOriginal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorInstituicaoOriginal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorUtente = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorInstituicao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoteDirectLinha789", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoteDirectLinha789_LoteDirect_LoteDirectId",
                        column: x => x.LoteDirectId,
                        principalSchema: "Credenciais",
                        principalTable: "LoteDirect",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoteDirectLinha789_Servico_ServicoId",
                        column: x => x.ServicoId,
                        principalSchema: "Servicos",
                        principalTable: "Servico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoteDirectDetalhe",
                schema: "Credenciais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoteDirectAgregadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoteDirectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Indice = table.Column<int>(type: "int", nullable: false),
                    NumeroLote = table.Column<int>(type: "int", nullable: false),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false),
                    CodigoOrganismo = table.Column<int>(type: "int", nullable: false),
                    TipoServico = table.Column<int>(type: "int", nullable: false),
                    TipoLote = table.Column<int>(type: "int", nullable: false),
                    Credencial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorTaxa = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Isencao = table.Column<int>(type: "int", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoteDirectDetalhe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoteDirectDetalhe_LoteDirectAgregado_LoteDirectAgregadoId",
                        column: x => x.LoteDirectAgregadoId,
                        principalSchema: "Credenciais",
                        principalTable: "LoteDirectAgregado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoteDirectDetalhe_LoteDirect_LoteDirectId",
                        column: x => x.LoteDirectId,
                        principalSchema: "Credenciais",
                        principalTable: "LoteDirect",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoteDirectAgregado_Ano_Mes",
                schema: "Credenciais",
                table: "LoteDirectAgregado",
                columns: new[] { "Ano", "Mes" });

            migrationBuilder.CreateIndex(
                name: "IX_LoteDirectAgregado_Ano_Mes_CodigoOrganismo_TipoLote_TipoServico_NumeroLote",
                schema: "Credenciais",
                table: "LoteDirectAgregado",
                columns: new[] { "Ano", "Mes", "CodigoOrganismo", "TipoLote", "TipoServico", "NumeroLote" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoteDirectDetalhe_Ano_Mes",
                schema: "Credenciais",
                table: "LoteDirectDetalhe",
                columns: new[] { "Ano", "Mes" });

            migrationBuilder.CreateIndex(
                name: "IX_LoteDirectDetalhe_LoteDirectAgregadoId",
                schema: "Credenciais",
                table: "LoteDirectDetalhe",
                column: "LoteDirectAgregadoId");

            migrationBuilder.CreateIndex(
                name: "IX_LoteDirectDetalhe_LoteDirectId",
                schema: "Credenciais",
                table: "LoteDirectDetalhe",
                column: "LoteDirectId");

            migrationBuilder.CreateIndex(
                name: "IX_LoteDirectLinha_LoteDirectId",
                schema: "Credenciais",
                table: "LoteDirectLinha",
                column: "LoteDirectId");

            migrationBuilder.CreateIndex(
                name: "IX_LoteDirectLinha_ServicoId",
                schema: "Credenciais",
                table: "LoteDirectLinha",
                column: "ServicoId");

            migrationBuilder.CreateIndex(
                name: "IX_LoteDirectLinha789_LoteDirectId",
                schema: "Credenciais",
                table: "LoteDirectLinha789",
                column: "LoteDirectId");

            migrationBuilder.CreateIndex(
                name: "IX_LoteDirectLinha789_ServicoId",
                schema: "Credenciais",
                table: "LoteDirectLinha789",
                column: "ServicoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoteDirectDetalhe",
                schema: "Credenciais");

            migrationBuilder.DropTable(
                name: "LoteDirectLinha",
                schema: "Credenciais");

            migrationBuilder.DropTable(
                name: "LoteDirectLinha789",
                schema: "Credenciais");

            migrationBuilder.DropTable(
                name: "LoteDirectAgregado",
                schema: "Credenciais");
        }
    }
}
