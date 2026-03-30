using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddViasAdministracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Artigos");

            migrationBuilder.CreateTable(
                name: "GrupoViasAdministracao",
                schema: "Artigos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupoViasAdministracao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ViaAdministracao",
                schema: "Artigos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViaAdministracao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GrupoViasAdministracaoLinha",
                schema: "Artigos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GrupoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ViaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Linha = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantidade = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupoViasAdministracaoLinha", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrupoViasAdministracaoLinha_GrupoViasAdministracao_GrupoId",
                        column: x => x.GrupoId,
                        principalSchema: "Artigos",
                        principalTable: "GrupoViasAdministracao",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GrupoViasAdministracaoLinha_ViaAdministracao_ViaId",
                        column: x => x.ViaId,
                        principalSchema: "Artigos",
                        principalTable: "ViaAdministracao",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrupoViasAdministracaoLinha_GrupoId",
                schema: "Artigos",
                table: "GrupoViasAdministracaoLinha",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_GrupoViasAdministracaoLinha_ViaId",
                schema: "Artigos",
                table: "GrupoViasAdministracaoLinha",
                column: "ViaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrupoViasAdministracaoLinha",
                schema: "Artigos");

            migrationBuilder.DropTable(
                name: "GrupoViasAdministracao",
                schema: "Artigos");

            migrationBuilder.DropTable(
                name: "ViaAdministracao",
                schema: "Artigos");
        }
    }
}
