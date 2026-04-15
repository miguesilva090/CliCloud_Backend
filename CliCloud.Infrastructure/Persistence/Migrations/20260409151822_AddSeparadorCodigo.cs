using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSeparadorCodigo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                schema: "ProcessoClinico",
                table: "Separador",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(
                """
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'antecedentes'             WHERE [Ordem] = 1;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'sinaisvitais'             WHERE [Ordem] = 2;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'historiaclinica'           WHERE [Ordem] = 3;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'tratamentos'              WHERE [Ordem] = 4;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'exames'                   WHERE [Ordem] = 5;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'dentaria'                 WHERE [Ordem] = 6;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'laboratorio'              WHERE [Ordem] = 7;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'blocooperatorio'          WHERE [Ordem] = 8;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'internamento'             WHERE [Ordem] = 9;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'relatorioatestado'        WHERE [Ordem] = 10;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'medicacao'                WHERE [Ordem] = 11;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'documentos'               WHERE [Ordem] = 12;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'tensaoarterial'           WHERE [Ordem] = 13;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'glicemiacapilar'          WHERE [Ordem] = 14;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'temperaturacorporal'      WHERE [Ordem] = 15;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'imc'                      WHERE [Ordem] = 16;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'processoclinico'          WHERE [Ordem] = 19;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'antecedentespessoais'     WHERE [Ordem] = 20;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'antecedentesfamiliares'   WHERE [Ordem] = 21;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'alergias'                 WHERE [Ordem] = 22;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'questionario'             WHERE [Ordem] = 23;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'habitosevicios'           WHERE [Ordem] = 24;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'fichatratamento'          WHERE [Ordem] = 25;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'evolucaotratamento'       WHERE [Ordem] = 26;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'gorduramassamuscular'     WHERE [Ordem] = 28;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'avaliacaoantropometrica'  WHERE [Ordem] = 29;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'avaliacaopostural'        WHERE [Ordem] = 30;
                UPDATE [ProcessoClinico].[Separador] SET [Codigo] = N'escaladedor'              WHERE [Ordem] = 31;
                """
            );

            migrationBuilder.CreateIndex(
                name: "IX_Separador_Codigo",
                schema: "ProcessoClinico",
                table: "Separador",
                column: "Codigo",
                unique: true,
                filter: "[Codigo] <> ''");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Separador_Codigo",
                schema: "ProcessoClinico",
                table: "Separador");

            migrationBuilder.DropColumn(
                name: "Codigo",
                schema: "ProcessoClinico",
                table: "Separador");
        }
    }
}
