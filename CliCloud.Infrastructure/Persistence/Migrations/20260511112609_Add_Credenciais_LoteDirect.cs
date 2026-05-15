using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Credenciais_LoteDirect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Credenciais");

            migrationBuilder.CreateTable(
                name: "LoteDirect",
                schema: "Credenciais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Credencial = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    NumeroLote = table.Column<int>(type: "int", nullable: true),
                    IndiceLote = table.Column<int>(type: "int", nullable: true),
                    CodigoOrganismo = table.Column<int>(type: "int", nullable: true),
                    Mes = table.Column<int>(type: "int", nullable: true),
                    Ano = table.Column<int>(type: "int", nullable: true),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TipoServico = table.Column<int>(type: "int", nullable: true),
                    TipoLote = table.Column<int>(type: "int", nullable: true),
                    ValorTaxas = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Historico = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoteDirect", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoteDirect_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoteDirect_CodigoOrganismo",
                schema: "Credenciais",
                table: "LoteDirect",
                column: "CodigoOrganismo");

            migrationBuilder.CreateIndex(
                name: "IX_LoteDirect_Credencial",
                schema: "Credenciais",
                table: "LoteDirect",
                column: "Credencial");

            migrationBuilder.CreateIndex(
                name: "IX_LoteDirect_Historico",
                schema: "Credenciais",
                table: "LoteDirect",
                column: "Historico");

            migrationBuilder.CreateIndex(
                name: "IX_LoteDirect_NumeroLote",
                schema: "Credenciais",
                table: "LoteDirect",
                column: "NumeroLote");

            migrationBuilder.CreateIndex(
                name: "IX_LoteDirect_UtenteId",
                schema: "Credenciais",
                table: "LoteDirect",
                column: "UtenteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sinistrado_Utente_UtenteId",
                schema: "Sinistros",
                table: "Sinistrado",
                column: "UtenteId",
                principalSchema: "Utentes",
                principalTable: "Utente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sinistrado_Utente_UtenteId",
                schema: "Sinistros",
                table: "Sinistrado");

            migrationBuilder.DropTable(
                name: "LoteDirect",
                schema: "Credenciais");
        }
    }
}
