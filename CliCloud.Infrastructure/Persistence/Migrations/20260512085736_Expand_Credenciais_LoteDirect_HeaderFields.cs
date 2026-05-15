using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Expand_Credenciais_LoteDirect_HeaderFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CentroSaude",
                schema: "Credenciais",
                table: "LoteDirect",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoMedico",
                schema: "Credenciais",
                table: "LoteDirect",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoServicoConsulta",
                schema: "Credenciais",
                table: "LoteDirect",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoSubsistemaConsulta",
                schema: "Credenciais",
                table: "LoteDirect",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CredencialExterna",
                schema: "Credenciais",
                table: "LoteDirect",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Especialidade",
                schema: "Credenciais",
                table: "LoteDirect",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Isencao",
                schema: "Credenciais",
                table: "LoteDirect",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MedicoExternoId",
                schema: "Credenciais",
                table: "LoteDirect",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MedicoId",
                schema: "Credenciais",
                table: "LoteDirect",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ProcedimentosEfetuados",
                schema: "Credenciais",
                table: "LoteDirect",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Proveniencia",
                schema: "Credenciais",
                table: "LoteDirect",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuantidadeConsulta",
                schema: "Credenciais",
                table: "LoteDirect",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServicoConsulta",
                schema: "Credenciais",
                table: "LoteDirect",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Servicos",
                schema: "Credenciais",
                table: "LoteDirect",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Subtotal",
                schema: "Credenciais",
                table: "LoteDirect",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxaConsulta",
                schema: "Credenciais",
                table: "LoteDirect",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorConsulta",
                schema: "Credenciais",
                table: "LoteDirect",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorTaxasLinhas",
                schema: "Credenciais",
                table: "LoteDirect",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorTotalV2",
                schema: "Credenciais",
                table: "LoteDirect",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorTotalV3",
                schema: "Credenciais",
                table: "LoteDirect",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoteDirect_MedicoExternoId",
                schema: "Credenciais",
                table: "LoteDirect",
                column: "MedicoExternoId");

            migrationBuilder.CreateIndex(
                name: "IX_LoteDirect_MedicoId",
                schema: "Credenciais",
                table: "LoteDirect",
                column: "MedicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoteDirect_MedicoExterno_MedicoExternoId",
                schema: "Credenciais",
                table: "LoteDirect",
                column: "MedicoExternoId",
                principalSchema: "Medicos",
                principalTable: "MedicoExterno",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoteDirect_Medico_MedicoId",
                schema: "Credenciais",
                table: "LoteDirect",
                column: "MedicoId",
                principalSchema: "Medicos",
                principalTable: "Medico",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoteDirect_MedicoExterno_MedicoExternoId",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropForeignKey(
                name: "FK_LoteDirect_Medico_MedicoId",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropIndex(
                name: "IX_LoteDirect_MedicoExternoId",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropIndex(
                name: "IX_LoteDirect_MedicoId",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropColumn(
                name: "CentroSaude",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropColumn(
                name: "CodigoMedico",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropColumn(
                name: "CodigoServicoConsulta",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropColumn(
                name: "CodigoSubsistemaConsulta",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropColumn(
                name: "CredencialExterna",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropColumn(
                name: "Especialidade",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropColumn(
                name: "Isencao",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropColumn(
                name: "MedicoExternoId",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropColumn(
                name: "MedicoId",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropColumn(
                name: "ProcedimentosEfetuados",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropColumn(
                name: "Proveniencia",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropColumn(
                name: "QuantidadeConsulta",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropColumn(
                name: "ServicoConsulta",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropColumn(
                name: "Servicos",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropColumn(
                name: "Subtotal",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropColumn(
                name: "TaxaConsulta",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropColumn(
                name: "ValorConsulta",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropColumn(
                name: "ValorTaxasLinhas",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropColumn(
                name: "ValorTotalV2",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropColumn(
                name: "ValorTotalV3",
                schema: "Credenciais",
                table: "LoteDirect");
        }
    }
}
