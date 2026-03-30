using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddExamesSchemaEntitiesAndExamePrescricao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Codigo",
                schema: "Exames",
                table: "Exame");

            migrationBuilder.DropColumn(
                name: "Nome",
                schema: "Exames",
                table: "Exame");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataPrescricao",
                schema: "Exames",
                table: "Exame",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "MedicoId",
                schema: "Exames",
                table: "Exame",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "NumeroPrescricao",
                schema: "Exames",
                table: "Exame",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observacoes",
                schema: "Exames",
                table: "Exame",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganismoId",
                schema: "Exames",
                table: "Exame",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PrioridadeId",
                schema: "Exames",
                table: "Exame",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UtenteId",
                schema: "Exames",
                table: "Exame",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Analises",
                schema: "Exames",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UnidadeMedida = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ValoresReferencia = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoriaProcedimento",
                schema: "Exames",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaProcedimento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoExame",
                schema: "Exames",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Designacao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoriaProcedimentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxaIvaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EAN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MotivoIsencaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RecomendacoesVariaveis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Laboratorio = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_TipoExame", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TipoExame_CategoriaProcedimento_CategoriaProcedimentoId",
                        column: x => x.CategoriaProcedimentoId,
                        principalSchema: "Exames",
                        principalTable: "CategoriaProcedimento",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TipoExame_MotivoIsencao_MotivoIsencaoId",
                        column: x => x.MotivoIsencaoId,
                        principalSchema: "Utility",
                        principalTable: "MotivoIsencao",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TipoExame_TaxaIva_TaxaIvaId",
                        column: x => x.TaxaIvaId,
                        principalSchema: "Utility",
                        principalTable: "TaxaIva",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExameLinha",
                schema: "Exames",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoExameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    Recomendacoes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExameLinha", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExameLinha_Exame_ExameId",
                        column: x => x.ExameId,
                        principalSchema: "Exames",
                        principalTable: "Exame",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExameLinha_TipoExame_TipoExameId",
                        column: x => x.TipoExameId,
                        principalSchema: "Exames",
                        principalTable: "TipoExame",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GrupoAnaliseLinha",
                schema: "Exames",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoExameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnaliseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UnidadeMedida = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ValoresReferencia = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupoAnaliseLinha", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrupoAnaliseLinha_Analises_AnaliseId",
                        column: x => x.AnaliseId,
                        principalSchema: "Exames",
                        principalTable: "Analises",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GrupoAnaliseLinha_TipoExame_TipoExameId",
                        column: x => x.TipoExameId,
                        principalSchema: "Exames",
                        principalTable: "TipoExame",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exame_MedicoId",
                schema: "Exames",
                table: "Exame",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Exame_OrganismoId",
                schema: "Exames",
                table: "Exame",
                column: "OrganismoId");

            migrationBuilder.CreateIndex(
                name: "IX_Exame_PrioridadeId",
                schema: "Exames",
                table: "Exame",
                column: "PrioridadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Exame_UtenteId",
                schema: "Exames",
                table: "Exame",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_ExameLinha_ExameId",
                schema: "Exames",
                table: "ExameLinha",
                column: "ExameId");

            migrationBuilder.CreateIndex(
                name: "IX_ExameLinha_TipoExameId",
                schema: "Exames",
                table: "ExameLinha",
                column: "TipoExameId");

            migrationBuilder.CreateIndex(
                name: "IX_GrupoAnaliseLinha_AnaliseId",
                schema: "Exames",
                table: "GrupoAnaliseLinha",
                column: "AnaliseId");

            migrationBuilder.CreateIndex(
                name: "IX_GrupoAnaliseLinha_TipoExameId",
                schema: "Exames",
                table: "GrupoAnaliseLinha",
                column: "TipoExameId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoExame_CategoriaProcedimentoId",
                schema: "Exames",
                table: "TipoExame",
                column: "CategoriaProcedimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoExame_MotivoIsencaoId",
                schema: "Exames",
                table: "TipoExame",
                column: "MotivoIsencaoId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoExame_TaxaIvaId",
                schema: "Exames",
                table: "TipoExame",
                column: "TaxaIvaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exame_Medico_MedicoId",
                schema: "Exames",
                table: "Exame",
                column: "MedicoId",
                principalSchema: "Medicos",
                principalTable: "Medico",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Exame_Organismo_OrganismoId",
                schema: "Exames",
                table: "Exame",
                column: "OrganismoId",
                principalSchema: "Organismos",
                principalTable: "Organismo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Exame_Prioridades_PrioridadeId",
                schema: "Exames",
                table: "Exame",
                column: "PrioridadeId",
                principalSchema: "Tratamentos",
                principalTable: "Prioridades",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Exame_Utente_UtenteId",
                schema: "Exames",
                table: "Exame",
                column: "UtenteId",
                principalSchema: "Utentes",
                principalTable: "Utente",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exame_Medico_MedicoId",
                schema: "Exames",
                table: "Exame");

            migrationBuilder.DropForeignKey(
                name: "FK_Exame_Organismo_OrganismoId",
                schema: "Exames",
                table: "Exame");

            migrationBuilder.DropForeignKey(
                name: "FK_Exame_Prioridades_PrioridadeId",
                schema: "Exames",
                table: "Exame");

            migrationBuilder.DropForeignKey(
                name: "FK_Exame_Utente_UtenteId",
                schema: "Exames",
                table: "Exame");

            migrationBuilder.DropTable(
                name: "ExameLinha",
                schema: "Exames");

            migrationBuilder.DropTable(
                name: "GrupoAnaliseLinha",
                schema: "Exames");

            migrationBuilder.DropTable(
                name: "Analises",
                schema: "Exames");

            migrationBuilder.DropTable(
                name: "TipoExame",
                schema: "Exames");

            migrationBuilder.DropTable(
                name: "CategoriaProcedimento",
                schema: "Exames");

            migrationBuilder.DropIndex(
                name: "IX_Exame_MedicoId",
                schema: "Exames",
                table: "Exame");

            migrationBuilder.DropIndex(
                name: "IX_Exame_OrganismoId",
                schema: "Exames",
                table: "Exame");

            migrationBuilder.DropIndex(
                name: "IX_Exame_PrioridadeId",
                schema: "Exames",
                table: "Exame");

            migrationBuilder.DropIndex(
                name: "IX_Exame_UtenteId",
                schema: "Exames",
                table: "Exame");

            migrationBuilder.DropColumn(
                name: "DataPrescricao",
                schema: "Exames",
                table: "Exame");

            migrationBuilder.DropColumn(
                name: "MedicoId",
                schema: "Exames",
                table: "Exame");

            migrationBuilder.DropColumn(
                name: "NumeroPrescricao",
                schema: "Exames",
                table: "Exame");

            migrationBuilder.DropColumn(
                name: "Observacoes",
                schema: "Exames",
                table: "Exame");

            migrationBuilder.DropColumn(
                name: "OrganismoId",
                schema: "Exames",
                table: "Exame");

            migrationBuilder.DropColumn(
                name: "PrioridadeId",
                schema: "Exames",
                table: "Exame");

            migrationBuilder.DropColumn(
                name: "UtenteId",
                schema: "Exames",
                table: "Exame");

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                schema: "Exames",
                table: "Exame",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                schema: "Exames",
                table: "Exame",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }
    }
}
