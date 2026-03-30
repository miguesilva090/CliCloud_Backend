using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConsultaRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"IF EXISTS (
                      SELECT 1
                      FROM sys.foreign_keys
                      WHERE name = 'FK_Consulta_Recibo_ReciboId'
                        AND parent_object_id = OBJECT_ID(N'[Consultas].[Consulta]')
                  )
                  ALTER TABLE [Consultas].[Consulta] DROP CONSTRAINT [FK_Consulta_Recibo_ReciboId];");

            migrationBuilder.DropTable(
                name: "MarcacaoConsulta",
                schema: "Consultas");

            migrationBuilder.Sql(
                @"IF COL_LENGTH('Consultas.Consulta', 'Utilizador') IS NOT NULL
                  EXEC sp_rename N'[Consultas].[Consulta].[Utilizador]', N'MotivoJustificacao', 'COLUMN';");

            migrationBuilder.Sql(
                @"IF COL_LENGTH('Consultas.Consulta', 'TipoCambio') IS NOT NULL
                  EXEC sp_rename N'[Consultas].[Consulta].[TipoCambio]', N'StatusConsulta', 'COLUMN';");

            migrationBuilder.Sql(
                @"IF COL_LENGTH('Consultas.Consulta', 'ReciboId') IS NOT NULL
                  EXEC sp_rename N'[Consultas].[Consulta].[ReciboId]', N'SalaId', 'COLUMN';");

            migrationBuilder.Sql(
                @"IF COL_LENGTH('Consultas.Consulta', 'InstituicaoEmpregadoraId') IS NOT NULL
                  EXEC sp_rename N'[Consultas].[Consulta].[InstituicaoEmpregadoraId]', N'MedicoExternoId', 'COLUMN';");

            migrationBuilder.Sql(
                @"IF COL_LENGTH('Consultas.Consulta', 'EspecialidadeCodigo') IS NOT NULL
                  EXEC sp_rename N'[Consultas].[Consulta].[EspecialidadeCodigo]', N'ConsultaMarcacaoId', 'COLUMN';");

            migrationBuilder.Sql(
                @"IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Consulta_ReciboId' AND object_id = OBJECT_ID(N'[Consultas].[Consulta]'))
                  EXEC sp_rename N'[Consultas].[Consulta].[IX_Consulta_ReciboId]', N'IX_Consulta_SalaId', 'INDEX';");

            migrationBuilder.Sql(
                @"IF COL_LENGTH('Consultas.Consulta', 'ConsultaMarcacaoId') IS NULL
                  ALTER TABLE [Consultas].[Consulta] ADD [ConsultaMarcacaoId] uniqueidentifier NULL;");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "HoraFim",
                schema: "Consultas",
                table: "Consulta",
                type: "time",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.Sql(
                @"IF COL_LENGTH('Consultas.Consulta', 'HoraInicio') IS NULL
                  ALTER TABLE [Consultas].[Consulta] ADD [HoraInicio] time NULL;");

            migrationBuilder.CreateTable(
                name: "MotivosConsulta",
                schema: "Consultas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Designacao = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotivosConsulta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sala",
                schema: "Consultas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroSala = table.Column<int>(type: "int", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ativa = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sala", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sala_Clinica_ClinicaId",
                        column: x => x.ClinicaId,
                        principalSchema: "Core",
                        principalTable: "Clinica",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TiposAdmissao",
                schema: "Consultas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Designacao = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposAdmissao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConsultaMarcacao",
                schema: "Consultas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsultaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EspecialidadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TecnicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FuncionarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MedicoExternoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HoraMarcacao = table.Column<TimeSpan>(type: "time", nullable: true),
                    MotivoConsultaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TipoAdmissaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NumDestacavel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmTratamento = table.Column<bool>(type: "bit", nullable: false),
                    StatusConsulta = table.Column<int>(type: "int", nullable: true),
                    Obs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultaMarcacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsultaMarcacao_Especialidade_EspecialidadeId",
                        column: x => x.EspecialidadeId,
                        principalSchema: "Especialidades",
                        principalTable: "Especialidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ConsultaMarcacao_Funcionario_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalSchema: "Funcionarios",
                        principalTable: "Funcionario",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConsultaMarcacao_MedicoExterno_MedicoExternoId",
                        column: x => x.MedicoExternoId,
                        principalSchema: "Medicos",
                        principalTable: "MedicoExterno",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConsultaMarcacao_Medico_MedicoId",
                        column: x => x.MedicoId,
                        principalSchema: "Medicos",
                        principalTable: "Medico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ConsultaMarcacao_MotivosConsulta_MotivoConsultaId",
                        column: x => x.MotivoConsultaId,
                        principalSchema: "Consultas",
                        principalTable: "MotivosConsulta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConsultaMarcacao_Sala_SalaId",
                        column: x => x.SalaId,
                        principalSchema: "Consultas",
                        principalTable: "Sala",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConsultaMarcacao_Tecnico_TecnicoId",
                        column: x => x.TecnicoId,
                        principalSchema: "Tecnicos",
                        principalTable: "Tecnico",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConsultaMarcacao_TiposAdmissao_TipoAdmissaoId",
                        column: x => x.TipoAdmissaoId,
                        principalSchema: "Consultas",
                        principalTable: "TiposAdmissao",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConsultaMarcacao_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Consulta_ConsultaMarcacaoId",
                schema: "Consultas",
                table: "Consulta",
                column: "ConsultaMarcacaoId",
                unique: true,
                filter: "[ConsultaMarcacaoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Consulta_MedicoExternoId",
                schema: "Consultas",
                table: "Consulta",
                column: "MedicoExternoId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultaMarcacao_EspecialidadeId",
                schema: "Consultas",
                table: "ConsultaMarcacao",
                column: "EspecialidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultaMarcacao_FuncionarioId",
                schema: "Consultas",
                table: "ConsultaMarcacao",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultaMarcacao_MedicoExternoId",
                schema: "Consultas",
                table: "ConsultaMarcacao",
                column: "MedicoExternoId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultaMarcacao_MedicoId",
                schema: "Consultas",
                table: "ConsultaMarcacao",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultaMarcacao_MotivoConsultaId",
                schema: "Consultas",
                table: "ConsultaMarcacao",
                column: "MotivoConsultaId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultaMarcacao_SalaId",
                schema: "Consultas",
                table: "ConsultaMarcacao",
                column: "SalaId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultaMarcacao_TecnicoId",
                schema: "Consultas",
                table: "ConsultaMarcacao",
                column: "TecnicoId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultaMarcacao_TipoAdmissaoId",
                schema: "Consultas",
                table: "ConsultaMarcacao",
                column: "TipoAdmissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultaMarcacao_UtenteId",
                schema: "Consultas",
                table: "ConsultaMarcacao",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Sala_ClinicaId",
                schema: "Consultas",
                table: "Sala",
                column: "ClinicaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consulta_ConsultaMarcacao_ConsultaMarcacaoId",
                schema: "Consultas",
                table: "Consulta",
                column: "ConsultaMarcacaoId",
                principalSchema: "Consultas",
                principalTable: "ConsultaMarcacao",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Consulta_MedicoExterno_MedicoExternoId",
                schema: "Consultas",
                table: "Consulta",
                column: "MedicoExternoId",
                principalSchema: "Medicos",
                principalTable: "MedicoExterno",
                principalColumn: "Id");

            migrationBuilder.Sql(
                @"IF COL_LENGTH('Consultas.Consulta', 'SalaId') IS NOT NULL
                  ALTER TABLE [Consultas].[Consulta] ALTER COLUMN [SalaId] uniqueidentifier NULL;");

            migrationBuilder.AddForeignKey(
                name: "FK_Consulta_Sala_SalaId",
                schema: "Consultas",
                table: "Consulta",
                column: "SalaId",
                principalSchema: "Consultas",
                principalTable: "Sala",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consulta_ConsultaMarcacao_ConsultaMarcacaoId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropForeignKey(
                name: "FK_Consulta_MedicoExterno_MedicoExternoId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropForeignKey(
                name: "FK_Consulta_Sala_SalaId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropTable(
                name: "ConsultaMarcacao",
                schema: "Consultas");

            migrationBuilder.DropTable(
                name: "MotivosConsulta",
                schema: "Consultas");

            migrationBuilder.DropTable(
                name: "Sala",
                schema: "Consultas");

            migrationBuilder.DropTable(
                name: "TiposAdmissao",
                schema: "Consultas");

            migrationBuilder.DropIndex(
                name: "IX_Consulta_ConsultaMarcacaoId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropIndex(
                name: "IX_Consulta_MedicoExternoId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropColumn(
                name: "HoraInicio",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.RenameColumn(
                name: "StatusConsulta",
                schema: "Consultas",
                table: "Consulta",
                newName: "TipoCambio");

            migrationBuilder.RenameColumn(
                name: "SalaId",
                schema: "Consultas",
                table: "Consulta",
                newName: "ReciboId");

            migrationBuilder.RenameColumn(
                name: "MotivoJustificacao",
                schema: "Consultas",
                table: "Consulta",
                newName: "Utilizador");

            migrationBuilder.RenameColumn(
                name: "MedicoExternoId",
                schema: "Consultas",
                table: "Consulta",
                newName: "InstituicaoEmpregadoraId");

            migrationBuilder.RenameColumn(
                name: "ConsultaMarcacaoId",
                schema: "Consultas",
                table: "Consulta",
                newName: "EspecialidadeCodigo");

            migrationBuilder.RenameIndex(
                name: "IX_Consulta_SalaId",
                schema: "Consultas",
                table: "Consulta",
                newName: "IX_Consulta_ReciboId");

            migrationBuilder.AlterColumn<string>(
                name: "HoraFim",
                schema: "Consultas",
                table: "Consulta",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcessoKioskSemPag",
                schema: "Consultas",
                table: "Consulta",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Apolice",
                schema: "Consultas",
                table: "Consulta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CExtramed",
                schema: "Consultas",
                table: "Consulta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CodMotivoConsulta",
                schema: "Consultas",
                table: "Consulta",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CodigoFatura",
                schema: "Consultas",
                table: "Consulta",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CodigoFaturaOrganismo",
                schema: "Consultas",
                table: "Consulta",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CodigoTipoDocumentoBase",
                schema: "Consultas",
                table: "Consulta",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ConfirmaConsulta",
                schema: "Consultas",
                table: "Consulta",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Confirmado",
                schema: "Consultas",
                table: "Consulta",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataFatura",
                schema: "Consultas",
                table: "Consulta",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataHoraMarcacao",
                schema: "Consultas",
                table: "Consulta",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataRecibo",
                schema: "Consultas",
                table: "Consulta",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescMotivoConsulta",
                schema: "Consultas",
                table: "Consulta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Desconto",
                schema: "Consultas",
                table: "Consulta",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescricaoJust",
                schema: "Consultas",
                table: "Consulta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Destino",
                schema: "Consultas",
                table: "Consulta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Efectuado",
                schema: "Consultas",
                table: "Consulta",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmTratamento",
                schema: "Consultas",
                table: "Consulta",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "EstadoI",
                schema: "Consultas",
                table: "Consulta",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstadoU",
                schema: "Consultas",
                table: "Consulta",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Faltou",
                schema: "Consultas",
                table: "Consulta",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Faturado",
                schema: "Consultas",
                table: "Consulta",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoraChegada",
                schema: "Consultas",
                table: "Consulta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoraGdh",
                schema: "Consultas",
                table: "Consulta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoraInic",
                schema: "Consultas",
                table: "Consulta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ICD91",
                schema: "Consultas",
                table: "Consulta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ICD92",
                schema: "Consultas",
                table: "Consulta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorQueue",
                schema: "Consultas",
                table: "Consulta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Isencao",
                schema: "Consultas",
                table: "Consulta",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoConsulta",
                schema: "Consultas",
                table: "Consulta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Movimento",
                schema: "Consultas",
                table: "Consulta",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NaoDiscriminar",
                schema: "Consultas",
                table: "Consulta",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumBenif",
                schema: "Consultas",
                table: "Consulta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumDestacavel",
                schema: "Consultas",
                table: "Consulta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumDevolucao",
                schema: "Consultas",
                table: "Consulta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumLinhas",
                schema: "Consultas",
                table: "Consulta",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroTFatura",
                schema: "Consultas",
                table: "Consulta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroTFaturaOrganismo",
                schema: "Consultas",
                table: "Consulta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Ordem",
                schema: "Consultas",
                table: "Consulta",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Pago",
                schema: "Consultas",
                table: "Consulta",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Pic",
                schema: "Consultas",
                table: "Consulta",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ProdAplic",
                schema: "Consultas",
                table: "Consulta",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sala",
                schema: "Consultas",
                table: "Consulta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Senha",
                schema: "Consultas",
                table: "Consulta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoAdmiss",
                schema: "Consultas",
                table: "Consulta",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoConsulta",
                schema: "Consultas",
                table: "Consulta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MarcacaoConsulta",
                schema: "Consultas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsultaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EspecialidadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MedicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EstadoDigital = table.Column<int>(type: "int", nullable: true),
                    EstadosCTH = table.Column<int>(type: "int", nullable: true),
                    HoraFim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoraInic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Obs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganismoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TipoAdmiss = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarcacaoConsulta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarcacaoConsulta_Consulta_ConsultaId",
                        column: x => x.ConsultaId,
                        principalSchema: "Consultas",
                        principalTable: "Consulta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MarcacaoConsulta_Especialidade_EspecialidadeId",
                        column: x => x.EspecialidadeId,
                        principalSchema: "Especialidades",
                        principalTable: "Especialidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MarcacaoConsulta_Medico_MedicoId",
                        column: x => x.MedicoId,
                        principalSchema: "Medicos",
                        principalTable: "Medico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MarcacaoConsulta_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarcacaoConsulta_ConsultaId",
                schema: "Consultas",
                table: "MarcacaoConsulta",
                column: "ConsultaId");

            migrationBuilder.CreateIndex(
                name: "IX_MarcacaoConsulta_EspecialidadeId",
                schema: "Consultas",
                table: "MarcacaoConsulta",
                column: "EspecialidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_MarcacaoConsulta_MedicoId",
                schema: "Consultas",
                table: "MarcacaoConsulta",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_MarcacaoConsulta_UtenteId",
                schema: "Consultas",
                table: "MarcacaoConsulta",
                column: "UtenteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consulta_Recibo_ReciboId",
                schema: "Consultas",
                table: "Consulta",
                column: "ReciboId",
                principalSchema: "Documentos",
                principalTable: "Recibo",
                principalColumn: "Id");
        }
    }
}
