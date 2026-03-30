using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAntecedentesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Antecedentes");

            migrationBuilder.CreateTable(
                name: "AntecedentesCirurgicos",
                schema: "Antecedentes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ano = table.Column<int>(type: "int", nullable: true),
                    TipoCirurgia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouveComplicacoes = table.Column<bool>(type: "bit", nullable: true),
                    Complicacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntecedentesCirurgicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AntecedentesCirurgicos_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AntecedentesFamiliaresUtente",
                schema: "Antecedentes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoencaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NomeDoenca = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ano = table.Column<int>(type: "int", nullable: true),
                    Idade = table.Column<int>(type: "int", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GrauParentescoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntecedentesFamiliaresUtente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AntecedentesFamiliaresUtente_Doenca_DoencaId",
                        column: x => x.DoencaId,
                        principalSchema: "Doencas",
                        principalTable: "Doenca",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AntecedentesFamiliaresUtente_GrauParentesco_GrauParentescoId",
                        column: x => x.GrauParentescoId,
                        principalSchema: "Utility",
                        principalTable: "GrauParentesco",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AntecedentesFamiliaresUtente_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AntecedentesPessoais",
                schema: "Antecedentes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoencaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NomeDoenca = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ano = table.Column<int>(type: "int", nullable: true),
                    Idade = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_AntecedentesPessoais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AntecedentesPessoais_Doenca_DoencaId",
                        column: x => x.DoencaId,
                        principalSchema: "Doencas",
                        principalTable: "Doenca",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AntecedentesPessoais_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AntecedentesCirurgicos_UtenteId",
                schema: "Antecedentes",
                table: "AntecedentesCirurgicos",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_AntecedentesFamiliaresUtente_DoencaId",
                schema: "Antecedentes",
                table: "AntecedentesFamiliaresUtente",
                column: "DoencaId");

            migrationBuilder.CreateIndex(
                name: "IX_AntecedentesFamiliaresUtente_GrauParentescoId",
                schema: "Antecedentes",
                table: "AntecedentesFamiliaresUtente",
                column: "GrauParentescoId");

            migrationBuilder.CreateIndex(
                name: "IX_AntecedentesFamiliaresUtente_UtenteId",
                schema: "Antecedentes",
                table: "AntecedentesFamiliaresUtente",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_AntecedentesPessoais_DoencaId",
                schema: "Antecedentes",
                table: "AntecedentesPessoais",
                column: "DoencaId");

            migrationBuilder.CreateIndex(
                name: "IX_AntecedentesPessoais_UtenteId",
                schema: "Antecedentes",
                table: "AntecedentesPessoais",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AntecedentesCirurgicos",
                schema: "Antecedentes");

            migrationBuilder.DropTable(
                name: "AntecedentesFamiliaresUtente",
                schema: "Antecedentes");

            migrationBuilder.DropTable(
                name: "AntecedentesPessoais",
                schema: "Antecedentes");
        }
    }
}
