using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class F12_ContaBancaria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContaBancaria",
                schema: "Bancos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Numero = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false),
                    TipoConta = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    BancoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataAbertura = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NIB = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    SaldoActual = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GestorConta = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    AlertaSaldo = table.Column<int>(type: "int", nullable: true),
                    ValorAlertaSaldo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OBS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IBAN = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: true),
                    BIC = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    Ficheiro = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContaBancaria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContaBancaria_Banco_BancoId",
                        column: x => x.BancoId,
                        principalSchema: "Bancos",
                        principalTable: "Banco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContaBancaria_BancoId",
                schema: "Bancos",
                table: "ContaBancaria",
                column: "BancoId");

            migrationBuilder.CreateIndex(
                name: "IX_ContaBancaria_Numero",
                schema: "Bancos",
                table: "ContaBancaria",
                column: "Numero");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContaBancaria",
                schema: "Bancos");
        }
    }
}
