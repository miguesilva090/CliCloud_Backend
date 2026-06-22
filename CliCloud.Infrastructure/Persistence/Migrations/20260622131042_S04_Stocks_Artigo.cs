using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class S04_Stocks_Artigo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ArtigoId",
                schema: "Documentos",
                table: "DocumentoLinha",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Artigo",
                schema: "Stocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<int>(type: "int", nullable: false),
                    NumeroArtigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EAN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CodigoBarras = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UrlFoto = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    UnidadeMedidaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FamiliaArtigoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaxaIvaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MotivoIsencaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ArmazemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoArtigo = table.Column<int>(type: "int", nullable: false),
                    Inativo = table.Column<bool>(type: "bit", nullable: false),
                    Descontinuado = table.Column<bool>(type: "bit", nullable: false),
                    PrecoUnitarioSemIva1 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PrecoUnitarioSemIva2 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PrecoUnitarioSemIva3 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PrecoVendaComIva1 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PrecoVendaComIva2 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PrecoVendaComIva3 = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PrecoCusto = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    StockMinimo = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    StockMaximo = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    StockReposicao = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    StockReal = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PermitirDescontos = table.Column<bool>(type: "bit", nullable: false),
                    PermitirAlterarPreco = table.Column<bool>(type: "bit", nullable: false),
                    ActHotel = table.Column<bool>(type: "bit", nullable: false),
                    ActPOS = table.Column<bool>(type: "bit", nullable: false),
                    CodigoInternoLegado = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artigo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Artigo_Armazem_ArmazemId",
                        column: x => x.ArmazemId,
                        principalSchema: "Stocks",
                        principalTable: "Armazem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Artigo_FamiliaArtigo_FamiliaArtigoId",
                        column: x => x.FamiliaArtigoId,
                        principalSchema: "Stocks",
                        principalTable: "FamiliaArtigo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Artigo_MotivoIsencao_MotivoIsencaoId",
                        column: x => x.MotivoIsencaoId,
                        principalSchema: "Utility",
                        principalTable: "MotivoIsencao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Artigo_TaxaIva_TaxaIvaId",
                        column: x => x.TaxaIvaId,
                        principalSchema: "Utility",
                        principalTable: "TaxaIva",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Artigo_UnidadeMedida_UnidadeMedidaId",
                        column: x => x.UnidadeMedidaId,
                        principalSchema: "Stocks",
                        principalTable: "UnidadeMedida",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoLinha_ArtigoId",
                schema: "Documentos",
                table: "DocumentoLinha",
                column: "ArtigoId");

            migrationBuilder.CreateIndex(
                name: "IX_Artigo_ArmazemId",
                schema: "Stocks",
                table: "Artigo",
                column: "ArmazemId");

            migrationBuilder.CreateIndex(
                name: "IX_Artigo_ClinicaId",
                schema: "Stocks",
                table: "Artigo",
                column: "ClinicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Artigo_ClinicaId_Codigo",
                schema: "Stocks",
                table: "Artigo",
                columns: new[] { "ClinicaId", "Codigo" },
                unique: true,
                filter: "[DeletedOn] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Artigo_ClinicaId_NumeroArtigo",
                schema: "Stocks",
                table: "Artigo",
                columns: new[] { "ClinicaId", "NumeroArtigo" },
                unique: true,
                filter: "[DeletedOn] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Artigo_FamiliaArtigoId",
                schema: "Stocks",
                table: "Artigo",
                column: "FamiliaArtigoId");

            migrationBuilder.CreateIndex(
                name: "IX_Artigo_MotivoIsencaoId",
                schema: "Stocks",
                table: "Artigo",
                column: "MotivoIsencaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Artigo_TaxaIvaId",
                schema: "Stocks",
                table: "Artigo",
                column: "TaxaIvaId");

            migrationBuilder.CreateIndex(
                name: "IX_Artigo_UnidadeMedidaId",
                schema: "Stocks",
                table: "Artigo",
                column: "UnidadeMedidaId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentoLinha_Artigo_ArtigoId",
                schema: "Documentos",
                table: "DocumentoLinha",
                column: "ArtigoId",
                principalSchema: "Stocks",
                principalTable: "Artigo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentoLinha_Artigo_ArtigoId",
                schema: "Documentos",
                table: "DocumentoLinha");

            migrationBuilder.DropTable(
                name: "Artigo",
                schema: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_DocumentoLinha_ArtigoId",
                schema: "Documentos",
                table: "DocumentoLinha");

            migrationBuilder.DropColumn(
                name: "ArtigoId",
                schema: "Documentos",
                table: "DocumentoLinha");
        }
    }
}
