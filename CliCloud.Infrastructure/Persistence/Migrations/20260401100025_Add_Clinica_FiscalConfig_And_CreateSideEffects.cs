using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Clinica_FiscalConfig_And_CreateSideEffects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClinicaArmazemDefault",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArmazemGeral = table.Column<bool>(type: "bit", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicaArmazemDefault", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClinicaConfiguracaoIva",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    TaxaIva0Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaxaIva1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaxaIva2Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaxaIva3Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaxaIva4Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaxaIva5Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaxaIva6Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaxaIva7Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaxaIva8Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaxaIva9Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicaConfiguracaoIva", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClinicaMotivoIsencaoDefault",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicaMotivoIsencaoDefault", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClinicaTipoConsultaDefault",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Designacao = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicaTipoConsultaDefault", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClinicaArmazemDefault_ClinicaId",
                schema: "Core",
                table: "ClinicaArmazemDefault",
                column: "ClinicaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClinicaConfiguracaoIva_ClinicaId_Ano",
                schema: "Core",
                table: "ClinicaConfiguracaoIva",
                columns: new[] { "ClinicaId", "Ano" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClinicaMotivoIsencaoDefault_ClinicaId_Codigo",
                schema: "Core",
                table: "ClinicaMotivoIsencaoDefault",
                columns: new[] { "ClinicaId", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClinicaTipoConsultaDefault_ClinicaId_Designacao",
                schema: "Core",
                table: "ClinicaTipoConsultaDefault",
                columns: new[] { "ClinicaId", "Designacao" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClinicaArmazemDefault",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "ClinicaConfiguracaoIva",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "ClinicaMotivoIsencaoDefault",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "ClinicaTipoConsultaDefault",
                schema: "Core");
        }
    }
}
