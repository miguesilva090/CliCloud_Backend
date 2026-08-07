using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Rename_SessaoTratamento_MotivoDesmarcacaoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessaoTratamento_MotivosDesmarcacao_MotivosDesmarcacaoId",
                schema: "Tratamentos",
                table: "SessaoTratamento");

            migrationBuilder.RenameColumn(
                name: "MotivosDesmarcacaoId",
                schema: "Tratamentos",
                table: "SessaoTratamento",
                newName: "MotivoDesmarcacaoId");

            migrationBuilder.RenameIndex(
                name: "IX_SessaoTratamento_MotivosDesmarcacaoId",
                schema: "Tratamentos",
                table: "SessaoTratamento",
                newName: "IX_SessaoTratamento_MotivoDesmarcacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_SessaoTratamento_MotivosDesmarcacao_MotivoDesmarcacaoId",
                schema: "Tratamentos",
                table: "SessaoTratamento",
                column: "MotivoDesmarcacaoId",
                principalSchema: "Tratamentos",
                principalTable: "MotivosDesmarcacao",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessaoTratamento_MotivosDesmarcacao_MotivoDesmarcacaoId",
                schema: "Tratamentos",
                table: "SessaoTratamento");

            migrationBuilder.RenameColumn(
                name: "MotivoDesmarcacaoId",
                schema: "Tratamentos",
                table: "SessaoTratamento",
                newName: "MotivosDesmarcacaoId");

            migrationBuilder.RenameIndex(
                name: "IX_SessaoTratamento_MotivoDesmarcacaoId",
                schema: "Tratamentos",
                table: "SessaoTratamento",
                newName: "IX_SessaoTratamento_MotivosDesmarcacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_SessaoTratamento_MotivosDesmarcacao_MotivosDesmarcacaoId",
                schema: "Tratamentos",
                table: "SessaoTratamento",
                column: "MotivosDesmarcacaoId",
                principalSchema: "Tratamentos",
                principalTable: "MotivosDesmarcacao",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
