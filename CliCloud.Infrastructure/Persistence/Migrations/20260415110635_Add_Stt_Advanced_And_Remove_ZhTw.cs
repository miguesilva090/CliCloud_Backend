using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Stt_Advanced_And_Remove_ZhTw : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Core",
                table: "ConfiguracaoVozOpcao",
                keyColumn: "Id",
                keyValue: new Guid("f1a10000-0000-0000-0000-000000000006"));

            migrationBuilder.AddColumn<string>(
                name: "SttIdioma",
                schema: "Core",
                table: "ConfiguracaoVoz",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "pt-PT");

            migrationBuilder.AddColumn<int>(
                name: "SttMaxAlternatives",
                schema: "Core",
                table: "ConfiguracaoVoz",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<bool>(
                name: "SttProfanityFilter",
                schema: "Core",
                table: "ConfiguracaoVoz",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SttSilenceTimeoutMs",
                schema: "Core",
                table: "ConfiguracaoVoz",
                type: "int",
                nullable: false,
                defaultValue: 2500);

            migrationBuilder.Sql(@"
UPDATE Core.ConfiguracaoVoz
SET SttIdioma = CASE
  WHEN IdiomaPadrao = 'zh-TW' THEN 'zh-CN'
  ELSE IdiomaPadrao
END
WHERE SttIdioma IS NULL OR LTRIM(RTRIM(SttIdioma)) = '' OR SttIdioma = 'pt-PT';

UPDATE Core.ConfiguracaoVoz
SET IdiomaPadrao = 'zh-CN'
WHERE IdiomaPadrao = 'zh-TW';
");

            migrationBuilder.UpdateData(
                schema: "Core",
                table: "ConfiguracaoVozOpcao",
                keyColumn: "Id",
                keyValue: new Guid("f1a10000-0000-0000-0000-000000000005"),
                column: "Descricao",
                value: "Mandarim");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SttIdioma",
                schema: "Core",
                table: "ConfiguracaoVoz");

            migrationBuilder.DropColumn(
                name: "SttMaxAlternatives",
                schema: "Core",
                table: "ConfiguracaoVoz");

            migrationBuilder.DropColumn(
                name: "SttProfanityFilter",
                schema: "Core",
                table: "ConfiguracaoVoz");

            migrationBuilder.DropColumn(
                name: "SttSilenceTimeoutMs",
                schema: "Core",
                table: "ConfiguracaoVoz");

            migrationBuilder.UpdateData(
                schema: "Core",
                table: "ConfiguracaoVozOpcao",
                keyColumn: "Id",
                keyValue: new Guid("f1a10000-0000-0000-0000-000000000005"),
                column: "Descricao",
                value: "Mandarim (China)");

            migrationBuilder.InsertData(
                schema: "Core",
                table: "ConfiguracaoVozOpcao",
                columns: new[] { "Id", "Ativo", "Codigo", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "Descricao", "LastModifiedBy", "LastModifiedOn", "Ordem", "Tipo" },
                values: new object[] { new Guid("f1a10000-0000-0000-0000-000000000006"), true, "zh-TW", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Mandarim (Taiwan)", null, null, 6, "Language" });
        }
    }
}
