using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_FichaClinicaSeparadoresDinamicos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FichaClinicaSecaoConteudo_FichaClinicaSecaoTemplate_TemplateId",
                schema: "ProcessoClinico",
                table: "FichaClinicaSecaoConteudo");

            migrationBuilder.RenameColumn(
                name: "TemplateId",
                schema: "ProcessoClinico",
                table: "FichaClinicaSecaoConteudo",
                newName: "CampoId");

            migrationBuilder.RenameIndex(
                name: "IX_FichaClinicaSecaoConteudo_TemplateId",
                schema: "ProcessoClinico",
                table: "FichaClinicaSecaoConteudo",
                newName: "IX_FichaClinicaSecaoConteudo_CampoId");

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                schema: "ProcessoClinico",
                table: "FichaClinicaSecaoTemplate",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "FichaClinicaSecaoCampo",
                schema: "ProcessoClinico",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeparadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FichaClinicaSecaoCampo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FichaClinicaSecaoCampo_FichaClinicaSecaoTemplate_SeparadorId",
                        column: x => x.SeparadorId,
                        principalSchema: "ProcessoClinico",
                        principalTable: "FichaClinicaSecaoTemplate",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FichaClinicaSecaoCampo_SeparadorId",
                schema: "ProcessoClinico",
                table: "FichaClinicaSecaoCampo",
                column: "SeparadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_FichaClinicaSecaoConteudo_FichaClinicaSecaoCampo_CampoId",
                schema: "ProcessoClinico",
                table: "FichaClinicaSecaoConteudo",
                column: "CampoId",
                principalSchema: "ProcessoClinico",
                principalTable: "FichaClinicaSecaoCampo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FichaClinicaSecaoConteudo_FichaClinicaSecaoCampo_CampoId",
                schema: "ProcessoClinico",
                table: "FichaClinicaSecaoConteudo");

            migrationBuilder.DropTable(
                name: "FichaClinicaSecaoCampo",
                schema: "ProcessoClinico");

            migrationBuilder.DropColumn(
                name: "Ativo",
                schema: "ProcessoClinico",
                table: "FichaClinicaSecaoTemplate");

            migrationBuilder.RenameColumn(
                name: "CampoId",
                schema: "ProcessoClinico",
                table: "FichaClinicaSecaoConteudo",
                newName: "TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_FichaClinicaSecaoConteudo_CampoId",
                schema: "ProcessoClinico",
                table: "FichaClinicaSecaoConteudo",
                newName: "IX_FichaClinicaSecaoConteudo_TemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_FichaClinicaSecaoConteudo_FichaClinicaSecaoTemplate_TemplateId",
                schema: "ProcessoClinico",
                table: "FichaClinicaSecaoConteudo",
                column: "TemplateId",
                principalSchema: "ProcessoClinico",
                principalTable: "FichaClinicaSecaoTemplate",
                principalColumn: "Id");
        }
    }
}
