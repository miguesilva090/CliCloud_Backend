using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEntidadeFinanceira : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "EntidadesFinanceiras");

            migrationBuilder.CreateTable(
                name: "EntidadeFinanceira",
                schema: "EntidadesFinanceiras",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Abreviatura = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaisPrefixo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TipoEntidadeFinanceiraId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CondicaoSns = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntidadeFinanceira", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntidadeFinanceira_Entidade_Id",
                        column: x => x.Id,
                        principalSchema: "Utility",
                        principalTable: "Entidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EntidadeFinanceira_TipoEntidadeFinanceira_TipoEntidadeFinanceiraId",
                        column: x => x.TipoEntidadeFinanceiraId,
                        principalSchema: "TipoEntidadeFinanceira",
                        principalTable: "TipoEntidadeFinanceira",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntidadeFinanceira_Codigo",
                schema: "EntidadesFinanceiras",
                table: "EntidadeFinanceira",
                column: "Codigo");

            migrationBuilder.CreateIndex(
                name: "IX_EntidadeFinanceira_Codigo_PaisPrefixo",
                schema: "EntidadesFinanceiras",
                table: "EntidadeFinanceira",
                columns: new[] { "Codigo", "PaisPrefixo" },
                unique: true,
                filter: "[Codigo] IS NOT NULL AND [PaisPrefixo] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EntidadeFinanceira_PaisPrefixo",
                schema: "EntidadesFinanceiras",
                table: "EntidadeFinanceira",
                column: "PaisPrefixo");

            migrationBuilder.CreateIndex(
                name: "IX_EntidadeFinanceira_TipoEntidadeFinanceiraId",
                schema: "EntidadesFinanceiras",
                table: "EntidadeFinanceira",
                column: "TipoEntidadeFinanceiraId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntidadeFinanceira",
                schema: "EntidadesFinanceiras");
        }
    }
}
