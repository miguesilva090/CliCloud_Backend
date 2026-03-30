using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAcordosTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Acordos",
                schema: "Exames",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoExameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganismoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoSubsistema = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ValTipoExame = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorOrganismo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MargemOrganismo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorUtente = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Inactivo = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Acordos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Acordos_Organismo_OrganismoId",
                        column: x => x.OrganismoId,
                        principalSchema: "Organismos",
                        principalTable: "Organismo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Acordos_TipoExame_TipoExameId",
                        column: x => x.TipoExameId,
                        principalSchema: "Exames",
                        principalTable: "TipoExame",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Acordos_OrganismoId",
                schema: "Exames",
                table: "Acordos",
                column: "OrganismoId");

            migrationBuilder.CreateIndex(
                name: "IX_Acordos_TipoExameId_OrganismoId",
                schema: "Exames",
                table: "Acordos",
                columns: new[] { "TipoExameId", "OrganismoId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Acordos",
                schema: "Exames");
        }
    }
}
