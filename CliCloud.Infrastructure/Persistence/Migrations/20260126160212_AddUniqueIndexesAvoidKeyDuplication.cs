using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexesAvoidKeyDuplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ServicoTratamento_TratamentoId",
                schema: "Tratamentos",
                table: "ServicoTratamento");

            migrationBuilder.DropIndex(
                name: "IX_ServicoSessao_SessaoTratamentoId",
                schema: "Tratamentos",
                table: "ServicoSessao");

            migrationBuilder.DropIndex(
                name: "IX_ServicoConsulta_ConsultaId",
                schema: "Consultas",
                table: "ServicoConsulta");

            migrationBuilder.CreateIndex(
                name: "IX_ServicoTratamento_TratamentoId_Ordem",
                schema: "Tratamentos",
                table: "ServicoTratamento",
                columns: new[] { "TratamentoId", "Ordem" },
                unique: true,
                filter: "[Ordem] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ServicoSessao_SessaoTratamentoId_Ordem",
                schema: "Tratamentos",
                table: "ServicoSessao",
                columns: new[] { "SessaoTratamentoId", "Ordem" },
                unique: true,
                filter: "[Ordem] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ServicoConsulta_ConsultaId_Linha",
                schema: "Consultas",
                table: "ServicoConsulta",
                columns: new[] { "ConsultaId", "Linha" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ServicoTratamento_TratamentoId_Ordem",
                schema: "Tratamentos",
                table: "ServicoTratamento");

            migrationBuilder.DropIndex(
                name: "IX_ServicoSessao_SessaoTratamentoId_Ordem",
                schema: "Tratamentos",
                table: "ServicoSessao");

            migrationBuilder.DropIndex(
                name: "IX_ServicoConsulta_ConsultaId_Linha",
                schema: "Consultas",
                table: "ServicoConsulta");

            migrationBuilder.CreateIndex(
                name: "IX_ServicoTratamento_TratamentoId",
                schema: "Tratamentos",
                table: "ServicoTratamento",
                column: "TratamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicoSessao_SessaoTratamentoId",
                schema: "Tratamentos",
                table: "ServicoSessao",
                column: "SessaoTratamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicoConsulta_ConsultaId",
                schema: "Consultas",
                table: "ServicoConsulta",
                column: "ConsultaId");
        }
    }
}
