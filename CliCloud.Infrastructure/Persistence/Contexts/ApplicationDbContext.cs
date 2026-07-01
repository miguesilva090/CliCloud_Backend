using CliCloud.Domain.Entities.Bancos;
using CliCloud.Domain.Entities.CartaConducao;
using CliCloud.Domain.Entities.CentroSaude;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Exames;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Domain.Entities.EntidadesFinanceiras;
using CliCloud.Domain.Entities.Especialidades;
using CliCloud.Domain.Entities.EstadosCivis;
using CliCloud.Domain.Entities.GrausParentesco;
using CliCloud.Domain.Entities.TaxasIva;
using CliCloud.Domain.Entities.ProvenienciasUtente;
using CliCloud.Domain.Entities.GruposSanguineos;
using CliCloud.Domain.Entities.Habilitacoes;
using CliCloud.Domain.Entities.Profissoes;
using CliCloud.Domain.Entities.Sexos;
using CliCloud.Domain.Entities.Moedas;
using CliCloud.Domain.Entities.Pagamentos;
using CliCloud.Domain.Entities.Fornecedores;
using CliCloud.Domain.Entities.Empresas;
using CliCloud.Domain.Entities.Funcionarios;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Entities.UnidadesLocaisSaude;
using CliCloud.Domain.Entities.Seguradoras;
using CliCloud.Domain.Entities.Servicos;
using CliCloud.Domain.Entities.Tecnicos;
using CliCloud.Domain.Entities.TipoEntidadeFinanceira;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Entities.Alergias;
using CliCloud.Domain.Entities.Doencas;
using CliCloud.Domain.Entities.Atestados;
using CliCloud.Domain.Entities.Artigos;
using CliCloud.Domain.Entities.RegioesCorpo;
using CliCloud.Domain.Entities.ProcessoClinico.HistoriaClinica;
using CliCloud.Domain.Entities.Antecedentes;
using CliCloud.Domain.Entities.ProcessoClinico;
using CliCloud.Domain.Entities.ProcessoClinico.SinaisVitais;
using CliCloud.Domain.Entities.ProcessoClinico.BodyChart;
using CliCloud.Domain.Entities.ProcessoClinico.RelatorioExames;
using CliCloud.Domain.Entities.ProcessoClinico.RelatorioAtestado;
using CliCloud.Infrastructure.Persistence.Configurations;
using CliCloud.Infrastructure.Persistence.Extensions;
using CliCloud.Domain.Entities.ProcessoClinico.Documentos;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;
using CliCloud.Domain.Entities.ProcessoClinico.Estomatologia;
using CliCloud.Domain.Entities.ProcessoClinico.Odontologia;
using CliCloud.Domain.Entities.Core.Sms;
using CliCloud.Domain.Entities.Core.Tratamentos;
using CliCloud.Domain.Entities.Common.Configurations;
using CliCloud.Domain.Entities.Core.ConfigReferenciaMB;
using CliCloud.Domain.Entities.Core.Email;
using CliCloud.Domain.Entities.Faturacao;
using CliCloud.Domain.Entities.ConfiguracaoADSE;
using CliCloud.Domain.Entities.Notificacoes;
using CliCloud.Domain.Entities.Sinistros;
using CliCloud.Domain.Entities.Credenciais;
using CliCloud.Domain.Entities.Stocks;
using Microsoft.EntityFrameworkCore;

//---------------------------------- CLI COMMANDS --------------------------------------------------

// Set default project to CliCloud.Infrastructure in Package Manager Console
// When scaffolding database migrations, you must specify which context (ApplicationDbContext), use the following command:

// add-migration -Context ApplicationDbContext -o Persistence/Migrations MigrationName
// update-database -Context ApplicationDbContext

// NOTE: if you use the update-database command, you'll likely see 'No migrations were applied. The database is already up to date' because the migrations are applied programatically during the build.

//--------------------------------------------------------------------------------------------------

namespace CliCloud.Infrastructure.Persistence.Contexts
{
  public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
  {
    public string CurrentUserId { get; set; }

    public void SetCurrentUserId(string userId)
    {
      CurrentUserId = userId;
    }

    // DbSets - Utility
    public DbSet<Pais> Paises { get; set; }
    public DbSet<Distrito> Distritos { get; set; }
    public DbSet<Concelho> Concelhos { get; set; }
    public DbSet<Freguesia> Freguesias { get; set; }
    public DbSet<CodigoPostal> CodigosPostais { get; set; }
    public DbSet<Rua> Ruas { get; set; }
    public DbSet<Entidade> Entidades { get; set; }
    public DbSet<EntidadePessoa> EntidadePessoas { get; set; }
    public DbSet<EntidadeContacto> EntidadeContactos { get; set; }
    public DbSet<Feriado> Feriados { get; set; }
    public DbSet<TipoCarta> TiposCarta { get; set; }
    
    // DbSets - Utentes
    public DbSet<Utente> Utentes { get; set; }
    public DbSet<UtenteSubsistemaLinha> UtenteSubsistemaLinhas { get; set; }

    // DbSets - Alergias
    public DbSet<Alergia> Alergias { get; set; }
    public DbSet<GrauAlergia> GrausAlergia { get; set; }
    public DbSet<AlergiaUtente> AlergiasUtente { get; set; }
    public DbSet<AlergiasUtenteObs> AlergiasUtenteObs { get; set; }
    
    // DbSets - Funcionarios
    public DbSet<Funcionario> Funcionarios { get; set; }
    
    // DbSets - Medicos
    public DbSet<Medico> Medicos { get; set; }
    public DbSet<MedicoExterno> MedicosExternos { get; set; }
    public DbSet<HorarioMedico> HorariosMedicos { get; set; }
    public DbSet<HorarioMedicoDia> HorariosMedicosDias { get; set; }
    public DbSet<HorarioMedicoVariavel> HorariosMedicosVariaveis { get; set; }
    public DbSet<FolgasMedico> FolgasMedicos { get; set; }
    public DbSet<MargemMedico> MargensMedicos { get; set; }

    // DbSets - Tecnicos
    public DbSet<Tecnico> Tecnicos { get; set; }
    public DbSet<HorarioTecnico> HorariosTecnicos { get; set; }
    public DbSet<HorarioTecnicoDia> HorariosTecnicosDias { get; set; }
    public DbSet<HorarioTecnicoVariavel> HorariosTecnicosVariaveis { get; set; }
    public DbSet<FolgasTecnico> FolgasTecnicos { get; set; }
    
    // DbSets - Fornecedores
    public DbSet<Fornecedor> Fornecedores { get; set; }
    
    // DbSets - Empresas
    public DbSet<Empresa> Empresas { get; set; }
    
    // DbSets - Especialidades
    public DbSet<CategoriaEspecialidade> CategoriasEspecialidades { get; set; }
    public DbSet<Especialidade> Especialidades { get; set; }

    // DbSets - Estados Civis (Utility)
    public DbSet<EstadoCivil> EstadosCivis { get; set; }

    // DbSets - Grupos Sanguíneos (Utility)
    public DbSet<GrupoSanguineo> GruposSanguineos { get; set; }

    // DbSets - Habilitações (Utility)
    public DbSet<Habilitacao> Habilitacoes { get; set; }

    // DbSets - Profissões (Utility)
    public DbSet<Profissao> Profissoes { get; set; }

    // DbSets - Sexos (Utility)
    public DbSet<Sexo> Sexos { get; set; }

    // DbSets - Graus Parentesco (Utility)
    public DbSet<GrauParentesco> GrausParentesco { get; set; }

    // DbSets - Taxas IVA (Utility)
    public DbSet<TaxaIva> TaxasIva { get; set; }
    public DbSet<MotivoIsencao> MotivosIsencao { get; set; }
    public DbSet<MotivoRetencao> MotivosRetencao { get; set; }

    // DbSets - Proveniências Utente (Utility)
    public DbSet<ProvenienciaUtente> ProvenienciasUtente { get; set; }

    // DbSets - Moedas (Utility)
    public DbSet<Moeda> Moedas { get; set; }

    // DbSets - Stocks
    public DbSet<Armazem> Armazens { get; set; }
    public DbSet<FamiliaArtigo> FamiliasArtigo { get; set; }
    public DbSet<UnidadeMedida> UnidadesMedida { get; set; }
    public DbSet<Artigo> Artigos { get; set; }
    public DbSet<SubsistemaArtigo> SubsistemasArtigo { get; set; }

    // DbSets - Faturacao
    public DbSet<ZonaComercial> ZonasComerciais { get; set; }
    public DbSet<ConfiguracaoADSE> ConfiguracoesADSE { get; set; }

    // DbSets - Pagamentos
    public DbSet<CondicaoPagamento> CondicoesPagamento { get; set; }
    public DbSet<TipoPagamento> TiposPagamentoSaft { get; set; }
    public DbSet<ModoPagamento> ModosPagamento { get; set; }
    
    // DbSets - TipoEntidadeFinanceira
    public DbSet<TipoEntidadeFinanceira> TiposEntidadeFinanceira { get; set; }
    
    // DbSets - EntidadesFinanceiras
    public DbSet<EntidadeFinanceira> EntidadesFinanceiras { get; set; }
    
    // DbSets - Bancos
    public DbSet<Banco> Bancos { get; set; }

    // DbSets - Contas Bancarias
    public DbSet<ContaBancaria> ContasBancarias { get; set; }
    
    // DbSets - CentroSaude
    public DbSet<CentroSaude> CentrosSaude { get; set; }
    
    // DbSets - UnidadesLocaisSaude
    public DbSet<UnidadesLocaisSaude> UnidadesLocaisSaude { get; set; }
    
    // DbSets - Organismos
    public DbSet<Organismo> Organismos { get; set; }

    // DbSets - Core
    public DbSet<Clinica> Clinicas { get; set; }
    public DbSet<ClinicaApiKey> ClinicasApiKeys { get; set; }
    public DbSet<ChamadaUtente> ChamadasUtentes { get; set; }
    public DbSet<ConfiguracaoVoz> ConfiguracoesVoz { get; set; }
    public DbSet<ConfiguracaoTeleconsulta> ConfiguracoesTeleconsulta { get; set; }
    public DbSet<ConfiguracaoVozOpcao> ConfiguracoesVozOpcoes { get; set; }
    public DbSet<ConfiguracaoTratamentos> ConfiguracoesTratamentos { get; set; }
    public DbSet<ClinicaConfiguracaoIva> ClinicasConfiguracoesIva { get; set; }
    public DbSet<ClinicaMotivoIsencaoDefault> ClinicasMotivosIsencaoDefault { get; set; }
    public DbSet<ClinicaTipoConsultaDefault> ClinicasTiposConsultaDefault { get; set; }
    public DbSet<ClinicaArmazemDefault> ClinicasArmazensDefault { get; set; }
    public DbSet<LicencaUserClinicaMap> LicencaUsersClinicasMap { get; set; }
    public DbSet<ConfigReferenciaMB> ConfiguracoesReferenciasMB { get; set; }

    // DbSets - Seguradoras
    public DbSet<Seguradora> Seguradoras { get; set; }
    
    // DbSets - Documentos
    public DbSet<Documento> Documentos { get; set; }
    public DbSet<DocumentoLinha> DocumentosLinhas { get; set; }
    public DbSet<DocumentoOrigemClinica> DocumentosOrigemClinica { get; set; }
    public DbSet<Recibo> Recibos { get; set; }
    public DbSet<TipoDocumento> TiposDocumento { get; set; }
    public DbSet<NaturezaDocumento> NaturezasDocumento { get; set; }
    public DbSet<ReferenciaMB> ReferenciasMB { get; set; }
    
    // DbSets - Servicos
    public DbSet<TipoServico> TiposServico { get; set; }
    public DbSet<Servico> Servicos { get; set; }
    
    // DbSets - Consultas
    public DbSet<Consulta> Consultas { get; set; }
    public DbSet<ConsultaMarcacao> MarcacoesConsultas { get; set; }
    public DbSet<ExamesSemPapelAssinaturaSessao> ExamesSemPapelAssinaturasSessao { get; set; }
    public DbSet<ExamesSemPapelOperacao> ExamesSemPapelOperacoes { get; set; }
    public DbSet<TeleconsultaSessao> TeleconsultasSessoes { get; set; }
    public DbSet<TeleconsultaAcessoLog> TeleconsultasAcessosLogs { get; set; }
    public DbSet<ServicoConsulta> ServicosConsultas { get; set; }
    public DbSet<ConsultaFaturacao> ConsultasFaturacao { get; set; }
    public DbSet<TipoConsultaItem> TiposConsulta { get; set; }
    public DbSet<Sala> Salas { get; set; }
    public DbSet<MotivoConsulta> MotivosConsulta { get; set; }
    public DbSet<Admissao> Admissoes { get; set; }
    public DbSet<AdmissaoServico> AdmissoesServicos { get; set; }
    public DbSet<TipoAdmissao> TiposAdmissao { get; set; }
    
    // DbSets - Tratamentos
    public DbSet<Tratamento> Tratamentos { get; set; }
    public DbSet<SessaoTratamento> SessoesTratamento { get; set; }
    public DbSet<ServicoTratamento> ServicosTratamento { get; set; }
    public DbSet<ServicoSessao> ServicosSessao { get; set; }
    public DbSet<MarcaAparelho> MarcasAparelho { get; set; }
    public DbSet<ModeloAparelho> ModelosAparelho { get; set; }
    public DbSet<TipoAparelho> TiposAparelho { get; set; }
    public DbSet<Aparelho> Aparelhos { get; set; }
    public DbSet<LocalTratamento> LocaisTratamento { get; set; }
    public DbSet<EstadoListaEspera> EstadosListaEspera { get; set; }
    public DbSet<Prioridade> Prioridades { get; set; }
    
    // DbSets - Patologias
    public DbSet<Patologia> Patologias { get; set; }
    public DbSet<PatologiaServico> PatologiaServicos { get; set; }
    public DbSet<PatologiaDoenca> PatologiaDoencas { get; set; }

    // DbSets - Exames
    public DbSet<CategoriaProcedimento> CategoriasProcedimento { get; set; }
    public DbSet<TipoExame> TiposExame { get; set; }
    public DbSet<Acordos> Acordos { get; set; }
    public DbSet<Analises> Analises { get; set; }
    public DbSet<GrupoAnaliseLinha> GrupoAnaliseLinhas { get; set; }
    public DbSet<Exame> Exames { get; set; }
    public DbSet<ExameLinha> ExameLinhas { get; set; }

    // DbSets - CartaConducao (atestados para carta de condução)
    public DbSet<CartaConducao> CartasConducao { get; set; }
    public DbSet<CartaConducaoRestricao> CartasConducaoRestricoes { get; set; }

    // DbSets - Doenças (ICD-11)
    public DbSet<Doenca> Doencas { get; set; }

    // DbSets - Atestados
    public DbSet<Atestado> Atestados { get; set; }
    public DbSet<AtestadoCategoria> AtestadosCategorias { get; set; }
    public DbSet<AtestadoRestricao> AtestadosRestricoes { get; set; }
    public DbSet<AtestadoRestricaoAnterior> AtestadosRestricoesAnteriores { get; set; }

    // DbSets - Artigos (Vias de Administração)
    public DbSet<ViaAdministracao> ViasAdministracao { get; set; }
    public DbSet<GrupoViasAdministracao> GruposViasAdministracao { get; set; }
    public DbSet<GrupoViasAdministracaoLinha> GruposViasAdministracaoLinhas { get; set; }

    // DbSets - RegioesCorpo
    public DbSet<RegiaoCorpo> RegioesCorpo { get; set; }
    
    // DbSets - HistoriaClinica
    public DbSet<HistoriaClinica> HistoriasClinicas { get; set; }

    // DbSets - Goniometrias
    public DbSet<Goniometrias> Goniometrias { get; set; }

    // DbSets - TiposDeDor
    public DbSet<TipoDeDor> TiposDeDor { get; set; }

    // DbSets - FraquezasMusculares
    public DbSet<FraquezasMusculares> FraquezasMusculares { get; set; }

    // DbSets - MotivoAlta
    public DbSet<MotivoAlta> MotivoAlta { get; set; }

    // DbSets - MotivosDesmarcacao
    public DbSet<MotivosDesmarcacao> MotivosDesmarcacao { get; set; }

    // DbSets - Antecedentes Pessoais
    public DbSet<AntecedentesPessoais> AntecedentesPessoais { get; set; }

    // DbSets - Antecedentes Familiares Utente
    public DbSet<AntecedentesFamiliaresUtente> AntecedentesFamiliaresUtente { get; set; }

    // DbSets - Antecedentes Cirurgicos
    public DbSet<AntecedentesCirurgicos> AntecedentesCirurgicos { get; set; }

    // DbSets - Questionarios Utente
    public DbSet<QuestionarioUtente> QuestionariosUtente { get; set; }

    // DbSets - HabitosEVicios
    public DbSet<HabitosEVicios> HabitosEVicios { get; set; }

    // DbSets - Sinais Vitais
    public DbSet<TensaoArterial> SinaisVitaisTensaoArterial { get; set; }
    public DbSet<GlicemiaCapilar> SinaisVitaisGlicemiaCapilar { get; set; }
    public DbSet<TemperaturaCorporal> SinaisVitaisTemperaturaCorporal { get; set; }
    public DbSet<IndiceMassaCorporal> SinaisVitaisIndiceMassaCorporal { get; set; }
    public DbSet<GorduraMassaMuscular> SinaisVitaisGorduraMassaMuscular { get; set; }
    public DbSet<AvaliacaoAntropometrica> SinaisVitaisAvaliacaoAntropometrica { get; set; }
    public DbSet<AvaliacaoPostural> SinaisVitaisAvaliacaoPostural { get; set; }


    // DbSets - Periocidade Tratamento
    public DbSet<PeriocidadeTratamento> PeriocidadeTratamento { get; set; }

    // DbSets - Evolucao Tratamento
    public DbSet<EvolucaoTratamento> EvolucaoTratamento { get; set; }

    // DbSets - Mapa Body Chart
    public DbSet<MapaBodyChart> MapasBodyChart { get; set; }

    // DbSets - Notas Body Chart
    public DbSet<NotaBodyChart> NotasBodyChart { get; set; }  

    // DbSets - RelatorioExames
    public DbSet<RelatorioExames> RelatorioExames { get; set; }

    // DbSets - RelatorioAtestado
    public DbSet<RelatorioAtestado> RelatorioAtestados { get; set; }

    // DbSets - Modelos RelatorioAtestado
    public DbSet<ModeloRelatorioAtestado> ModelosRelatorioAtestado { get; set; }

    // DbSets - Documentos Ficha Clinica
    public DbSet<DocumentosFichaClinica> DocumentosFichaClinica { get; set; }

    // DbSets - FichaClinicaSecaoTemplate (Separadores)
    public DbSet<FichaClinicaSecaoTemplate> FichaClinicaSecaoTemplates { get; set; }

    // DbSets - FichaClinicaSecaoCampo (Campos por separador)
    public DbSet<FichaClinicaSecaoCampo> FichaClinicaSecaoCampos { get; set; }

    // DbSets - FichaClinicaSecaoConteudo (Conteúdo por utente/campo)
    public DbSet<FichaClinicaSecaoConteudo> FichaClinicaSecaoConteudos { get; set; }

    // DbSets - Separadores (Legado: gestão de separadores)
    public DbSet<Separador> Separadores { get; set; }

    // DbSets - Separadores Personalizados (Legado: liga formulário ao separador visível)
    public DbSet<SeparadorPersonalizado> SeparadoresPersonalizados { get; set; }

    // DbSets - Vinculos de visibilidade (médico/especialidade)
    public DbSet<SeparadorPersonalizadoVinculo> SeparadoresPersonalizadosVinculos { get; set; }

    // DbSets - Vinculos de separadores base (médico/especialidade)
    public DbSet<SeparadorVinculo> SeparadoresVinculos { get; set; }

    // DbSets - AnamneseOdontopediatria
    public DbSet<AnamneseOdontopediatria> AnamneseOdontopediatria { get; set; }

    // DbSets - AnamneseOrtodonticaAnaliseGeral
    public DbSet<AnamneseOrtodonticaAnaliseGeral> AnamneseOrtodonticaAnaliseGeral { get; set; }

    // DbSets - AnamneseOrtodonticaAnaliseDentaria
    public DbSet<AnamneseOrtodonticaAnaliseDentaria> AnamneseOrtodonticaAnaliseDentaria { get; set; }

    // DbSets - AnamneseOrtodonticaDenticaoDeciduaeMista
    public DbSet<AnamneseOrtodonticaDenticaoDeciduaeMista> AnamneseOrtodonticaDenticaoDeciduaeMista { get; set; }

    // DbSets - AnamneseOrtodonticaATM
    public DbSet<AnamneseOrtodonticaATM> AnamneseOrtodonticaATM { get; set; }

    // DbSets - AnamneseOrtodonticaAnaliseFuncional
    public DbSet<AnamneseOrtodonticaAnaliseFuncional> AnamneseOrtodonticaAnaliseFuncional { get; set; }

    // DbSets - EstadosDentarios
    public DbSet<EstadosDentarios> EstadosDentarios { get; set; }

    // DbSets - TiposTratamentoDentario
    public DbSet<TipoTratamentoDentario> TiposTratamentoDentario { get; set; }

    // DbSets - OdontogramaDefinitivo
    public DbSet<OdontogramaDefinitivo> OdontogramaDefinitivo { get; set; }

    // DbSets - História dentária (relatório acumulado)
    public DbSet<HistoriaDentaria> HistoriaDentaria { get; set; }

    // DbSets - ConfiguracaoSms
    public DbSet<ConfiguracaoSms> ConfiguracaoSms { get; set; }
    public DbSet<ConfiguracaoSmsAutomatica> ConfiguracaoSmsAutomaticas { get; set; }
    public DbSet<ConfiguracaoSmsAutomaticaMedico> ConfiguracaoSmsAutomaticasMedicos { get; set; }
    public DbSet<HistoricoSms> HistoricosSms { get; set; }
    public DbSet<SmsRecebido> SmsRecebidos { get; set; }

    // DbSets - ConfigCartaConducao
    public DbSet<ConfigCartaConducao> ConfigCartasConducao { get; set; }

    // DbSets - ConfiguracaoEmail
    public DbSet<ConfiguracaoEmail> ConfiguracoesEmail { get; set; }
    public DbSet<ConfiguracaoEmailAutomatica> ConfiguracoesEmailAutomaticas { get; set; }
    public DbSet<HistoricoEmail> HistoricosEmail { get; set; }

    // DbSets - ConfigWebService
    public DbSet<ConfigWebService> ConfigWebservices { get; set; }

    // DbSets - ConfigExamesSemPapel
    public DbSet<ConfigExamesSemPapel> ConfigExamesSemPapel { get; set; }

    // DbSets - Modelos Documentos
    public DbSet<ModeloDocumento> ModelosDocumentos { get; set; }
    public DbSet<InstanciaDocumento> InstanciasDocumentos { get; set; }
    public DbSet<FicheiroDocumento> FicheirosDocumentos { get; set; }

    // DbSets - Pedidos de Consentimento
    public DbSet<PedidoConsentimento> PedidosConsentimento { get; set; }

    // DbSets - Notificacoes
    public DbSet<NotificacaoTipo> NotificacaoTipos { get; set; }
    public DbSet<Notificacao> Notificacoes { get; set; }

    // DbSets - LicencaUserClinicaMap
    public DbSet<LicencaUserClinicaMap> LicencaUserClinicaMap { get; set; }

    // DbSets - Sinistros
    public DbSet<Sinistrado> Sinistrados { get; set; }
    public DbSet<SinistradoLinhaServico> SinistradoLinhasServico { get; set; }
    public DbSet<EstadoSinistroItem> EstadoSinistroItens { get; set; }

    // DbSets - LoteDirect
    public DbSet<LoteDirect> LotesDirect { get; set; }
    public DbSet<TipoLote> TiposLote { get; set; }

    public DbSet<LoteDirectLinha> LotesDirectLinha { get; set; }
    public DbSet<LoteDirectLinha789> LotesDirectLinha789 { get; set; }
    public DbSet<LoteDirectAgregado> LotesDirectAgregado { get; set; }
    public DbSet<LoteDirectDetalhe> LotesDirectDetalhe { get; set; }

    // DbSets - ListaEsperaConsulta
    public DbSet<ListaEsperaConsulta> ListaEsperaConsultas { get; set; }

    // DbSets - GlobalBooking (tabelas legado dbo)
    public DbSet<PedidoConsulta> PedidosConsulta { get; set; }
    public DbSet<PedidoConsultaUtente> PedidosConsultaUtentes { get; set; }

    // DbSets - FicheiroEletronico
    public DbSet<FicheiroEletronicoRegisto> FicheiroEletronicoRegistos => Set<FicheiroEletronicoRegisto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Disable cascade delete globally
      foreach (
        var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())
      )
      {
        relationship.DeleteBehavior = DeleteBehavior.NoAction;
      }

      // query filters
      _ = modelBuilder.AppendGlobalQueryFilter<ISoftDelete>(s => s.DeletedOn == null);

      // Apply configurations
      _ = modelBuilder.ApplyConfiguration(new EntidadeConfiguration());
      _ = modelBuilder.ApplyConfiguration(new EntidadePessoaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new FeriadoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new TipoCartaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new UtenteConfiguration());
      _ = modelBuilder.ApplyConfiguration(new FuncionarioConfiguration());
      
      // Medicos configurations
      _ = modelBuilder.ApplyConfiguration(new MedicoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new MedicoExternoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new HorarioMedicoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new HorarioMedicoDiaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new HorarioMedicoVariavelConfiguration());
      _ = modelBuilder.ApplyConfiguration(new FolgasMedicoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new MargemMedicoConfiguration());
      
      // Tecnicos configurations
      _ = modelBuilder.ApplyConfiguration(new TecnicoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new HorarioTecnicoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new HorarioTecnicoDiaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new HorarioTecnicoVariavelConfiguration());
      _ = modelBuilder.ApplyConfiguration(new FolgasTecnicoConfiguration());
      
      // Fornecedores configurations
      _ = modelBuilder.ApplyConfiguration(new FornecedorConfiguration());
      
      // Empresas configurations
      _ = modelBuilder.ApplyConfiguration(new EmpresaConfiguration());
      
      // Especialidades configurations
      _ = modelBuilder.ApplyConfiguration(new CategoriaEspecialidadeConfiguration());
      _ = modelBuilder.ApplyConfiguration(new EspecialidadeConfiguration());

      // Estados Civis configurations
      _ = modelBuilder.ApplyConfiguration(new EstadoCivilConfiguration());

      // Grupos Sanguíneos configurations
      _ = modelBuilder.ApplyConfiguration(new GrupoSanguineoConfiguration());

      // Habilitações configurations
      _ = modelBuilder.ApplyConfiguration(new HabilitacaoConfiguration());

      // Profissões configurations
      _ = modelBuilder.ApplyConfiguration(new ProfissaoConfiguration());

      // Sexos configurations
      _ = modelBuilder.ApplyConfiguration(new SexoConfiguration());

      // Graus Parentesco configurations
      _ = modelBuilder.ApplyConfiguration(new GrauParentescoConfiguration());

      // Taxas IVA configurations
      _ = modelBuilder.ApplyConfiguration(new TaxaIvaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new MotivoIsencaoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new MotivoRetencaoConfiguration());

      // Stocks configurations
      _ = modelBuilder.ApplyConfiguration(new ArmazemConfiguration());
      _ = modelBuilder.ApplyConfiguration(new FamiliaArtigoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new UnidadeMedidaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new ArtigoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new SubsistemaArtigoConfiguration());

      // Faturacao configurations
      _ = modelBuilder.ApplyConfiguration(new ZonaComercialConfiguration());
      _ = modelBuilder.ApplyConfiguration(new ConfiguracaoADSEConfiguration());

      // Pagamentos configurations
      _ = modelBuilder.ApplyConfiguration(new CondicaoPagamentoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new TipoPagamentoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new ModoPagamentoConfiguration());

      // Proveniências Utente configurations
      _ = modelBuilder.ApplyConfiguration(new ProvenienciaUtenteConfiguration());

      // Moedas configurations
      _ = modelBuilder.ApplyConfiguration(new MoedaConfiguration());
      
      // TipoEntidadeFinanceira configurations
      _ = modelBuilder.ApplyConfiguration(new TipoEntidadeFinanceiraConfiguration());
      
      // EntidadesFinanceiras configurations
      _ = modelBuilder.ApplyConfiguration(new EntidadeFinanceiraConfiguration());
      
      // Bancos configurations
      _ = modelBuilder.ApplyConfiguration(new BancoConfiguration());

      // Contas Bancarias configurations
      _ = modelBuilder.ApplyConfiguration(new ContaBancariaConfiguration());
      
      // CentroSaude configurations
      _ = modelBuilder.ApplyConfiguration(new CentroSaudeConfiguration());
      
      // UnidadesLocaisSaude configurations
      _ = modelBuilder.ApplyConfiguration(new UnidadesLocaisSaudeConfiguration());

      // Organismos configurations
      _ = modelBuilder.ApplyConfiguration(new OrganismoConfiguration());

      // Core configurations
      _ = modelBuilder.ApplyConfiguration(new ClinicaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new ClinicaApiKeyConfiguration());
      _ = modelBuilder.ApplyConfiguration(new ChamadaUtenteConfiguration());
      _ = modelBuilder.ApplyConfiguration(new ConfiguracaoVozConfiguration());
      _ = modelBuilder.ApplyConfiguration(new ConfiguracaoTeleconsultaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new ConfiguracaoVozOpcaoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new ClinicaConfiguracaoIvaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new ClinicaMotivoIsencaoDefaultConfiguration());
      _ = modelBuilder.ApplyConfiguration(new ClinicaTipoConsultaDefaultConfiguration());
      _ = modelBuilder.ApplyConfiguration(new ClinicaArmazemDefaultConfiguration());

      // Seguradoras configurations
      _ = modelBuilder.ApplyConfiguration(new SeguradoraConfiguration());
      
      // Documentos configurations
      _ = modelBuilder.ApplyConfiguration(new DocumentoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new DocumentoLinhaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new DocumentoOrigemClinicaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new ReciboConfiguration());
      _ = modelBuilder.ApplyConfiguration(new TipoDocumentoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new NaturezaDocumentoConfiguration());
      
      // Servicos configurations
      _ = modelBuilder.ApplyConfiguration(new TipoServicoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new ServicoConfiguration());
      
      // Consultas configurations
      _ = modelBuilder.ApplyConfiguration(new ConsultaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new MarcacaoConsultaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new SalaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new MotivoConsultaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new TeleconsultaSessaoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new TeleconsultaAcessoLogConfiguration());
      _ = modelBuilder.ApplyConfiguration(new ServicoConsultaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new AdmissaoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new AdmissaoServicoConfiguration());
      
      // Tratamentos configurations
      _ = modelBuilder.ApplyConfiguration(new TratamentoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new SessaoTratamentoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new ServicoTratamentoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new ServicoSessaoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new MarcaAparelhoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new ModeloAparelhoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new TipoAparelhoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new AparelhoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new LocalTratamentoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new EstadoListaEsperaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new PrioridadeConfiguration());

      // Exames
      _ = modelBuilder.ApplyConfiguration(new CategoriaProcedimentoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new TipoExameConfiguration());
      _ = modelBuilder.ApplyConfiguration(new AcordoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new AnalisesConfiguration());
      _ = modelBuilder.ApplyConfiguration(new GrupoAnaliseLinhaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new ExameConfiguration());
      _ = modelBuilder.ApplyConfiguration(new ExameLinhaConfiguration());

      // Artigos (Vias de Administração)
      _ = modelBuilder.ApplyConfiguration(new ViaAdministracaoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new GrupoViasAdministracaoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new GrupoViasAdministracaoLinhaConfiguration());

      // Doenças (ICD-11) configurations
      _ = modelBuilder.ApplyConfiguration(new DoencaConfiguration());

      // Tipos de Consulta
      _ = modelBuilder.ApplyConfiguration(new TipoConsultaItemConfiguration());

      // Alergias configurations
      _ = modelBuilder.ApplyConfiguration(new AlergiaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new GrauAlergiaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new AlergiaUtenteConfiguration());
      _ = modelBuilder.ApplyConfiguration(new AlergiasUtenteObsConfiguration());

      // Patologias configurations
      _ = modelBuilder.ApplyConfiguration(new PatologiaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new PatologiaServicoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new PatologiaDoencaConfiguration());

      // RegioesCorpo configurations
      _ = modelBuilder.ApplyConfiguration(new RegiaoCorpoConfiguration());

      // Historia Clinica configurations
      _ = modelBuilder.ApplyConfiguration(new HistoriaClinicaConfiguration());

      // Goniometrias configurations
      _ = modelBuilder.ApplyConfiguration(new GoniometriasConfiguration());

      // Tipos de Dor configurations
      _ = modelBuilder.ApplyConfiguration(new TipoDeDorConfiguration());

      // FraquezasMusculares configurations
      _ = modelBuilder.ApplyConfiguration(new FraquezasMuscularesConfiguration());

      // MotivoAlta configurations
      _ = modelBuilder.ApplyConfiguration(new MotivoAltaConfiguration());

      // MotivosDesmarcacao configurations
      _ = modelBuilder.ApplyConfiguration(new MotivosDesmarcacaoConfiguration());

      // Antecedentes Pessoais configurations
      _ = modelBuilder.ApplyConfiguration(new AntecedentesPessoaisConfiguration());

      // Antecedentes Familiares Utente configurations
      _ = modelBuilder.ApplyConfiguration(new AntecedentesFamiliaresUtenteConfiguration());

      // Antecedentes Cirurgicos configurations
      _ = modelBuilder.ApplyConfiguration(new AntecedentesCirurgicosConfiguration());

      // Questionarios Utente configurations
      _ = modelBuilder.ApplyConfiguration(new QuestionarioUtenteConfiguration());

      // HabitosEVicios configurations
      _ = modelBuilder.ApplyConfiguration(new HabitosEViciosConfiguration());

      // Periocidade Tratamento configurations
      _ = modelBuilder.ApplyConfiguration(new PeriocidadeTratamentoConfiguration());

      // Evolucao Tratamento configurations
      _ = modelBuilder.ApplyConfiguration(new EvolucaoTratamentoConfiguration());

      // Mapa Body Chart configurations
      _ = modelBuilder.ApplyConfiguration(new MapaBodyChartConfiguration());

      // Notas Body Chart configurations
      _ = modelBuilder.ApplyConfiguration(new NotaBodyChartConfiguration());

      // RelatorioExames configurations
      _ = modelBuilder.ApplyConfiguration(new RelatorioExamesConfiguration());

      // HistoriaDentaria configurations
      _ = modelBuilder.ApplyConfiguration(new HistoriaDentariaConfiguration());

      // RelatorioAtestado configurations
      _ = modelBuilder.ApplyConfiguration(new RelatorioAtestadoConfiguration());

      // Modelos RelatorioAtestado configurations
      _ = modelBuilder.ApplyConfiguration(new ModeloRelatorioAtestadoConfiguration());

      // Documentos Ficha Clinica configurations
      _ = modelBuilder.ApplyConfiguration(new DocumentosFichaClinicaConfiguration());

      // FichaClinicaSecaoTemplate configurations
      _ = modelBuilder.ApplyConfiguration(new FichaClinicaSecaoTemplateConfiguration());

      // FichaClinicaSecaoCampo configurations
      _ = modelBuilder.ApplyConfiguration(new FichaClinicaSecaoCampoConfiguration());

      // FichaClinicaSecaoConteudo configurations
      _ = modelBuilder.ApplyConfiguration(new FichaClinicaSecaoConteudoConfiguration());

      // ConfiguracaoSms configurations
      _ = modelBuilder.ApplyConfiguration(new ConfiguracaoSmsConfiguration());
      _ = modelBuilder.ApplyConfiguration(new ConfiguracaoSmsAutomaticaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new ConfiguracaoSmsAutomaticaMedicoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new HistoricoSmsConfiguration());
      _ = modelBuilder.ApplyConfiguration(new SmsRecebidoConfiguration());

      // ConfigCartaConducao configurations
      _ = modelBuilder.ApplyConfiguration(new ConfigCartaConducaoConfiguration());

      // ConfiguracaoEmail configurations
      _ = modelBuilder.ApplyConfiguration(new ConfiguracaoEmailConfiguration());
      _ = modelBuilder.ApplyConfiguration(new ConfiguracaoEmailAutomaticoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new HistoricoEmailConfiguration());

      // ConfigWebService configurations
      _ = modelBuilder.ApplyConfiguration(new ConfigWebServiceConfiguration());

      // ConfigExamesSemPapel configurations
      _ = modelBuilder.ApplyConfiguration(new ConfigExamesSemPapelConfiguration());

      // Separadores and Formularios Personalizados configurations
      _ = modelBuilder.ApplyConfiguration(new SeparadorConfiguration());
      _ = modelBuilder.ApplyConfiguration(new SeparadorVinculoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new SeparadorPersonalizadoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new SeparadorPersonalizadoVinculoConfiguration());

      // Modelos Documentos configurations
      _ = modelBuilder.ApplyConfiguration(new ModeloDocumentoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new InstanciaDocumentoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new FicheiroDocumentoConfiguration());

      // Pedidos de Consentimento configurations
      _ = modelBuilder.ApplyConfiguration(new PedidoConsentimentoConfiguration());

      // Notificacoes configurations
      _ = modelBuilder.ApplyConfiguration(new NotificacaoTipoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new NotificacaoConfiguration());

      // Sinistros configurations
      _ = modelBuilder.ApplyConfiguration(new SinistradoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new SinistradoLinhaServicoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new EstadoSinistroItemConfiguration());

      // LoteDirect configurations
      _ = modelBuilder.ApplyConfiguration(new LoteDirectConfiguration());
      _ = modelBuilder.ApplyConfiguration(new TipoLoteConfiguration());

      // LoteDirectLinha configurations
      _ = modelBuilder.ApplyConfiguration(new LoteDirectLinhaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new LoteDirectLinha789Configuration());
      _ = modelBuilder.ApplyConfiguration(new LoteDirectAgregadoConfiguration());
      _ = modelBuilder.ApplyConfiguration(new LoteDirectDetalheConfiguration());

      // ListaEsperaConsulta configurations
      _ = modelBuilder.ApplyConfiguration(new ListaEsperaConsultaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new PedidoConsultaConfiguration());
      _ = modelBuilder.ApplyConfiguration(new PedidoConsultaUtenteConfiguration());

      // Odontologia - chaves alternativas e FKs por código
      modelBuilder.Entity<EstadosDentarios>(b =>
      {
        b.HasAlternateKey(e => e.Codigo);
      });

      modelBuilder.Entity<TipoTratamentoDentario>(b =>
      {
        b.HasAlternateKey(t => t.Codigo);
      });

      modelBuilder.Entity<OdontogramaDefinitivo>(b =>
      {
        b.HasOne(o => o.EstadoPadrao)
          .WithMany()
          .HasForeignKey(o => o.CodigoEstadoPadrao)
          .HasPrincipalKey(e => e.Codigo);

        b.HasOne(o => o.TiposTratamentoPadrao)
          .WithMany()
          .HasForeignKey(o => o.CodigoTratamentoPadrao)
          .HasPrincipalKey(t => t.Codigo);
      });

      modelBuilder.SeedStaticData();

    }

    public override async Task<int> SaveChangesAsync(
      CancellationToken cancellationToken = new CancellationToken()
    )
    {
      this.AuditFields(CurrentUserId);
      int result = await base.SaveChangesAsync(cancellationToken);
      return result;
    }
  }
}
