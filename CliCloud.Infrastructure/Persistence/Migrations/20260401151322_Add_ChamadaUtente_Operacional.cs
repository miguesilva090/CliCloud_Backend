using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_ChamadaUtente_Operacional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChamadaUtente",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReferenciaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NomeUtente = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NomeProfissional = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Sala = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Senha = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DataHoraChamada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChamadaUtente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChamadaUtente_Clinica_ClinicaId",
                        column: x => x.ClinicaId,
                        principalSchema: "Core",
                        principalTable: "Clinica",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChamadaUtente_ClinicaId_DataHoraChamada",
                schema: "Core",
                table: "ChamadaUtente",
                columns: new[] { "ClinicaId", "DataHoraChamada" });

            migrationBuilder.CreateIndex(
                name: "IX_ChamadaUtente_ClinicaId_Tipo_ReferenciaId_Estado",
                schema: "Core",
                table: "ChamadaUtente",
                columns: new[] { "ClinicaId", "Tipo", "ReferenciaId", "Estado" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChamadaUtente",
                schema: "Core");
        }
    }
}
