using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMotorDocumentalFase1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModeloDocumento",
                schema: "Documentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Versao = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    ConteudoHtml = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModeloDocumento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstanciaDocumento",
                schema: "Documentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModeloDocumentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VersaoModelo = table.Column<int>(type: "int", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Titulo = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ConteudoHtml = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Assinado = table.Column<bool>(type: "bit", nullable: false),
                    AssinadoEm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssinadoPor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstanciaDocumento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstanciaDocumento_ModeloDocumento_ModeloDocumentoId",
                        column: x => x.ModeloDocumentoId,
                        principalSchema: "Documentos",
                        principalTable: "ModeloDocumento",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FicheiroDocumento",
                schema: "Documentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstanciaDocumentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeOriginal = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NomeArmazenamento = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CaminhoRelativo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    TipoMime = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TamanhoBytes = table.Column<long>(type: "bigint", nullable: false),
                    ChecksumSha256 = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FicheiroDocumento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FicheiroDocumento_InstanciaDocumento_InstanciaDocumentoId",
                        column: x => x.InstanciaDocumentoId,
                        principalSchema: "Documentos",
                        principalTable: "InstanciaDocumento",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FicheiroDocumento_ClinicaId_InstanciaDocumentoId",
                schema: "Documentos",
                table: "FicheiroDocumento",
                columns: new[] { "ClinicaId", "InstanciaDocumentoId" });

            migrationBuilder.CreateIndex(
                name: "IX_FicheiroDocumento_InstanciaDocumentoId",
                schema: "Documentos",
                table: "FicheiroDocumento",
                column: "InstanciaDocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_InstanciaDocumento_ClinicaId_ModeloDocumentoId_CreatedOn",
                schema: "Documentos",
                table: "InstanciaDocumento",
                columns: new[] { "ClinicaId", "ModeloDocumentoId", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_InstanciaDocumento_ModeloDocumentoId",
                schema: "Documentos",
                table: "InstanciaDocumento",
                column: "ModeloDocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_ModeloDocumento_ClinicaId_Codigo_Versao",
                schema: "Documentos",
                table: "ModeloDocumento",
                columns: new[] { "ClinicaId", "Codigo", "Versao" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FicheiroDocumento",
                schema: "Documentos");

            migrationBuilder.DropTable(
                name: "InstanciaDocumento",
                schema: "Documentos");

            migrationBuilder.DropTable(
                name: "ModeloDocumento",
                schema: "Documentos");
        }
    }
}
