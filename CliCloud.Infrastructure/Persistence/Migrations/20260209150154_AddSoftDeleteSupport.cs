using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Artigos",
                table: "ViaAdministracao",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Artigos",
                table: "ViaAdministracao",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Tratamentos",
                table: "Tratamento",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Tratamentos",
                table: "Tratamento",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Servicos",
                table: "TipoServico",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Servicos",
                table: "TipoServico",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "TipoEntidadeFinanceira",
                table: "TipoEntidadeFinanceira",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "TipoEntidadeFinanceira",
                table: "TipoEntidadeFinanceira",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Documentos",
                table: "TipoDocumento",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Documentos",
                table: "TipoDocumento",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Tratamentos",
                table: "TipoAparelho",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Tratamentos",
                table: "TipoAparelho",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Utility",
                table: "TaxaIva",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Utility",
                table: "TaxaIva",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Utility",
                table: "Sexo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Utility",
                table: "Sexo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Tratamentos",
                table: "SessaoTratamento",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Tratamentos",
                table: "SessaoTratamento",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Tratamentos",
                table: "ServicoTratamento",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Tratamentos",
                table: "ServicoTratamento",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Tratamentos",
                table: "ServicoSessao",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Tratamentos",
                table: "ServicoSessao",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Consultas",
                table: "ServicoConsulta",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Consultas",
                table: "ServicoConsulta",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Servicos",
                table: "Servico",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Servicos",
                table: "Servico",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Utility",
                table: "Rua",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Utility",
                table: "Rua",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Utility",
                table: "ProvenienciaUtente",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Utility",
                table: "ProvenienciaUtente",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Utility",
                table: "Profissao",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Utility",
                table: "Profissao",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Utility",
                table: "Pais",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Utility",
                table: "Pais",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Utility",
                table: "Moeda",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Utility",
                table: "Moeda",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Consultas",
                table: "MarcacaoConsulta",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Consultas",
                table: "MarcacaoConsulta",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Tecnicos",
                table: "HorarioTecnicoVariavel",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Tecnicos",
                table: "HorarioTecnicoVariavel",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Tecnicos",
                table: "HorarioTecnicoDia",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Tecnicos",
                table: "HorarioTecnicoDia",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Tecnicos",
                table: "HorarioTecnico",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Tecnicos",
                table: "HorarioTecnico",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Medicos",
                table: "HorarioMedicoVariavel",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Medicos",
                table: "HorarioMedicoVariavel",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Medicos",
                table: "HorarioMedicoDia",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Medicos",
                table: "HorarioMedicoDia",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Medicos",
                table: "HorarioMedico",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Medicos",
                table: "HorarioMedico",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Utility",
                table: "Habilitacao",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Utility",
                table: "Habilitacao",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Artigos",
                table: "GrupoViasAdministracaoLinha",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Artigos",
                table: "GrupoViasAdministracaoLinha",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Artigos",
                table: "GrupoViasAdministracao",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Artigos",
                table: "GrupoViasAdministracao",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Utility",
                table: "GrupoSanguineo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Utility",
                table: "GrupoSanguineo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Utility",
                table: "GrauParentesco",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Utility",
                table: "GrauParentesco",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Utility",
                table: "Freguesia",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Utility",
                table: "Freguesia",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Tecnicos",
                table: "FolgasTecnico",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Tecnicos",
                table: "FolgasTecnico",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Medicos",
                table: "FolgasMedico",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Medicos",
                table: "FolgasMedico",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Consultas",
                table: "Exame",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Consultas",
                table: "Exame",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Utility",
                table: "EstadoCivil",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Utility",
                table: "EstadoCivil",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Especialidades",
                table: "Especialidade",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Especialidades",
                table: "Especialidade",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Utility",
                table: "EntidadeContacto",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Utility",
                table: "EntidadeContacto",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Utility",
                table: "Entidade",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Utility",
                table: "Entidade",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Documentos",
                table: "Documento",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Documentos",
                table: "Documento",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Utility",
                table: "Distrito",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Utility",
                table: "Distrito",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Consultas",
                table: "Consulta",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Consultas",
                table: "Consulta",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Utility",
                table: "Concelho",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Utility",
                table: "Concelho",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Utility",
                table: "CodigoPostal",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Utility",
                table: "CodigoPostal",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Core",
                table: "ClinicaAPIKey",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Core",
                table: "ClinicaAPIKey",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Especialidades",
                table: "CategoriaEspecialidade",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Especialidades",
                table: "CategoriaEspecialidade",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "CartaConducao",
                table: "CartaConducaoRestricoes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "CartaConducao",
                table: "CartaConducaoRestricoes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "CartaConducao",
                table: "CartaConducao",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "CartaConducao",
                table: "CartaConducao",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Atestados",
                table: "AtestadoRestricaoAnterior",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Atestados",
                table: "AtestadoRestricaoAnterior",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Atestados",
                table: "AtestadoRestricao",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Atestados",
                table: "AtestadoRestricao",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Atestados",
                table: "AtestadoCategoria",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Atestados",
                table: "AtestadoCategoria",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Atestados",
                table: "Atestado",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Atestados",
                table: "Atestado",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "Tratamentos",
                table: "Aparelho",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "Tratamentos",
                table: "Aparelho",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Artigos",
                table: "ViaAdministracao");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Artigos",
                table: "ViaAdministracao");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Tratamentos",
                table: "Tratamento");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Tratamentos",
                table: "Tratamento");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Servicos",
                table: "TipoServico");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Servicos",
                table: "TipoServico");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "TipoEntidadeFinanceira",
                table: "TipoEntidadeFinanceira");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "TipoEntidadeFinanceira",
                table: "TipoEntidadeFinanceira");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Documentos",
                table: "TipoDocumento");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Documentos",
                table: "TipoDocumento");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Tratamentos",
                table: "TipoAparelho");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Tratamentos",
                table: "TipoAparelho");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Utility",
                table: "TaxaIva");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Utility",
                table: "TaxaIva");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Utility",
                table: "Sexo");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Utility",
                table: "Sexo");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Tratamentos",
                table: "SessaoTratamento");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Tratamentos",
                table: "SessaoTratamento");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Tratamentos",
                table: "ServicoTratamento");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Tratamentos",
                table: "ServicoTratamento");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Tratamentos",
                table: "ServicoSessao");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Tratamentos",
                table: "ServicoSessao");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Consultas",
                table: "ServicoConsulta");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Consultas",
                table: "ServicoConsulta");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Servicos",
                table: "Servico");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Servicos",
                table: "Servico");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Utility",
                table: "Rua");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Utility",
                table: "Rua");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Utility",
                table: "ProvenienciaUtente");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Utility",
                table: "ProvenienciaUtente");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Utility",
                table: "Profissao");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Utility",
                table: "Profissao");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Utility",
                table: "Pais");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Utility",
                table: "Pais");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Utility",
                table: "Moeda");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Utility",
                table: "Moeda");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Consultas",
                table: "MarcacaoConsulta");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Consultas",
                table: "MarcacaoConsulta");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Tecnicos",
                table: "HorarioTecnicoVariavel");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Tecnicos",
                table: "HorarioTecnicoVariavel");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Tecnicos",
                table: "HorarioTecnicoDia");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Tecnicos",
                table: "HorarioTecnicoDia");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Tecnicos",
                table: "HorarioTecnico");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Tecnicos",
                table: "HorarioTecnico");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Medicos",
                table: "HorarioMedicoVariavel");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Medicos",
                table: "HorarioMedicoVariavel");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Medicos",
                table: "HorarioMedicoDia");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Medicos",
                table: "HorarioMedicoDia");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Medicos",
                table: "HorarioMedico");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Medicos",
                table: "HorarioMedico");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Utility",
                table: "Habilitacao");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Utility",
                table: "Habilitacao");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Artigos",
                table: "GrupoViasAdministracaoLinha");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Artigos",
                table: "GrupoViasAdministracaoLinha");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Artigos",
                table: "GrupoViasAdministracao");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Artigos",
                table: "GrupoViasAdministracao");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Utility",
                table: "GrupoSanguineo");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Utility",
                table: "GrupoSanguineo");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Utility",
                table: "GrauParentesco");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Utility",
                table: "GrauParentesco");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Utility",
                table: "Freguesia");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Utility",
                table: "Freguesia");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Tecnicos",
                table: "FolgasTecnico");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Tecnicos",
                table: "FolgasTecnico");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Medicos",
                table: "FolgasMedico");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Medicos",
                table: "FolgasMedico");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Consultas",
                table: "Exame");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Consultas",
                table: "Exame");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Utility",
                table: "EstadoCivil");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Utility",
                table: "EstadoCivil");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Especialidades",
                table: "Especialidade");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Especialidades",
                table: "Especialidade");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Utility",
                table: "EntidadeContacto");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Utility",
                table: "EntidadeContacto");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Utility",
                table: "Entidade");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Utility",
                table: "Entidade");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Utility",
                table: "Distrito");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Utility",
                table: "Distrito");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Utility",
                table: "Concelho");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Utility",
                table: "Concelho");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Utility",
                table: "CodigoPostal");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Utility",
                table: "CodigoPostal");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Core",
                table: "ClinicaAPIKey");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Core",
                table: "ClinicaAPIKey");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Especialidades",
                table: "CategoriaEspecialidade");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Especialidades",
                table: "CategoriaEspecialidade");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "CartaConducao",
                table: "CartaConducaoRestricoes");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "CartaConducao",
                table: "CartaConducaoRestricoes");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "CartaConducao",
                table: "CartaConducao");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "CartaConducao",
                table: "CartaConducao");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Atestados",
                table: "AtestadoRestricaoAnterior");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Atestados",
                table: "AtestadoRestricaoAnterior");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Atestados",
                table: "AtestadoRestricao");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Atestados",
                table: "AtestadoRestricao");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Atestados",
                table: "AtestadoCategoria");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Atestados",
                table: "AtestadoCategoria");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Atestados",
                table: "Atestado");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Atestados",
                table: "Atestado");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "Tratamentos",
                table: "Aparelho");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "Tratamentos",
                table: "Aparelho");
        }
    }
}
