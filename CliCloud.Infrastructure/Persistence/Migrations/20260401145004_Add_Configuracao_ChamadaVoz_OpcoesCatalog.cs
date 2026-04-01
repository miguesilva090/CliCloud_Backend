using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Configuracao_ChamadaVoz_OpcoesCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfiguracaoChamadaVozOpcao",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracaoChamadaVozOpcao", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "Core",
                table: "ConfiguracaoChamadaVozOpcao",
                columns: new[] { "Id", "Ativo", "Codigo", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "Descricao", "LastModifiedBy", "LastModifiedOn", "Ordem", "Tipo" },
                values: new object[,]
                {
                    { new Guid("f1a10000-0000-0000-0000-000000000001"), true, "pt", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Português", null, null, 1, "Language" },
                    { new Guid("f1a10000-0000-0000-0000-000000000002"), true, "en", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Inglês", null, null, 2, "Language" },
                    { new Guid("f1a10000-0000-0000-0000-000000000003"), true, "fr", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Francês", null, null, 3, "Language" },
                    { new Guid("f1a10000-0000-0000-0000-000000000004"), true, "es", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Espanhol", null, null, 4, "Language" },
                    { new Guid("f1a10000-0000-0000-0000-000000000005"), true, "zh-CN", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Mandarim (China)", null, null, 5, "Language" },
                    { new Guid("f1a10000-0000-0000-0000-000000000006"), true, "zh-TW", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Mandarim (Taiwan)", null, null, 6, "Language" },
                    { new Guid("f1a10000-0000-0000-0000-000000000101"), true, "pt", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Português (PT)", null, null, 1, "Tld" },
                    { new Guid("f1a10000-0000-0000-0000-000000000102"), true, "com.br", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Português (BR)", null, null, 2, "Tld" },
                    { new Guid("f1a10000-0000-0000-0000-000000000103"), true, "com.au", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Inglês (AU)", null, null, 3, "Tld" },
                    { new Guid("f1a10000-0000-0000-0000-000000000104"), true, "co.uk", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Inglês (UK)", null, null, 4, "Tld" },
                    { new Guid("f1a10000-0000-0000-0000-000000000105"), true, "com", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Inglês (US)", null, null, 5, "Tld" },
                    { new Guid("f1a10000-0000-0000-0000-000000000106"), true, "ca", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Inglês/Francês (CA)", null, null, 6, "Tld" },
                    { new Guid("f1a10000-0000-0000-0000-000000000107"), true, "co.in", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Inglês (IN)", null, null, 7, "Tld" },
                    { new Guid("f1a10000-0000-0000-0000-000000000108"), true, "ie", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Inglês (IE)", null, null, 8, "Tld" },
                    { new Guid("f1a10000-0000-0000-0000-000000000109"), true, "co.za", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Inglês (ZA)", null, null, 9, "Tld" },
                    { new Guid("f1a10000-0000-0000-0000-000000000110"), true, "fr", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Francês (FR)", null, null, 10, "Tld" },
                    { new Guid("f1a10000-0000-0000-0000-000000000111"), true, "es", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Espanhol (ES)", null, null, 11, "Tld" },
                    { new Guid("f1a10000-0000-0000-0000-000000000112"), true, "com.mx", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Espanhol (MX)", null, null, 12, "Tld" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracaoChamadaVozOpcao_Tipo_Ativo_Ordem",
                schema: "Core",
                table: "ConfiguracaoChamadaVozOpcao",
                columns: new[] { "Tipo", "Ativo", "Ordem" });

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracaoChamadaVozOpcao_Tipo_Codigo",
                schema: "Core",
                table: "ConfiguracaoChamadaVozOpcao",
                columns: new[] { "Tipo", "Codigo" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracaoChamadaVozOpcao",
                schema: "Core");
        }
    }
}
