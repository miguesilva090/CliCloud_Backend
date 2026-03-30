using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionarioUtente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ProcessoClinico");

            migrationBuilder.CreateTable(
                name: "QuestionariosUtente",
                schema: "ProcessoClinico",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstaFazerTratamento = table.Column<bool>(type: "bit", nullable: false),
                    DuracaoTratamento = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DescTratamento = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProblemasNeurologicos = table.Column<bool>(type: "bit", nullable: false),
                    ProblemasNeuromusculares = table.Column<bool>(type: "bit", nullable: false),
                    ProblemasRespiratorios = table.Column<bool>(type: "bit", nullable: false),
                    ProblemasArticularesOuReumatismo = table.Column<bool>(type: "bit", nullable: false),
                    TonturasEZumbidosNosOuvidos = table.Column<bool>(type: "bit", nullable: false),
                    ProblemasComportamento = table.Column<bool>(type: "bit", nullable: false),
                    ProblemasRenais = table.Column<bool>(type: "bit", nullable: false),
                    TonturasCardiovasculares = table.Column<bool>(type: "bit", nullable: false),
                    ProblemasDigestivos = table.Column<bool>(type: "bit", nullable: false),
                    ProblemasGastricos = table.Column<bool>(type: "bit", nullable: false),
                    FebreReumatica = table.Column<bool>(type: "bit", nullable: false),
                    ProblemasIntestinais = table.Column<bool>(type: "bit", nullable: false),
                    ProblemasUrinarios = table.Column<bool>(type: "bit", nullable: false),
                    ProblemasComAnestesia = table.Column<bool>(type: "bit", nullable: false),
                    ProblemasDeHomorragia = table.Column<bool>(type: "bit", nullable: false),
                    ProblemasFormigueiroDormencia = table.Column<bool>(type: "bit", nullable: false),
                    ProblemasCicratizacao = table.Column<bool>(type: "bit", nullable: false),
                    HepatiteSida = table.Column<bool>(type: "bit", nullable: false),
                    Gravidez = table.Column<bool>(type: "bit", nullable: false),
                    SedeEBocaSeca = table.Column<bool>(type: "bit", nullable: false),
                    UsaPacemaker = table.Column<bool>(type: "bit", nullable: false),
                    DoencaAutoImune = table.Column<bool>(type: "bit", nullable: true),
                    DoencaCronica = table.Column<bool>(type: "bit", nullable: true),
                    DoencaGenetica = table.Column<bool>(type: "bit", nullable: true),
                    ProblemasDiabetes = table.Column<bool>(type: "bit", nullable: false),
                    TipoDiabetes = table.Column<int>(type: "int", nullable: true),
                    TensaoArterial = table.Column<bool>(type: "bit", nullable: false),
                    TipoTensaoArterial = table.Column<int>(type: "int", nullable: true),
                    Colestrol = table.Column<bool>(type: "bit", nullable: false),
                    TipoColestrol = table.Column<int>(type: "int", nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionariosUtente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionariosUtente_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionariosUtente_UtenteId",
                schema: "ProcessoClinico",
                table: "QuestionariosUtente",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionariosUtente",
                schema: "ProcessoClinico");
        }
    }
}
