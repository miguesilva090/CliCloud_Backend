using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOdontologiaOdontogramaEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Odontologia");

            migrationBuilder.CreateTable(
                name: "EstadosDentarios",
                schema: "Odontologia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EstadoPadrao = table.Column<bool>(type: "bit", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosDentarios", x => x.Id);
                    table.UniqueConstraint("AK_EstadosDentarios_Codigo", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "TiposTratamentoDentario",
                schema: "Odontologia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Faturavel = table.Column<bool>(type: "bit", nullable: false),
                    CodigoServicoAssociado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NomeServicoAssociado = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposTratamentoDentario", x => x.Id);
                    table.UniqueConstraint("AK_TiposTratamentoDentario_Codigo", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "OdontogramaDefinitivo",
                schema: "Odontologia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsultaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroDente = table.Column<int>(type: "int", nullable: false),
                    NumeroDenteAte = table.Column<int>(type: "int", nullable: true),
                    CodigoSuperficie = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CodigoEstadoPadrao = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CodigoTratamentoPadrao = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CodigoEstadoPersonalizado = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CodigoTratamentoPersonalizado = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Faturar = table.Column<bool>(type: "bit", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    ValorServico = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorUtente = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorEntidade = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LinhaFaturacaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdontogramaDefinitivo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OdontogramaDefinitivo_Consulta_ConsultaId",
                        column: x => x.ConsultaId,
                        principalSchema: "Consultas",
                        principalTable: "Consulta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OdontogramaDefinitivo_EstadosDentarios_CodigoEstadoPadrao",
                        column: x => x.CodigoEstadoPadrao,
                        principalSchema: "Odontologia",
                        principalTable: "EstadosDentarios",
                        principalColumn: "Codigo");
                    table.ForeignKey(
                        name: "FK_OdontogramaDefinitivo_TiposTratamentoDentario_CodigoTratamentoPadrao",
                        column: x => x.CodigoTratamentoPadrao,
                        principalSchema: "Odontologia",
                        principalTable: "TiposTratamentoDentario",
                        principalColumn: "Codigo");
                    table.ForeignKey(
                        name: "FK_OdontogramaDefinitivo_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OdontogramaDefinitivo_CodigoEstadoPadrao",
                schema: "Odontologia",
                table: "OdontogramaDefinitivo",
                column: "CodigoEstadoPadrao");

            migrationBuilder.CreateIndex(
                name: "IX_OdontogramaDefinitivo_CodigoTratamentoPadrao",
                schema: "Odontologia",
                table: "OdontogramaDefinitivo",
                column: "CodigoTratamentoPadrao");

            migrationBuilder.CreateIndex(
                name: "IX_OdontogramaDefinitivo_ConsultaId",
                schema: "Odontologia",
                table: "OdontogramaDefinitivo",
                column: "ConsultaId");

            migrationBuilder.CreateIndex(
                name: "IX_OdontogramaDefinitivo_UtenteId",
                schema: "Odontologia",
                table: "OdontogramaDefinitivo",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OdontogramaDefinitivo",
                schema: "Odontologia");

            migrationBuilder.DropTable(
                name: "EstadosDentarios",
                schema: "Odontologia");

            migrationBuilder.DropTable(
                name: "TiposTratamentoDentario",
                schema: "Odontologia");
        }
    }
}
