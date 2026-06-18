using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class S02_Stocks_FamiliaArtigo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FamiliaArtigo",
                schema: "Stocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Nivel = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UrlFoto = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamiliaArtigo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FamiliaArtigo_FamiliaArtigo_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Stocks",
                        principalTable: "FamiliaArtigo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FamiliaArtigo_ClinicaId",
                schema: "Stocks",
                table: "FamiliaArtigo",
                column: "ClinicaId");

            migrationBuilder.CreateIndex(
                name: "IX_FamiliaArtigo_ClinicaId_Codigo",
                schema: "Stocks",
                table: "FamiliaArtigo",
                columns: new[] { "ClinicaId", "Codigo" },
                unique: true,
                filter: "[DeletedOn] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FamiliaArtigo_ClinicaId_ParentId",
                schema: "Stocks",
                table: "FamiliaArtigo",
                columns: new[] { "ClinicaId", "ParentId" });

            migrationBuilder.CreateIndex(
                name: "IX_FamiliaArtigo_ParentId",
                schema: "Stocks",
                table: "FamiliaArtigo",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FamiliaArtigo",
                schema: "Stocks");
        }
    }
}
