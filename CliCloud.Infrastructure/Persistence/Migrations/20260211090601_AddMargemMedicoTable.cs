using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMargemMedicoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MargemMedico",
                schema: "Medicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValorMargem = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PercentagemMargem = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MargemMedico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MargemMedico_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalSchema: "Empresas",
                        principalTable: "Empresa",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MargemMedico_Medico_MedicoId",
                        column: x => x.MedicoId,
                        principalSchema: "Medicos",
                        principalTable: "Medico",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MargemMedico_Servico_ServicoId",
                        column: x => x.ServicoId,
                        principalSchema: "Servicos",
                        principalTable: "Servico",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MargemMedico_EmpresaId_ServicoId_MedicoId",
                schema: "Medicos",
                table: "MargemMedico",
                columns: new[] { "EmpresaId", "ServicoId", "MedicoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MargemMedico_EmpresaId",
                schema: "Medicos",
                table: "MargemMedico",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_MargemMedico_MedicoId",
                schema: "Medicos",
                table: "MargemMedico",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_MargemMedico_ServicoId",
                schema: "Medicos",
                table: "MargemMedico",
                column: "ServicoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // IF EXISTS: a migração original estava vazia, a tabela pode nunca ter existido
            migrationBuilder.Sql("IF OBJECT_ID(N'[Medicos].[MargemMedico]', N'U') IS NOT NULL DROP TABLE [Medicos].[MargemMedico];");
        }
    }
}
