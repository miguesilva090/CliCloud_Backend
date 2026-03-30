using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class HistoriaClinicaInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "HistoriaClinica");

            migrationBuilder.CreateTable(
                name: "HistoriasClinicas",
                schema: "HistoriaClinica",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EspecialidadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ObsHtml = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_HistoriasClinicas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoriasClinicas_Especialidade_EspecialidadeId",
                        column: x => x.EspecialidadeId,
                        principalSchema: "Especialidades",
                        principalTable: "Especialidade",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HistoriasClinicas_Medico_MedicoId",
                        column: x => x.MedicoId,
                        principalSchema: "Medicos",
                        principalTable: "Medico",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HistoriasClinicas_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoriasClinicas_EspecialidadeId",
                schema: "HistoriaClinica",
                table: "HistoriasClinicas",
                column: "EspecialidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoriasClinicas_MedicoId",
                schema: "HistoriaClinica",
                table: "HistoriasClinicas",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoriasClinicas_UtenteId",
                schema: "HistoriaClinica",
                table: "HistoriasClinicas",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoriasClinicas",
                schema: "HistoriaClinica");
        }
    }
}
