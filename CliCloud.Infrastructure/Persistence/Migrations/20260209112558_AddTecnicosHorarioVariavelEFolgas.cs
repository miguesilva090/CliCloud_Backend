using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTecnicosHorarioVariavelEFolgas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FolgasTecnico",
                schema: "Tecnicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TecnicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataDe = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAte = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TodoDia = table.Column<bool>(type: "bit", nullable: false),
                    MesInteiro = table.Column<bool>(type: "bit", nullable: false),
                    ManhaInicio = table.Column<TimeSpan>(type: "time", nullable: true),
                    ManhaFim = table.Column<TimeSpan>(type: "time", nullable: true),
                    TardeInicio = table.Column<TimeSpan>(type: "time", nullable: true),
                    TardeFim = table.Column<TimeSpan>(type: "time", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolgasTecnico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FolgasTecnico_Tecnico_TecnicoId",
                        column: x => x.TecnicoId,
                        principalSchema: "Tecnicos",
                        principalTable: "Tecnico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HorarioTecnicoVariavel",
                schema: "Tecnicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TecnicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ManhaInicio = table.Column<TimeSpan>(type: "time", nullable: true),
                    ManhaFim = table.Column<TimeSpan>(type: "time", nullable: true),
                    TardeInicio = table.Column<TimeSpan>(type: "time", nullable: true),
                    TardeFim = table.Column<TimeSpan>(type: "time", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorarioTecnicoVariavel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HorarioTecnicoVariavel_Tecnico_TecnicoId",
                        column: x => x.TecnicoId,
                        principalSchema: "Tecnicos",
                        principalTable: "Tecnico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FolgasTecnico_TecnicoId",
                schema: "Tecnicos",
                table: "FolgasTecnico",
                column: "TecnicoId");

            migrationBuilder.CreateIndex(
                name: "IX_HorarioTecnicoVariavel_TecnicoId_Data",
                schema: "Tecnicos",
                table: "HorarioTecnicoVariavel",
                columns: new[] { "TecnicoId", "Data" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FolgasTecnico",
                schema: "Tecnicos");

            migrationBuilder.DropTable(
                name: "HorarioTecnicoVariavel",
                schema: "Tecnicos");
        }
    }
}
