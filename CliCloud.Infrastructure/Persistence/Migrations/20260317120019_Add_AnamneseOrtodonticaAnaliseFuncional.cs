using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_AnamneseOrtodonticaAnaliseFuncional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnamneseOrtodonticaAnaliseFuncional",
                schema: "Estomatologia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LabioSuperior = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabioSuperiorTonicidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabioInferior = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabioInferiorTonicidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AspetoLabioSuperiorInferior = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinguaAspeto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinguaTonicidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinguaPosicionamento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MusculaturaFacial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MusculaturaMentoniana = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoRespiracao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Forracao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mastigacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MusculosMastigatorios = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComentariosAdicionais = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnamneseOrtodonticaAnaliseFuncional", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnamneseOrtodonticaAnaliseFuncional_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnamneseOrtodonticaAnaliseFuncional_UtenteId",
                schema: "Estomatologia",
                table: "AnamneseOrtodonticaAnaliseFuncional",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnamneseOrtodonticaAnaliseFuncional",
                schema: "Estomatologia");
        }
    }
}
