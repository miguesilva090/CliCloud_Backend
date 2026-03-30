using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEvolucaoTratamentoFicheiros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EvolucaoTratamentoFicheiro",
                schema: "Tratamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvolucaoTratamentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoragePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TamanhoBytes = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvolucaoTratamentoFicheiro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvolucaoTratamentoFicheiro_EvolucaoTratamento_EvolucaoTratamentoId",
                        column: x => x.EvolucaoTratamentoId,
                        principalSchema: "Tratamentos",
                        principalTable: "EvolucaoTratamento",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EvolucaoTratamentoFicheiro_EvolucaoTratamentoId",
                schema: "Tratamentos",
                table: "EvolucaoTratamentoFicheiro",
                column: "EvolucaoTratamentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvolucaoTratamentoFicheiro",
                schema: "Tratamentos");
        }
    }
}
