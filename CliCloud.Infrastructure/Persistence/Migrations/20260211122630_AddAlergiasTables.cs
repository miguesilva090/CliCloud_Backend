using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAlergiasTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Alergias");

            migrationBuilder.CreateTable(
                name: "Alergia",
                schema: "Alergias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alergia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AlergiasUtenteObs",
                schema: "Alergias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InformacaoImportante = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlergiasUtenteObs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlergiasUtenteObs_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GrauAlergia",
                schema: "Alergias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrauAlergia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AlergiaUtente",
                schema: "Alergias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlergiaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GrauAlergiaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataDesde = table.Column<DateOnly>(type: "date", nullable: true),
                    DataAte = table.Column<DateOnly>(type: "date", nullable: true),
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
                    table.PrimaryKey("PK_AlergiaUtente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlergiaUtente_Alergia_AlergiaId",
                        column: x => x.AlergiaId,
                        principalSchema: "Alergias",
                        principalTable: "Alergia",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AlergiaUtente_GrauAlergia_GrauAlergiaId",
                        column: x => x.GrauAlergiaId,
                        principalSchema: "Alergias",
                        principalTable: "GrauAlergia",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AlergiaUtente_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlergiasUtenteObs_UtenteId",
                schema: "Alergias",
                table: "AlergiasUtenteObs",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_AlergiaUtente_AlergiaId",
                schema: "Alergias",
                table: "AlergiaUtente",
                column: "AlergiaId");

            migrationBuilder.CreateIndex(
                name: "IX_AlergiaUtente_GrauAlergiaId",
                schema: "Alergias",
                table: "AlergiaUtente",
                column: "GrauAlergiaId");

            migrationBuilder.CreateIndex(
                name: "IX_AlergiaUtente_UtenteId",
                schema: "Alergias",
                table: "AlergiaUtente",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlergiasUtenteObs",
                schema: "Alergias");

            migrationBuilder.DropTable(
                name: "AlergiaUtente",
                schema: "Alergias");

            migrationBuilder.DropTable(
                name: "Alergia",
                schema: "Alergias");

            migrationBuilder.DropTable(
                name: "GrauAlergia",
                schema: "Alergias");
        }
    }
}
