using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigReferenciaMbAndReferenciaMb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Faturacao");

            migrationBuilder.CreateTable(
                name: "ConfigReferenciaMB",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValorMinimo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrazoPagamento = table.Column<int>(type: "int", nullable: false),
                    ServicoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoEntidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubEntidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChaveBackOffice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IfThenKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigReferenciaMB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReferenciaMB",
                schema: "Faturacao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClienteNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mensagem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntidadeMb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenciaCodigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataReferenciaGerada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataLimitePagamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataPagamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Liquidada = table.Column<bool>(type: "bit", nullable: false),
                    Anulada = table.Column<bool>(type: "bit", nullable: false),
                    RequestId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoEmpresaServico = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferenciaMB", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigReferenciaMB",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "ReferenciaMB",
                schema: "Faturacao");
        }
    }
}
