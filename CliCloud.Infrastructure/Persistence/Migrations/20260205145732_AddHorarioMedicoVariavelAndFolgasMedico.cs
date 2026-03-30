using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddHorarioMedicoVariavelAndFolgasMedico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FolgasMedico",
                schema: "Medicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_FolgasMedico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FolgasMedico_Medico_MedicoId",
                        column: x => x.MedicoId,
                        principalSchema: "Medicos",
                        principalTable: "Medico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HorarioMedicoVariavel",
                schema: "Medicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_HorarioMedicoVariavel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HorarioMedicoVariavel_Medico_MedicoId",
                        column: x => x.MedicoId,
                        principalSchema: "Medicos",
                        principalTable: "Medico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FolgasMedico_MedicoId",
                schema: "Medicos",
                table: "FolgasMedico",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_HorarioMedicoVariavel_MedicoId_Data",
                schema: "Medicos",
                table: "HorarioMedicoVariavel",
                columns: new[] { "MedicoId", "Data" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FolgasMedico",
                schema: "Medicos");

            migrationBuilder.DropTable(
                name: "HorarioMedicoVariavel",
                schema: "Medicos");
        }
    }
}
