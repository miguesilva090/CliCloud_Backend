using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class F16_Pagamentos : Migration
    {
        private const string SeedUserId = "00000000-0000-0000-0000-000000000001";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Pagamentos");

            migrationBuilder.CreateTable(
                name: "CondicaoPagamento",
                schema: "Pagamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NDiasPagamento = table.Column<int>(type: "int", nullable: true),
                    Desconto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CondicaoPagamento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoPagamento",
                schema: "Pagamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoPagamento", x => x.Id);
                    table.UniqueConstraint("AK_TipoPagamento_Codigo", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "ModoPagamento",
                schema: "Pagamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Abreviatura = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    TemNumAssociado = table.Column<bool>(type: "bit", nullable: false),
                    TemContaBancaria = table.Column<bool>(type: "bit", nullable: false),
                    ContaBancariaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Historico = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModoPagamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModoPagamento_ContaBancaria_ContaBancariaId",
                        column: x => x.ContaBancariaId,
                        principalSchema: "Bancos",
                        principalTable: "ContaBancaria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModoPagamento_TipoPagamento_Abreviatura",
                        column: x => x.Abreviatura,
                        principalSchema: "Pagamentos",
                        principalTable: "TipoPagamento",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CondicaoPagamento_ClinicaId",
                schema: "Pagamentos",
                table: "CondicaoPagamento",
                column: "ClinicaId");

            migrationBuilder.CreateIndex(
                name: "IX_CondicaoPagamento_ClinicaId_Codigo",
                schema: "Pagamentos",
                table: "CondicaoPagamento",
                columns: new[] { "ClinicaId", "Codigo" },
                unique: true,
                filter: "[DeletedOn] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ModoPagamento_Abreviatura",
                schema: "Pagamentos",
                table: "ModoPagamento",
                column: "Abreviatura");

            migrationBuilder.CreateIndex(
                name: "IX_ModoPagamento_ClinicaId",
                schema: "Pagamentos",
                table: "ModoPagamento",
                column: "ClinicaId");

            migrationBuilder.CreateIndex(
                name: "IX_ModoPagamento_ClinicaId_Codigo",
                schema: "Pagamentos",
                table: "ModoPagamento",
                columns: new[] { "ClinicaId", "Codigo" },
                unique: true,
                filter: "[DeletedOn] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ModoPagamento_ContaBancariaId",
                schema: "Pagamentos",
                table: "ModoPagamento",
                column: "ContaBancariaId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoPagamento_Codigo",
                schema: "Pagamentos",
                table: "TipoPagamento",
                column: "Codigo",
                unique: true,
                filter: "[DeletedOn] IS NULL");

            migrationBuilder.Sql($"""
                DECLARE @CreatedBy uniqueidentifier = '{SeedUserId}';
                DECLARE @Now datetime2 = SYSUTCDATETIME();

                INSERT INTO [Pagamentos].[TipoPagamento] (Id, Codigo, Descricao, CreatedBy, CreatedOn)
                SELECT NEWID(), v.Codigo, v.Descricao, @CreatedBy, @Now
                FROM (VALUES
                    (N'AC', N'Acerto Contas'),
                    (N'CC', N'Cartão Crédito'),
                    (N'CD', N'Cartão Débito'),
                    (N'CH', N'Cheque'),
                    (N'CS', N'Compensação Saldos'),
                    (N'LC', N'Letra Comercial'),
                    (N'MB', N'Multibanco'),
                    (N'NU', N'Numerário'),
                    (N'PR', N'Permuta'),
                    (N'TB', N'Transferência Bancária'),
                    (N'TR', N'Ticket Restaurante')
                ) v(Codigo, Descricao)
                WHERE NOT EXISTS (
                    SELECT 1 FROM [Pagamentos].[TipoPagamento] t WHERE t.Codigo = v.Codigo
                );

                INSERT INTO [Pagamentos].[ModoPagamento]
                    (Id, ClinicaId, Codigo, Descricao, Abreviatura, TemNumAssociado, TemContaBancaria, Historico, CreatedBy, CreatedOn)
                SELECT NEWID(), c.Id, v.Codigo, v.Descricao, v.Abreviatura, v.TemNumAssociado, v.TemContaBancaria, 0, @CreatedBy, @Now
                FROM [Core].[Clinica] c
                CROSS JOIN (VALUES
                    (1,  N'Cartão crédito',                          N'CC', 0, 0),
                    (2,  N'Cartão débito',                           N'CD', 0, 0),
                    (3,  N'Cheque',                                  N'CH', 1, 1),
                    (4,  N'Compensação de saldos em conta corrente', N'CS', 0, 0),
                    (5,  N'Letra comercial',                         N'LC', 0, 0),
                    (6,  N'Multibanco',                              N'MB', 0, 1),
                    (7,  N'Numerário',                               N'NU', 0, 0),
                    (8,  N'Permuta',                                 N'PR', 0, 0),
                    (9,  N'Transferência bancária',                  N'TB', 0, 1),
                    (10, N'Ticket restaurante',                      N'TR', 0, 0)
                ) v(Codigo, Descricao, Abreviatura, TemNumAssociado, TemContaBancaria)
                WHERE EXISTS (SELECT 1 FROM [Pagamentos].[TipoPagamento] tp WHERE tp.Codigo = v.Abreviatura)
                  AND NOT EXISTS (
                      SELECT 1 FROM [Pagamentos].[ModoPagamento] m
                      WHERE m.ClinicaId = c.Id AND m.Codigo = v.Codigo
                  );
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CondicaoPagamento",
                schema: "Pagamentos");

            migrationBuilder.DropTable(
                name: "ModoPagamento",
                schema: "Pagamentos");

            migrationBuilder.DropTable(
                name: "TipoPagamento",
                schema: "Pagamentos");
        }
    }
}
