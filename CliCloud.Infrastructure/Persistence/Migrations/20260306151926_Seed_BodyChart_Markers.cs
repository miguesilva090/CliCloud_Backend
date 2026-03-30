using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Seed_BodyChart_Markers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "ProcessoClinico",
                table: "MapaBodyChart",
                columns: new[] { "Id", "CaminhoImagem", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "LastModifiedBy", "LastModifiedOn", "Nome" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "/UserFiles/MapasBodyChart/sistema-osseo.jpg", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, "Mapa Ósseo" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "/UserFiles/MapasBodyChart/body-chart2.jpg", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, "Body Chart" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "/UserFiles/MapasBodyChart/sistema-muscular.jpg", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, "Mapa Muscular" }
                });

            migrationBuilder.InsertData(
                schema: "ProcessoClinico",
                table: "MarcadorBodyChart",
                columns: new[] { "Id", "CorHex", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "LastModifiedBy", "LastModifiedOn", "MapaBodyChartId", "Titulo" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "#f59e0b", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, new Guid("22222222-2222-2222-2222-222222222222"), "Bloqueio/disfunção" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaab"), "#ef4444", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, new Guid("22222222-2222-2222-2222-222222222222"), "Hipertonicidade" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaac"), "#3b82f6", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, new Guid("22222222-2222-2222-2222-222222222222"), "Hipotonicidade" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaad"), "#a855f7", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, new Guid("22222222-2222-2222-2222-222222222222"), "Irradiação da Dor" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbba"), "#f97316", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, new Guid("11111111-1111-1111-1111-111111111111"), "Contusão" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "#ef4444", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, new Guid("11111111-1111-1111-1111-111111111111"), "Fratura" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"), "#fde047", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, new Guid("11111111-1111-1111-1111-111111111111"), "Lesão Lítica" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"), "#22c55e", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, new Guid("11111111-1111-1111-1111-111111111111"), "Lesão benigna" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbe"), "#0ea5e9", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, new Guid("11111111-1111-1111-1111-111111111111"), "Doença Metabólica" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "#22c55e", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, new Guid("33333333-3333-3333-3333-333333333333"), "Estiramento" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccd"), "#f97316", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, new Guid("33333333-3333-3333-3333-333333333333"), "Contusão" },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccce"), "#e11d48", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, new Guid("33333333-3333-3333-3333-333333333333"), "Contratura" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccf"), "#7c3aed", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, new Guid("33333333-3333-3333-3333-333333333333"), "Ruptura" },
                    { new Guid("cccccccc-cccc-cccc-cccc-ccccccccccd0"), "#14b8a6", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, new Guid("33333333-3333-3333-3333-333333333333"), "Dor Muscular Tardia" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "ProcessoClinico",
                table: "MarcadorBodyChart",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                schema: "ProcessoClinico",
                table: "MarcadorBodyChart",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaab"));

            migrationBuilder.DeleteData(
                schema: "ProcessoClinico",
                table: "MarcadorBodyChart",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaac"));

            migrationBuilder.DeleteData(
                schema: "ProcessoClinico",
                table: "MarcadorBodyChart",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaad"));

            migrationBuilder.DeleteData(
                schema: "ProcessoClinico",
                table: "MarcadorBodyChart",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbba"));

            migrationBuilder.DeleteData(
                schema: "ProcessoClinico",
                table: "MarcadorBodyChart",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                schema: "ProcessoClinico",
                table: "MarcadorBodyChart",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"));

            migrationBuilder.DeleteData(
                schema: "ProcessoClinico",
                table: "MarcadorBodyChart",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"));

            migrationBuilder.DeleteData(
                schema: "ProcessoClinico",
                table: "MarcadorBodyChart",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbe"));

            migrationBuilder.DeleteData(
                schema: "ProcessoClinico",
                table: "MarcadorBodyChart",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                schema: "ProcessoClinico",
                table: "MarcadorBodyChart",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccd"));

            migrationBuilder.DeleteData(
                schema: "ProcessoClinico",
                table: "MarcadorBodyChart",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccce"));

            migrationBuilder.DeleteData(
                schema: "ProcessoClinico",
                table: "MarcadorBodyChart",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccf"));

            migrationBuilder.DeleteData(
                schema: "ProcessoClinico",
                table: "MarcadorBodyChart",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-ccccccccccd0"));

            migrationBuilder.DeleteData(
                schema: "ProcessoClinico",
                table: "MapaBodyChart",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                schema: "ProcessoClinico",
                table: "MapaBodyChart",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                schema: "ProcessoClinico",
                table: "MapaBodyChart",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));
        }
    }
}
