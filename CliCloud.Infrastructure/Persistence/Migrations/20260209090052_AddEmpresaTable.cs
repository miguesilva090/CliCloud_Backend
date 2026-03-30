using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEmpresaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Empresas");

            migrationBuilder.CreateTable(
                name: "Empresa",
                schema: "Empresas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrazoPagamento = table.Column<int>(type: "int", nullable: true),
                    Desconto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DescontoUtente = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CondicaoPagamento = table.Column<int>(type: "int", nullable: true),
                    TipoModoPagamento = table.Column<int>(type: "int", nullable: true),
                    BancoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NumeroIdentificacaoBancaria = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: true),
                    Apolice = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Avenca = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DataInicioContrato = table.Column<DateOnly>(type: "date", nullable: true),
                    DataFimContrato = table.Column<DateOnly>(type: "date", nullable: true),
                    NumeroPagamentos = table.Column<int>(type: "int", nullable: true),
                    Categoria = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Actividade = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    Cae = table.Column<int>(type: "int", nullable: true),
                    CodigoClinica = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    NumeroTrabalhadores = table.Column<int>(type: "int", nullable: true),
                    ValorTrabalhador = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Rescindindo = table.Column<int>(type: "int", nullable: true),
                    Contacto = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Empresa_Banco_BancoId",
                        column: x => x.BancoId,
                        principalSchema: "Bancos",
                        principalTable: "Banco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Empresa_Entidade_Id",
                        column: x => x.Id,
                        principalSchema: "Utility",
                        principalTable: "Entidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_BancoId",
                schema: "Empresas",
                table: "Empresa",
                column: "BancoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Empresa",
                schema: "Empresas");
        }
    }
}
