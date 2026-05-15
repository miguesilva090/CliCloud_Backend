using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Admissao_AdmissaoServico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CodigoLegado",
                schema: "Consultas",
                table: "TiposAdmissao",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AdmissaoId",
                schema: "Consultas",
                table: "Consulta",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DoencaPrincipalId",
                schema: "Consultas",
                table: "Consulta",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DoencaSecundariaId",
                schema: "Consultas",
                table: "Consulta",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TipoAdmissaoId",
                schema: "Consultas",
                table: "Consulta",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Admissao",
                schema: "Consultas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsultaMarcacaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MedicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EspecialidadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TecnicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FuncionarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MedicoExternoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MotivoConsultaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TipoAdmissaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TipoConsultaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrganismoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SeguradoraId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TratamentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HoraInicio = table.Column<TimeSpan>(type: "time", nullable: true),
                    HoraFim = table.Column<TimeSpan>(type: "time", nullable: true),
                    HoraChegada = table.Column<TimeSpan>(type: "time", nullable: true),
                    StatusConsulta = table.Column<int>(type: "int", nullable: true),
                    Origem = table.Column<int>(type: "int", nullable: false),
                    Confirmado = table.Column<bool>(type: "bit", nullable: true),
                    Efetuado = table.Column<bool>(type: "bit", nullable: true),
                    Credencial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CredencialExterna = table.Column<int>(type: "int", nullable: true),
                    NumDestacavel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ordem = table.Column<int>(type: "int", nullable: true),
                    Sinistrado = table.Column<int>(type: "int", nullable: true),
                    Justificacao = table.Column<int>(type: "int", nullable: true),
                    MotivoJustificacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Diagnostico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoencaPrincipalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DoencaSecundariaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Obs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataHoraMarcacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admissao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admissao_ConsultaMarcacao_ConsultaMarcacaoId",
                        column: x => x.ConsultaMarcacaoId,
                        principalSchema: "Consultas",
                        principalTable: "ConsultaMarcacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Admissao_Doenca_DoencaPrincipalId",
                        column: x => x.DoencaPrincipalId,
                        principalSchema: "Doencas",
                        principalTable: "Doenca",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Admissao_Doenca_DoencaSecundariaId",
                        column: x => x.DoencaSecundariaId,
                        principalSchema: "Doencas",
                        principalTable: "Doenca",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Admissao_Especialidade_EspecialidadeId",
                        column: x => x.EspecialidadeId,
                        principalSchema: "Especialidades",
                        principalTable: "Especialidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Admissao_Funcionario_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalSchema: "Funcionarios",
                        principalTable: "Funcionario",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Admissao_MedicoExterno_MedicoExternoId",
                        column: x => x.MedicoExternoId,
                        principalSchema: "Medicos",
                        principalTable: "MedicoExterno",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Admissao_Medico_MedicoId",
                        column: x => x.MedicoId,
                        principalSchema: "Medicos",
                        principalTable: "Medico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Admissao_MotivosConsulta_MotivoConsultaId",
                        column: x => x.MotivoConsultaId,
                        principalSchema: "Consultas",
                        principalTable: "MotivosConsulta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Admissao_Organismo_OrganismoId",
                        column: x => x.OrganismoId,
                        principalSchema: "Organismos",
                        principalTable: "Organismo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Admissao_Sala_SalaId",
                        column: x => x.SalaId,
                        principalSchema: "Consultas",
                        principalTable: "Sala",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Admissao_Seguradora_SeguradoraId",
                        column: x => x.SeguradoraId,
                        principalSchema: "Seguradoras",
                        principalTable: "Seguradora",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Admissao_Tecnico_TecnicoId",
                        column: x => x.TecnicoId,
                        principalSchema: "Tecnicos",
                        principalTable: "Tecnico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Admissao_TiposAdmissao_TipoAdmissaoId",
                        column: x => x.TipoAdmissaoId,
                        principalSchema: "Consultas",
                        principalTable: "TiposAdmissao",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Admissao_TiposConsulta_TipoConsultaId",
                        column: x => x.TipoConsultaId,
                        principalSchema: "Consultas",
                        principalTable: "TiposConsulta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Admissao_Tratamento_TratamentoId",
                        column: x => x.TratamentoId,
                        principalSchema: "Tratamentos",
                        principalTable: "Tratamento",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Admissao_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdmissaoServico",
                schema: "Consultas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdmissaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ValorServico = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CodigoArtigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomeArtigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValorArtigo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Quantidade = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MargemMed = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MargemIns = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RecMed = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RecInst = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DescInst = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DescCli = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorDesc = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Ordem = table.Column<int>(type: "int", nullable: true),
                    Dente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExameId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Linha = table.Column<int>(type: "int", nullable: false),
                    NCheque = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Electrocardiograma = table.Column<int>(type: "int", nullable: true),
                    ValorUt = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdmissaoServico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdmissaoServico_Admissao_AdmissaoId",
                        column: x => x.AdmissaoId,
                        principalSchema: "Consultas",
                        principalTable: "Admissao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdmissaoServico_Exame_ExameId",
                        column: x => x.ExameId,
                        principalSchema: "Exames",
                        principalTable: "Exame",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdmissaoServico_Servico_ServicoId",
                        column: x => x.ServicoId,
                        principalSchema: "Servicos",
                        principalTable: "Servico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Consulta_AdmissaoId",
                schema: "Consultas",
                table: "Consulta",
                column: "AdmissaoId",
                unique: true,
                filter: "[AdmissaoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Consulta_DoencaPrincipalId",
                schema: "Consultas",
                table: "Consulta",
                column: "DoencaPrincipalId");

            migrationBuilder.CreateIndex(
                name: "IX_Consulta_DoencaSecundariaId",
                schema: "Consultas",
                table: "Consulta",
                column: "DoencaSecundariaId");

            migrationBuilder.CreateIndex(
                name: "IX_Consulta_TipoAdmissaoId",
                schema: "Consultas",
                table: "Consulta",
                column: "TipoAdmissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissao_ConsultaMarcacaoId",
                schema: "Consultas",
                table: "Admissao",
                column: "ConsultaMarcacaoId",
                unique: true,
                filter: "[ConsultaMarcacaoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Admissao_Data",
                schema: "Consultas",
                table: "Admissao",
                column: "Data");

            migrationBuilder.CreateIndex(
                name: "IX_Admissao_Data_UtenteId",
                schema: "Consultas",
                table: "Admissao",
                columns: new[] { "Data", "UtenteId" });

            migrationBuilder.CreateIndex(
                name: "IX_Admissao_DoencaPrincipalId",
                schema: "Consultas",
                table: "Admissao",
                column: "DoencaPrincipalId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissao_DoencaSecundariaId",
                schema: "Consultas",
                table: "Admissao",
                column: "DoencaSecundariaId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissao_EspecialidadeId",
                schema: "Consultas",
                table: "Admissao",
                column: "EspecialidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissao_FuncionarioId",
                schema: "Consultas",
                table: "Admissao",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissao_MedicoExternoId",
                schema: "Consultas",
                table: "Admissao",
                column: "MedicoExternoId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissao_MedicoId",
                schema: "Consultas",
                table: "Admissao",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissao_MotivoConsultaId",
                schema: "Consultas",
                table: "Admissao",
                column: "MotivoConsultaId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissao_OrganismoId",
                schema: "Consultas",
                table: "Admissao",
                column: "OrganismoId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissao_SalaId",
                schema: "Consultas",
                table: "Admissao",
                column: "SalaId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissao_SeguradoraId",
                schema: "Consultas",
                table: "Admissao",
                column: "SeguradoraId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissao_TecnicoId",
                schema: "Consultas",
                table: "Admissao",
                column: "TecnicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissao_TipoAdmissaoId",
                schema: "Consultas",
                table: "Admissao",
                column: "TipoAdmissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissao_TipoConsultaId",
                schema: "Consultas",
                table: "Admissao",
                column: "TipoConsultaId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissao_TratamentoId",
                schema: "Consultas",
                table: "Admissao",
                column: "TratamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissao_UtenteId",
                schema: "Consultas",
                table: "Admissao",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissaoServico_AdmissaoId_Linha",
                schema: "Consultas",
                table: "AdmissaoServico",
                columns: new[] { "AdmissaoId", "Linha" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdmissaoServico_ExameId",
                schema: "Consultas",
                table: "AdmissaoServico",
                column: "ExameId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissaoServico_ServicoId",
                schema: "Consultas",
                table: "AdmissaoServico",
                column: "ServicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consulta_Admissao_AdmissaoId",
                schema: "Consultas",
                table: "Consulta",
                column: "AdmissaoId",
                principalSchema: "Consultas",
                principalTable: "Admissao",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Consulta_Doenca_DoencaPrincipalId",
                schema: "Consultas",
                table: "Consulta",
                column: "DoencaPrincipalId",
                principalSchema: "Doencas",
                principalTable: "Doenca",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Consulta_Doenca_DoencaSecundariaId",
                schema: "Consultas",
                table: "Consulta",
                column: "DoencaSecundariaId",
                principalSchema: "Doencas",
                principalTable: "Doenca",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Consulta_TiposAdmissao_TipoAdmissaoId",
                schema: "Consultas",
                table: "Consulta",
                column: "TipoAdmissaoId",
                principalSchema: "Consultas",
                principalTable: "TiposAdmissao",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consulta_Admissao_AdmissaoId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropForeignKey(
                name: "FK_Consulta_Doenca_DoencaPrincipalId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropForeignKey(
                name: "FK_Consulta_Doenca_DoencaSecundariaId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropForeignKey(
                name: "FK_Consulta_TiposAdmissao_TipoAdmissaoId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropTable(
                name: "AdmissaoServico",
                schema: "Consultas");

            migrationBuilder.DropTable(
                name: "Admissao",
                schema: "Consultas");

            migrationBuilder.DropIndex(
                name: "IX_Consulta_AdmissaoId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropIndex(
                name: "IX_Consulta_DoencaPrincipalId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropIndex(
                name: "IX_Consulta_DoencaSecundariaId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropIndex(
                name: "IX_Consulta_TipoAdmissaoId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropColumn(
                name: "CodigoLegado",
                schema: "Consultas",
                table: "TiposAdmissao");

            migrationBuilder.DropColumn(
                name: "AdmissaoId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropColumn(
                name: "DoencaPrincipalId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropColumn(
                name: "DoencaSecundariaId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropColumn(
                name: "TipoAdmissaoId",
                schema: "Consultas",
                table: "Consulta");
        }
    }
}
