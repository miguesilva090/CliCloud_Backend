using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_ListaEsperaTratamentoServico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ListaEsperaTratamentoServico",
                schema: "Tratamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListaEsperaTratamentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubsistemaServicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CodigoServico = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Designacao = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    SubsistemaDesignacao = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Duracao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IDuraca = table.Column<int>(type: "int", nullable: true),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListaEsperaTratamentoServico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListaEsperaTratamentoServico_ListaEsperaTratamento_ListaEsperaTratamentoId",
                        column: x => x.ListaEsperaTratamentoId,
                        principalSchema: "Tratamentos",
                        principalTable: "ListaEsperaTratamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListaEsperaTratamentoServico_Servico_ServicoId",
                        column: x => x.ServicoId,
                        principalSchema: "Servicos",
                        principalTable: "Servico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListaEsperaTratamentoServico_ListaEsperaTratamentoId_Ordem",
                schema: "Tratamentos",
                table: "ListaEsperaTratamentoServico",
                columns: new[] { "ListaEsperaTratamentoId", "Ordem" });

            migrationBuilder.CreateIndex(
                name: "IX_ListaEsperaTratamentoServico_ServicoId",
                schema: "Tratamentos",
                table: "ListaEsperaTratamentoServico",
                column: "ServicoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListaEsperaTratamentoServico",
                schema: "Tratamentos");
        }
    }
}
