using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Separadores_Gestao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Separador",
                schema: "ProcessoClinico",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Separador", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeparadorPersonalizado",
                schema: "ProcessoClinico",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeSeparador = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FormularioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeparadorPersonalizado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeparadorPersonalizado_FichaClinicaSecaoTemplate_FormularioId",
                        column: x => x.FormularioId,
                        principalSchema: "ProcessoClinico",
                        principalTable: "FichaClinicaSecaoTemplate",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SeparadorPersonalizadoVinculo",
                schema: "ProcessoClinico",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeparadorPersonalizadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    EntidadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeparadorPersonalizadoVinculo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeparadorPersonalizadoVinculo_SeparadorPersonalizado_SeparadorPersonalizadoId",
                        column: x => x.SeparadorPersonalizadoId,
                        principalSchema: "ProcessoClinico",
                        principalTable: "SeparadorPersonalizado",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeparadorPersonalizado_ClinicaId_Ordem",
                schema: "ProcessoClinico",
                table: "SeparadorPersonalizado",
                columns: new[] { "ClinicaId", "Ordem" });

            migrationBuilder.CreateIndex(
                name: "IX_SeparadorPersonalizado_FormularioId",
                schema: "ProcessoClinico",
                table: "SeparadorPersonalizado",
                column: "FormularioId");

            migrationBuilder.CreateIndex(
                name: "IX_SeparadorPersonalizadoVinculo_SeparadorPersonalizadoId_Tipo_EntidadeId",
                schema: "ProcessoClinico",
                table: "SeparadorPersonalizadoVinculo",
                columns: new[] { "SeparadorPersonalizadoId", "Tipo", "EntidadeId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Separador",
                schema: "ProcessoClinico");

            migrationBuilder.DropTable(
                name: "SeparadorPersonalizadoVinculo",
                schema: "ProcessoClinico");

            migrationBuilder.DropTable(
                name: "SeparadorPersonalizado",
                schema: "ProcessoClinico");
        }
    }
}
