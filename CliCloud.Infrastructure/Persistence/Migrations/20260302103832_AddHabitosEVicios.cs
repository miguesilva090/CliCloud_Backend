using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddHabitosEVicios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HabitosEVicios",
                schema: "ProcessoClinico",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsumoDeFrutas = table.Column<bool>(type: "bit", nullable: false),
                    ConsumoAgua = table.Column<bool>(type: "bit", nullable: false),
                    QuantidadeAgua = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ConsumoPeixe = table.Column<bool>(type: "bit", nullable: false),
                    ConsumoCarne = table.Column<bool>(type: "bit", nullable: false),
                    TipoCarne = table.Column<int>(type: "int", nullable: true),
                    ConsumoVegetais = table.Column<bool>(type: "bit", nullable: false),
                    IngestaoLeite = table.Column<bool>(type: "bit", nullable: false),
                    ConsumoSalgados = table.Column<bool>(type: "bit", nullable: false),
                    ConsumoAcucarados = table.Column<bool>(type: "bit", nullable: false),
                    ConsumoBebidasAlcoolicas = table.Column<bool>(type: "bit", nullable: false),
                    BebidasAlcoolicas = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    QuantidadeAlcool = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AlcoolDesdeQuando = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Fuma = table.Column<bool>(type: "bit", nullable: false),
                    QuantosFumaDia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TabacoDesdeQuando = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConsumoDrogas = table.Column<bool>(type: "bit", nullable: false),
                    Drogas = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DrogasDesdeQuando = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OutrosVicios = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OutrosViciosDesdeQuando = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ObservacoesHabitosAlimentaresEVicios = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ObservacoesHabitosMedicamentosExercicioFisico = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TomaFarmacosPrescritos = table.Column<bool>(type: "bit", nullable: false),
                    TomaFarmacosSemReceita = table.Column<bool>(type: "bit", nullable: false),
                    FarmacosSemReceita = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PraticaExercicioFisico = table.Column<bool>(type: "bit", nullable: false),
                    TipoExercicioFisico = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FrequenciaExFisico = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitosEVicios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HabitosEVicios_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HabitosEVicios_UtenteId",
                schema: "ProcessoClinico",
                table: "HabitosEVicios",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HabitosEVicios",
                schema: "ProcessoClinico");
        }
    }
}
