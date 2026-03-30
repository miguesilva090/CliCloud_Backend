using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddServicosAndSubsistemasAdjustments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TipoServico_Nome",
                schema: "Servicos",
                table: "TipoServico");

            migrationBuilder.DropIndex(
                name: "IX_Servico_Codigo",
                schema: "Servicos",
                table: "Servico");

            migrationBuilder.DropColumn(
                name: "Mad",
                schema: "Servicos",
                table: "TipoServico");

            migrationBuilder.DropColumn(
                name: "Nome",
                schema: "Servicos",
                table: "TipoServico");

            migrationBuilder.DropColumn(
                name: "VTaxa",
                schema: "Servicos",
                table: "TipoServico");

            migrationBuilder.DropColumn(
                name: "ValorC",
                schema: "Servicos",
                table: "TipoServico");

            migrationBuilder.DropColumn(
                name: "Codigo",
                schema: "Servicos",
                table: "Servico");

            migrationBuilder.DropColumn(
                name: "UsaAuxiliar",
                schema: "Servicos",
                table: "Servico");

            migrationBuilder.DropColumn(
                name: "UsaFisioter",
                schema: "Servicos",
                table: "Servico");

            migrationBuilder.RenameColumn(
                name: "ValorK",
                schema: "Servicos",
                table: "TipoServico",
                newName: "TaxaModeradoraSns");

            migrationBuilder.RenameColumn(
                name: "Nome",
                schema: "Servicos",
                table: "Servico",
                newName: "Designacao");

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                schema: "Servicos",
                table: "TipoServico",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            // TaxaIvaId existia como int; para evitar conflitos na alteração de tipo,
            // removemos a coluna antiga e criamos uma nova Guid? em branco.
            migrationBuilder.DropColumn(
                name: "TaxaIvaId",
                schema: "Servicos",
                table: "Servico");

            migrationBuilder.AddColumn<Guid>(
                name: "TaxaIvaId",
                schema: "Servicos",
                table: "Servico",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubsistemaServico",
                schema: "Servicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubsistemaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganismoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValorServico = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorOrganismo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MargemOrganismoPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorUtente = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MargemUtentePercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Inativo = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubsistemaServico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubsistemaServico_Servico_ServicoId",
                        column: x => x.ServicoId,
                        principalSchema: "Servicos",
                        principalTable: "Servico",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TipoServico_Descricao",
                schema: "Servicos",
                table: "TipoServico",
                column: "Descricao");

            migrationBuilder.CreateIndex(
                name: "IX_Servico_Designacao",
                schema: "Servicos",
                table: "Servico",
                column: "Designacao");

            migrationBuilder.CreateIndex(
                name: "IX_Servico_TaxaIvaId",
                schema: "Servicos",
                table: "Servico",
                column: "TaxaIvaId");

            migrationBuilder.CreateIndex(
                name: "IX_SubsistemaServico_ServicoId",
                schema: "Servicos",
                table: "SubsistemaServico",
                column: "ServicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Servico_TaxaIva_TaxaIvaId",
                schema: "Servicos",
                table: "Servico",
                column: "TaxaIvaId",
                principalSchema: "Utility",
                principalTable: "TaxaIva",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servico_TaxaIva_TaxaIvaId",
                schema: "Servicos",
                table: "Servico");

            migrationBuilder.DropTable(
                name: "SubsistemaServico",
                schema: "Servicos");

            migrationBuilder.DropIndex(
                name: "IX_TipoServico_Descricao",
                schema: "Servicos",
                table: "TipoServico");

            migrationBuilder.DropIndex(
                name: "IX_Servico_Designacao",
                schema: "Servicos",
                table: "Servico");

            migrationBuilder.DropIndex(
                name: "IX_Servico_TaxaIvaId",
                schema: "Servicos",
                table: "Servico");

            migrationBuilder.DropColumn(
                name: "Descricao",
                schema: "Servicos",
                table: "TipoServico");

            migrationBuilder.RenameColumn(
                name: "TaxaModeradoraSns",
                schema: "Servicos",
                table: "TipoServico",
                newName: "ValorK");

            migrationBuilder.RenameColumn(
                name: "Designacao",
                schema: "Servicos",
                table: "Servico",
                newName: "Nome");

            migrationBuilder.AddColumn<int>(
                name: "Mad",
                schema: "Servicos",
                table: "TipoServico",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                schema: "Servicos",
                table: "TipoServico",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "VTaxa",
                schema: "Servicos",
                table: "TipoServico",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorC",
                schema: "Servicos",
                table: "TipoServico",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TaxaIvaId",
                schema: "Servicos",
                table: "Servico",
                type: "int",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                schema: "Servicos",
                table: "Servico",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "UsaAuxiliar",
                schema: "Servicos",
                table: "Servico",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UsaFisioter",
                schema: "Servicos",
                table: "Servico",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TipoServico_Nome",
                schema: "Servicos",
                table: "TipoServico",
                column: "Nome");

            migrationBuilder.CreateIndex(
                name: "IX_Servico_Codigo",
                schema: "Servicos",
                table: "Servico",
                column: "Codigo",
                unique: true);
        }
    }
}
