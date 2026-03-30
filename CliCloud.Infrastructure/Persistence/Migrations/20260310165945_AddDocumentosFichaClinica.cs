using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentosFichaClinica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentosFichaClinica",
                schema: "ProcessoClinico",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Categoria = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NomeFicheiro = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    CaminhoRelativo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Terminacao = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsVideo = table.Column<bool>(type: "bit", nullable: false),
                    UploadedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentosFichaClinica", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentosFichaClinica_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosFichaClinica_UtenteId",
                schema: "ProcessoClinico",
                table: "DocumentosFichaClinica",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentosFichaClinica",
                schema: "ProcessoClinico");
        }
    }
}
