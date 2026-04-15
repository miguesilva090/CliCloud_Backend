using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Voz_Idiomas_Italiano_Alemao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Core",
                table: "ConfiguracaoVozOpcao",
                columns: new[] { "Id", "Ativo", "Codigo", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "Descricao", "LastModifiedBy", "LastModifiedOn", "Ordem", "Tipo" },
                values: new object[,]
                {
                    { new Guid("f1a10000-0000-0000-0000-000000000007"), true, "fr-FR", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Francês (FR)", null, null, 7, "Language" },
                    { new Guid("f1a10000-0000-0000-0000-000000000008"), true, "it-IT", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Italiano (IT)", null, null, 8, "Language" },
                    { new Guid("f1a10000-0000-0000-0000-000000000009"), true, "de-DE", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Alemão (DE)", null, null, 9, "Language" },
                    { new Guid("f1a10000-0000-0000-0000-000000000113"), true, "it-IT-Female", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Italiano (IT) - Feminina", null, null, 13, "Voice" },
                    { new Guid("f1a10000-0000-0000-0000-000000000114"), true, "it-IT-Male", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Italiano (IT) - Masculina", null, null, 14, "Voice" },
                    { new Guid("f1a10000-0000-0000-0000-000000000115"), true, "de-DE-Female", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Alemão (DE) - Feminina", null, null, 15, "Voice" },
                    { new Guid("f1a10000-0000-0000-0000-000000000116"), true, "de-DE-Male", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Alemão (DE) - Masculina", null, null, 16, "Voice" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Core",
                table: "ConfiguracaoVozOpcao",
                keyColumn: "Id",
                keyValue: new Guid("f1a10000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                schema: "Core",
                table: "ConfiguracaoVozOpcao",
                keyColumn: "Id",
                keyValue: new Guid("f1a10000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                schema: "Core",
                table: "ConfiguracaoVozOpcao",
                keyColumn: "Id",
                keyValue: new Guid("f1a10000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                schema: "Core",
                table: "ConfiguracaoVozOpcao",
                keyColumn: "Id",
                keyValue: new Guid("f1a10000-0000-0000-0000-000000000113"));

            migrationBuilder.DeleteData(
                schema: "Core",
                table: "ConfiguracaoVozOpcao",
                keyColumn: "Id",
                keyValue: new Guid("f1a10000-0000-0000-0000-000000000114"));

            migrationBuilder.DeleteData(
                schema: "Core",
                table: "ConfiguracaoVozOpcao",
                keyColumn: "Id",
                keyValue: new Guid("f1a10000-0000-0000-0000-000000000115"));

            migrationBuilder.DeleteData(
                schema: "Core",
                table: "ConfiguracaoVozOpcao",
                keyColumn: "Id",
                keyValue: new Guid("f1a10000-0000-0000-0000-000000000116"));
        }
    }
}
