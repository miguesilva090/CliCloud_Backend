using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingFKsExistingEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tratamento_DocumentoId",
                schema: "Tratamentos",
                table: "Tratamento",
                column: "DocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Tratamento_TratamentoPredId",
                schema: "Tratamentos",
                table: "Tratamento",
                column: "TratamentoPredId");

            migrationBuilder.CreateIndex(
                name: "IX_SessaoTratamento_DocumentoId",
                schema: "Tratamentos",
                table: "SessaoTratamento",
                column: "DocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_SessaoTratamento_TipoDocumentoId",
                schema: "Tratamentos",
                table: "SessaoTratamento",
                column: "TipoDocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Documento_CodigoPostalId",
                schema: "Documentos",
                table: "Documento",
                column: "CodigoPostalId");

            migrationBuilder.CreateIndex(
                name: "IX_Consulta_DocumentoId",
                schema: "Consultas",
                table: "Consulta",
                column: "DocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Consulta_FuncionarioId",
                schema: "Consultas",
                table: "Consulta",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Consulta_OrganismoId",
                schema: "Consultas",
                table: "Consulta",
                column: "OrganismoId");

            migrationBuilder.CreateIndex(
                name: "IX_Consulta_TipoDocumentoId",
                schema: "Consultas",
                table: "Consulta",
                column: "TipoDocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Consulta_TratamentoId",
                schema: "Consultas",
                table: "Consulta",
                column: "TratamentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consulta_Documento_DocumentoId",
                schema: "Consultas",
                table: "Consulta",
                column: "DocumentoId",
                principalSchema: "Documentos",
                principalTable: "Documento",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Consulta_Funcionario_FuncionarioId",
                schema: "Consultas",
                table: "Consulta",
                column: "FuncionarioId",
                principalSchema: "Funcionarios",
                principalTable: "Funcionario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Consulta_Organismo_OrganismoId",
                schema: "Consultas",
                table: "Consulta",
                column: "OrganismoId",
                principalSchema: "Organismos",
                principalTable: "Organismo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Consulta_TipoDocumento_TipoDocumentoId",
                schema: "Consultas",
                table: "Consulta",
                column: "TipoDocumentoId",
                principalSchema: "Documentos",
                principalTable: "TipoDocumento",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Consulta_Tratamento_TratamentoId",
                schema: "Consultas",
                table: "Consulta",
                column: "TratamentoId",
                principalSchema: "Tratamentos",
                principalTable: "Tratamento",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Documento_CodigoPostal_CodigoPostalId",
                schema: "Documentos",
                table: "Documento",
                column: "CodigoPostalId",
                principalSchema: "Utility",
                principalTable: "CodigoPostal",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SessaoTratamento_Documento_DocumentoId",
                schema: "Tratamentos",
                table: "SessaoTratamento",
                column: "DocumentoId",
                principalSchema: "Documentos",
                principalTable: "Documento",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SessaoTratamento_TipoDocumento_TipoDocumentoId",
                schema: "Tratamentos",
                table: "SessaoTratamento",
                column: "TipoDocumentoId",
                principalSchema: "Documentos",
                principalTable: "TipoDocumento",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tratamento_Documento_DocumentoId",
                schema: "Tratamentos",
                table: "Tratamento",
                column: "DocumentoId",
                principalSchema: "Documentos",
                principalTable: "Documento",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tratamento_Tratamento_TratamentoPredId",
                schema: "Tratamentos",
                table: "Tratamento",
                column: "TratamentoPredId",
                principalSchema: "Tratamentos",
                principalTable: "Tratamento",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consulta_Documento_DocumentoId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropForeignKey(
                name: "FK_Consulta_Funcionario_FuncionarioId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropForeignKey(
                name: "FK_Consulta_Organismo_OrganismoId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropForeignKey(
                name: "FK_Consulta_TipoDocumento_TipoDocumentoId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropForeignKey(
                name: "FK_Consulta_Tratamento_TratamentoId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropForeignKey(
                name: "FK_Documento_CodigoPostal_CodigoPostalId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropForeignKey(
                name: "FK_SessaoTratamento_Documento_DocumentoId",
                schema: "Tratamentos",
                table: "SessaoTratamento");

            migrationBuilder.DropForeignKey(
                name: "FK_SessaoTratamento_TipoDocumento_TipoDocumentoId",
                schema: "Tratamentos",
                table: "SessaoTratamento");

            migrationBuilder.DropForeignKey(
                name: "FK_Tratamento_Documento_DocumentoId",
                schema: "Tratamentos",
                table: "Tratamento");

            migrationBuilder.DropForeignKey(
                name: "FK_Tratamento_Tratamento_TratamentoPredId",
                schema: "Tratamentos",
                table: "Tratamento");

            migrationBuilder.DropIndex(
                name: "IX_Tratamento_DocumentoId",
                schema: "Tratamentos",
                table: "Tratamento");

            migrationBuilder.DropIndex(
                name: "IX_Tratamento_TratamentoPredId",
                schema: "Tratamentos",
                table: "Tratamento");

            migrationBuilder.DropIndex(
                name: "IX_SessaoTratamento_DocumentoId",
                schema: "Tratamentos",
                table: "SessaoTratamento");

            migrationBuilder.DropIndex(
                name: "IX_SessaoTratamento_TipoDocumentoId",
                schema: "Tratamentos",
                table: "SessaoTratamento");

            migrationBuilder.DropIndex(
                name: "IX_Documento_CodigoPostalId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropIndex(
                name: "IX_Consulta_DocumentoId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropIndex(
                name: "IX_Consulta_FuncionarioId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropIndex(
                name: "IX_Consulta_OrganismoId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropIndex(
                name: "IX_Consulta_TipoDocumentoId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropIndex(
                name: "IX_Consulta_TratamentoId",
                schema: "Consultas",
                table: "Consulta");
        }
    }
}
