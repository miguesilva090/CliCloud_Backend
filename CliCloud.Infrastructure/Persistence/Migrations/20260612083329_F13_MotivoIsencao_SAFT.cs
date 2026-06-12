using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class F13_MotivoIsencao_SAFT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                schema: "Utility",
                table: "MotivoIsencao",
                type: "nvarchar(254)",
                maxLength: 254,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "CodigoSaft",
                schema: "Utility",
                table: "MotivoIsencao",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE [Utility].[MotivoIsencao]
                SET [CodigoSaft] = LEFT([Codigo], 12)
                WHERE [CodigoSaft] IS NULL OR LTRIM(RTRIM([CodigoSaft])) = ''
                """);

            migrationBuilder.AddColumn<string>(
                name: "Mencao",
                schema: "Utility",
                table: "MotivoIsencao",
                type: "nvarchar(254)",
                maxLength: 254,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Norma",
                schema: "Utility",
                table: "MotivoIsencao",
                type: "nvarchar(254)",
                maxLength: 254,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MotivoIsencao_CodigoSaft",
                schema: "Utility",
                table: "MotivoIsencao",
                column: "CodigoSaft");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MotivoIsencao_CodigoSaft",
                schema: "Utility",
                table: "MotivoIsencao");

            migrationBuilder.DropColumn(
                name: "CodigoSaft",
                schema: "Utility",
                table: "MotivoIsencao");

            migrationBuilder.DropColumn(
                name: "Mencao",
                schema: "Utility",
                table: "MotivoIsencao");

            migrationBuilder.DropColumn(
                name: "Norma",
                schema: "Utility",
                table: "MotivoIsencao");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                schema: "Utility",
                table: "MotivoIsencao",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(254)",
                oldMaxLength: 254);
        }
    }
}
