using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class S08_Stocks_SubsistemaArtigo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubsistemaArtigo",
                schema: "Stocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtigoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganismoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoCartaoInstituicao = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ValorServico = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MargemOrganismoPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorOrganismo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorUtente = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Inativo = table.Column<bool>(type: "bit", nullable: false),
                    CodigoComplementarAdse = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubsistemaArtigo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubsistemaArtigo_Artigo_ArtigoId",
                        column: x => x.ArtigoId,
                        principalSchema: "Stocks",
                        principalTable: "Artigo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubsistemaArtigo_Organismo_OrganismoId",
                        column: x => x.OrganismoId,
                        principalSchema: "Organismos",
                        principalTable: "Organismo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubsistemaArtigo_ArtigoId",
                schema: "Stocks",
                table: "SubsistemaArtigo",
                column: "ArtigoId");

            migrationBuilder.CreateIndex(
                name: "IX_SubsistemaArtigo_ClinicaId",
                schema: "Stocks",
                table: "SubsistemaArtigo",
                column: "ClinicaId");

            migrationBuilder.CreateIndex(
                name: "IX_SubsistemaArtigo_ClinicaId_ArtigoId_OrganismoId",
                schema: "Stocks",
                table: "SubsistemaArtigo",
                columns: new[] { "ClinicaId", "ArtigoId", "OrganismoId" },
                unique: true,
                filter: "[DeletedOn] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SubsistemaArtigo_CodigoCartaoInstituicao",
                schema: "Stocks",
                table: "SubsistemaArtigo",
                column: "CodigoCartaoInstituicao");

            migrationBuilder.CreateIndex(
                name: "IX_SubsistemaArtigo_OrganismoId",
                schema: "Stocks",
                table: "SubsistemaArtigo",
                column: "OrganismoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubsistemaArtigo",
                schema: "Stocks");
        }
    }
}
