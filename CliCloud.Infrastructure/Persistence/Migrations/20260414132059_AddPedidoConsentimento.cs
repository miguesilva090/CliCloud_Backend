using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPedidoConsentimento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PedidoConsentimento",
                schema: "Documentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstanciaDocumentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    TipoConsentimento = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Canal = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    ExpiraEm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssinadoEm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssinadoPor = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
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
                    table.PrimaryKey("PK_PedidoConsentimento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoConsentimento_InstanciaDocumento_InstanciaDocumentoId",
                        column: x => x.InstanciaDocumentoId,
                        principalSchema: "Documentos",
                        principalTable: "InstanciaDocumento",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PedidoConsentimento_ClinicaId_Estado_CreatedOn",
                schema: "Documentos",
                table: "PedidoConsentimento",
                columns: new[] { "ClinicaId", "Estado", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_PedidoConsentimento_ClinicaId_UtenteId_TipoConsentimento",
                schema: "Documentos",
                table: "PedidoConsentimento",
                columns: new[] { "ClinicaId", "UtenteId", "TipoConsentimento" });

            migrationBuilder.CreateIndex(
                name: "IX_PedidoConsentimento_InstanciaDocumentoId",
                schema: "Documentos",
                table: "PedidoConsentimento",
                column: "InstanciaDocumentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PedidoConsentimento",
                schema: "Documentos");
        }
    }
}
