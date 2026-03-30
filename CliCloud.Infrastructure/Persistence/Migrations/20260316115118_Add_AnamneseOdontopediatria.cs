using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_AnamneseOdontopediatria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Estomatologia");

            migrationBuilder.CreateTable(
                name: "AnamneseOdontopediatria",
                schema: "Estomatologia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Peso = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Altura = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PesoPai = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AlturaPai = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PesoMae = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AlturaMae = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CaracteristicasGeraisDesenvolvimento = table.Column<int>(type: "int", nullable: true),
                    TipoAmamentacao = table.Column<int>(type: "int", nullable: true),
                    ObservacaoCardiaca = table.Column<int>(type: "int", nullable: true),
                    ObservacaoRespiracao = table.Column<int>(type: "int", nullable: true),
                    ObservacaoDiccao = table.Column<int>(type: "int", nullable: true),
                    ObservacoesAdicionais = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnamneseOdontopediatria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnamneseOdontopediatria_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnamneseOdontopediatria_UtenteId",
                schema: "Estomatologia",
                table: "AnamneseOdontopediatria",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnamneseOdontopediatria",
                schema: "Estomatologia");
        }
    }
}
