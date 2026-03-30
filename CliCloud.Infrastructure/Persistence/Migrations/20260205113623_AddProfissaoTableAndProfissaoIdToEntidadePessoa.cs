using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProfissaoTableAndProfissaoIdToEntidadePessoa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Profissao",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.AddColumn<Guid>(
                name: "ProfissaoId",
                schema: "Utility",
                table: "EntidadePessoa",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Profissao",
                schema: "Utility",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profissao", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntidadePessoa_ProfissaoId",
                schema: "Utility",
                table: "EntidadePessoa",
                column: "ProfissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Profissao_Codigo",
                schema: "Utility",
                table: "Profissao",
                column: "Codigo",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EntidadePessoa_Profissao_ProfissaoId",
                schema: "Utility",
                table: "EntidadePessoa",
                column: "ProfissaoId",
                principalSchema: "Utility",
                principalTable: "Profissao",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntidadePessoa_Profissao_ProfissaoId",
                schema: "Utility",
                table: "EntidadePessoa");

            migrationBuilder.DropTable(
                name: "Profissao",
                schema: "Utility");

            migrationBuilder.DropIndex(
                name: "IX_EntidadePessoa_ProfissaoId",
                schema: "Utility",
                table: "EntidadePessoa");

            migrationBuilder.DropColumn(
                name: "ProfissaoId",
                schema: "Utility",
                table: "EntidadePessoa");

            migrationBuilder.AddColumn<string>(
                name: "Profissao",
                schema: "Utentes",
                table: "Utente",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
