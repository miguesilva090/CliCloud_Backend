using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCodigoColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TaxaIva_Codigo",
                schema: "Utility",
                table: "TaxaIva");

            migrationBuilder.DropIndex(
                name: "IX_Sexo_Codigo",
                schema: "Utility",
                table: "Sexo");

            migrationBuilder.DropIndex(
                name: "IX_ProvenienciaUtente_Codigo",
                schema: "Utility",
                table: "ProvenienciaUtente");

            migrationBuilder.DropIndex(
                name: "IX_Profissao_Codigo",
                schema: "Utility",
                table: "Profissao");

            migrationBuilder.DropIndex(
                name: "IX_Moeda_Codigo",
                schema: "Utility",
                table: "Moeda");

            migrationBuilder.DropIndex(
                name: "IX_Habilitacao_Codigo",
                schema: "Utility",
                table: "Habilitacao");

            migrationBuilder.DropIndex(
                name: "IX_GrupoSanguineo_Codigo",
                schema: "Utility",
                table: "GrupoSanguineo");

            migrationBuilder.DropIndex(
                name: "IX_GrauParentesco_Codigo",
                schema: "Utility",
                table: "GrauParentesco");

            migrationBuilder.DropIndex(
                name: "IX_EstadoCivil_Codigo",
                schema: "Utility",
                table: "EstadoCivil");

            migrationBuilder.DropIndex(
                name: "IX_EntidadeFinanceira_Codigo",
                schema: "EntidadesFinanceiras",
                table: "EntidadeFinanceira");

            migrationBuilder.DropIndex(
                name: "IX_EntidadeFinanceira_Codigo_PaisPrefixo",
                schema: "EntidadesFinanceiras",
                table: "EntidadeFinanceira");

            migrationBuilder.DropIndex(
                name: "IX_CategoriaEspecialidade_Codigo",
                schema: "Especialidades",
                table: "CategoriaEspecialidade");

            migrationBuilder.DropColumn(
                name: "Codigo",
                schema: "Utility",
                table: "TaxaIva");

            migrationBuilder.DropColumn(
                name: "Codigo",
                schema: "Utility",
                table: "Sexo");

            migrationBuilder.DropColumn(
                name: "Codigo",
                schema: "Utility",
                table: "ProvenienciaUtente");

            migrationBuilder.DropColumn(
                name: "Codigo",
                schema: "Utility",
                table: "Profissao");

            migrationBuilder.DropColumn(
                name: "Codigo",
                schema: "Utility",
                table: "Moeda");

            migrationBuilder.DropColumn(
                name: "Codigo",
                schema: "Utility",
                table: "Habilitacao");

            migrationBuilder.DropColumn(
                name: "Codigo",
                schema: "Utility",
                table: "GrupoSanguineo");

            migrationBuilder.DropColumn(
                name: "Codigo",
                schema: "Utility",
                table: "GrauParentesco");

            migrationBuilder.DropColumn(
                name: "Codigo",
                schema: "Utility",
                table: "EstadoCivil");

            migrationBuilder.DropColumn(
                name: "Codigo",
                schema: "EntidadesFinanceiras",
                table: "EntidadeFinanceira");

            migrationBuilder.DropColumn(
                name: "Codigo",
                schema: "Especialidades",
                table: "CategoriaEspecialidade");

            migrationBuilder.RenameColumn(
                name: "Codigo",
                schema: "TipoEntidadeFinanceira",
                table: "TipoEntidadeFinanceira",
                newName: "Sigla");

            migrationBuilder.RenameIndex(
                name: "IX_TipoEntidadeFinanceira_Codigo",
                schema: "TipoEntidadeFinanceira",
                table: "TipoEntidadeFinanceira",
                newName: "IX_TipoEntidadeFinanceira_Sigla");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Sigla",
                schema: "TipoEntidadeFinanceira",
                table: "TipoEntidadeFinanceira",
                newName: "Codigo");

            migrationBuilder.RenameIndex(
                name: "IX_TipoEntidadeFinanceira_Sigla",
                schema: "TipoEntidadeFinanceira",
                table: "TipoEntidadeFinanceira",
                newName: "IX_TipoEntidadeFinanceira_Codigo");

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                schema: "Utility",
                table: "TaxaIva",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                schema: "Utility",
                table: "Sexo",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                schema: "Utility",
                table: "ProvenienciaUtente",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                schema: "Utility",
                table: "Profissao",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Codigo",
                schema: "Utility",
                table: "Moeda",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                schema: "Utility",
                table: "Habilitacao",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                schema: "Utility",
                table: "GrupoSanguineo",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                schema: "Utility",
                table: "GrauParentesco",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                schema: "Utility",
                table: "EstadoCivil",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                schema: "EntidadesFinanceiras",
                table: "EntidadeFinanceira",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                schema: "Especialidades",
                table: "CategoriaEspecialidade",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TaxaIva_Codigo",
                schema: "Utility",
                table: "TaxaIva",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sexo_Codigo",
                schema: "Utility",
                table: "Sexo",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProvenienciaUtente_Codigo",
                schema: "Utility",
                table: "ProvenienciaUtente",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profissao_Codigo",
                schema: "Utility",
                table: "Profissao",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Moeda_Codigo",
                schema: "Utility",
                table: "Moeda",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Habilitacao_Codigo",
                schema: "Utility",
                table: "Habilitacao",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GrupoSanguineo_Codigo",
                schema: "Utility",
                table: "GrupoSanguineo",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GrauParentesco_Codigo",
                schema: "Utility",
                table: "GrauParentesco",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstadoCivil_Codigo",
                schema: "Utility",
                table: "EstadoCivil",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EntidadeFinanceira_Codigo",
                schema: "EntidadesFinanceiras",
                table: "EntidadeFinanceira",
                column: "Codigo");

            migrationBuilder.CreateIndex(
                name: "IX_EntidadeFinanceira_Codigo_PaisPrefixo",
                schema: "EntidadesFinanceiras",
                table: "EntidadeFinanceira",
                columns: new[] { "Codigo", "PaisPrefixo" },
                unique: true,
                filter: "[Codigo] IS NOT NULL AND [PaisPrefixo] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaEspecialidade_Codigo",
                schema: "Especialidades",
                table: "CategoriaEspecialidade",
                column: "Codigo",
                unique: true);
        }
    }
}
