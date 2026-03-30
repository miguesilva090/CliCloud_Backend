using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_AnamneseOrtodonticaDenticaoDeciduaeMista : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnamneseOrtodonticaDenticaoDeciduaeMista",
                schema: "Estomatologia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RelacaoMolarDecidua = table.Column<int>(type: "int", nullable: true),
                    DentaduraMista = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SequenciaEsfoliacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SequenciaErupcao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstagioCalcificacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnamneseOrtodonticaDenticaoDeciduaeMista", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnamneseOrtodonticaDenticaoDeciduaeMista_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnamneseOrtodonticaDenticaoDeciduaeMista_UtenteId",
                schema: "Estomatologia",
                table: "AnamneseOrtodonticaDenticaoDeciduaeMista",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnamneseOrtodonticaDenticaoDeciduaeMista",
                schema: "Estomatologia");
        }
    }
}
