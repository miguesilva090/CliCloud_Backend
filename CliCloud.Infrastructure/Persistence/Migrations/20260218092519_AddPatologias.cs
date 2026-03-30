using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPatologias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Patologias",
                schema: "Tratamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Designacao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LocalTratamentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrganismoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EspecificacaoTecnica = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Doencas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Inativo = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patologias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patologias_LocaisTratamento_LocalTratamentoId",
                        column: x => x.LocalTratamentoId,
                        principalSchema: "Tratamentos",
                        principalTable: "LocaisTratamento",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Patologias_Organismo_OrganismoId",
                        column: x => x.OrganismoId,
                        principalSchema: "Organismos",
                        principalTable: "Organismo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PatologiaDoenca",
                schema: "Tratamentos",
                columns: table => new
                {
                    PatologiaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoencaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatologiaDoenca", x => new { x.PatologiaId, x.DoencaId });
                    table.ForeignKey(
                        name: "FK_PatologiaDoenca_Doenca_DoencaId",
                        column: x => x.DoencaId,
                        principalSchema: "Doencas",
                        principalTable: "Doenca",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatologiaDoenca_Patologias_PatologiaId",
                        column: x => x.PatologiaId,
                        principalSchema: "Tratamentos",
                        principalTable: "Patologias",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PatologiaServico",
                schema: "Tratamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatologiaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubsistemaServicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Duracao = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    Fisioterapia = table.Column<bool>(type: "bit", nullable: false),
                    Auxiliar = table.Column<bool>(type: "bit", nullable: false),
                    ValorUtente = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorOrganismo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PercentagemInstituicao = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PrecoEur = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatologiaServico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatologiaServico_Patologias_PatologiaId",
                        column: x => x.PatologiaId,
                        principalSchema: "Tratamentos",
                        principalTable: "Patologias",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatologiaServico_SubsistemaServico_SubsistemaServicoId",
                        column: x => x.SubsistemaServicoId,
                        principalSchema: "Servicos",
                        principalTable: "SubsistemaServico",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatologiaDoenca_DoencaId",
                schema: "Tratamentos",
                table: "PatologiaDoenca",
                column: "DoencaId");

            migrationBuilder.CreateIndex(
                name: "IX_Patologias_LocalTratamentoId",
                schema: "Tratamentos",
                table: "Patologias",
                column: "LocalTratamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Patologias_OrganismoId",
                schema: "Tratamentos",
                table: "Patologias",
                column: "OrganismoId");

            migrationBuilder.CreateIndex(
                name: "IX_PatologiaServico_PatologiaId",
                schema: "Tratamentos",
                table: "PatologiaServico",
                column: "PatologiaId");

            migrationBuilder.CreateIndex(
                name: "IX_PatologiaServico_SubsistemaServicoId",
                schema: "Tratamentos",
                table: "PatologiaServico",
                column: "SubsistemaServicoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatologiaDoenca",
                schema: "Tratamentos");

            migrationBuilder.DropTable(
                name: "PatologiaServico",
                schema: "Tratamentos");

            migrationBuilder.DropTable(
                name: "Patologias",
                schema: "Tratamentos");
        }
    }
}
