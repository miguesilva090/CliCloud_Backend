using AutoMapper;
using CliCloud.Domain.Entities.Atestados;
using CliCloud.Domain.Entities.CartaConducao;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Exames;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Core.Email;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Domain.Entities.Servicos;
using CliCloud.Domain.Entities.Seguradoras;
using CliCloud.Domain.Entities.Bancos;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Domain.Entities.Alergias;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Entities.CentroSaude;
using CliCloud.Domain.Entities.Funcionarios;
using CliCloud.Domain.Entities.Fornecedores;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Entities.UnidadesLocaisSaude;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Domain.Entities.TipoEntidadeFinanceira;
using CliCloud.Domain.Entities.EntidadesFinanceiras;
using CliCloud.Domain.Entities.Doencas;
using CliCloud.Domain.Entities.ConfiguracaoADSE;
using CliCloud.Domain.Entities.Empresas;
using CliCloud.Domain.Enums;
using CliCloud.Domain.Entities.RegioesCorpo;
using CliCloud.Domain.Entities.ProcessoClinico.HistoriaClinica;
using CliCloud.Domain.Entities.Antecedentes;
using CliCloud.Domain.Entities.ProcessoClinico;
using CliCloud.Domain.Entities.ProcessoClinico.SinaisVitais;
using CliCloud.Domain.Entities.ProcessoClinico.RelatorioExames;
using CliCloud.Application.Common;
using System.Globalization;
using DoencaDtos = CliCloud.Application.Services.Doencas.DoencaService.DTOs;
using ConfiguracaoADSEDtos = CliCloud.Application.Services.Faturacao.ConfiguracaoADSEService.DTOs;
using TipoConsultaDtos = CliCloud.Application.Services.TiposConsulta.TipoConsultaService.DTOs;
using CliCloud.Domain.Entities.ProcessoClinico.Estomatologia;
using CliCloud.Domain.Entities.ProcessoClinico.Odontologia;
using CliCloud.Domain.Entities.Notificacoes;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Sinistros;
using CliCloud.Domain.Entities.Credenciais;

using TipoAparelhoDtos = CliCloud.Application.Services.Tratamentos.TipoAparelhoService.DTOs;
using MarcaAparelhoDtos = CliCloud.Application.Services.Tratamentos.MarcaAparelhoService.DTOs;
using ModeloAparelhoDtos = CliCloud.Application.Services.Tratamentos.ModeloAparelhoService.DTOs;
using LocalTratamentoDtos = CliCloud.Application.Services.Tratamentos.LocalTratamentoService.DTOs;
using EstadoListaEsperaDtos = CliCloud.Application.Services.Tratamentos.EstadoListaEsperaService.DTOs;
using PrioridadeDtos = CliCloud.Application.Services.Tratamentos.PrioridadeService.DTOs;
using ListaEsperaTratamentoAdministrativoDtos =
  CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService.DTOs;
using PatologiaDtos = CliCloud.Application.Services.Tratamentos.PatologiaService.DTOs;
using ExameDtos = CliCloud.Application.Services.Exames.ExameService.DTOs;
using TipoExameDtos = CliCloud.Application.Services.Exames.TipoExameService.DTOs;
using AcordosDtos = CliCloud.Application.Services.Exames.AcordosService.DTOs;
using CategoriaProcedimentoDtos = CliCloud.Application.Services.Exames.CategoriaProcedimentoService.DTOs;
using AnalisesDtos = CliCloud.Application.Services.Exames.AnalisesService.DTOs;
using ConsultaDtos = CliCloud.Application.Services.Consultas.ConsultaService.DTOs;
using HistoricoConsultaAdministrativoDtos =
  CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService.DTOs;
using MarcacaoConsultaDtos = CliCloud.Application.Services.Consultas.MarcacaoConsultaService.DTOs;
using ServicoConsultaDtos = CliCloud.Application.Services.Consultas.ServicoConsultaService.DTOs;
using TipoServicoDtos = CliCloud.Application.Services.Servicos.TipoServicoService.DTOs;
using ServicoDtos = CliCloud.Application.Services.Servicos.ServicoService.DTOs;
using SubsistemaServicoDtos = CliCloud.Application.Services.Servicos.SubsistemaServicoService.DTOs;
using TratamentoDtos = CliCloud.Application.Services.Tratamentos.TratamentoService.DTOs;
using SessaoTratamentoDtos = CliCloud.Application.Services.Tratamentos.SessaoTratamentoService.DTOs;
using ServicoTratamentoDtos = CliCloud.Application.Services.Tratamentos.ServicoTratamentoService.DTOs;
using ServicoSessaoDtos = CliCloud.Application.Services.Tratamentos.ServicoSessaoService.DTOs;
using AparelhoDtos = CliCloud.Application.Services.Tratamentos.AparelhoService.DTOs;
using ClinicaDtos = CliCloud.Application.Services.Core.ClinicaService.DTOs;
using SeguradoraDtos = CliCloud.Application.Services.Seguradoras.SeguradoraService.DTOs;
using ReciboDtos = CliCloud.Application.Services.Documentos.ReciboService.DTOs;
using DocumentoDtos = CliCloud.Application.Services.Documentos.DocumentoService.DTOs;
using TipoDocumentoDtos = CliCloud.Application.Services.Documentos.TipoDocumentoService.DTOs;
using NaturezaDocumentoDtos = CliCloud.Application.Services.Documentos.NaturezaDocumentoService.DTOs;
using UtenteDtos = CliCloud.Application.Services.Utentes.UtenteService.DTOs;
using MedicoDtos = CliCloud.Application.Services.Medicos.MedicoService.DTOs;
using MedicoExternoDtos = CliCloud.Application.Services.Medicos.MedicoExternoService.DTOs;
using HorarioMedicoDtos = CliCloud.Application.Services.Medicos.HorarioMedicoService.DTOs;
using HorarioMedicoDiaDtos = CliCloud.Application.Services.Medicos.HorarioMedicoDiaService.DTOs;
using HorarioMedicoVariavelDtos = CliCloud.Application.Services.Medicos.HorarioMedicoVariavelService.DTOs;
using FolgasMedicoDtos = CliCloud.Application.Services.Medicos.FolgasMedicoService.DTOs;
using AlergiaDtos = CliCloud.Application.Services.Alergias.AlergiaService.DTOs;
using GrauAlergiaDtos = CliCloud.Application.Services.GrauAlergiaService.DTOs;
using AlergiaUtenteDtos = CliCloud.Application.Services.AlergiaUtenteService.DTOs;
using AlergiasUtenteObsDtos = CliCloud.Application.Services.AlergiasUtenteObsService.DTOs;
using MargemMedicoDtos = CliCloud.Application.Services.Medicos.MargemMedicoService.DTOs;
using TecnicoDtos = CliCloud.Application.Services.Tecnicos.TecnicoService.DTOs;
using ViaAdministracaoDtos = CliCloud.Application.Services.Artigos.ViaAdministracaoService.DTOs;
using GrupoViasAdministracaoDtos = CliCloud.Application.Services.Artigos.GrupoViasAdministracaoService.DTOs;
using HorarioTecnicoDiaDtos = CliCloud.Application.Services.Tecnicos.HorarioTecnicoDiaService.DTOs;
using FuncionarioDtos = CliCloud.Application.Services.Funcionarios.FuncionarioService.DTOs;
using EntidadePessoaDtos = CliCloud.Application.Services.Utility.EntidadePessoaService.DTOs;
using RuaDtos = CliCloud.Application.Services.Utility.RuaService.DTOs;
using FreguesiaDtos = CliCloud.Application.Services.Utility.FreguesiaService.DTOs;
using CodigoPostalDtos = CliCloud.Application.Services.Utility.CodigoPostalService.DTOs;
using DistritoDtos = CliCloud.Application.Services.Utility.DistritoService.DTOs;
using PaisDtos = CliCloud.Application.Services.Utility.PaisService.DTOs;
using FeriadoDtos = CliCloud.Application.Services.Utility.FeriadoService.DTOs;
using ConcelhoDtos = CliCloud.Application.Services.Utility.ConcelhoService.DTOs;
using EntidadeContactoDtos = CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using EntidadeDtos = CliCloud.Application.Services.Utility.EntidadeService.DTOs;
using EntidadeFinanceiraDtos = CliCloud.Application.Services.EntidadesFinanceiras.EntidadeFinanceiraService.DTOs;
using CartaConducaoDtos = CliCloud.Application.Services.CartasConducao.CartaConducaoService.DTOs;
using CartaConducaoRestricoesDtos = CliCloud.Application.Services.CartasConducao.CartaConducaoRestricoesService.DTOs;
using AtestadoDtos = CliCloud.Application.Services.Atestados.AtestadoService.DTOs;
using TipoEntidadeFinanceiraDtos = CliCloud.Application.Services.TipoEntidadeFinanceira.TipoEntidadeFinanceiraService.DTOs;
using BancoDtos = CliCloud.Application.Services.Bancos.BancoService.DTOs;
using CategoriaEspecialidadeDtos = CliCloud.Application.Services.Especialidades.CategoriaEspecialidadeService.DTOs;
using EspecialidadeDtos = CliCloud.Application.Services.Especialidades.EspecialidadeService.DTOs;
using EstadoCivilDtos = CliCloud.Application.Services.EstadosCivis.EstadoCivilService.DTOs;
using GrupoSanguineoDtos = CliCloud.Application.Services.Utility.GrupoSanguineoService.DTOs;
using HabilitacaoDtos = CliCloud.Application.Services.Habilitacoes.HabilitacaoService.DTOs;
using SexoDtos = CliCloud.Application.Services.Sexos.SexoService.DTOs;
using GrauParentescoDtos = CliCloud.Application.Services.GrausParentesco.GrauParentescoService.DTOs;
using TaxaIvaDtos = CliCloud.Application.Services.TaxasIva.TaxaIvaService.DTOs;
using MotivoIsencaoDtos = CliCloud.Application.Services.TaxasIva.MotivoIsencaoService.DTOs;
using MotivoRetencaoDtos = CliCloud.Application.Services.TaxasIva.MotivoRetencaoService.DTOs;
using CondicaoPagamentoDtos = CliCloud.Application.Services.Pagamentos.CondicaoPagamentoService.DTOs;
using TipoPagamentoDtos = CliCloud.Application.Services.Pagamentos.TipoPagamentoService.DTOs;
using ModoPagamentoDtos = CliCloud.Application.Services.Pagamentos.ModoPagamentoService.DTOs;
using ArmazemDtos = CliCloud.Application.Services.Stocks.ArmazemService.DTOs;
using UnidadeMedidaDtos = CliCloud.Application.Services.Stocks.UnidadeMedidaService.DTOs;
using ZonaComercialDtos = CliCloud.Application.Services.Faturacao.ZonaComercialService.DTOs;
using ArtigoDtos = CliCloud.Application.Services.Stocks.ArtigoService.DTOs;
using FamiliaArtigoDtos = CliCloud.Application.Services.Stocks.FamiliaArtigoService.DTOs;
using CondicaoPagamentoEntity = CliCloud.Domain.Entities.Pagamentos.CondicaoPagamento;
using TipoPagamentoEntity = CliCloud.Domain.Entities.Pagamentos.TipoPagamento;
using ModoPagamentoEntity = CliCloud.Domain.Entities.Pagamentos.ModoPagamento;
using ArmazemEntity = CliCloud.Domain.Entities.Stocks.Armazem;
using UnidadeMedidaEntity = CliCloud.Domain.Entities.Stocks.UnidadeMedida;
using ZonaComercialEntity = CliCloud.Domain.Entities.Faturacao.ZonaComercial;
using ArtigoEntity = CliCloud.Domain.Entities.Stocks.Artigo;
using FamiliaArtigoEntity = CliCloud.Domain.Entities.Stocks.FamiliaArtigo;
using SubsistemaArtigoDtos = CliCloud.Application.Services.Stocks.SubsistemaArtigoService.DTOs;
using SubsistemaArtigoEntity = CliCloud.Domain.Entities.Stocks.SubsistemaArtigo;
using ProvenienciaUtenteDtos = CliCloud.Application.Services.ProvenienciasUtente.ProvenienciaUtenteService.DTOs;
using ProfissaoDtos = CliCloud.Application.Services.Profissoes.ProfissaoService.DTOs;
using MoedaDtos = CliCloud.Application.Services.Moedas.MoedaService.DTOs;
using OrganismoDtos = CliCloud.Application.Services.Organismos.OrganismoService.DTOs;
using CentroSaudeDtos = CliCloud.Application.Services.CentroSaude.CentroSaudeService.DTOs;
using UnidadesLocaisSaudeDtos = CliCloud.Application.Services.UnidadesLocaisSaude.UnidadesLocaisSaudeService.DTOs;
using FornecedorDtos = CliCloud.Application.Services.Fornecedores.FornecedorService.DTOs;
using EmpresaDtos = CliCloud.Application.Services.Empresas.EmpresaService.DTOs;
using RegiaoCorpoDtos = CliCloud.Application.Services.RegioesCorpo.RegiaoCorpoService.DTOs;
using GoniometriasDtos = CliCloud.Application.Services.Tratamentos.GoniometriasService.DTOs;
using EstadosDentariosDtos = CliCloud.Application.Services.ProcessoClinico.Odontologia.EstadosDentariosService.DTOs;
using OdontogramaDefinitivoDtos = CliCloud.Application.Services.ProcessoClinico.Odontologia.OdontogramaDefinitivoService.DTOs;
using TiposTratamentoDentarioDtos = CliCloud.Application.Services.ProcessoClinico.Odontologia.TiposTratamentoDentarioService.DTOs;
using FraquezasMuscularesDtos = CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService.DTOs;
using MotivoAltaDtos = CliCloud.Application.Services.Tratamentos.MotivoAltaService.DTOs;
using MotivosDesmarcacaoDtos = CliCloud.Application.Services.Tratamentos.MotivosDesmarcacaoService.DTOs;
using AntecedentesPessoaisDtos = CliCloud.Application.Services.Antecedentes.AntecedentesPessoaisService.DTOs;
using AntecedentesFamiliaresUtenteDtos = CliCloud.Application.Services.Antecedentes.AntecedentesFamiliaresUtenteService.DTOs;
using AntecedentesCirurgicosDtos = CliCloud.Application.Services.Antecedentes.AntecedentesCirurgicosService.DTOs;
using QuestionarioUtenteDtos = CliCloud.Application.Services.ProcessoClinico.QuestionarioUtenteService.DTOs;
using HabitosEViciosDtos = CliCloud.Application.Services.ProcessoClinico.HabitosEViciosService.DTOs;
using TensaoArterialDtos = CliCloud.Application.Services.ProcessoClinico.TensaoArterialService.DTOs;
using GlicemiaCapilarDtos = CliCloud.Application.Services.GlicemiaCapilarService.DTOs;
using TemperaturaCorporalDtos = CliCloud.Application.Services.TemperaturaCorporalService.DTOs;
using IndiceMassaCorporalDtos = CliCloud.Application.Services.IndiceMassaCorporalService.DTOs;
using HistoriaClinicaDtos = CliCloud.Application.Services.ProcessoClinico.HistoriaClinicaService.DTOs;
using GorduraMassaMuscularDtos = CliCloud.Application.Services.ProcessoClinico.GorduraMassaMuscularService.DTOs;
using AvaliacaoAntropometricaDtos = CliCloud.Application.Services.ProcessoClinico.AvaliacaoAntropometricaService.DTOs;
using AvaliacaoPosturalDtos = CliCloud.Application.Services.ProcessoClinico.AvaliacaoPosturalService.DTOs;
using PeriocidadeTratamentoDtos = CliCloud.Application.Services.Tratamentos.PeriocidadeTratamentoService.DTOs;
using EvolucaoTratamentoDtos = CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoService.DTOs;
using EvolucaoTratamentoFicheiroDtos = CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoFicheiroService.DTOs;
using MapaBodyChartDtos = CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService.DTOs;
using NotasBodyChartDtos = CliCloud.Application.Services.ProcessoClinico.NotasBodyChartService.DTOs;
using RelatorioExamesDtos = CliCloud.Application.Services.ProcessoClinico.RelatorioExamesService.DTOs;
using RelatorioAtestadoDtos = CliCloud.Application.Services.ProcessoClinico.RelatorioAtestadoService.DTOs;
using ModeloRelatorioAtestadoDtos = CliCloud.Application.Services.ProcessoClinico.ModeloRelatorioAtestadoService.DTOs;
using DocumentosFichaClinicaDtos = CliCloud.Application.Services.ProcessoClinico.DocumentosFichaClinicaService.DTOs;
using RelatorioAtestado = CliCloud.Domain.Entities.ProcessoClinico.RelatorioAtestado.RelatorioAtestado;
using ModeloRelatorioAtestado = CliCloud.Domain.Entities.ProcessoClinico.RelatorioAtestado.ModeloRelatorioAtestado;
using DocumentosFichaClinica = CliCloud.Domain.Entities.ProcessoClinico.Documentos.DocumentosFichaClinica;
using FichaClinicaSecaoConteudo = CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados.FichaClinicaSecaoConteudo;
using FichaClinicaSecaoTemplate = CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados.FichaClinicaSecaoTemplate;
using FichaClinicaSecaoCampo = CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados.FichaClinicaSecaoCampo;
using Separador = CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados.Separador;
using SeparadorVinculo = CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados.SeparadorVinculo;
using SeparadorPersonalizado = CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados.SeparadorPersonalizado;
using SeparadorPersonalizadoVinculo = CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados.SeparadorPersonalizadoVinculo;
using FichaClinicaSecaoConteudoDtos = CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoConteudoService.DTOs;
using FichaClinicaSecaoTemplateDtos = CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoTemplateService.DTOs;
using FichaClinicaSecaoCampoDtos = CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoCampoService.DTOs;
using SeparadorDtos = CliCloud.Application.Services.ProcessoClinico.SeparadorService.DTOs;
using SeparadorVinculoDtos = CliCloud.Application.Services.ProcessoClinico.SeparadorVinculoService.DTOs;
using SeparadorPersonalizadoDtos = CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoService.DTOs;
using SeparadorPersonalizadoVinculoDtos = CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoVinculoService.DTOs;
using AnamneseOdontopediatriaDtos = CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOdontopediatriaService.DTOs;
using AnamneseOrtodonticaAnaliseGeralDtos = CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseGeralService.DTOs;
using AnamneseOrtodonticaAnaliseDentariaDtos = CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseDentariaService.DTOs;
using AnamneseOrtodonticaDenticaoDeciduaeMistaDtos = CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaDenticaoDeciduaeMistaService.DTOs;
using AnamneseOrtodonticaATMDtos = CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaATMService.DTOs;
using AnamneseOrtodonticaAnaliseFuncionalDtos = CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseFuncionalService.DTOs;
using HistoriaDentariaDtos = CliCloud.Application.Services.ProcessoClinico.Estomatologia.HistoriaDentariaService.DTOs;
using SmsDtos = CliCloud.Application.Services.Core.SmsService.DTOs;
using NotificacaoTipoDtos = CliCloud.Application.Services.Notificacoes.NotificacaoTipoService.DTOs;
using NotificacaoDtos = CliCloud.Application.Services.Notificacoes.NotificacaoService.DTOs;
using ContaBancariaDtos = CliCloud.Application.Services.Bancos.ContaBancariaService.DTOs;
using UtentePatologiaComparticipacaoDtos = CliCloud.Application.Services.Utentes.UtentePatologiaComparticipacaoService.DTOs;



namespace CliCloud.Infrastructure.Mapper
{
  public class MappingProfiles : Profile
  {
    public MappingProfiles()
    {
      // ---- SMS ----
      _ = CreateMap<CliCloud.Domain.Entities.Core.Sms.HistoricoSms, SmsDtos.HistoricoSmsTabelaDTO>();

      // ---- Rua ----
      _ = CreateMap<Rua, RuaDtos.RuaDTO>();
      _ = CreateMap<Rua, RuaDtos.RuaLightDTO>();
      _ = CreateMap<Rua, RuaDtos.RuaTableDTO>();
      // Nested DTOs para RuaTableDTO (listagem)
      _ = CreateMap<Freguesia, RuaDtos.RuaTableFreguesiaDTO>();
      _ = CreateMap<Concelho, RuaDtos.RuaTableConcelhoDTO>();
      _ = CreateMap<Distrito, RuaDtos.RuaTableDistritoDTO>();
      _ = CreateMap<Pais, RuaDtos.RuaTablePaisDTO>();
      _ = CreateMap<CodigoPostal, RuaDtos.RuaTableCodigoPostalDTO>();
      _ = CreateMap<RuaDtos.CreateRuaRequest, Rua>();
      _ = CreateMap<RuaDtos.UpdateRuaRequest, Rua>();
      // Mapeamentos usados em DTOs "table" de Entidade/Utente (nested DTOs)
      _ = CreateMap<Rua, EntidadeDtos.EntidadeTableRuaDTO>();

      // ---- Freguesia ----
      _ = CreateMap<Freguesia, FreguesiaDtos.FreguesiaDTO>();
      _ = CreateMap<Freguesia, FreguesiaDtos.FreguesiaLightDTO>();
      _ = CreateMap<Freguesia, FreguesiaDtos.FreguesiaTableDTO>();
      _ = CreateMap<FreguesiaDtos.CreateFreguesiaRequest, Freguesia>();
      _ = CreateMap<FreguesiaDtos.UpdateFreguesiaRequest, Freguesia>();
      _ = CreateMap<Freguesia, EntidadeDtos.EntidadeTableFreguesiaDTO>();
      // Nested DTOs para FreguesiaTableDTO (listagem)
      _ = CreateMap<Concelho, FreguesiaDtos.FreguesiaTableConcelhoDTO>();
      _ = CreateMap<Distrito, FreguesiaDtos.FreguesiaTableDistritoDTO>();
      _ = CreateMap<Pais, FreguesiaDtos.FreguesiaTablePaisDTO>();

      // ---- CodigoPostal ----
      _ = CreateMap<CodigoPostal, CodigoPostalDtos.CodigoPostalDTO>();
      _ = CreateMap<CodigoPostal, CodigoPostalDtos.CodigoPostalLightDTO>();
      _ = CreateMap<CodigoPostal, CodigoPostalDtos.CodigoPostalTableDTO>();
      _ = CreateMap<CodigoPostalDtos.CreateCodigoPostalRequest, CodigoPostal>();
      _ = CreateMap<CodigoPostalDtos.UpdateCodigoPostalRequest, CodigoPostal>();
      _ = CreateMap<CodigoPostal, EntidadeDtos.EntidadeTableCodigoPostalDTO>();

      // ---- Distrito ----
      _ = CreateMap<Distrito, DistritoDtos.DistritoDTO>();
      _ = CreateMap<Distrito, DistritoDtos.DistritoLightDTO>();
      _ = CreateMap<Distrito, DistritoDtos.DistritoTableDTO>();
      _ = CreateMap<DistritoDtos.CreateDistritoRequest, Distrito>();
      _ = CreateMap<DistritoDtos.UpdateDistritoRequest, Distrito>();
      _ = CreateMap<Distrito, EntidadeDtos.EntidadeTableDistritoDTO>();
      // Nested DTO para DistritoTableDTO.Pais
      _ = CreateMap<Pais, DistritoDtos.DistritoTablePaisDTO>();

      // ---- Pais ----
      _ = CreateMap<Pais, PaisDtos.PaisDTO>();
      _ = CreateMap<Pais, PaisDtos.PaisLightDTO>();
      _ = CreateMap<Pais, PaisDtos.PaisTableDTO>();
      _ = CreateMap<PaisDtos.CreatePaisRequest, Pais>();
      _ = CreateMap<PaisDtos.UpdatePaisRequest, Pais>();
      _ = CreateMap<Pais, EntidadeDtos.EntidadeTablePaisDTO>();

      // ---- Feriado ----
      _ = CreateMap<Feriado, FeriadoDtos.FeriadoDTO>();
      _ = CreateMap<FeriadoDtos.CreateFeriadoRequest, Feriado>();
      _ = CreateMap<FeriadoDtos.UpdateFeriadoRequest, Feriado>();

      // ---- Concelho ----
      _ = CreateMap<Concelho, ConcelhoDtos.ConcelhoDTO>();
      _ = CreateMap<Concelho, ConcelhoDtos.ConcelhoLightDTO>();
      _ = CreateMap<Concelho, ConcelhoDtos.ConcelhoTableDTO>();
      _ = CreateMap<ConcelhoDtos.CreateConcelhoRequest, Concelho>();
      _ = CreateMap<ConcelhoDtos.UpdateConcelhoRequest, Concelho>();
      _ = CreateMap<Concelho, EntidadeDtos.EntidadeTableConcelhoDTO>();
      // Nested DTOs para ConcelhoTableDTO (listagem)
      _ = CreateMap<Distrito, ConcelhoDtos.ConcelhoTableDistritoDTO>();
      _ = CreateMap<Pais, ConcelhoDtos.ConcelhoTablePaisDTO>();

      // ---- EntidadeContacto ----
      _ = CreateMap<EntidadeContacto, EntidadeContactoDtos.EntidadeContactoDTO>();
      _ = CreateMap<EntidadeContactoDtos.CreateEntidadeContactoRequest, EntidadeContacto>();
      _ = CreateMap<EntidadeContactoDtos.UpdateEntidadeContactoRequest, EntidadeContacto>();

      // ---- TipoEntidadeFinanceira ----
      _ = CreateMap<TipoEntidadeFinanceira, TipoEntidadeFinanceiraDtos.TipoEntidadeFinanceiraDTO>();
      _ = CreateMap<TipoEntidadeFinanceira, TipoEntidadeFinanceiraDtos.TipoEntidadeFinanceiraLightDTO>();
      _ = CreateMap<TipoEntidadeFinanceira, TipoEntidadeFinanceiraDtos.TipoEntidadeFinanceiraTableDTO>();
      _ = CreateMap<TipoEntidadeFinanceiraDtos.CreateTipoEntidadeFinanceiraRequest, TipoEntidadeFinanceira>();
      _ = CreateMap<TipoEntidadeFinanceiraDtos.UpdateTipoEntidadeFinanceiraRequest, TipoEntidadeFinanceira>();

      // ---- EntidadeFinanceira ----
      _ = CreateMap<EntidadeFinanceira, EntidadeFinanceiraDtos.EntidadeFinanceiraDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => (int?)s.Status));
      _ = CreateMap<EntidadeFinanceira, EntidadeFinanceiraDtos.EntidadeFinanceiraLightDTO>();
      _ = CreateMap<EntidadeFinanceira, EntidadeFinanceiraDtos.EntidadeFinanceiraTableDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => (int?)s.Status));
      _ = CreateMap<EntidadeFinanceiraDtos.CreateEntidadeFinanceiraRequest, EntidadeFinanceira>();
      _ = CreateMap<EntidadeFinanceiraDtos.UpdateEntidadeFinanceiraRequest, EntidadeFinanceira>();

      // ---- TipoAparelho ----
      _ = CreateMap<TipoAparelho, TipoAparelhoDtos.TipoAparelhoDTO>();
      _ = CreateMap<TipoAparelho, TipoAparelhoDtos.TipoAparelhoLightDTO>();
      _ = CreateMap<TipoAparelho, TipoAparelhoDtos.TipoAparelhoTableDTO>();
      _ = CreateMap<TipoAparelhoDtos.CreateTipoAparelhoRequest, TipoAparelho>();
      _ = CreateMap<TipoAparelhoDtos.UpdateTipoAparelhoRequest, TipoAparelho>();

      // ---- MarcaAparelho ----
      _ = CreateMap<MarcaAparelho, MarcaAparelhoDtos.MarcaAparelhoDTO>();
      _ = CreateMap<MarcaAparelho, MarcaAparelhoDtos.MarcaAparelhoLightDTO>();
      _ = CreateMap<MarcaAparelho, MarcaAparelhoDtos.MarcaAparelhoTableDTO>();
      _ = CreateMap<MarcaAparelhoDtos.CreateMarcaAparelhoRequest, MarcaAparelho>();
      _ = CreateMap<MarcaAparelhoDtos.UpdateMarcaAparelhoRequest, MarcaAparelho>();

      // ---- ModeloAparelho ----
      _ = CreateMap<ModeloAparelho, ModeloAparelhoDtos.ModeloAparelhoDTO>()
        .ForMember(d => d.MarcaAparelhoDesignacao, o => o.MapFrom(s => s.MarcaAparelho != null ? s.MarcaAparelho.Designacao : null));
      _ = CreateMap<ModeloAparelho, ModeloAparelhoDtos.ModeloAparelhoLightDTO>()
        .ForMember(d => d.MarcaAparelhoDesignacao, o => o.MapFrom(s => s.MarcaAparelho != null ? s.MarcaAparelho.Designacao : null));
      _ = CreateMap<ModeloAparelho, ModeloAparelhoDtos.ModeloAparelhoTableDTO>()
        .ForMember(d => d.MarcaAparelhoDesignacao, o => o.MapFrom(s => s.MarcaAparelho != null ? s.MarcaAparelho.Designacao : null));
      _ = CreateMap<ModeloAparelhoDtos.CreateModeloAparelhoRequest, ModeloAparelho>().ForMember(d => d.MarcaAparelho, o => o.Ignore());
      _ = CreateMap<ModeloAparelhoDtos.UpdateModeloAparelhoRequest, ModeloAparelho>().ForMember(d => d.MarcaAparelho, o => o.Ignore());

      // ---- LocalTratamento ----
      _ = CreateMap<LocalTratamento, LocalTratamentoDtos.LocalTratamentoDTO>();
      _ = CreateMap<LocalTratamento, LocalTratamentoDtos.LocalTratamentoLightDTO>();
      _ = CreateMap<LocalTratamento, LocalTratamentoDtos.LocalTratamentoTableDTO>();
      _ = CreateMap<LocalTratamentoDtos.CreateLocalTratamentoRequest, LocalTratamento>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<LocalTratamentoDtos.UpdateLocalTratamentoRequest, LocalTratamento>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- EstadoListaEspera ----
      _ = CreateMap<EstadoListaEspera, EstadoListaEsperaDtos.EstadoListaEsperaDTO>();
      _ = CreateMap<EstadoListaEspera, EstadoListaEsperaDtos.EstadoListaEsperaLightDTO>();
      _ = CreateMap<EstadoListaEspera, EstadoListaEsperaDtos.EstadoListaEsperaTableDTO>();
      _ = CreateMap<EstadoListaEsperaDtos.CreateEstadoListaEsperaRequest, EstadoListaEspera>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<EstadoListaEsperaDtos.UpdateEstadoListaEsperaRequest, EstadoListaEspera>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- Prioridade ----
      _ = CreateMap<Prioridade, PrioridadeDtos.PrioridadeDTO>();
      _ = CreateMap<Prioridade, PrioridadeDtos.PrioridadeLightDTO>();
      _ = CreateMap<Prioridade, PrioridadeDtos.PrioridadeTableDTO>();
      _ = CreateMap<PrioridadeDtos.CreatePrioridadeRequest, Prioridade>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<PrioridadeDtos.UpdatePrioridadeRequest, Prioridade>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- ListaEsperaTratamento (administrativo) ----
      _ = CreateMap<ListaEsperaTratamento, ListaEsperaTratamentoAdministrativoDtos.ListaEsperaTratamentoDTO>()
        .ForMember(d => d.CodigoLegado, o => o.MapFrom(s => s.CodigoLegado ?? 0))
        .ForMember(d => d.UtenteNome, o => o.MapFrom(s => s.Utente != null ? s.Utente.Nome : null))
        .ForMember(d => d.MedicoNome, o => o.MapFrom(s => s.Medico != null ? s.Medico.Nome : null))
        .ForMember(d => d.OrganismoNome, o => o.MapFrom(s => s.Organismo != null ? s.Organismo.Nome : null))
        .ForMember(d => d.PrioridadeDesignacao, o => o.MapFrom(s => s.Prioridade != null ? s.Prioridade.Descricao : null))
        .ForMember(d => d.EstadoDesignacao, o => o.MapFrom(s => s.EstadoListaEspera != null ? s.EstadoListaEspera.Descricao : null))
        .ForMember(d => d.LocalTratamentoDesignacao, o => o.MapFrom(s => s.LocalTratamento != null ? s.LocalTratamento.Designacao : null))
        .ForMember(d => d.PatologiaDesignacao, o => o.MapFrom(s => s.Patologia != null ? s.Patologia.Designacao : null))
        .ForMember(d => d.Servicos, o => o.Ignore());

      _ = CreateMap<ListaEsperaTratamento, ListaEsperaTratamentoAdministrativoDtos.ListaEsperaTratamentoTableDTO>()
        .ForMember(d => d.UtenteNome, o => o.MapFrom(s => s.Utente != null ? s.Utente.Nome : null))
        .ForMember(d => d.OrganismoNome, o => o.MapFrom(s => s.Organismo != null ? s.Organismo.Nome : null))
        .ForMember(d => d.PrioridadeDesignacao, o => o.MapFrom(s => s.Prioridade != null ? s.Prioridade.Descricao : null))
        .ForMember(d => d.EstadoDesignacao, o => o.MapFrom(s => s.EstadoListaEspera != null ? s.EstadoListaEspera.Descricao : null))
        .ForMember(d => d.LocalTratamentoDesignacao, o => o.MapFrom(s => s.LocalTratamento != null ? s.LocalTratamento.Designacao : null));

      _ = CreateMap<ListaEsperaTratamentoAdministrativoDtos.CreateListaEsperaTratamentoRequest, ListaEsperaTratamento>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.Ordem, o => o.Ignore())
        .ForMember(d => d.OrdemOrigem, o => o.Ignore())
        .ForMember(d => d.DataEntrada, o => o.Ignore())
        .ForMember(d => d.Historico, o => o.Ignore())
        .ForMember(d => d.Obs, o => o.Ignore())
        .ForMember(d => d.CodigoLegado, o => o.Ignore())
        .ForMember(d => d.Servicos, o => o.Ignore());

      _ = CreateMap<ListaEsperaTratamentoServico, ListaEsperaTratamentoAdministrativoDtos.ListaEsperaTratamentoServicoDTO>()
        .ForMember(d => d.Designacao, o => o.MapFrom(s => s.Designacao ?? (s.Servico != null ? s.Servico.Designacao : null)));

      _ = CreateMap<ListaEsperaTratamentoAdministrativoDtos.UpdateListaEsperaTratamentoRequest, ListaEsperaTratamento>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.UtenteId, o => o.Ignore())
        .ForMember(d => d.Ordem, o => o.Ignore())
        .ForMember(d => d.OrdemOrigem, o => o.Ignore())
        .ForMember(d => d.DataEntrada, o => o.Ignore())
        .ForMember(d => d.Historico, o => o.Ignore())
        .ForMember(d => d.Obs, o => o.Ignore())
        .ForMember(d => d.CodigoLegado, o => o.Ignore())
        .ForMember(d => d.Servicos, o => o.Ignore());

      // ---- HistoriaClinica ----
      _ = CreateMap<HistoriaClinica, HistoriaClinicaDtos.HistoriaClinicaDTO>();
      _ = CreateMap<HistoriaClinica, HistoriaClinicaDtos.HistoriaClinicaTableDTO>()
        .ForMember(d => d.UtenteNome, o => o.MapFrom(s => s.Utente != null ? s.Utente.Nome : string.Empty))
        .ForMember(d => d.MedicoNome, o => o.MapFrom(s => s.Medico != null ? s.Medico.Nome : string.Empty))
        .ForMember(d => d.EspecialidadeNome, o => o.MapFrom(s => s.Especialidade != null ? s.Especialidade.Nome : null));
      _ = CreateMap<HistoriaClinicaDtos.HistoriaClinicaDTO, HistoriaClinica>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- Exame (prescrição de exames) ----
      _ = CreateMap<ExameLinha, ExameDtos.ExameLinhaDTO>()
        .ForMember(d => d.TipoExameDesignacao, o => o.MapFrom(s => s.TipoExame != null ? s.TipoExame.Designacao : null));
      _ = CreateMap<Exame, ExameDtos.ExameDTO>()
        .ForMember(d => d.Linhas, o => o.MapFrom(s => s.Linhas));
      _ = CreateMap<Exame, ExameDtos.ExameLightDTO>();
      _ = CreateMap<Exame, ExameDtos.ExameTableDTO>();
      _ = CreateMap<ExameDtos.CreateExameRequest, Exame>()
        .ForMember(d => d.Linhas, o => o.MapFrom(s => s.Linhas))
        .ForMember(d => d.Utente, o => o.Ignore())
        .ForMember(d => d.Medico, o => o.Ignore())
        .ForMember(d => d.Prioridade, o => o.Ignore())
        .ForMember(d => d.Organismo, o => o.Ignore());
      _ = CreateMap<ExameDtos.CreateExameLinhaRequest, ExameLinha>()
        .ForMember(d => d.ExameId, o => o.Ignore())
        .ForMember(d => d.Exame, o => o.Ignore())
        .ForMember(d => d.TipoExame, o => o.Ignore());
      _ = CreateMap<ExameDtos.UpdateExameRequest, Exame>()
        .ForMember(d => d.Linhas, o => o.Ignore())
        .ForMember(d => d.Utente, o => o.Ignore())
        .ForMember(d => d.Medico, o => o.Ignore())
        .ForMember(d => d.Prioridade, o => o.Ignore())
        .ForMember(d => d.Organismo, o => o.Ignore());

      // ---- FichaClinicaSecaoTemplate (Separadores) ----
      _ = CreateMap<FichaClinicaSecaoTemplate, FichaClinicaSecaoTemplateDtos.FichaClinicaSecaoTemplateDTO>();
      _ = CreateMap<FichaClinicaSecaoTemplateDtos.CreateFichaClinicaSecaoTemplateRequest, FichaClinicaSecaoTemplate>();
      _ = CreateMap<FichaClinicaSecaoTemplateDtos.UpdateFichaClinicaSecaoTemplateRequest, FichaClinicaSecaoTemplate>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.Codigo, o => o.Ignore());

      // ---- FichaClinicaSecaoCampo (Campos) ----
      _ = CreateMap<FichaClinicaSecaoCampo, FichaClinicaSecaoCampoDtos.FichaClinicaSecaoCampoDTO>()
        .ForMember(d => d.SeparadorNome, o => o.MapFrom(s => s.Separador != null ? s.Separador.Nome : string.Empty));
      _ = CreateMap<FichaClinicaSecaoCampoDtos.CreateFichaClinicaSecaoCampoRequest, FichaClinicaSecaoCampo>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.Separador, o => o.Ignore());
      _ = CreateMap<FichaClinicaSecaoCampoDtos.UpdateFichaClinicaSecaoCampoRequest, FichaClinicaSecaoCampo>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.SeparadorId, o => o.Ignore())
        .ForMember(d => d.Separador, o => o.Ignore());

      // ---- FichaClinicaSecaoConteudo (Conteúdo por utente/campo) ----
      _ = CreateMap<FichaClinicaSecaoConteudo, FichaClinicaSecaoConteudoDtos.FichaClinicaSecaoConteudoDTO>()
        .ForMember(d => d.CampoId, o => o.MapFrom(s => s.CampoId))
        .ForMember(d => d.CampoNome, o => o.MapFrom(s => s.Campo != null ? s.Campo.Nome : string.Empty))
        .ForMember(d => d.SeparadorId, o => o.MapFrom(s => s.Campo != null ? s.Campo.SeparadorId : Guid.Empty))
        .ForMember(d => d.SeparadorNome, o => o.MapFrom(s => s.Campo != null && s.Campo.Separador != null ? s.Campo.Separador.Nome : string.Empty));
      _ = CreateMap<FichaClinicaSecaoConteudoDtos.CreateFichaClinicaSecaoConteudoRequest, FichaClinicaSecaoConteudo>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.Campo, o => o.Ignore())
        .ForMember(d => d.Utente, o => o.Ignore());
      _ = CreateMap<FichaClinicaSecaoConteudoDtos.UpdateFichaClinicaSecaoConteudoRequest, FichaClinicaSecaoConteudo>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.CampoId, o => o.Ignore())
        .ForMember(d => d.Campo, o => o.Ignore())
        .ForMember(d => d.UtenteId, o => o.Ignore())
        .ForMember(d => d.Utente, o => o.Ignore());

      // ---- Separador ----
      _ = CreateMap<Separador, SeparadorDtos.SeparadorDTO>();
      _ = CreateMap<SeparadorDtos.CreateSeparadorRequest, Separador>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.Codigo, o => o.Ignore());
      _ = CreateMap<SeparadorDtos.UpdateSeparadorRequest, Separador>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.Codigo, o => o.Ignore());

      // ---- SeparadorVinculo ----
      _ = CreateMap<SeparadorVinculo, SeparadorVinculoDtos.SeparadorVinculoDTO>();

      // ---- SeparadorPersonalizado ----
      _ = CreateMap<SeparadorPersonalizado, SeparadorPersonalizadoDtos.SeparadorPersonalizadoDTO>();
      _ = CreateMap<SeparadorPersonalizadoDtos.CreateSeparadorPersonalizadoRequest, SeparadorPersonalizado>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.ClinicaId, o => o.Ignore())
        .ForMember(d => d.Formulario, o => o.Ignore());
      _ = CreateMap<SeparadorPersonalizadoDtos.UpdateSeparadorPersonalizadoRequest, SeparadorPersonalizado>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.ClinicaId, o => o.Ignore())
        .ForMember(d => d.Formulario, o => o.Ignore());

      // ---- SeparadorPersonalizadoVinculo ----
      _ = CreateMap<SeparadorPersonalizadoVinculo, SeparadorPersonalizadoVinculoDtos.SeparadorPersonalizadoVinculoDTO>();


      // ---- RelatorioExames ----
      _ = CreateMap<RelatorioExames, RelatorioExamesDtos.RelatorioExamesDTO>();

      // ---- HistoriaDentaria (Estomatologia) ----
      _ = CreateMap<HistoriaDentaria, HistoriaDentariaDtos.HistoriaDentariaDTO>();

      // ---- CategoriaProcedimento (Exames) ----
      _ = CreateMap<CategoriaProcedimento, CategoriaProcedimentoDtos.CategoriaProcedimentoTableDTO>();
      _ = CreateMap<CategoriaProcedimento, CategoriaProcedimentoDtos.CategoriaProcedimentoLightDTO>();
      _ = CreateMap<CategoriaProcedimento, CategoriaProcedimentoDtos.CategoriaProcedimentoDTO>();
      _ = CreateMap<CategoriaProcedimentoDtos.CreateCategoriaProcedimentoRequest, CategoriaProcedimento>();
      _ = CreateMap<CategoriaProcedimentoDtos.UpdateCategoriaProcedimentoRequest, CategoriaProcedimento>();

      // ---- Analises (Exames) ----
      _ = CreateMap<Analises, AnalisesDtos.AnaliseTableDTO>();
      _ = CreateMap<Analises, AnalisesDtos.AnaliseLightDTO>();
      _ = CreateMap<Analises, AnalisesDtos.AnaliseDTO>();
      _ = CreateMap<AnalisesDtos.CreateAnaliseRequest, Analises>();
      _ = CreateMap<AnalisesDtos.UpdateAnaliseRequest, Analises>();

      // ---- TipoExame ----
      _ = CreateMap<GrupoAnaliseLinha, TipoExameDtos.GrupoAnaliseLinhaDTO>()
        .ForMember(d => d.AnaliseNome, o => o.MapFrom(s => s.Analise != null ? s.Analise.Nome : null));
      _ = CreateMap<TipoExame, TipoExameDtos.TipoExameDTO>()
        .ForMember(d => d.GrupoAnaliseLinhas, o => o.MapFrom(s => s.GrupoAnaliseLinhas));
      _ = CreateMap<TipoExame, TipoExameDtos.TipoExameLightDTO>();
      _ = CreateMap<TipoExame, TipoExameDtos.TipoExameTableDTO>();
      _ = CreateMap<TipoExameDtos.CreateTipoExameRequest, TipoExame>()
        .ForMember(d => d.GrupoAnaliseLinhas, o => o.Ignore())
        .ForMember(d => d.CategoriaProcedimento, o => o.Ignore())
        .ForMember(d => d.TaxaIva, o => o.Ignore())
        .ForMember(d => d.MotivoIsencao, o => o.Ignore());
      _ = CreateMap<TipoExameDtos.UpdateTipoExameRequest, TipoExame>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.CreatedBy, o => o.Ignore())
        .ForMember(d => d.CreatedOn, o => o.Ignore())
        .ForMember(d => d.DeletedBy, o => o.Ignore())
        .ForMember(d => d.DeletedOn, o => o.Ignore())
        .ForMember(d => d.GrupoAnaliseLinhas, o => o.Ignore())
        .ForMember(d => d.CategoriaProcedimento, o => o.Ignore())
        .ForMember(d => d.TaxaIva, o => o.Ignore())
        .ForMember(d => d.MotivoIsencao, o => o.Ignore());

      // ---- Acordos ----
      _ = CreateMap<Acordos, AcordosDtos.AcordosDTO>()
        .ForMember(d => d.TipoExameDesignacao, o => o.MapFrom(s => s.TipoExame != null ? s.TipoExame.Designacao : null))
        .ForMember(d => d.OrganismoNome, o => o.MapFrom(s => s.Organismo != null ? s.Organismo.Nome : null));
      _ = CreateMap<Acordos, AcordosDtos.AcordosLightDTO>()
        .ForMember(d => d.TipoExameDesignacao, o => o.MapFrom(s => s.TipoExame != null ? s.TipoExame.Designacao : null))
        .ForMember(d => d.OrganismoNome, o => o.MapFrom(s => s.Organismo != null ? s.Organismo.Nome : null));
      _ = CreateMap<Acordos, AcordosDtos.AcordosTableDTO>()
        .ForMember(d => d.TipoExameDesignacao, o => o.MapFrom(s => s.TipoExame != null ? s.TipoExame.Designacao : null))
        .ForMember(d => d.OrganismoNome, o => o.MapFrom(s => s.Organismo != null ? s.Organismo.Nome : null));
      _ = CreateMap<AcordosDtos.CreateAcordosRequest, Acordos>()
        .ForMember(d => d.TipoExame, o => o.Ignore())
        .ForMember(d => d.Organismo, o => o.Ignore());
      _ = CreateMap<AcordosDtos.UpdateAcordosRequest, Acordos>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.CreatedBy, o => o.Ignore())
        .ForMember(d => d.CreatedOn, o => o.Ignore())
        .ForMember(d => d.DeletedBy, o => o.Ignore())
        .ForMember(d => d.DeletedOn, o => o.Ignore())
        .ForMember(d => d.TipoExame, o => o.Ignore())
        .ForMember(d => d.Organismo, o => o.Ignore());

      // ---- Consulta ----
      _ = CreateMap<Consulta, ConsultaDtos.ConsultaDTO>()
        .ForMember(d => d.TipoConsultaDesignacao, o => o.MapFrom(s => s.TipoConsultaItem != null ? s.TipoConsultaItem.Designacao : null));
      _ = CreateMap<Consulta, ConsultaDtos.ConsultaLightDTO>();
      _ = CreateMap<Consulta, ConsultaDtos.ConsultaTableDTO>()
        .ForMember(d => d.TipoConsultaDesignacao, o => o.MapFrom(s => s.TipoConsultaItem != null ? s.TipoConsultaItem.Designacao : null))
        .ForMember(d => d.MotivoConsultaDesignacao, o => o.MapFrom(s => s.MotivoConsulta != null ? s.MotivoConsulta.Designacao : null))
        .ForMember(d => d.Efectuado, o => o.MapFrom(s => s.Efetuado))
        .ForMember(d => d.StatusConsulta, o => o.MapFrom(s => (int?)s.StatusConsulta))
        .ForMember(d => d.StatusConsultaLabel, o => o.MapFrom(s => s.StatusConsulta.HasValue ? EnumDisplayHelper.GetDisplayName(s.StatusConsulta.Value) : null))
        .ForMember(d => d.UtenteNumero, o => o.MapFrom(s => s.Utente != null ? s.Utente.NumeroUtente : null))
        .ForMember(d => d.UtenteNome, o => o.MapFrom(s => s.Utente != null ? s.Utente.Nome : null))
        .ForMember(d => d.OrganismoNome, o => o.MapFrom(s => s.Organismo != null ? s.Organismo.Nome : null))
        .ForMember(d => d.Sala, o => o.MapFrom(s => s.Sala != null ? s.Sala.Nome : null))
        .ForMember(d => d.MedicoNome, o => o.MapFrom(s => s.Medico != null ? s.Medico.Nome : null))
        .ForMember(d => d.EspecialidadeDesignacao, o => o.MapFrom(s => s.Especialidade != null ? s.Especialidade.Nome : s.ConsultaMarcacao != null && s.ConsultaMarcacao.TipoAdmissao != null ? s.ConsultaMarcacao.TipoAdmissao.Designacao : null))
        .ForMember(d => d.HoraInic, o => o.MapFrom(s => s.HoraInicio.HasValue ? s.HoraInicio.Value.ToString(@"hh\:mm", CultureInfo.InvariantCulture) : null))
        .ForMember(d => d.HoraFim, o => o.MapFrom(s => s.HoraFim.HasValue ? s.HoraFim.Value.ToString(@"hh\:mm", CultureInfo.InvariantCulture) : null))
        .ForMember(d => d.HoraChegada, o => o.MapFrom(s => s.HoraChegada.HasValue ? s.HoraChegada.Value.ToString(@"hh\:mm", CultureInfo.InvariantCulture) : null));
      _ = CreateMap<Consulta, HistoricoConsultaAdministrativoDtos.HistoricoConsultaAdministrativoRowDTO>()
        .ForMember(d => d.UtenteNumero, o => o.MapFrom(s => s.Utente != null ? s.Utente.NumeroUtente : null))
        .ForMember(d => d.UtenteNome, o => o.MapFrom(s => s.Utente != null ? s.Utente.Nome : null))
        .ForMember(d => d.OrganismoNome, o => o.MapFrom(s => s.Organismo != null ? s.Organismo.Nome : null))
        .ForMember(d => d.MedicoNome, o => o.MapFrom(s => s.Medico != null ? s.Medico.Nome : null))
        .ForMember(d => d.EspecialidadeDesignacao, o => o.MapFrom(s => s.Especialidade != null ? s.Especialidade.Nome : s.ConsultaMarcacao != null && s.ConsultaMarcacao.TipoAdmissao != null ? s.ConsultaMarcacao.TipoAdmissao.Designacao : null))
        .ForMember(d => d.TipoConsultaDesignacao, o => o.MapFrom(s => s.TipoConsultaItem != null ? s.TipoConsultaItem.Designacao : null))
        .ForMember(d => d.MotivoConsultaDesignacao, o => o.MapFrom(s => s.MotivoConsulta != null ? s.MotivoConsulta.Designacao : null))
        .ForMember(d => d.HoraInic, o => o.MapFrom(s => s.HoraInicio.HasValue ? s.HoraInicio.Value.ToString(@"hh\:mm", CultureInfo.InvariantCulture) : null))
        .ForMember(d => d.HoraFim, o => o.MapFrom(s => s.HoraFim.HasValue ? s.HoraFim.Value.ToString(@"hh\:mm", CultureInfo.InvariantCulture) : null))
        .ForMember(d => d.StatusConsulta, o => o.MapFrom(s => (int?)s.StatusConsulta))
        .ForMember(d => d.StatusConsultaLabel, o => o.MapFrom(s => s.StatusConsulta.HasValue ? EnumDisplayHelper.GetDisplayName(s.StatusConsulta.Value) : null))
        .ForMember(d => d.Pago, o => o.Ignore())
        .ForMember(d => d.Faturado, o => o.Ignore());
      _ = CreateMap<ConsultaDtos.CreateConsultaRequest, Consulta>();
      _ = CreateMap<ConsultaDtos.UpdateConsultaRequest, Consulta>();

      // ---- ConsultaMarcacao ----
      _ = CreateMap<ConsultaMarcacao, MarcacaoConsultaDtos.MarcacaoConsultaDTO>()
        .ForMember(d => d.StatusConsulta, o => o.MapFrom(s => s.StatusConsulta != null ? (int?)s.StatusConsulta : null));

      _ = CreateMap<ConsultaMarcacao, MarcacaoConsultaDtos.MarcacaoConsultaLightDTO>();
      _ = CreateMap<ConsultaMarcacao, MarcacaoConsultaDtos.MarcacaoConsultaTableDTO>()
        .ForMember(d => d.StatusConsulta, o => o.MapFrom(s => s.StatusConsulta != null ? (int?)s.StatusConsulta : null))
        .ForMember(d => d.UtenteNumero, o => o.MapFrom(s => s.Utente != null ? s.Utente.NumeroUtente : null))
        .ForMember(d => d.UtenteNome, o => o.MapFrom(s => s.Utente != null ? s.Utente.Nome : null))
        .ForMember(d => d.OrganismoCodigo, o => o.MapFrom(s => s.Utente != null && s.Utente.Organismo != null ? s.Utente.Organismo.CodigoClinica : null))
        .ForMember(d => d.OrganismoNome, o => o.MapFrom(s => s.Utente != null && s.Utente.Organismo != null ? s.Utente.Organismo.Nome : null))
        .ForMember(d => d.DataLabel, o => o.MapFrom(s => s.Data.HasValue ? s.Data.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : null))
        .ForMember(d => d.HoraMarcacaoLabel, o => o.MapFrom(s => s.HoraMarcacao.HasValue ? s.HoraMarcacao.Value.ToString(@"hh\:mm", CultureInfo.InvariantCulture) : null))
        .ForMember(d => d.StatusConsultaLabel, o => o.MapFrom(s => s.StatusConsulta.HasValue ? EnumDisplayHelper.GetDisplayName(s.StatusConsulta.Value) : null));
      _ = CreateMap<MarcacaoConsultaDtos.CreateMarcacaoConsultaRequest, ConsultaMarcacao>()
        .ForMember(d => d.ConsultaId, o => o.MapFrom(s => ToNullableGuid(s.ConsultaId)))
        .ForMember(d => d.UtenteId, o => o.MapFrom(s => ToGuid(s.UtenteId)))
        .ForMember(d => d.MedicoId, o => o.MapFrom(s => ToNullableGuid(s.MedicoId)))
        .ForMember(d => d.EspecialidadeId, o => o.MapFrom(s => ToNullableGuid(s.EspecialidadeId)))
        .ForMember(d => d.MotivoConsultaId, o => o.MapFrom(s => ToNullableGuid(s.MotivoConsultaId)))
        .ForMember(d => d.TipoAdmissaoId, o => o.MapFrom(s => ToNullableGuid(s.TipoAdmissaoId)))
        .ForMember(d => d.TipoConsultaId, o => o.MapFrom(s => ToNullableGuid(s.TipoConsultaId)))
        .ForMember(d => d.Data, o => o.MapFrom(s => s.Data))
        .ForMember(d => d.HoraMarcacao, o => o.MapFrom(s => TimeSpan.Parse(s.HoraInic, CultureInfo.InvariantCulture)))
        .ForMember(d => d.Obs, o => o.MapFrom(s => s.Obs))
        .ForMember(d => d.Consulta, o => o.Ignore())
        .ForMember(d => d.Utente, o => o.Ignore())
        .ForMember(d => d.Medico, o => o.Ignore())
        .ForMember(d => d.Especialidade, o => o.Ignore());
      _ = CreateMap<MarcacaoConsultaDtos.UpdateMarcacaoConsultaRequest, ConsultaMarcacao>()
        .ForMember(d => d.ConsultaId, o => o.MapFrom(s => ToNullableGuid(s.ConsultaId)))
        .ForMember(d => d.UtenteId, o => o.MapFrom(s => ToGuid(s.UtenteId)))
        .ForMember(d => d.MedicoId, o => o.MapFrom(s => ToNullableGuid(s.MedicoId)))
        .ForMember(d => d.EspecialidadeId, o => o.MapFrom(s => ToNullableGuid(s.EspecialidadeId)))
        .ForMember(d => d.MotivoConsultaId, o => o.MapFrom(s => ToNullableGuid(s.MotivoConsultaId)))
        .ForMember(d => d.TipoAdmissaoId, o => o.MapFrom(s => ToNullableGuid(s.TipoAdmissaoId)))
        .ForMember(d => d.TipoConsultaId, o => o.MapFrom(s => ToNullableGuid(s.TipoConsultaId)))
        .ForMember(d => d.Data, o => o.MapFrom(s => s.Data))
        .ForMember(d => d.HoraMarcacao, o => o.MapFrom(s => TimeSpan.Parse(s.HoraInic, CultureInfo.InvariantCulture)))
        .ForMember(d => d.Obs, o => o.MapFrom(s => s.Obs))
        .ForMember(d => d.Consulta, o => o.Ignore())
        .ForMember(d => d.Utente, o => o.Ignore())
        .ForMember(d => d.Medico, o => o.Ignore())
        .ForMember(d => d.Especialidade, o => o.Ignore());

      // ---- ServicoConsulta ----
      _ = CreateMap<ServicoConsulta, ServicoConsultaDtos.ServicoConsultaDTO>();
      _ = CreateMap<ServicoConsulta, ServicoConsultaDtos.ServicoConsultaLightDTO>();
      _ = CreateMap<ServicoConsulta, ServicoConsultaDtos.ServicoConsultaTableDTO>();
      _ = CreateMap<ServicoConsultaDtos.CreateServicoConsultaRequest, ServicoConsulta>()
        .ForMember(d => d.ConsultaId, o => o.MapFrom(s => ToGuid(s.ConsultaId)))
        .ForMember(d => d.ServicoId, o => o.MapFrom(s => ToNullableGuid(s.ServicoId)))
        .ForMember(d => d.ExameId, o => o.MapFrom(s => ToNullableGuid(s.ExameId)))
        .ForMember(d => d.Consulta, o => o.Ignore())
        .ForMember(d => d.Servico, o => o.Ignore())
        .ForMember(d => d.Exame, o => o.Ignore());
      _ = CreateMap<ServicoConsultaDtos.UpdateServicoConsultaRequest, ServicoConsulta>()
        .ForMember(d => d.ConsultaId, o => o.MapFrom(s => ToGuid(s.ConsultaId)))
        .ForMember(d => d.ServicoId, o => o.MapFrom(s => ToNullableGuid(s.ServicoId)))
        .ForMember(d => d.ExameId, o => o.MapFrom(s => ToNullableGuid(s.ExameId)))
        .ForMember(d => d.Consulta, o => o.Ignore())
        .ForMember(d => d.Servico, o => o.Ignore())
        .ForMember(d => d.Exame, o => o.Ignore());

      // ---- TipoServico ----
      _ = CreateMap<TipoServico, TipoServicoDtos.TipoServicoDTO>();
      _ = CreateMap<TipoServico, TipoServicoDtos.TipoServicoLightDTO>();
      _ = CreateMap<TipoServico, TipoServicoDtos.TipoServicoTableDTO>()
        .ForMember(d => d.ServicosCount, o => o.MapFrom(s => s.Servicos.Count));
      _ = CreateMap<TipoServicoDtos.CreateTipoServicoRequest, TipoServico>();
      _ = CreateMap<TipoServicoDtos.UpdateTipoServicoRequest, TipoServico>();

      // ---- CartaConducao ----
      _ = CreateMap<CartaConducao, CartaConducaoDtos.CartaConducaoDTO>();
      _ = CreateMap<CartaConducao, CartaConducaoDtos.CartaConducaoLightDTO>();
      _ = CreateMap<CartaConducao, CartaConducaoDtos.CartaConducaoTableDTO>();
      _ = CreateMap<CartaConducaoDtos.CreateCartaConducaoRequest, CartaConducao>();
      _ = CreateMap<CartaConducaoDtos.UpdateCartaConducaoRequest, CartaConducao>();

      // ---- CartaConducaoRestricao ----
      _ = CreateMap<CartaConducaoRestricao, CartaConducaoRestricoesDtos.CartaConducaoRestricoesDTO>();
      _ = CreateMap<CartaConducaoRestricao, CartaConducaoRestricoesDtos.CartaConducaoRestricoesLightDTO>();
      _ = CreateMap<CartaConducaoRestricao, CartaConducaoRestricoesDtos.CartaConducaoRestricoesTableDTO>();
      _ = CreateMap<CartaConducaoRestricoesDtos.CreateCartaConducaoRestricoesRequest, CartaConducaoRestricao>();
      _ = CreateMap<CartaConducaoRestricoesDtos.UpdateCartaConducaoRestricoesRequest, CartaConducaoRestricao>();

      // ---- Atestado ----
      _ = CreateMap<Atestado, AtestadoDtos.AtestadoDTO>();
      _ = CreateMap<Atestado, AtestadoDtos.AtestadoTableDTO>()
        .ForMember(d => d.NomeUtente, o => o.MapFrom(s => s.Utente != null ? s.Utente.Nome : null))
        .ForMember(d => d.NomeMedico, o => o.MapFrom(s => s.Medico != null ? s.Medico.Nome : null));
      _ = CreateMap<AtestadoDtos.CreateAtestadoRequest, Atestado>()
        .ForMember(d => d.Categorias, o => o.Ignore())
        .ForMember(d => d.Restricoes, o => o.Ignore())
        .ForMember(d => d.RestricoesAnteriores, o => o.Ignore())
        .ForMember(d => d.Utente, o => o.Ignore())
        .ForMember(d => d.Medico, o => o.Ignore())
        .ForMember(d => d.Clinica, o => o.Ignore())
        .ForMember(d => d.CodigoPostal, o => o.Ignore());

      // ---- Servico ----
      _ = CreateMap<Servico, ServicoDtos.ServicoDTO>()
        .ForMember(d => d.TipoServicoDescricao, o => o.MapFrom(s => s.TipoServico != null ? s.TipoServico.Descricao : null))
        .ForMember(d => d.TipoAparelhoDesignacao, o => o.MapFrom(s => s.TipoAparelho != null ? s.TipoAparelho.Designacao : null));
      _ = CreateMap<Servico, ServicoDtos.ServicoLightDTO>();
      _ = CreateMap<Servico, ServicoDtos.ServicoTableDTO>()
        .ForMember(d => d.TipoServicoDescricao, o => o.MapFrom(s => s.TipoServico != null ? s.TipoServico.Descricao : null));
      _ = CreateMap<ServicoDtos.CreateServicoRequest, Servico>()
        .ForMember(d => d.TipoServico, o => o.Ignore())
        .ForMember(d => d.TipoAparelho, o => o.Ignore())
        .ForMember(d => d.TaxaIva, o => o.Ignore())
        .ForMember(d => d.MotivoIsencao, o => o.Ignore());
      _ = CreateMap<ServicoDtos.UpdateServicoRequest, Servico>()
        .ForMember(d => d.TipoServico, o => o.Ignore())
        .ForMember(d => d.TipoAparelho, o => o.Ignore())
        .ForMember(d => d.TaxaIva, o => o.Ignore())
        .ForMember(d => d.MotivoIsencao, o => o.Ignore());

      // ---- SubsistemaServico ----
      _ = CreateMap<SubsistemaServico, SubsistemaServicoDtos.SubsistemaServicoDTO>();
      _ = CreateMap<SubsistemaServicoDtos.CreateSubsistemaServicoRequest, SubsistemaServico>()
        .ForMember(d => d.Servico, o => o.Ignore());
      _ = CreateMap<SubsistemaServicoDtos.UpdateSubsistemaServicoRequest, SubsistemaServico>()
        .ForMember(d => d.Servico, o => o.Ignore());

      // ---- Tratamento ----
      _ = CreateMap<Tratamento, TratamentoDtos.TratamentoDTO>();
      _ = CreateMap<Tratamento, TratamentoDtos.TratamentoLightDTO>();
      _ = CreateMap<Tratamento, TratamentoDtos.TratamentoTableDTO>()
        .ForMember(d => d.SessoesCount, o => o.MapFrom(s => s.Sessoes.Count))
        .ForMember(d => d.ServicosCount, o => o.MapFrom(s => s.Servicos.Count))
        .ForMember(d => d.OrganismoNome,o => o.MapFrom(s => s.Organismo != null ? (s.Organismo.Nome ?? s.Organismo.NomeComercial ?? s.Organismo.Abreviatura) : null))
        .ForMember(d => d.LocalTratamentoNome,o => o.MapFrom(s => s.LocalTratamento != null ? s.LocalTratamento.Designacao : null))
        .ForMember(d => d.MedicoNome,o => o.MapFrom(s => s.Medico != null ? s.Medico.Nome : null))
        .ForMember(d => d.VemListEsp,o => o.MapFrom(s => s.VemListEsp));
      _ = CreateMap<TratamentoDtos.CreateTratamentoRequest, Tratamento>()
        .ForMember(d => d.UtenteId, o => o.MapFrom(s => ToNullableGuid(s.UtenteId)))
        .ForMember(d => d.MedicoId, o => o.MapFrom(s => ToNullableGuid(s.MedicoId)))
        .ForMember(d => d.FisioterapeutaId, o => o.MapFrom(s => ToNullableGuid(s.FisioterapeutaId)))
        .ForMember(d => d.AuxiliarId, o => o.MapFrom(s => ToNullableGuid(s.AuxiliarId)))
        .ForMember(d => d.OutroTecnicoId, o => o.MapFrom(s => ToNullableGuid(s.OutroTecnicoId)))
        .ForMember(d => d.OrganismoId, o => o.MapFrom(s => ToNullableGuid(s.OrganismoId)))
        .ForMember(d => d.LocalTratamentoId, o => o.MapFrom(s => ToNullableGuid(s.LocalTratamentoId)))
        .ForMember(d => d.TratamentoPredId, o => o.MapFrom(s => ToNullableGuid(s.TratamentoPredId)))
        .ForMember(d => d.LocalOrigemId, o => o.MapFrom(s => ToNullableGuid(s.LocalOrigemId)))
        .ForMember(d => d.ReciboId, o => o.MapFrom(s => ToNullableGuid(s.ReciboId)))
        .ForMember(d => d.SinistroId, o => o.MapFrom(s => ToNullableGuid(s.SinistroId)))
        .ForMember(d => d.SeguradoraId, o => o.MapFrom(s => ToNullableGuid(s.SeguradoraId)))
        .ForMember(d => d.DocumentoId, o => o.MapFrom(s => ToNullableGuid(s.DocumentoId)))
        .ForMember(d => d.TratamentoPred, o => o.Ignore())
        .ForMember(d => d.Sessoes, o => o.Ignore())
        .ForMember(d => d.Servicos, o => o.Ignore())
        .ForMember(d => d.Utente, o => o.Ignore())
        .ForMember(d => d.Medico, o => o.Ignore())
        .ForMember(d => d.Fisioterapeuta, o => o.Ignore())
        .ForMember(d => d.Auxiliar, o => o.Ignore())
        .ForMember(d => d.OutroTecnico, o => o.Ignore())
        .ForMember(d => d.Organismo, o => o.Ignore())
        .ForMember(d => d.Recibo, o => o.Ignore())
        .ForMember(d => d.Seguradora, o => o.Ignore())
        .ForMember(d => d.Documento, o => o.Ignore());
      _ = CreateMap<TratamentoDtos.UpdateTratamentoRequest, Tratamento>()
        .ForMember(d => d.UtenteId, o => o.MapFrom(s => ToNullableGuid(s.UtenteId)))
        .ForMember(d => d.MedicoId, o => o.MapFrom(s => ToNullableGuid(s.MedicoId)))
        .ForMember(d => d.FisioterapeutaId, o => o.MapFrom(s => ToNullableGuid(s.FisioterapeutaId)))
        .ForMember(d => d.AuxiliarId, o => o.MapFrom(s => ToNullableGuid(s.AuxiliarId)))
        .ForMember(d => d.OutroTecnicoId, o => o.MapFrom(s => ToNullableGuid(s.OutroTecnicoId)))
        .ForMember(d => d.OrganismoId, o => o.MapFrom(s => ToNullableGuid(s.OrganismoId)))
        .ForMember(d => d.LocalTratamentoId, o => o.MapFrom(s => ToNullableGuid(s.LocalTratamentoId)))
        .ForMember(d => d.TratamentoPredId, o => o.MapFrom(s => ToNullableGuid(s.TratamentoPredId)))
        .ForMember(d => d.LocalOrigemId, o => o.MapFrom(s => ToNullableGuid(s.LocalOrigemId)))
        .ForMember(d => d.ReciboId, o => o.MapFrom(s => ToNullableGuid(s.ReciboId)))
        .ForMember(d => d.SinistroId, o => o.MapFrom(s => ToNullableGuid(s.SinistroId)))
        .ForMember(d => d.SeguradoraId, o => o.MapFrom(s => ToNullableGuid(s.SeguradoraId)))
        .ForMember(d => d.DocumentoId, o => o.MapFrom(s => ToNullableGuid(s.DocumentoId)))
        .ForMember(d => d.TratamentoPred, o => o.Ignore())
        .ForMember(d => d.Sessoes, o => o.Ignore())
        .ForMember(d => d.Servicos, o => o.Ignore())
        .ForMember(d => d.Utente, o => o.Ignore())
        .ForMember(d => d.Medico, o => o.Ignore())
        .ForMember(d => d.Fisioterapeuta, o => o.Ignore())
        .ForMember(d => d.Auxiliar, o => o.Ignore())
        .ForMember(d => d.OutroTecnico, o => o.Ignore())
        .ForMember(d => d.Organismo, o => o.Ignore())
        .ForMember(d => d.Recibo, o => o.Ignore())
        .ForMember(d => d.Seguradora, o => o.Ignore())
        .ForMember(d => d.Documento, o => o.Ignore());
      
      // ---- CategoriaEspecialidade ----
      _ = CreateMap<CliCloud.Domain.Entities.Especialidades.CategoriaEspecialidade, CategoriaEspecialidadeDtos.CategoriaEspecialidadeDTO>()
        .ForMember(d => d.EspecialidadesCount, o => o.MapFrom(s => s.Especialidades.Count));
      _ = CreateMap<CliCloud.Domain.Entities.Especialidades.CategoriaEspecialidade, CategoriaEspecialidadeDtos.CategoriaEspecialidadeLightDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.Especialidades.CategoriaEspecialidade, CategoriaEspecialidadeDtos.CategoriaEspecialidadeTableDTO>()
        .ForMember(d => d.EspecialidadesCount, o => o.MapFrom(s => s.Especialidades.Count));
      _ = CreateMap<CategoriaEspecialidadeDtos.CreateCategoriaEspecialidadeRequest, CliCloud.Domain.Entities.Especialidades.CategoriaEspecialidade>();
      _ = CreateMap<CategoriaEspecialidadeDtos.UpdateCategoriaEspecialidadeRequest, CliCloud.Domain.Entities.Especialidades.CategoriaEspecialidade>();

      // ---- Especialidade ----
      _ = CreateMap<CliCloud.Domain.Entities.Especialidades.Especialidade, EspecialidadeDtos.EspecialidadeDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.Especialidades.Especialidade, EspecialidadeDtos.EspecialidadeLightDTO>()
        .ForMember(d => d.CategoriaEspecialidadeDescricao, o => o.MapFrom(s => s.CategoriaEspecialidade != null ? s.CategoriaEspecialidade.Descricao : null));
      _ = CreateMap<CliCloud.Domain.Entities.Especialidades.Especialidade, EspecialidadeDtos.EspecialidadeTableDTO>()
        .ForMember(d => d.CategoriaEspecialidadeDescricao, o => o.MapFrom(s => s.CategoriaEspecialidade != null ? s.CategoriaEspecialidade.Descricao : null));
      _ = CreateMap<EspecialidadeDtos.CreateEspecialidadeRequest, CliCloud.Domain.Entities.Especialidades.Especialidade>()
        .ForMember(d => d.CategoriaEspecialidadeId, o => o.MapFrom(s => ToNullableGuid(s.CategoriaEspecialidadeId)))
        .ForMember(d => d.CategoriaEspecialidade, o => o.Ignore());
      _ = CreateMap<EspecialidadeDtos.UpdateEspecialidadeRequest, CliCloud.Domain.Entities.Especialidades.Especialidade>()
        .ForMember(d => d.CategoriaEspecialidadeId, o => o.MapFrom(s => ToNullableGuid(s.CategoriaEspecialidadeId)))
        .ForMember(d => d.CategoriaEspecialidade, o => o.Ignore());

      // ---- EstadoCivil ----
      _ = CreateMap<CliCloud.Domain.Entities.EstadosCivis.EstadoCivil, EstadoCivilDtos.EstadoCivilDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.EstadosCivis.EstadoCivil, EstadoCivilDtos.EstadoCivilLightDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.EstadosCivis.EstadoCivil, EstadoCivilDtos.EstadoCivilTableDTO>();
      _ = CreateMap<EstadoCivilDtos.CreateEstadoCivilRequest, CliCloud.Domain.Entities.EstadosCivis.EstadoCivil>();
      _ = CreateMap<EstadoCivilDtos.UpdateEstadoCivilRequest, CliCloud.Domain.Entities.EstadosCivis.EstadoCivil>();

      // ---- GrupoSanguineo ----
      _ = CreateMap<CliCloud.Domain.Entities.GruposSanguineos.GrupoSanguineo, GrupoSanguineoDtos.GrupoSanguineoDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.GruposSanguineos.GrupoSanguineo, GrupoSanguineoDtos.GrupoSanguineoLightDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.GruposSanguineos.GrupoSanguineo, GrupoSanguineoDtos.GrupoSanguineoTableDTO>();
      _ = CreateMap<GrupoSanguineoDtos.CreateGrupoSanguineoRequest, CliCloud.Domain.Entities.GruposSanguineos.GrupoSanguineo>();
      _ = CreateMap<GrupoSanguineoDtos.UpdateGrupoSanguineoRequest, CliCloud.Domain.Entities.GruposSanguineos.GrupoSanguineo>();

      // ---- Habilitacao ----
      _ = CreateMap<CliCloud.Domain.Entities.Habilitacoes.Habilitacao, HabilitacaoDtos.HabilitacaoDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.Habilitacoes.Habilitacao, HabilitacaoDtos.HabilitacaoLightDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.Habilitacoes.Habilitacao, HabilitacaoDtos.HabilitacaoTableDTO>();
      _ = CreateMap<HabilitacaoDtos.CreateHabilitacaoRequest, CliCloud.Domain.Entities.Habilitacoes.Habilitacao>();
      _ = CreateMap<HabilitacaoDtos.UpdateHabilitacaoRequest, CliCloud.Domain.Entities.Habilitacoes.Habilitacao>();

      // ---- Sexo ----
      _ = CreateMap<CliCloud.Domain.Entities.Sexos.Sexo, SexoDtos.SexoDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.Sexos.Sexo, SexoDtos.SexoLightDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.Sexos.Sexo, SexoDtos.SexoTableDTO>();
      _ = CreateMap<SexoDtos.CreateSexoRequest, CliCloud.Domain.Entities.Sexos.Sexo>();
      _ = CreateMap<SexoDtos.UpdateSexoRequest, CliCloud.Domain.Entities.Sexos.Sexo>();

      // ---- GrauParentesco ----
      _ = CreateMap<CliCloud.Domain.Entities.GrausParentesco.GrauParentesco, GrauParentescoDtos.GrauParentescoDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.GrausParentesco.GrauParentesco, GrauParentescoDtos.GrauParentescoLightDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.GrausParentesco.GrauParentesco, GrauParentescoDtos.GrauParentescoTableDTO>();
      _ = CreateMap<GrauParentescoDtos.CreateGrauParentescoRequest, CliCloud.Domain.Entities.GrausParentesco.GrauParentesco>();
      _ = CreateMap<GrauParentescoDtos.UpdateGrauParentescoRequest, CliCloud.Domain.Entities.GrausParentesco.GrauParentesco>();

      // ---- TaxaIva ----
      _ = CreateMap<CliCloud.Domain.Entities.TaxasIva.TaxaIva, TaxaIvaDtos.TaxaIvaDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.TaxasIva.TaxaIva, TaxaIvaDtos.TaxaIvaLightDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.TaxasIva.TaxaIva, TaxaIvaDtos.TaxaIvaTableDTO>();
      _ = CreateMap<TaxaIvaDtos.CreateTaxaIvaRequest, CliCloud.Domain.Entities.TaxasIva.TaxaIva>();
      _ = CreateMap<TaxaIvaDtos.UpdateTaxaIvaRequest, CliCloud.Domain.Entities.TaxasIva.TaxaIva>();

      // ---- MotivoIsencao ----
      _ = CreateMap<CliCloud.Domain.Entities.TaxasIva.MotivoIsencao, MotivoIsencaoDtos.MotivoIsencaoDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.TaxasIva.MotivoIsencao, MotivoIsencaoDtos.MotivoIsencaoLightDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.TaxasIva.MotivoIsencao, MotivoIsencaoDtos.MotivoIsencaoTableDTO>();
      _ = CreateMap<MotivoIsencaoDtos.CreateMotivoIsencaoRequest, CliCloud.Domain.Entities.TaxasIva.MotivoIsencao>();
      _ = CreateMap<MotivoIsencaoDtos.UpdateMotivoIsencaoRequest, CliCloud.Domain.Entities.TaxasIva.MotivoIsencao>();

      // ---- MotivoRetencao ----
      _ = CreateMap<CliCloud.Domain.Entities.TaxasIva.MotivoRetencao, MotivoRetencaoDtos.MotivoRetencaoDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.TaxasIva.MotivoRetencao, MotivoRetencaoDtos.MotivoRetencaoLightDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.TaxasIva.MotivoRetencao, MotivoRetencaoDtos.MotivoRetencaoTableDTO>();
      _ = CreateMap<MotivoRetencaoDtos.CreateMotivoRetencaoRequest, CliCloud.Domain.Entities.TaxasIva.MotivoRetencao>();
      _ = CreateMap<MotivoRetencaoDtos.UpdateMotivoRetencaoRequest, CliCloud.Domain.Entities.TaxasIva.MotivoRetencao>();

      // ---- CondicaoPagamento ----
      _ = CreateMap<CondicaoPagamentoEntity, CondicaoPagamentoDtos.CondicaoPagamentoDTO>();
      _ = CreateMap<CondicaoPagamentoEntity, CondicaoPagamentoDtos.CondicaoPagamentoLightDTO>();
      _ = CreateMap<CondicaoPagamentoEntity, CondicaoPagamentoDtos.CondicaoPagamentoTableDTO>();
      _ = CreateMap<CondicaoPagamentoDtos.CreateCondicaoPagamentoRequest, CondicaoPagamentoEntity>()
          .ForMember(d => d.Id, o => o.Ignore())
          .ForMember(d => d.ClinicaId, o => o.Ignore())
          .ForMember(d => d.Codigo, o => o.Ignore());
      _ = CreateMap<CondicaoPagamentoDtos.UpdateCondicaoPagamentoRequest, CondicaoPagamentoEntity>()
          .ForMember(d => d.Id, o => o.Ignore())
          .ForMember(d => d.ClinicaId, o => o.Ignore())
          .ForMember(d => d.Codigo, o => o.Ignore());

      // ---- TipoPagamento (lookup SAFT) ----
      _ = CreateMap<TipoPagamentoEntity, TipoPagamentoDtos.TipoPagamentoLightDTO>();

      // ---- Armazem (Stocks) ----
      _ = CreateMap<ArmazemEntity, ArmazemDtos.ArmazemDTO>()
          .ForMember(d => d.CodigoPostalCodigo, o => o.MapFrom(s => s.CodigoPostal != null ? s.CodigoPostal.Codigo : null))
          .ForMember(d => d.CodigoPostalLocalidade, o => o.MapFrom(s => s.CodigoPostal != null ? s.CodigoPostal.Localidade : null));
      _ = CreateMap<ArmazemEntity, ArmazemDtos.ArmazemLightDTO>();
      _ = CreateMap<ArmazemEntity, ArmazemDtos.ArmazemTableDTO>();
      _ = CreateMap<ArmazemDtos.CreateArmazemRequest, ArmazemEntity>()
          .ForMember(d => d.Id, o => o.Ignore())
          .ForMember(d => d.ClinicaId, o => o.Ignore())
          .ForMember(d => d.Codigo, o => o.Ignore())
          .ForMember(d => d.CodigoPostal, o => o.Ignore());
      _ = CreateMap<ArmazemDtos.UpdateArmazemRequest, ArmazemEntity>()
          .ForMember(d => d.Id, o => o.Ignore())
          .ForMember(d => d.ClinicaId, o => o.Ignore())
          .ForMember(d => d.Codigo, o => o.Ignore())
          .ForMember(d => d.CodigoPostal, o => o.Ignore());

      // ---- ZonaComercial (Faturacao) ----
      _ = CreateMap<ZonaComercialEntity, ZonaComercialDtos.ZonaComercialDTO>();
      _ = CreateMap<ZonaComercialEntity, ZonaComercialDtos.ZonaComercialLightDTO>();
      _ = CreateMap<ZonaComercialEntity, ZonaComercialDtos.ZonaComercialTableDTO>();
      _ = CreateMap<ZonaComercialDtos.CreateZonaComercialRequest, ZonaComercialEntity>()
          .ForMember(d => d.Id, o => o.Ignore())
          .ForMember(d => d.ClinicaId, o => o.Ignore())
          .ForMember(d => d.Codigo, o => o.Ignore());
      _ = CreateMap<ZonaComercialDtos.UpdateZonaComercialRequest, ZonaComercialEntity>()
          .ForMember(d => d.Id, o => o.Ignore())
          .ForMember(d => d.ClinicaId, o => o.Ignore())
          .ForMember(d => d.Codigo, o => o.Ignore());

      // ---- UnidadeMedida (Stocks) ----
      _ = CreateMap<UnidadeMedidaEntity, UnidadeMedidaDtos.UnidadeMedidaDTO>();
      _ = CreateMap<UnidadeMedidaEntity, UnidadeMedidaDtos.UnidadeMedidaLightDTO>();
      _ = CreateMap<UnidadeMedidaEntity, UnidadeMedidaDtos.UnidadeMedidaTableDTO>();
      _ = CreateMap<UnidadeMedidaDtos.CreateUnidadeMedidaRequest, UnidadeMedidaEntity>()
          .ForMember(d => d.Id, o => o.Ignore())
          .ForMember(d => d.ClinicaId, o => o.Ignore())
          .ForMember(d => d.Codigo, o => o.Ignore());

      // ---- Artigo (Stocks) ----
      _ = CreateMap<ArtigoEntity, ArtigoDtos.ArtigoDTO>()
          .ForMember(d => d.UnidadeMedidaDescricao, o => o.MapFrom(s => s.UnidadeMedida.Descricao))
          .ForMember(d => d.FamiliaArtigoDescricao, o => o.MapFrom(s => s.FamiliaArtigo != null ? s.FamiliaArtigo.Descricao : null))
          .ForMember(d => d.TaxaIvaDescricao, o => o.MapFrom(s => s.TaxaIva.Descricao))
          .ForMember(d => d.TaxaIvaPercentagem, o => o.MapFrom(s => s.TaxaIva.Taxa))
          .ForMember(d => d.MotivoIsencaoDescricao, o => o.MapFrom(s => s.MotivoIsencao != null ? s.MotivoIsencao.Descricao : null))
          .ForMember(d => d.ArmazemNome, o => o.MapFrom(s => s.Armazem.Nome));
      _ = CreateMap<ArtigoEntity, ArtigoDtos.ArtigoLightDTO>();
      _ = CreateMap<ArtigoEntity, ArtigoDtos.ArtigoTableDTO>()
          .ForMember(d => d.ArmazemNome, o => o.MapFrom(s => s.Armazem.Nome));
      // ---- SubsistemaArtigo (Stocks) ----
      _ = CreateMap<SubsistemaArtigoEntity, SubsistemaArtigoDtos.SubsistemaArtigoDTO>()
          .ForMember(d => d.ArtigoCodigo, o => o.MapFrom(s => s.Artigo.Codigo))
          .ForMember(d => d.ArtigoNumero, o => o.MapFrom(s => s.Artigo.NumeroArtigo))
          .ForMember(d => d.ArtigoDescricao, o => o.MapFrom(s => s.Artigo.Descricao))
          .ForMember(d => d.OrganismoNome, o => o.MapFrom(s => s.Organismo.Nome));
      _ = CreateMap<SubsistemaArtigoEntity, SubsistemaArtigoDtos.SubsistemaArtigoTableDTO>()
          .ForMember(d => d.ArtigoNumero, o => o.MapFrom(s => s.Artigo.NumeroArtigo))
          .ForMember(d => d.ArtigoDescricao, o => o.MapFrom(s => s.Artigo.Descricao))
          .ForMember(d => d.OrganismoNome, o => o.MapFrom(s => s.Organismo.Nome));
      _ = CreateMap<SubsistemaArtigoDtos.CreateSubsistemaArtigoRequest, SubsistemaArtigoEntity>()
          .ForMember(d => d.Id, o => o.Ignore())
          .ForMember(d => d.ClinicaId, o => o.Ignore())
          .ForMember(d => d.Artigo, o => o.Ignore())
          .ForMember(d => d.Organismo, o => o.Ignore());

      _ = CreateMap<ArtigoDtos.CreateArtigoRequest, ArtigoEntity>()
          .ForMember(d => d.Id, o => o.Ignore())
          .ForMember(d => d.ClinicaId, o => o.Ignore())
          .ForMember(d => d.Codigo, o => o.Ignore())
          .ForMember(d => d.StockReal, o => o.Ignore())
          .ForMember(d => d.UltimoPrecoFinal, o => o.Ignore())
          .ForMember(d => d.PrecoMedioFinal, o => o.Ignore())
          .ForMember(d => d.UltimoPrecoVenda, o => o.Ignore())
          .ForMember(d => d.PrecoMedioVenda, o => o.Ignore())
          .ForMember(d => d.CodigoInternoLegado, o => o.Ignore())
          .ForMember(d => d.UnidadeMedida, o => o.Ignore())
          .ForMember(d => d.FamiliaArtigo, o => o.Ignore())
          .ForMember(d => d.TaxaIva, o => o.Ignore())
          .ForMember(d => d.MotivoIsencao, o => o.Ignore())
          .ForMember(d => d.Armazem, o => o.Ignore());
      _ = CreateMap<ArtigoDtos.UpdateArtigoRequest, ArtigoEntity>()
          .ForMember(d => d.Id, o => o.Ignore())
          .ForMember(d => d.ClinicaId, o => o.Ignore())
          .ForMember(d => d.Codigo, o => o.Ignore())
          .ForMember(d => d.StockReal, o => o.Ignore())
          .ForMember(d => d.UltimoPrecoFinal, o => o.Ignore())
          .ForMember(d => d.PrecoMedioFinal, o => o.Ignore())
          .ForMember(d => d.UltimoPrecoVenda, o => o.Ignore())
          .ForMember(d => d.PrecoMedioVenda, o => o.Ignore())
          .ForMember(d => d.CodigoInternoLegado, o => o.Ignore())
          .ForMember(d => d.UnidadeMedida, o => o.Ignore())
          .ForMember(d => d.FamiliaArtigo, o => o.Ignore())
          .ForMember(d => d.TaxaIva, o => o.Ignore())
          .ForMember(d => d.MotivoIsencao, o => o.Ignore())
          .ForMember(d => d.Armazem, o => o.Ignore());

      // ---- FamiliaArtigo (Stocks) ----
      _ = CreateMap<FamiliaArtigoEntity, FamiliaArtigoDtos.FamiliaArtigoDTO>()
          .ForMember(d => d.Path, o => o.Ignore());
      _ = CreateMap<FamiliaArtigoEntity, FamiliaArtigoDtos.FamiliaArtigoTableDTO>()
          .ForMember(d => d.TemFilhos, o => o.Ignore());
      _ = CreateMap<FamiliaArtigoDtos.CreateFamiliaArtigoRequest, FamiliaArtigoEntity>()
          .ForMember(d => d.Id, o => o.Ignore())
          .ForMember(d => d.ClinicaId, o => o.Ignore())
          .ForMember(d => d.Codigo, o => o.Ignore())
          .ForMember(d => d.Nivel, o => o.Ignore())
          .ForMember(d => d.Parent, o => o.Ignore())
          .ForMember(d => d.Children, o => o.Ignore());
      _ = CreateMap<FamiliaArtigoDtos.UpdateFamiliaArtigoRequest, FamiliaArtigoEntity>()
          .ForMember(d => d.Id, o => o.Ignore())
          .ForMember(d => d.ClinicaId, o => o.Ignore())
          .ForMember(d => d.Codigo, o => o.Ignore())
          .ForMember(d => d.ParentId, o => o.Ignore())
          .ForMember(d => d.Nivel, o => o.Ignore())
          .ForMember(d => d.Parent, o => o.Ignore())
          .ForMember(d => d.Children, o => o.Ignore());

      // ---- ModoPagamento ----
      _ = CreateMap<ModoPagamentoEntity, ModoPagamentoDtos.ModoPagamentoDTO>()
          .ForMember(d => d.TipoPagamentoDescricao, o => o.MapFrom(s => s.TipoPagamento != null ? s.TipoPagamento.Descricao : string.Empty))
          .ForMember(d => d.ContaBancariaNumero, o => o.MapFrom(s => s.ContaBancaria != null ? s.ContaBancaria.Numero : null));
      _ = CreateMap<ModoPagamentoEntity, ModoPagamentoDtos.ModoPagamentoLightDTO>()
          .ForMember(d => d.ContaBancariaNumero, o => o.MapFrom(s => s.ContaBancaria != null ? s.ContaBancaria.Numero : null));
      _ = CreateMap<ModoPagamentoEntity, ModoPagamentoDtos.ModoPagamentoTableDTO>();
      _ = CreateMap<ModoPagamentoDtos.CreateModoPagamentoRequest, ModoPagamentoEntity>()
          .ForMember(d => d.Id, o => o.Ignore())
          .ForMember(d => d.ClinicaId, o => o.Ignore())
          .ForMember(d => d.Codigo, o => o.Ignore())
          .ForMember(d => d.Historico, o => o.Ignore())
          .ForMember(d => d.TipoPagamento, o => o.Ignore())
          .ForMember(d => d.ContaBancaria, o => o.Ignore());
      _ = CreateMap<ModoPagamentoDtos.UpdateModoPagamentoRequest, ModoPagamentoEntity>()
          .ForMember(d => d.Id, o => o.Ignore())
          .ForMember(d => d.ClinicaId, o => o.Ignore())
          .ForMember(d => d.Codigo, o => o.Ignore())
          .ForMember(d => d.Historico, o => o.Ignore())
          .ForMember(d => d.TipoPagamento, o => o.Ignore())
          .ForMember(d => d.ContaBancaria, o => o.Ignore());

      // ---- ProvenienciaUtente ----
      _ = CreateMap<CliCloud.Domain.Entities.ProvenienciasUtente.ProvenienciaUtente, ProvenienciaUtenteDtos.ProvenienciaUtenteDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.ProvenienciasUtente.ProvenienciaUtente, ProvenienciaUtenteDtos.ProvenienciaUtenteLightDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.ProvenienciasUtente.ProvenienciaUtente, ProvenienciaUtenteDtos.ProvenienciaUtenteTableDTO>();
      _ = CreateMap<ProvenienciaUtenteDtos.CreateProvenienciaUtenteRequest, CliCloud.Domain.Entities.ProvenienciasUtente.ProvenienciaUtente>();
      _ = CreateMap<ProvenienciaUtenteDtos.UpdateProvenienciaUtenteRequest, CliCloud.Domain.Entities.ProvenienciasUtente.ProvenienciaUtente>();

      // ---- Profissao ----
      _ = CreateMap<CliCloud.Domain.Entities.Profissoes.Profissao, ProfissaoDtos.ProfissaoDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.Profissoes.Profissao, ProfissaoDtos.ProfissaoLightDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.Profissoes.Profissao, ProfissaoDtos.ProfissaoTableDTO>();
      _ = CreateMap<ProfissaoDtos.CreateProfissaoRequest, CliCloud.Domain.Entities.Profissoes.Profissao>();
      _ = CreateMap<ProfissaoDtos.UpdateProfissaoRequest, CliCloud.Domain.Entities.Profissoes.Profissao>();

      // ---- Moeda ----
      _ = CreateMap<CliCloud.Domain.Entities.Moedas.Moeda, MoedaDtos.MoedaDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.Moedas.Moeda, MoedaDtos.MoedaLightDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.Moedas.Moeda, MoedaDtos.MoedaTableDTO>();
      _ = CreateMap<MoedaDtos.CreateMoedaRequest, CliCloud.Domain.Entities.Moedas.Moeda>();
      _ = CreateMap<MoedaDtos.UpdateMoedaRequest, CliCloud.Domain.Entities.Moedas.Moeda>();

      // ---- SessaoTratamento ----
      _ = CreateMap<SessaoTratamento, SessaoTratamentoDtos.SessaoTratamentoDTO>();
      _ = CreateMap<SessaoTratamento, SessaoTratamentoDtos.SessaoTratamentoLightDTO>();
      _ = CreateMap<SessaoTratamento, SessaoTratamentoDtos.SessaoTratamentoTableDTO>()
        .ForMember(d => d.ServicosCount, o => o.MapFrom(s => s.Servicos.Count));
      _ = CreateMap<SessaoTratamentoDtos.CreateSessaoTratamentoRequest, SessaoTratamento>()
        .ForMember(d => d.TratamentoId, o => o.MapFrom(s => ToGuid(s.TratamentoId)))
        .ForMember(d => d.FisioterapeutaId, o => o.MapFrom(s => ToNullableGuid(s.FisioterapeutaId)))
        .ForMember(d => d.AuxiliarId, o => o.MapFrom(s => ToNullableGuid(s.AuxiliarId)))
        .ForMember(d => d.OutroTecnicoId, o => o.MapFrom(s => ToNullableGuid(s.OutroTecnicoId)))
        .ForMember(d => d.ReciboId, o => o.MapFrom(s => ToNullableGuid(s.ReciboId)))
        .ForMember(d => d.TipoDocumentoId, o => o.MapFrom(s => ToNullableGuid(s.TipoDocumentoId)))
        .ForMember(d => d.DocumentoId, o => o.MapFrom(s => ToNullableGuid(s.DocumentoId)))
        .ForMember(d => d.Tratamento, o => o.Ignore())
        .ForMember(d => d.Fisioterapeuta, o => o.Ignore())
        .ForMember(d => d.Auxiliar, o => o.Ignore())
        .ForMember(d => d.OutroTecnico, o => o.Ignore())
        .ForMember(d => d.Recibo, o => o.Ignore())
        .ForMember(d => d.TipoDocumento, o => o.Ignore())
        .ForMember(d => d.Documento, o => o.Ignore())
        .ForMember(d => d.Servicos, o => o.Ignore());
      _ = CreateMap<SessaoTratamentoDtos.UpdateSessaoTratamentoRequest, SessaoTratamento>()
        .ForMember(d => d.TratamentoId, o => o.MapFrom(s => ToGuid(s.TratamentoId)))
        .ForMember(d => d.FisioterapeutaId, o => o.MapFrom(s => ToNullableGuid(s.FisioterapeutaId)))
        .ForMember(d => d.AuxiliarId, o => o.MapFrom(s => ToNullableGuid(s.AuxiliarId)))
        .ForMember(d => d.OutroTecnicoId, o => o.MapFrom(s => ToNullableGuid(s.OutroTecnicoId)))
        .ForMember(d => d.ReciboId, o => o.MapFrom(s => ToNullableGuid(s.ReciboId)))
        .ForMember(d => d.TipoDocumentoId, o => o.MapFrom(s => ToNullableGuid(s.TipoDocumentoId)))
        .ForMember(d => d.DocumentoId, o => o.MapFrom(s => ToNullableGuid(s.DocumentoId)))
        .ForMember(d => d.Tratamento, o => o.Ignore())
        .ForMember(d => d.Fisioterapeuta, o => o.Ignore())
        .ForMember(d => d.Auxiliar, o => o.Ignore())
        .ForMember(d => d.OutroTecnico, o => o.Ignore())
        .ForMember(d => d.Recibo, o => o.Ignore())
        .ForMember(d => d.TipoDocumento, o => o.Ignore())
        .ForMember(d => d.Documento, o => o.Ignore())
        .ForMember(d => d.Servicos, o => o.Ignore());

      // ---- ServicoTratamento ----
      _ = CreateMap<ServicoTratamento, ServicoTratamentoDtos.ServicoTratamentoDTO>();
      _ = CreateMap<ServicoTratamento, ServicoTratamentoDtos.ServicoTratamentoLightDTO>();
      _ = CreateMap<ServicoTratamento, ServicoTratamentoDtos.ServicoTratamentoTableDTO>();
      _ = CreateMap<ServicoTratamentoDtos.CreateServicoTratamentoRequest, ServicoTratamento>()
        .ForMember(d => d.TratamentoId, o => o.MapFrom(s => ToGuid(s.TratamentoId)))
        .ForMember(d => d.ServicoId, o => o.MapFrom(s => ToNullableGuid(s.ServicoId)))
        .ForMember(d => d.SessaoTratamentoId, o => o.MapFrom(s => ToNullableGuid(s.SessaoTratamentoId)))
        .ForMember(d => d.Tratamento, o => o.Ignore())
        .ForMember(d => d.Servico, o => o.Ignore());
      _ = CreateMap<ServicoTratamentoDtos.UpdateServicoTratamentoRequest, ServicoTratamento>()
        .ForMember(d => d.TratamentoId, o => o.MapFrom(s => ToGuid(s.TratamentoId)))
        .ForMember(d => d.ServicoId, o => o.MapFrom(s => ToNullableGuid(s.ServicoId)))
        .ForMember(d => d.SessaoTratamentoId, o => o.MapFrom(s => ToNullableGuid(s.SessaoTratamentoId)))
        .ForMember(d => d.Tratamento, o => o.Ignore())
        .ForMember(d => d.Servico, o => o.Ignore());

      // ---- ServicoSessao ----
      _ = CreateMap<ServicoSessao, ServicoSessaoDtos.ServicoSessaoDTO>();
      _ = CreateMap<ServicoSessao, ServicoSessaoDtos.ServicoSessaoLightDTO>();
      _ = CreateMap<ServicoSessao, ServicoSessaoDtos.ServicoSessaoTableDTO>();
      _ = CreateMap<ServicoSessaoDtos.CreateServicoSessaoRequest, ServicoSessao>()
        .ForMember(d => d.SessaoTratamentoId, o => o.MapFrom(s => ToGuid(s.SessaoTratamentoId)))
        .ForMember(d => d.FisioterapeutaId, o => o.MapFrom(s => ToNullableGuid(s.FisioterapeutaId)))
        .ForMember(d => d.AuxiliarId, o => o.MapFrom(s => ToNullableGuid(s.AuxiliarId)))
        .ForMember(d => d.ServicoId, o => o.MapFrom(s => ToNullableGuid(s.ServicoId)))
        .ForMember(d => d.AparelhoId, o => o.MapFrom(s => ToNullableGuid(s.AparelhoId)))
        .ForMember(d => d.SessaoTratamento, o => o.Ignore())
        .ForMember(d => d.Fisioterapeuta, o => o.Ignore())
        .ForMember(d => d.Auxiliar, o => o.Ignore())
        .ForMember(d => d.Servico, o => o.Ignore())
        .ForMember(d => d.Aparelho, o => o.Ignore());
      _ = CreateMap<ServicoSessaoDtos.UpdateServicoSessaoRequest, ServicoSessao>()
        .ForMember(d => d.SessaoTratamentoId, o => o.MapFrom(s => ToGuid(s.SessaoTratamentoId)))
        .ForMember(d => d.FisioterapeutaId, o => o.MapFrom(s => ToNullableGuid(s.FisioterapeutaId)))
        .ForMember(d => d.AuxiliarId, o => o.MapFrom(s => ToNullableGuid(s.AuxiliarId)))
        .ForMember(d => d.ServicoId, o => o.MapFrom(s => ToNullableGuid(s.ServicoId)))
        .ForMember(d => d.AparelhoId, o => o.MapFrom(s => ToNullableGuid(s.AparelhoId)))
        .ForMember(d => d.SessaoTratamento, o => o.Ignore())
        .ForMember(d => d.Fisioterapeuta, o => o.Ignore())
        .ForMember(d => d.Auxiliar, o => o.Ignore())
        .ForMember(d => d.Servico, o => o.Ignore())
        .ForMember(d => d.Aparelho, o => o.Ignore());

      // ---- Aparelho ----
      _ = CreateMap<Aparelho, AparelhoDtos.AparelhoDTO>()
        .ForMember(d => d.TipoAparelhoDesignacao, o => o.MapFrom(s => s.TipoAparelho != null ? s.TipoAparelho.Designacao : null))
        .ForMember(d => d.ModeloAparelhoDesignacao, o => o.MapFrom(s => s.ModeloAparelho != null ? s.ModeloAparelho.Designacao : null))
        .ForMember(d => d.MarcaAparelhoDesignacao, o => o.MapFrom(s => s.ModeloAparelho != null && s.ModeloAparelho.MarcaAparelho != null ? s.ModeloAparelho.MarcaAparelho.Designacao : null));
      _ = CreateMap<Aparelho, AparelhoDtos.AparelhoLightDTO>();
      _ = CreateMap<Aparelho, AparelhoDtos.AparelhoTableDTO>()
        .ForMember(d => d.TipoAparelhoDesignacao, o => o.MapFrom(s => s.TipoAparelho != null ? s.TipoAparelho.Designacao : null))
        .ForMember(d => d.ModeloAparelhoDesignacao, o => o.MapFrom(s => s.ModeloAparelho != null ? s.ModeloAparelho.Designacao : null))
        .ForMember(d => d.MarcaAparelhoDesignacao, o => o.MapFrom(s => s.ModeloAparelho != null && s.ModeloAparelho.MarcaAparelho != null ? s.ModeloAparelho.MarcaAparelho.Designacao : null));
      _ = CreateMap<AparelhoDtos.CreateAparelhoRequest, Aparelho>().ForMember(d => d.TipoAparelhoId, o => o.Ignore()).ForMember(d => d.ModeloAparelho, o => o.Ignore());
      _ = CreateMap<AparelhoDtos.UpdateAparelhoRequest, Aparelho>().ForMember(d => d.TipoAparelhoId, o => o.Ignore()).ForMember(d => d.ModeloAparelho, o => o.Ignore());

      // ---- Clinica ----
      _ = CreateMap<Clinica, ClinicaDtos.ClinicaDTO>();
      _ = CreateMap<Clinica, ClinicaDtos.ClinicaLightDTO>();
      _ = CreateMap<Clinica, ClinicaDtos.ClinicaTableDTO>();
      _ = CreateMap<HistoricoEmail, CliCloud.Application.Services.Core.EmailService.DTOs.HistoricoEmailTabelaDTO>();
      // Não queremos que o payload parcial (quando ainda nem todas as abas estão mapeadas)
      // apague valores existentes. Se o campo vier null, mantemos o que está.
      var updateClinicaMap = CreateMap<ClinicaDtos.UpdateClinicaRequest, Clinica>();
      updateClinicaMap.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
      _ = updateClinicaMap
        .ForMember(d => d.NomeComercial, o => o.MapFrom(s => s.NomeComercial))
        .ForMember(d => d.Morada, o => o.MapFrom(s => s.Morada))
        .ForMember(d => d.CCPostal, o => o.MapFrom(s => s.CCPostal))
        .ForMember(d => d.Localidade, o => o.MapFrom(s => s.Localidade))
        .ForMember(d => d.IndicativoTelefone, o => o.MapFrom(s => s.IndicativoTelefone))
        .ForMember(d => d.Telefone, o => o.MapFrom(s => s.Telefone))
        .ForMember(d => d.Telemovel, o => o.MapFrom(s => s.Telemovel))
        .ForMember(d => d.Fax, o => o.MapFrom(s => s.Fax))
        .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
        .ForMember(d => d.Web, o => o.MapFrom(s => s.Web))
        .ForMember(d => d.Sucursal, o => o.MapFrom(s => s.Sucursal))
        .ForMember(d => d.NumeroContribuinte, o => o.MapFrom(s => s.NumeroContribuinte))
        .ForMember(d => d.NIB, o => o.MapFrom(s => s.NIB))
        .ForMember(d => d.Observacoes, o => o.MapFrom(s => s.Observacoes))
        .ForMember(d => d.UrlFoto, o => o.MapFrom(s => s.UrlFoto))
        .ForMember(d => d.EntidadeUtilizadora, o => o.MapFrom(s => s.EntidadeUtilizadora))
        .ForMember(d => d.LocalPrescricao, o => o.MapFrom(s => s.LocalPrescricao))
        .ForMember(d => d.NomeEtiqueta, o => o.MapFrom(s => s.NomeEtiqueta))
        .ForMember(d => d.CodSb, o => o.MapFrom(s => s.CodSb))
        .ForMember(d => d.CccDescLocalEmissao, o => o.MapFrom(s => s.CccDescLocalEmissao))
        .ForMember(d => d.Regiao, o => o.MapFrom(s => s.Regiao))
        .ForMember(d => d.DiretoriaDocumentos, o => o.MapFrom(s => s.DiretoriaDocumentos))
        .ForMember(d => d.CaminhoSaft, o => o.MapFrom(s => s.CaminhoSaft))
        .ForMember(d => d.ExportContabilidadeFa, o => o.MapFrom(s => s.ExportContabilidadeFa))
        .ForMember(d => d.ExportTipoContaFa, o => o.MapFrom(s => s.ExportTipoContaFa))
        .ForMember(d => d.ExportContabilidadeFr, o => o.MapFrom(s => s.ExportContabilidadeFr))
        .ForMember(d => d.ExportTipoContaFr, o => o.MapFrom(s => s.ExportTipoContaFr))
        .ForMember(d => d.LabelAuxiliares, o => o.MapFrom(s => s.LabelAuxiliares))
        .ForMember(d => d.MsgFaltaPagamento, o => o.MapFrom(s => s.MsgFaltaPagamento))
        .ForMember(d => d.MsgCredenciais, o => o.MapFrom(s => s.MsgCredenciais))
        .ForMember(d => d.KqueueMensagemAvisoAtraso, o => o.MapFrom(s => s.KqueueMensagemAvisoAtraso));

      // ---- Seguradora ----
      _ = CreateMap<Seguradora, SeguradoraDtos.SeguradoraDTO>();
      _ = CreateMap<Seguradora, SeguradoraDtos.SeguradoraLightDTO>();
      _ = CreateMap<Seguradora, SeguradoraDtos.SeguradoraTableDTO>();
      _ = CreateMap<SeguradoraDtos.CreateSeguradoraRequest, Seguradora>().ForMember(d => d.BancoId, o => o.Ignore());
      _ = CreateMap<SeguradoraDtos.UpdateSeguradoraRequest, Seguradora>().ForMember(d => d.BancoId, o => o.Ignore());

      // ---- Banco ----
      _ = CreateMap<Banco, BancoDtos.BancoDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => (int?)s.Status));
      _ = CreateMap<Banco, BancoDtos.BancoLightDTO>();
      _ = CreateMap<Banco, BancoDtos.BancoTableDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => (int?)s.Status));

      // ---- ContaBancaria ----
      _ = CreateMap<ContaBancaria, ContaBancariaDtos.ContaBancariaDTO>()
        .ForMember(d => d.BancoNome, o => o.MapFrom(s => s.Banco != null ? s.Banco.Nome : null));
      _ = CreateMap<ContaBancaria, ContaBancariaDtos.ContaBancariaLightDTO>();
      _ = CreateMap<ContaBancaria, ContaBancariaDtos.ContaBancariaTableDTO>()
        .ForMember(d => d.BancoNome, o => o.MapFrom(s => s.Banco != null ? s.Banco.Nome : null));
      _ = CreateMap<ContaBancariaDtos.CreateContaBancariaRequest, ContaBancaria>()
        .ForMember(d => d.Banco, o => o.Ignore());
      _ = CreateMap<ContaBancariaDtos.UpdateContaBancariaRequest, ContaBancaria>()
        .ForMember(d => d.Banco, o => o.Ignore());

      // Na criação/atualização, usamos Nome como Descricao e mapeamos Abreviatura para a entidade específica.
      _ = CreateMap<BancoDtos.CreateBancoRequest, Banco>()
        .ForMember(d => d.Descricao, o => o.MapFrom(s => s.Nome))
        .ForMember(d => d.Abreviatura, o => o.MapFrom(s => s.Abreviatura));

      _ = CreateMap<BancoDtos.UpdateBancoRequest, Banco>()
        .ForMember(d => d.Descricao, o => o.MapFrom(s => s.Nome))
        .ForMember(d => d.Abreviatura, o => o.MapFrom(s => s.Abreviatura));

      // ---- Recibo (read-only) ----
      _ = CreateMap<Recibo, ReciboDtos.ReciboDTO>();
      _ = CreateMap<Recibo, ReciboDtos.ReciboLightDTO>();
      _ = CreateMap<Recibo, ReciboDtos.ReciboTableDTO>();

      // ---- Documento fiscal / TipoDocumento ----
      _ = CreateMap<DocumentoLinha, DocumentoDtos.DocumentoLinhaDTO>();

      _ = CreateMap<Documento, DocumentoDtos.DocumentoDTO>()
        .ForMember(d => d.EstadoDocumento, o => o.MapFrom(s => (int?)s.EstadoDocumento))
        .ForMember(d => d.EstadoDocumentoLabel, o => o.MapFrom(s => s.EstadoDocumento.HasValue ? EnumDisplayHelper.GetDisplayName(s.EstadoDocumento.Value) : null))
        .ForMember(d => d.OrigemLabel, o => o.MapFrom(s => ResolveDocumentoOrigemLabel(s)))
        .ForMember(d => d.CodigoPostalCodigo, o => o.MapFrom(s => s.CodigoPostal != null ? s.CodigoPostal.Codigo : null))
        .ForMember(d => d.Linhas, o => o.MapFrom(s => s.Linhas.OrderBy(l => l.NumeroLinha)));

      _ = CreateMap<Documento, DocumentoDtos.DocumentoTableDTO>()
        .ForMember(d => d.TipoDocumentoAbreviatura, o => o.MapFrom(s => s.TipoDocumento != null ? s.TipoDocumento.Abreviatura : null))
        .ForMember(d => d.TipoSerie, o => o.MapFrom(s => s.TipoSerie))
        .ForMember(d => d.UtenteNome, o => o.MapFrom(s => s.Utente != null ? s.Utente.Nome : null))
        .ForMember(d => d.OrganismoNome, o => o.MapFrom(s => s.Organismo != null ? s.Organismo.Nome : null))
        .ForMember(d => d.FuncionarioNome, o => o.MapFrom(s => s.Funcionario != null ? s.Funcionario.Nome : null))
        .ForMember(d => d.EstadoDocumento, o => o.MapFrom(s => (int?)s.EstadoDocumento))
        .ForMember(d => d.EstadoDocumentoLabel, o => o.MapFrom(s => s.EstadoDocumento.HasValue ? EnumDisplayHelper.GetDisplayName(s.EstadoDocumento.Value) : null))
        .ForMember(d => d.OrigemLabel, o => o.MapFrom(s => ResolveDocumentoOrigemLabel(s)))
        .ForMember(d => d.ReferenciaDocumento, o => o.MapFrom(s => ResolveDocumentoReferenciaListagem(s)))
        .ForMember(d => d.AdmissoesResumo, o => o.MapFrom(s => ResolveDocumentoAdmissoesResumo(s)));

      _ = CreateMap<Documento, DocumentoDtos.DocumentoLightDTO>()
        .ForMember(d => d.TipoDocumentoAbreviatura, o => o.MapFrom(s => s.TipoDocumento != null ? s.TipoDocumento.Abreviatura : null));

      _ = CreateMap<TipoDocumento, TipoDocumentoDtos.TipoDocumentoDTO>();
      _ = CreateMap<TipoDocumento, TipoDocumentoDtos.TipoDocumentoTableDTO>();
      _ = CreateMap<TipoDocumento, TipoDocumentoDtos.TipoDocumentoLightDTO>();

      // ---- NaturezaDocumento ----
      _ = CreateMap<NaturezaDocumento, NaturezaDocumentoDtos.NaturezaDocumentoDTO>();
      _ = CreateMap<NaturezaDocumento, NaturezaDocumentoDtos.NaturezaDocumentoLightDTO>();
      _ = CreateMap<NaturezaDocumento, NaturezaDocumentoDtos.NaturezaDocumentoTableDTO>();
      _ = CreateMap<NaturezaDocumentoDtos.CreateNaturezaDocumentoRequest, NaturezaDocumento>();
      _ = CreateMap<NaturezaDocumentoDtos.UpdateNaturezaDocumentoRequest, NaturezaDocumento>();

      // ---- Utente ----
      _ = CreateMap<UtenteSubsistemaLinha, UtenteDtos.UtenteSubsistemaLinhaDTO>()
        .ForMember(d => d.Organismo, o => o.MapFrom(s => s.Organismo))
        .ForMember(d => d.Empresa, o => o.MapFrom(s => ResolveSubsistemaLinhaEmpresa(s)));
      _ = CreateMap<Utente, UtenteDtos.UtenteDTO>()
        .ForMember(d => d.EstadoCivil, o => o.MapFrom(s => s.EstadoCivil))
        .ForMember(d => d.GrupoSanguineo, o => o.MapFrom(s => s.GrupoSanguineo))
        .ForMember(d => d.ProvenienciaUtente, o => o.MapFrom(s => s.ProvenienciaUtente))
        .ForMember(d => d.Organismo, o => o.MapFrom(s => s.Organismo))
        .ForMember(d => d.Seguradora, o => o.MapFrom(s => s.SeguradoraOrganismo))
        .ForMember(d => d.Empresa, o => o.MapFrom(s => s.Empresa))
        .ForMember(d => d.CentroSaude, o => o.MapFrom(s => s.CentroSaude))
        .ForMember(d => d.MedicoExterno, o => o.MapFrom(s => s.MedicoExterno))
        .ForMember(d => d.Medico, o => o.MapFrom(s => s.Medico))
        .ForMember(d => d.SubsistemaLinhas, o => o.MapFrom(s => s.SubsistemaLinhas))
        .ForMember(d => d.Habilitacao, o => o.MapFrom(s => s.Habilitacao))
        .ForMember(d => d.Profissao, o => o.MapFrom(s => s.Profissao))
        .ForMember(d => d.Sexo, o => o.MapFrom(s => s.Sexo));
      _ = CreateMap<Utente, UtenteDtos.UtenteLightDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.HasValue ? (int?)s.Status.Value : null))
        .ForMember(d => d.Sexo, o => o.MapFrom(s => s.Sexo))
        .ForMember(d => d.EstadoCivil, o => o.MapFrom(s => s.EstadoCivil));
      _ = CreateMap<Utente, UtenteDtos.UtenteNumeroLookupDTO>();
      _ = CreateMap<Utente, UtenteDtos.UtenteTableDTO>()
        // Entidade.Status é enum nullable; no DTO usamos int? (para tabelas simples)
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.HasValue ? (int?)s.Status.Value : null))
        .ForMember(d => d.EstadoCivil, o => o.MapFrom(s => s.EstadoCivil))
        .ForMember(d => d.GrupoSanguineo, o => o.MapFrom(s => s.GrupoSanguineo))
        .ForMember(d => d.ProvenienciaUtente, o => o.MapFrom(s => s.ProvenienciaUtente))
        .ForMember(d => d.Habilitacao, o => o.MapFrom(s => s.Habilitacao))
        .ForMember(d => d.Profissao, o => o.MapFrom(s => s.Profissao))
        .ForMember(d => d.Sexo, o => o.MapFrom(s => s.Sexo))
        .ForMember(d => d.EntidadeContactos, o => o.MapFrom(s => s.EntidadeContactos));
      _ = CreateMap<UtenteDtos.CreateUtenteRequest, Utente>()
        .ForMember(d => d.EstadoCivilId, o => o.MapFrom(s => ToNullableGuid(s.EstadoCivilId)))
        .ForMember(d => d.GrupoSanguineoId, o => o.MapFrom(s => ToNullableGuid(s.GrupoSanguineoId)))
        .ForMember(d => d.ProvenienciaUtenteId, o => o.MapFrom(s => ToNullableGuid(s.ProvenienciaUtenteId)))
        .ForMember(d => d.OrganismoId, o => o.MapFrom(s => ToNullableGuid(s.OrganismoId)))
        .ForMember(d => d.SeguradoraId, o => o.MapFrom(s => ToNullableGuid(s.SeguradoraId)))
        .ForMember(d => d.EmpresaId, o => o.MapFrom(s => ToNullableGuid(s.EmpresaId)))
        .ForMember(d => d.CentroSaudeId, o => o.MapFrom(s => ToNullableGuid(s.CentroSaudeId)))
        .ForMember(d => d.MedicoExternoId, o => o.MapFrom(s => ToNullableGuid(s.MedicoExternoId)))
        .ForMember(d => d.MedicoId, o => o.MapFrom(s => ToNullableGuid(s.MedicoId)))
        .ForMember(d => d.ProvenienciaUtente, o => o.Ignore())
        .ForMember(d => d.Organismo, o => o.Ignore())
        .ForMember(d => d.SeguradoraOrganismo, o => o.Ignore())
        .ForMember(d => d.Empresa, o => o.Ignore())
        .ForMember(d => d.CentroSaude, o => o.Ignore())
        .ForMember(d => d.MedicoExterno, o => o.Ignore())
        .ForMember(d => d.Medico, o => o.Ignore())
        .ForMember(d => d.HabilitacaoId, o => o.MapFrom(s => ToNullableGuid(s.HabilitacaoId)))
        .ForMember(d => d.ProfissaoId, o => o.MapFrom(s => ToNullableGuid(s.ProfissaoId)))
        .ForMember(d => d.SexoId, o => o.MapFrom(s => ToNullableGuid(s.SexoId)))
        .ForMember(d => d.EstadoCivil, o => o.Ignore())
        .ForMember(d => d.GrupoSanguineo, o => o.Ignore())
        .ForMember(d => d.Habilitacao, o => o.Ignore())
        .ForMember(d => d.Profissao, o => o.Ignore())
        .ForMember(d => d.Sexo, o => o.Ignore())
        // Contactos são tratados fora do AutoMapper (CreateEntidadeContactoBulkAsync)
        .ForMember(d => d.EntidadeContactos, o => o.Ignore())
        .ForMember(d => d.SubsistemaLinhas, o => o.Ignore())
        .ForMember(d => d.EntidadeFinanceiraResponsavelId, o => o.MapFrom(s => ToNullableGuid(s.EntidadeFinanceiraResponsavelId)))
        .ForMember(d => d.IdUtilizador, o => o.MapFrom(s => ToNullableGuid(s.IdUtilizador)));
      _ = CreateMap<UtenteDtos.UpdateUtenteRequest, Utente>()
        .ForMember(d => d.EstadoCivilId, o => o.MapFrom(s => ToNullableGuid(s.EstadoCivilId)))
        .ForMember(d => d.GrupoSanguineoId, o => o.MapFrom(s => ToNullableGuid(s.GrupoSanguineoId)))
        .ForMember(d => d.ProvenienciaUtenteId, o => o.MapFrom(s => ToNullableGuid(s.ProvenienciaUtenteId)))
        .ForMember(d => d.OrganismoId, o => o.MapFrom(s => ToNullableGuid(s.OrganismoId)))
        .ForMember(d => d.SeguradoraId, o => o.MapFrom(s => ToNullableGuid(s.SeguradoraId)))
        .ForMember(d => d.EmpresaId, o => o.MapFrom(s => ToNullableGuid(s.EmpresaId)))
        .ForMember(d => d.CentroSaudeId, o => o.MapFrom(s => ToNullableGuid(s.CentroSaudeId)))
        .ForMember(d => d.MedicoExternoId, o => o.MapFrom(s => ToNullableGuid(s.MedicoExternoId)))
        .ForMember(d => d.MedicoId, o => o.MapFrom(s => ToNullableGuid(s.MedicoId)))
        .ForMember(d => d.ProvenienciaUtente, o => o.Ignore())
        .ForMember(d => d.Organismo, o => o.Ignore())
        .ForMember(d => d.SeguradoraOrganismo, o => o.Ignore())
        .ForMember(d => d.Empresa, o => o.Ignore())
        .ForMember(d => d.CentroSaude, o => o.Ignore())
        .ForMember(d => d.MedicoExterno, o => o.Ignore())
        .ForMember(d => d.Medico, o => o.Ignore())
        .ForMember(d => d.HabilitacaoId, o => o.MapFrom(s => ToNullableGuid(s.HabilitacaoId)))
        .ForMember(d => d.ProfissaoId, o => o.MapFrom(s => ToNullableGuid(s.ProfissaoId)))
        .ForMember(d => d.SexoId, o => o.MapFrom(s => ToNullableGuid(s.SexoId)))
        .ForMember(d => d.EstadoCivil, o => o.Ignore())
        .ForMember(d => d.GrupoSanguineo, o => o.Ignore())
        .ForMember(d => d.Habilitacao, o => o.Ignore())
        .ForMember(d => d.Profissao, o => o.Ignore())
        .ForMember(d => d.Sexo, o => o.Ignore())
        // Contactos são tratados fora do AutoMapper (UpsertEntidadeContactoBulkAsync)
        .ForMember(d => d.EntidadeContactos, o => o.Ignore())
        .ForMember(d => d.SubsistemaLinhas, o => o.Ignore())
        .ForMember(d => d.EntidadeFinanceiraResponsavelId, o => o.MapFrom(s => ToNullableGuid(s.EntidadeFinanceiraResponsavelId)))
        .ForMember(d => d.IdUtilizador, o => o.MapFrom(s => ToNullableGuid(s.IdUtilizador)));
        _ = CreateMap<UtentePatologiaComparticipacao, UtentePatologiaComparticipacaoDtos.UtentePatologiaComparticipacaoDTO>();
        _ = CreateMap<UtentePatologiaComparticipacaoDtos.CreateUtentePatologiaComparticipacaoRequest, UtentePatologiaComparticipacao>();

      // ---- Medico ----
      _ = CreateMap<Medico, MedicoDtos.MedicoDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.HasValue ? (int?)s.Status.Value : null))
        .ForMember(d => d.EstadoCivil, o => o.MapFrom(s => s.EstadoCivil))
        .ForMember(d => d.Sexo, o => o.MapFrom(s => s.Sexo));
      _ = CreateMap<Medico, MedicoDtos.MedicoLightDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.HasValue ? (int?)s.Status.Value : null))
        .ForMember(d => d.Sexo, o => o.MapFrom(s => s.Sexo))
        .ForMember(d => d.EstadoCivil, o => o.MapFrom(s => s.EstadoCivil));
      _ = CreateMap<Medico, MedicoDtos.MedicoTableDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.HasValue ? (int?)s.Status.Value : null))
        .ForMember(d => d.Sexo, o => o.MapFrom(s => s.Sexo))
        .ForMember(d => d.EstadoCivil, o => o.MapFrom(s => s.EstadoCivil));
      _ = CreateMap<MedicoDtos.CreateMedicoRequest, Medico>()
        .ForMember(d => d.EstadoCivilId, o => o.MapFrom(s => ToNullableGuid(s.EstadoCivilId)))
        .ForMember(d => d.SexoId, o => o.MapFrom(s => ToNullableGuid(s.SexoId)))
        .ForMember(d => d.EstadoCivil, o => o.Ignore())
        .ForMember(d => d.Sexo, o => o.Ignore())
        .ForMember(d => d.EntidadeContactos, o => o.Ignore());
      _ = CreateMap<MedicoDtos.UpdateMedicoRequest, Medico>()
        .ForMember(d => d.EstadoCivilId, o => o.MapFrom(s => ToNullableGuid(s.EstadoCivilId)))
        .ForMember(d => d.SexoId, o => o.MapFrom(s => ToNullableGuid(s.SexoId)))
        .ForMember(d => d.EstadoCivil, o => o.Ignore())
        .ForMember(d => d.Sexo, o => o.Ignore())
        .ForMember(d => d.EntidadeContactos, o => o.Ignore());

      // ---- MedicoExterno ----
      _ = CreateMap<MedicoExterno, MedicoExternoDtos.MedicoExternoDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.HasValue ? (int?)s.Status.Value : null))
        .ForMember(d => d.Sexo, o => o.MapFrom(s => s.Sexo))
        .ForMember(d => d.EstadoCivil, o => o.MapFrom(s => s.EstadoCivil))
        .ForMember(d => d.Rua, o => o.MapFrom(s => s.Rua))
        .ForMember(d => d.CodigoPostal, o => o.MapFrom(s => s.CodigoPostal))
        .ForMember(d => d.Freguesia, o => o.MapFrom(s => s.Freguesia))
        .ForMember(d => d.Concelho, o => o.MapFrom(s => s.Concelho))
        .ForMember(d => d.Distrito, o => o.MapFrom(s => s.Distrito))
        .ForMember(d => d.Pais, o => o.MapFrom(s => s.Pais))
        .ForMember(d => d.EntidadeContactos, o => o.MapFrom(s => s.EntidadeContactos));
      _ = CreateMap<MedicoExterno, MedicoExternoDtos.MedicoExternoLightDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.HasValue ? (int?)s.Status.Value : null))
        .ForMember(d => d.Sexo, o => o.MapFrom(s => s.Sexo))
        .ForMember(d => d.EstadoCivil, o => o.MapFrom(s => s.EstadoCivil))
        .ForMember(d => d.RuaNome, o => o.MapFrom(s => s.Rua != null ? s.Rua.Nome : null))
        .ForMember(d => d.CodigoPostalCodigo, o => o.MapFrom(s => s.CodigoPostal != null ? s.CodigoPostal.Codigo : null))
        .ForMember(d => d.CodigoPostalLocalidade, o => o.MapFrom(s => s.CodigoPostal != null ? s.CodigoPostal.Localidade : null))
        .ForMember(d => d.FreguesiaNome, o => o.MapFrom(s => s.Freguesia != null ? s.Freguesia.Nome : null))
        .ForMember(d => d.ConcelhoNome, o => o.MapFrom(s => s.Concelho != null ? s.Concelho.Nome : null))
        .ForMember(d => d.DistritoNome, o => o.MapFrom(s => s.Distrito != null ? s.Distrito.Nome : null))
        .ForMember(d => d.PaisNome, o => o.MapFrom(s => s.Pais != null ? s.Pais.Nome : null));
      _ = CreateMap<MedicoExterno, MedicoExternoDtos.MedicoExternoTableDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.HasValue ? (int?)s.Status.Value : null))
        .ForMember(d => d.Sexo, o => o.MapFrom(s => s.Sexo))
        .ForMember(d => d.EstadoCivil, o => o.MapFrom(s => s.EstadoCivil))
        .ForMember(d => d.Rua, o => o.MapFrom(s => s.Rua))
        .ForMember(d => d.CodigoPostal, o => o.MapFrom(s => s.CodigoPostal))
        .ForMember(d => d.Freguesia, o => o.MapFrom(s => s.Freguesia))
        .ForMember(d => d.Concelho, o => o.MapFrom(s => s.Concelho))
        .ForMember(d => d.Distrito, o => o.MapFrom(s => s.Distrito))
        .ForMember(d => d.Pais, o => o.MapFrom(s => s.Pais))
        .ForMember(d => d.ContactoCount, o => o.MapFrom(s => s.EntidadeContactos != null ? s.EntidadeContactos.Count : 0));

      // ---- Funcionario ----
      _ = CreateMap<Funcionario, FuncionarioDtos.FuncionarioDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.HasValue ? (int?)s.Status.Value : null))
        .ForMember(d => d.Sexo, o => o.MapFrom(s => s.Sexo))
        .ForMember(d => d.EstadoCivil, o => o.MapFrom(s => s.EstadoCivil))
        .ForMember(d => d.Rua, o => o.MapFrom(s => s.Rua))
        .ForMember(d => d.CodigoPostal, o => o.MapFrom(s => s.CodigoPostal))
        .ForMember(d => d.Freguesia, o => o.MapFrom(s => s.Freguesia))
        .ForMember(d => d.Concelho, o => o.MapFrom(s => s.Concelho))
        .ForMember(d => d.Distrito, o => o.MapFrom(s => s.Distrito))
        .ForMember(d => d.Pais, o => o.MapFrom(s => s.Pais))
        .ForMember(d => d.EntidadeContactos, o => o.MapFrom(s => s.EntidadeContactos));
      _ = CreateMap<Funcionario, FuncionarioDtos.FuncionarioLightDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.HasValue ? (int?)s.Status.Value : null))
        .ForMember(d => d.Sexo, o => o.MapFrom(s => s.Sexo))
        .ForMember(d => d.EstadoCivil, o => o.MapFrom(s => s.EstadoCivil))
        .ForMember(d => d.RuaNome, o => o.MapFrom(s => s.Rua != null ? s.Rua.Nome : null))
        .ForMember(d => d.CodigoPostalCodigo, o => o.MapFrom(s => s.CodigoPostal != null ? s.CodigoPostal.Codigo : null))
        .ForMember(d => d.CodigoPostalLocalidade, o => o.MapFrom(s => s.CodigoPostal != null ? s.CodigoPostal.Localidade : null))
        .ForMember(d => d.FreguesiaNome, o => o.MapFrom(s => s.Freguesia != null ? s.Freguesia.Nome : null))
        .ForMember(d => d.ConcelhoNome, o => o.MapFrom(s => s.Concelho != null ? s.Concelho.Nome : null))
        .ForMember(d => d.DistritoNome, o => o.MapFrom(s => s.Distrito != null ? s.Distrito.Nome : null))
        .ForMember(d => d.PaisNome, o => o.MapFrom(s => s.Pais != null ? s.Pais.Nome : null));
      _ = CreateMap<Funcionario, FuncionarioDtos.FuncionarioTableDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.HasValue ? (int?)s.Status.Value : null))
        .ForMember(d => d.Sexo, o => o.MapFrom(s => s.Sexo))
        .ForMember(d => d.EstadoCivil, o => o.MapFrom(s => s.EstadoCivil))
        .ForMember(d => d.Rua, o => o.MapFrom(s => s.Rua))
        .ForMember(d => d.CodigoPostal, o => o.MapFrom(s => s.CodigoPostal))
        .ForMember(d => d.Freguesia, o => o.MapFrom(s => s.Freguesia))
        .ForMember(d => d.Concelho, o => o.MapFrom(s => s.Concelho))
        .ForMember(d => d.Distrito, o => o.MapFrom(s => s.Distrito))
        .ForMember(d => d.Pais, o => o.MapFrom(s => s.Pais))
        .ForMember(d => d.ContactoCount, o => o.MapFrom(s => s.EntidadeContactos != null ? s.EntidadeContactos.Count : 0))
        .ForMember(d => d.Contacto, o => o.MapFrom(s => s.EntidadeContactos != null ? s.EntidadeContactos.Where(c => c.EntidadeContactoTipoId == 1).Select(c => c.Valor).FirstOrDefault() : null));
      _ = CreateMap<FuncionarioDtos.CreateFuncionarioRequest, Funcionario>()
        .ForMember(d => d.SexoId, o => o.MapFrom(s => ToNullableGuid(s.SexoId)))
        .ForMember(d => d.RuaId, o => o.MapFrom(s => ToNullableGuid(s.RuaId)))
        .ForMember(d => d.CodigoPostalId, o => o.MapFrom(s => ToNullableGuid(s.CodigoPostalId)))
        .ForMember(d => d.FreguesiaId, o => o.MapFrom(s => ToNullableGuid(s.FreguesiaId)))
        .ForMember(d => d.ConcelhoId, o => o.MapFrom(s => ToNullableGuid(s.ConcelhoId)))
        .ForMember(d => d.DistritoId, o => o.MapFrom(s => ToNullableGuid(s.DistritoId)))
        .ForMember(d => d.PaisId, o => o.MapFrom(s => ToNullableGuid(s.PaisId)))
        .ForMember(d => d.Sexo, o => o.Ignore())
        .ForMember(d => d.Rua, o => o.Ignore())
        .ForMember(d => d.CodigoPostal, o => o.Ignore())
        .ForMember(d => d.Freguesia, o => o.Ignore())
        .ForMember(d => d.Concelho, o => o.Ignore())
        .ForMember(d => d.Distrito, o => o.Ignore())
        .ForMember(d => d.Pais, o => o.Ignore())
        .ForMember(d => d.EntidadeContactos, o => o.Ignore());
      _ = CreateMap<FuncionarioDtos.UpdateFuncionarioRequest, Funcionario>()
        .ForMember(d => d.SexoId, o => o.MapFrom(s => ToNullableGuid(s.SexoId)))
        .ForMember(d => d.RuaId, o => o.MapFrom(s => ToNullableGuid(s.RuaId)))
        .ForMember(d => d.CodigoPostalId, o => o.MapFrom(s => ToNullableGuid(s.CodigoPostalId)))
        .ForMember(d => d.FreguesiaId, o => o.MapFrom(s => ToNullableGuid(s.FreguesiaId)))
        .ForMember(d => d.ConcelhoId, o => o.MapFrom(s => ToNullableGuid(s.ConcelhoId)))
        .ForMember(d => d.DistritoId, o => o.MapFrom(s => ToNullableGuid(s.DistritoId)))
        .ForMember(d => d.PaisId, o => o.MapFrom(s => ToNullableGuid(s.PaisId)))
        .ForMember(d => d.Sexo, o => o.Ignore())
        .ForMember(d => d.Rua, o => o.Ignore())
        .ForMember(d => d.CodigoPostal, o => o.Ignore())
        .ForMember(d => d.Freguesia, o => o.Ignore())
        .ForMember(d => d.Concelho, o => o.Ignore())
        .ForMember(d => d.Distrito, o => o.Ignore())
        .ForMember(d => d.Pais, o => o.Ignore())
        .ForMember(d => d.EntidadeContactos, o => o.Ignore());

      // ---- Tecnico ----
      _ = CreateMap<CliCloud.Domain.Entities.Tecnicos.Tecnico, TecnicoDtos.TecnicoDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.HasValue ? (int?)s.Status.Value : null))
        .ForMember(d => d.Rua, o => o.MapFrom(s => s.Rua))
        .ForMember(d => d.CodigoPostal, o => o.MapFrom(s => s.CodigoPostal))
        .ForMember(d => d.Freguesia, o => o.MapFrom(s => s.Freguesia))
        .ForMember(d => d.Concelho, o => o.MapFrom(s => s.Concelho))
        .ForMember(d => d.Distrito, o => o.MapFrom(s => s.Distrito))
        .ForMember(d => d.Pais, o => o.MapFrom(s => s.Pais))
        .ForMember(d => d.Sexo, o => o.MapFrom(s => s.Sexo))
        .ForMember(d => d.EstadoCivil, o => o.MapFrom(s => s.EstadoCivil))
        .ForMember(d => d.EntidadeContactos, o => o.MapFrom(s => s.EntidadeContactos));

      _ = CreateMap<CliCloud.Domain.Entities.Tecnicos.Tecnico, TecnicoDtos.TecnicoLightDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.HasValue ? (int?)s.Status.Value : null))
        .ForMember(d => d.RuaNome, o => o.MapFrom(s => s.Rua != null ? s.Rua.Nome : null))
        .ForMember(d => d.CodigoPostalCodigo, o => o.MapFrom(s => s.CodigoPostal != null ? s.CodigoPostal.Codigo : null))
        .ForMember(d => d.CodigoPostalLocalidade, o => o.MapFrom(s => s.CodigoPostal != null ? s.CodigoPostal.Localidade : null))
        .ForMember(d => d.FreguesiaNome, o => o.MapFrom(s => s.Freguesia != null ? s.Freguesia.Nome : null))
        .ForMember(d => d.ConcelhoNome, o => o.MapFrom(s => s.Concelho != null ? s.Concelho.Nome : null))
        .ForMember(d => d.DistritoNome, o => o.MapFrom(s => s.Distrito != null ? s.Distrito.Nome : null))
        .ForMember(d => d.PaisNome, o => o.MapFrom(s => s.Pais != null ? s.Pais.Nome : null))
        .ForMember(d => d.Sexo, o => o.MapFrom(s => s.Sexo))
        .ForMember(d => d.EstadoCivil, o => o.MapFrom(s => s.EstadoCivil));

      _ = CreateMap<CliCloud.Domain.Entities.Tecnicos.Tecnico, TecnicoDtos.TecnicoTableDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.HasValue ? (int?)s.Status.Value : null))
        .ForMember(d => d.Rua, o => o.MapFrom(s => s.Rua))
        .ForMember(d => d.CodigoPostal, o => o.MapFrom(s => s.CodigoPostal))
        .ForMember(d => d.Freguesia, o => o.MapFrom(s => s.Freguesia))
        .ForMember(d => d.Concelho, o => o.MapFrom(s => s.Concelho))
        .ForMember(d => d.Distrito, o => o.MapFrom(s => s.Distrito))
        .ForMember(d => d.Pais, o => o.MapFrom(s => s.Pais))
        .ForMember(d => d.ContactoCount, o => o.MapFrom(s => s.EntidadeContactos != null ? s.EntidadeContactos.Count : 0))
        .ForMember(d => d.Contacto, o => o.MapFrom(s => s.EntidadeContactos != null ? s.EntidadeContactos.Where(c => c.EntidadeContactoTipoId == 1).Select(c => c.Valor).FirstOrDefault() ?? s.EntidadeContactos.Where(c => c.EntidadeContactoTipoId == 2).Select(c => c.Valor).FirstOrDefault() : null));

      _ = CreateMap<TecnicoDtos.CreateTecnicoRequest, CliCloud.Domain.Entities.Tecnicos.Tecnico>()
        .ForMember(d => d.SexoId, o => o.MapFrom(s => ToNullableGuid(s.SexoId)))
        .ForMember(d => d.RuaId, o => o.MapFrom(s => ToNullableGuid(s.RuaId)))
        .ForMember(d => d.CodigoPostalId, o => o.MapFrom(s => ToNullableGuid(s.CodigoPostalId)))
        .ForMember(d => d.FreguesiaId, o => o.MapFrom(s => ToNullableGuid(s.FreguesiaId)))
        .ForMember(d => d.ConcelhoId, o => o.MapFrom(s => ToNullableGuid(s.ConcelhoId)))
        .ForMember(d => d.DistritoId, o => o.MapFrom(s => ToNullableGuid(s.DistritoId)))
        .ForMember(d => d.PaisId, o => o.MapFrom(s => ToNullableGuid(s.PaisId)))
        .ForMember(d => d.Sexo, o => o.Ignore())
        .ForMember(d => d.Rua, o => o.Ignore())
        .ForMember(d => d.CodigoPostal, o => o.Ignore())
        .ForMember(d => d.Freguesia, o => o.Ignore())
        .ForMember(d => d.Concelho, o => o.Ignore())
        .ForMember(d => d.Distrito, o => o.Ignore())
        .ForMember(d => d.Pais, o => o.Ignore())
        .ForMember(d => d.EntidadeContactos, o => o.Ignore());

      _ = CreateMap<TecnicoDtos.UpdateTecnicoRequest, CliCloud.Domain.Entities.Tecnicos.Tecnico>()
        .ForMember(d => d.SexoId, o => o.MapFrom(s => ToNullableGuid(s.SexoId)))
        .ForMember(d => d.RuaId, o => o.MapFrom(s => ToNullableGuid(s.RuaId)))
        .ForMember(d => d.CodigoPostalId, o => o.MapFrom(s => ToNullableGuid(s.CodigoPostalId)))
        .ForMember(d => d.FreguesiaId, o => o.MapFrom(s => ToNullableGuid(s.FreguesiaId)))
        .ForMember(d => d.ConcelhoId, o => o.MapFrom(s => ToNullableGuid(s.ConcelhoId)))
        .ForMember(d => d.DistritoId, o => o.MapFrom(s => ToNullableGuid(s.DistritoId)))
        .ForMember(d => d.PaisId, o => o.MapFrom(s => ToNullableGuid(s.PaisId)))
        .ForMember(d => d.Sexo, o => o.Ignore())
        .ForMember(d => d.Rua, o => o.Ignore())
        .ForMember(d => d.CodigoPostal, o => o.Ignore())
        .ForMember(d => d.Freguesia, o => o.Ignore())
        .ForMember(d => d.Concelho, o => o.Ignore())
        .ForMember(d => d.Distrito, o => o.Ignore())
        .ForMember(d => d.Pais, o => o.Ignore())
        .ForMember(d => d.EntidadeContactos, o => o.Ignore());

      // ---- MedicoExterno - SexoId mapping (Request -> Entity)
      _ = CreateMap<MedicoExternoDtos.CreateMedicoExternoRequest, MedicoExterno>()
        .ForMember(d => d.SexoId, o => o.MapFrom(s => ToNullableGuid(s.SexoId)))
        .ForMember(d => d.Sexo, o => o.Ignore());
      _ = CreateMap<MedicoExternoDtos.UpdateMedicoExternoRequest, MedicoExterno>()
        .ForMember(d => d.SexoId, o => o.MapFrom(s => ToNullableGuid(s.SexoId)))
        .ForMember(d => d.Sexo, o => o.Ignore());

      // ---- HorarioMedico ----
      _ = CreateMap<HorarioMedico, HorarioMedicoDtos.HorarioMedicoDTO>();
      _ = CreateMap<HorarioMedicoDtos.CreateHorarioMedicoRequest, HorarioMedico>()
        .ForMember(d => d.MedicoId, o => o.Ignore())
        .ForMember(d => d.MinMarcacao, o => o.Ignore())
        .ForMember(d => d.PrimeiraConsulta, o => o.Ignore())
        .ForMember(d => d.Medico, o => o.Ignore())
        .ForMember(d => d.Horarios, o => o.Ignore());
      _ = CreateMap<HorarioMedicoDtos.UpdateHorarioMedicoRequest, HorarioMedico>()
        .ForMember(d => d.MedicoId, o => o.Ignore())
        .ForMember(d => d.MinMarcacao, o => o.Ignore())
        .ForMember(d => d.PrimeiraConsulta, o => o.Ignore())
        .ForMember(d => d.Medico, o => o.Ignore())
        .ForMember(d => d.Horarios, o => o.Ignore());
      // HorarioMedicoDiaDTO.HorarioMedico é HorarioMedicoLightDTO (não a entidade)
      _ = CreateMap<HorarioMedico, HorarioMedicoDtos.HorarioMedicoLightDTO>()
        .ForMember(d => d.MedicoNome, o => o.MapFrom(s => s.Medico != null ? s.Medico.Nome : null));

      // ---- ViaAdministracao ----
      _ = CreateMap<CliCloud.Domain.Entities.Artigos.ViaAdministracao, ViaAdministracaoDtos.ViaAdministracaoDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.Artigos.ViaAdministracao, ViaAdministracaoDtos.ViaAdministracaoTableDTO>();
      _ = CreateMap<ViaAdministracaoDtos.CreateViaAdministracaoRequest, CliCloud.Domain.Entities.Artigos.ViaAdministracao>();
      _ = CreateMap<ViaAdministracaoDtos.UpdateViaAdministracaoRequest, CliCloud.Domain.Entities.Artigos.ViaAdministracao>();

      // ---- GrupoViasAdministracao ----
      _ = CreateMap<CliCloud.Domain.Entities.Artigos.GrupoViasAdministracaoLinha, GrupoViasAdministracaoDtos.GrupoViasAdministracaoLinhaItemDTO>()
        .ForMember(d => d.ViaDescricao, o => o.MapFrom(s => s.Via != null ? s.Via.Descricao : (string?)null));
      _ = CreateMap<CliCloud.Domain.Entities.Artigos.GrupoViasAdministracao, GrupoViasAdministracaoDtos.GrupoViasAdministracaoDTO>()
        .ForMember(d => d.Vias, o => o.MapFrom(s => s.Vias.OrderBy(l => l.Linha)));
      _ = CreateMap<CliCloud.Domain.Entities.Artigos.GrupoViasAdministracao, GrupoViasAdministracaoDtos.GrupoViasAdministracaoTableDTO>();
      _ = CreateMap<GrupoViasAdministracaoDtos.CreateGrupoViasAdministracaoRequest, CliCloud.Domain.Entities.Artigos.GrupoViasAdministracao>()
        .ForMember(d => d.Vias, o => o.Ignore());
      _ = CreateMap<GrupoViasAdministracaoDtos.UpdateGrupoViasAdministracaoRequest, CliCloud.Domain.Entities.Artigos.GrupoViasAdministracao>()
        .ForMember(d => d.Vias, o => o.Ignore());

      // ---- HorarioTecnico ----
      _ = CreateMap<CliCloud.Domain.Entities.Tecnicos.HorarioTecnico, CliCloud.Application.Services.Tecnicos.HorarioTecnicoService.DTOs.HorarioTecnicoDTO>();
      _ = CreateMap<CliCloud.Application.Services.Tecnicos.HorarioTecnicoService.DTOs.CreateHorarioTecnicoRequest, CliCloud.Domain.Entities.Tecnicos.HorarioTecnico>()
        .ForMember(d => d.TecnicoId, o => o.Ignore())
        .ForMember(d => d.Tecnico, o => o.Ignore())
        .ForMember(d => d.Horarios, o => o.Ignore());
      _ = CreateMap<CliCloud.Application.Services.Tecnicos.HorarioTecnicoService.DTOs.UpdateHorarioTecnicoRequest, CliCloud.Domain.Entities.Tecnicos.HorarioTecnico>()
        .ForMember(d => d.TecnicoId, o => o.Ignore())
        .ForMember(d => d.Tecnico, o => o.Ignore())
        .ForMember(d => d.Horarios, o => o.Ignore());

      // ---- HorarioMedicoDia ----
      _ = CreateMap<HorarioMedicoDia, HorarioMedicoDiaDtos.HorarioMedicoDiaDTO>();
      _ = CreateMap<HorarioMedicoDiaDtos.CreateHorarioMedicoDiaRequest, HorarioMedicoDia>()
        .ForMember(d => d.HorarioMedicoId, o => o.Ignore())
        .ForMember(d => d.Inicio, o => o.Ignore())
        .ForMember(d => d.Fim, o => o.Ignore())
        .ForMember(d => d.HorarioMedico, o => o.Ignore());
      _ = CreateMap<HorarioMedicoDiaDtos.UpdateHorarioMedicoDiaRequest, HorarioMedicoDia>()
        .ForMember(d => d.HorarioMedicoId, o => o.Ignore())
        .ForMember(d => d.Inicio, o => o.Ignore())
        .ForMember(d => d.Fim, o => o.Ignore())
        .ForMember(d => d.HorarioMedico, o => o.Ignore());

      // ---- HorarioTecnicoDia ----
      _ = CreateMap<CliCloud.Domain.Entities.Tecnicos.HorarioTecnicoDia, HorarioTecnicoDiaDtos.HorarioTecnicoDiaDTO>();
      _ = CreateMap<HorarioTecnicoDiaDtos.CreateHorarioTecnicoDiaRequest, CliCloud.Domain.Entities.Tecnicos.HorarioTecnicoDia>()
        .ForMember(d => d.HorarioTecnicoId, o => o.Ignore())
        .ForMember(d => d.HorarioTecnico, o => o.Ignore());
      _ = CreateMap<HorarioTecnicoDiaDtos.UpdateHorarioTecnicoDiaRequest, CliCloud.Domain.Entities.Tecnicos.HorarioTecnicoDia>()
        .ForMember(d => d.HorarioTecnicoId, o => o.Ignore())
        .ForMember(d => d.HorarioTecnico, o => o.Ignore());

      // ---- HorarioMedicoVariavel ----
      _ = CreateMap<HorarioMedicoVariavel, HorarioMedicoVariavelDtos.HorarioMedicoVariavelDTO>()
        .ForMember(d => d.ManhaInicio, o => o.Ignore())
        .ForMember(d => d.ManhaFim, o => o.Ignore())
        .ForMember(d => d.TardeInicio, o => o.Ignore())
        .ForMember(d => d.TardeFim, o => o.Ignore());
      _ = CreateMap<HorarioMedicoVariavelDtos.CreateHorarioMedicoVariavelRequest, HorarioMedicoVariavel>()
        .ForMember(d => d.MedicoId, o => o.Ignore())
        .ForMember(d => d.ManhaInicio, o => o.Ignore())
        .ForMember(d => d.ManhaFim, o => o.Ignore())
        .ForMember(d => d.TardeInicio, o => o.Ignore())
        .ForMember(d => d.TardeFim, o => o.Ignore())
        .ForMember(d => d.Medico, o => o.Ignore());

      // ---- HorarioTecnicoVariavel ----
      _ = CreateMap<CliCloud.Domain.Entities.Tecnicos.HorarioTecnicoVariavel, CliCloud.Application.Services.Tecnicos.HorarioTecnicoVariavelService.DTOs.HorarioTecnicoVariavelDTO>()
        .ForMember(d => d.ManhaInicio, o => o.Ignore())
        .ForMember(d => d.ManhaFim, o => o.Ignore())
        .ForMember(d => d.TardeInicio, o => o.Ignore())
        .ForMember(d => d.TardeFim, o => o.Ignore());
      _ = CreateMap<CliCloud.Application.Services.Tecnicos.HorarioTecnicoVariavelService.DTOs.CreateHorarioTecnicoVariavelRequest, CliCloud.Domain.Entities.Tecnicos.HorarioTecnicoVariavel>()
        .ForMember(d => d.TecnicoId, o => o.Ignore())
        .ForMember(d => d.ManhaInicio, o => o.Ignore())
        .ForMember(d => d.ManhaFim, o => o.Ignore())
        .ForMember(d => d.TardeInicio, o => o.Ignore())
        .ForMember(d => d.TardeFim, o => o.Ignore())
        .ForMember(d => d.Tecnico, o => o.Ignore());
      _ = CreateMap<CliCloud.Application.Services.Tecnicos.HorarioTecnicoVariavelService.DTOs.UpdateHorarioTecnicoVariavelRequest, CliCloud.Domain.Entities.Tecnicos.HorarioTecnicoVariavel>()
        .ForMember(d => d.TecnicoId, o => o.Ignore())
        .ForMember(d => d.ManhaInicio, o => o.Ignore())
        .ForMember(d => d.ManhaFim, o => o.Ignore())
        .ForMember(d => d.TardeInicio, o => o.Ignore())
        .ForMember(d => d.TardeFim, o => o.Ignore())
        .ForMember(d => d.Tecnico, o => o.Ignore());
      _ = CreateMap<HorarioMedicoVariavelDtos.UpdateHorarioMedicoVariavelRequest, HorarioMedicoVariavel>()
        .ForMember(d => d.MedicoId, o => o.Ignore())
        .ForMember(d => d.ManhaInicio, o => o.Ignore())
        .ForMember(d => d.ManhaFim, o => o.Ignore())
        .ForMember(d => d.TardeInicio, o => o.Ignore())
        .ForMember(d => d.TardeFim, o => o.Ignore())
        .ForMember(d => d.Medico, o => o.Ignore());

      // ---- FolgasMedico ----
      _ = CreateMap<FolgasMedico, FolgasMedicoDtos.FolgasMedicoDTO>()
        .ForMember(d => d.ManhaInicio, o => o.Ignore())
        .ForMember(d => d.ManhaFim, o => o.Ignore())
        .ForMember(d => d.TardeInicio, o => o.Ignore())
        .ForMember(d => d.TardeFim, o => o.Ignore());
      _ = CreateMap<FolgasMedicoDtos.CreateFolgasMedicoRequest, FolgasMedico>()
        .ForMember(d => d.MedicoId, o => o.Ignore())
        .ForMember(d => d.ManhaInicio, o => o.Ignore())
        .ForMember(d => d.ManhaFim, o => o.Ignore())
        .ForMember(d => d.TardeInicio, o => o.Ignore())
        .ForMember(d => d.TardeFim, o => o.Ignore())
        .ForMember(d => d.Medico, o => o.Ignore());

      // ---- FolgasTecnico ----
      _ = CreateMap<CliCloud.Domain.Entities.Tecnicos.FolgasTecnico, CliCloud.Application.Services.Tecnicos.FolgasTecnicoService.DTOs.FolgasTecnicoDTO>()
        .ForMember(d => d.ManhaInicio, o => o.Ignore())
        .ForMember(d => d.ManhaFim, o => o.Ignore())
        .ForMember(d => d.TardeInicio, o => o.Ignore())
        .ForMember(d => d.TardeFim, o => o.Ignore());
      _ = CreateMap<CliCloud.Application.Services.Tecnicos.FolgasTecnicoService.DTOs.CreateFolgasTecnicoRequest, CliCloud.Domain.Entities.Tecnicos.FolgasTecnico>()
        .ForMember(d => d.TecnicoId, o => o.Ignore())
        .ForMember(d => d.ManhaInicio, o => o.Ignore())
        .ForMember(d => d.ManhaFim, o => o.Ignore())
        .ForMember(d => d.TardeInicio, o => o.Ignore())
        .ForMember(d => d.TardeFim, o => o.Ignore())
        .ForMember(d => d.Tecnico, o => o.Ignore());
      _ = CreateMap<CliCloud.Application.Services.Tecnicos.FolgasTecnicoService.DTOs.UpdateFolgasTecnicoRequest, CliCloud.Domain.Entities.Tecnicos.FolgasTecnico>()
        .ForMember(d => d.TecnicoId, o => o.Ignore())
        .ForMember(d => d.ManhaInicio, o => o.Ignore())
        .ForMember(d => d.ManhaFim, o => o.Ignore())
        .ForMember(d => d.TardeInicio, o => o.Ignore())
        .ForMember(d => d.TardeFim, o => o.Ignore())
        .ForMember(d => d.Tecnico, o => o.Ignore());
      _ = CreateMap<FolgasMedicoDtos.UpdateFolgasMedicoRequest, FolgasMedico>()
        .ForMember(d => d.MedicoId, o => o.Ignore())
        .ForMember(d => d.ManhaInicio, o => o.Ignore())
        .ForMember(d => d.ManhaFim, o => o.Ignore())
        .ForMember(d => d.TardeInicio, o => o.Ignore())
        .ForMember(d => d.TardeFim, o => o.Ignore())
        .ForMember(d => d.Medico, o => o.Ignore());

      // ---- MargemMedico ----
      _ = CreateMap<MargemMedico, MargemMedicoDtos.MargemMedicoDTO>()
        .ForMember(d => d.ServicoDesignacao, o => o.MapFrom(s => s.Servico != null ? s.Servico.Designacao : null));
      _ = CreateMap<MargemMedico, MargemMedicoDtos.MargemMedicoLightDTO>()
        .ForMember(d => d.ServicoDesignacao, o => o.MapFrom(s => s.Servico != null ? s.Servico.Designacao : null))
        .ForMember(d => d.MedicoNome, o => o.MapFrom(s => s.Medico != null ? s.Medico.Nome : null));
      _ = CreateMap<MargemMedico, MargemMedicoDtos.MargemMedicoTableDTO>()
        .ForMember(d => d.ServicoDesignacao, o => o.MapFrom(s => s.Servico != null ? s.Servico.Designacao : null))
        .ForMember(d => d.MedicoNome, o => o.MapFrom(s => s.Medico != null ? s.Medico.Nome : null))
        .ForMember(d => d.MedicoNumeroContribuinte, o => o.MapFrom(s => s.Medico != null ? s.Medico.NumeroContribuinte : null));
      _ = CreateMap<MargemMedicoDtos.CreateMargemMedicoRequest, MargemMedico>()
        .ForMember(d => d.ServicoId, o => o.Ignore())
        .ForMember(d => d.MedicoId, o => o.Ignore())
        .ForMember(d => d.Servico, o => o.Ignore())
        .ForMember(d => d.Medico, o => o.Ignore());
      _ = CreateMap<MargemMedicoDtos.UpdateMargemMedicoRequest, MargemMedico>()
        .ForMember(d => d.ServicoId, o => o.Ignore())
        .ForMember(d => d.MedicoId, o => o.Ignore())
        .ForMember(d => d.Servico, o => o.Ignore())
        .ForMember(d => d.Medico, o => o.Ignore());

      // ---- Alergia ----
      _ = CreateMap<Alergia, AlergiaDtos.AlergiaDTO>();
      _ = CreateMap<Alergia, AlergiaDtos.AlergiaLightDTO>();
      _ = CreateMap<Alergia, AlergiaDtos.AlergiaTableDTO>();
      _ = CreateMap<AlergiaDtos.CreateAlergiaRequest, Alergia>();
      _ = CreateMap<AlergiaDtos.UpdateAlergiaRequest, Alergia>();

      // ---- GrauAlergia ----
      _ = CreateMap<GrauAlergia, GrauAlergiaDtos.GrauAlergiaDTO>();
      _ = CreateMap<GrauAlergia, GrauAlergiaDtos.GrauAlergiaLightDTO>();
      _ = CreateMap<GrauAlergia, GrauAlergiaDtos.GrauAlergiaTableDTO>();
      _ = CreateMap<GrauAlergiaDtos.CreateGrauAlergiaRequest, GrauAlergia>();
      _ = CreateMap<GrauAlergiaDtos.UpdateGrauAlergiaRequest, GrauAlergia>();

      // ---- AlergiaUtente ----
      _ = CreateMap<AlergiaUtente, AlergiaUtenteDtos.AlergiaUtenteDTO>()
        .ForMember(d => d.AlergiaDescricao, o => o.MapFrom(s => s.Alergia != null ? s.Alergia.Descricao : null));
      _ = CreateMap<AlergiaUtenteDtos.CreateAlergiaUtenteRequest, AlergiaUtente>();
      _ = CreateMap<AlergiaUtenteDtos.UpdateAlergiaUtenteRequest, AlergiaUtente>();

      // ---- AlergiasUtenteObs ----
      _ = CreateMap<AlergiasUtenteObs, AlergiasUtenteObsDtos.AlergiasUtenteObsDTO>();
      _ = CreateMap<AlergiasUtenteObsDtos.CreateAlergiasUtenteObsRequest, AlergiasUtenteObs>();
      _ = CreateMap<AlergiasUtenteObsDtos.UpdateAlergiasUtenteObsRequest, AlergiasUtenteObs>();

      _ = CreateMap<EntidadePessoaDtos.CreateEntidadePessoaRequest, EntidadePessoa>()
        .ForMember(d => d.SexoId, o => o.MapFrom(s => ToNullableGuid(s.SexoId)))
        .ForMember(d => d.Sexo, o => o.Ignore());
      _ = CreateMap<EntidadePessoaDtos.UpdateEntidadePessoaRequest, EntidadePessoa>()
        .ForMember(d => d.SexoId, o => o.MapFrom(s => ToNullableGuid(s.SexoId)))
        .ForMember(d => d.Sexo, o => o.Ignore());

      // ---- Organismo (igual CentroSaude: mapeamento direto, sem specifications especiais) ----
      _ = CreateMap<Organismo, OrganismoDtos.OrganismoDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.HasValue ? (int?)s.Status.Value : null))
        .ForMember(d => d.Rua, o => o.MapFrom(s => s.Rua))
        .ForMember(d => d.CodigoPostal, o => o.MapFrom(s => s.CodigoPostal))
        .ForMember(d => d.Freguesia, o => o.MapFrom(s => s.Freguesia))
        .ForMember(d => d.Concelho, o => o.MapFrom(s => s.Concelho))
        .ForMember(d => d.Distrito, o => o.MapFrom(s => s.Distrito))
        .ForMember(d => d.Pais, o => o.MapFrom(s => s.Pais))
        .ForMember(d => d.Banco, o => o.MapFrom(s => s.Banco))
        .ForMember(d => d.EntidadeContactos, o => o.MapFrom(s => s.EntidadeContactos));
      _ = CreateMap<Organismo, OrganismoDtos.OrganismoLightDTO>();
      _ = CreateMap<Organismo, SeguradoraDtos.SeguradoraLightDTO>();
      _ = CreateMap<Organismo, OrganismoDtos.OrganismoTableDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.HasValue ? (int?)s.Status.Value : null))
        .ForMember(d => d.Rua, o => o.MapFrom(s => s.Rua))
        .ForMember(d => d.CodigoPostal, o => o.MapFrom(s => s.CodigoPostal))
        .ForMember(d => d.Freguesia, o => o.MapFrom(s => s.Freguesia))
        .ForMember(d => d.Concelho, o => o.MapFrom(s => s.Concelho))
        .ForMember(d => d.Distrito, o => o.MapFrom(s => s.Distrito))
        .ForMember(d => d.Pais, o => o.MapFrom(s => s.Pais))
        .ForMember(d => d.BancoNome, o => o.MapFrom(s => s.Banco != null ? s.Banco.Nome : null))
        .ForMember(d => d.ContactoCount, o => o.MapFrom(s => s.EntidadeContactos != null ? s.EntidadeContactos.Count : 0))
        .ForMember(d => d.Contacto, o => o.MapFrom(s => s.EntidadeContactos != null ? s.EntidadeContactos.Where(c => c.EntidadeContactoTipoId == 1).Select(c => c.Valor).FirstOrDefault() : s.Contacto));
      _ = CreateMap<OrganismoDtos.CreateOrganismoRequest, Organismo>()
        .ForMember(d => d.RuaId, o => o.MapFrom(s => ToNullableGuid(s.RuaId)))
        .ForMember(d => d.CodigoPostalId, o => o.MapFrom(s => ToNullableGuid(s.CodigoPostalId)))
        .ForMember(d => d.FreguesiaId, o => o.MapFrom(s => ToNullableGuid(s.FreguesiaId)))
        .ForMember(d => d.ConcelhoId, o => o.MapFrom(s => ToNullableGuid(s.ConcelhoId)))
        .ForMember(d => d.DistritoId, o => o.MapFrom(s => ToNullableGuid(s.DistritoId)))
        .ForMember(d => d.PaisId, o => o.MapFrom(s => ToNullableGuid(s.PaisId)))
        .ForMember(d => d.BancoId, o => o.MapFrom(s => ToNullableGuid(s.BancoId)))
        .ForMember(d => d.Rua, o => o.Ignore())
        .ForMember(d => d.CodigoPostal, o => o.Ignore())
        .ForMember(d => d.Freguesia, o => o.Ignore())
        .ForMember(d => d.Concelho, o => o.Ignore())
        .ForMember(d => d.Distrito, o => o.Ignore())
        .ForMember(d => d.Pais, o => o.Ignore())
        .ForMember(d => d.Banco, o => o.Ignore())
        .ForMember(d => d.EntidadeContactos, o => o.Ignore());
      _ = CreateMap<OrganismoDtos.UpdateOrganismoRequest, Organismo>()
        .ForMember(d => d.RuaId, o => o.MapFrom(s => ToNullableGuid(s.RuaId)))
        .ForMember(d => d.CodigoPostalId, o => o.MapFrom(s => ToNullableGuid(s.CodigoPostalId)))
        .ForMember(d => d.FreguesiaId, o => o.MapFrom(s => ToNullableGuid(s.FreguesiaId)))
        .ForMember(d => d.ConcelhoId, o => o.MapFrom(s => ToNullableGuid(s.ConcelhoId)))
        .ForMember(d => d.DistritoId, o => o.MapFrom(s => ToNullableGuid(s.DistritoId)))
        .ForMember(d => d.PaisId, o => o.MapFrom(s => ToNullableGuid(s.PaisId)))
        .ForMember(d => d.BancoId, o => o.MapFrom(s => ToNullableGuid(s.BancoId)))
        .ForMember(d => d.Rua, o => o.Ignore())
        .ForMember(d => d.CodigoPostal, o => o.Ignore())
        .ForMember(d => d.Freguesia, o => o.Ignore())
        .ForMember(d => d.Concelho, o => o.Ignore())
        .ForMember(d => d.Distrito, o => o.Ignore())
        .ForMember(d => d.Pais, o => o.Ignore())
        .ForMember(d => d.Banco, o => o.Ignore())
        .ForMember(d => d.EntidadeContactos, o => o.Ignore());

      // ---- CentroSaude ----
      _ = CreateMap<CentroSaude, CentroSaudeDtos.CentroSaudeDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.HasValue ? (int?)s.Status.Value : null))
        .ForMember(d => d.Rua, o => o.MapFrom(s => s.Rua))
        .ForMember(d => d.CodigoPostal, o => o.MapFrom(s => s.CodigoPostal))
        .ForMember(d => d.Freguesia, o => o.MapFrom(s => s.Freguesia))
        .ForMember(d => d.Concelho, o => o.MapFrom(s => s.Concelho))
        .ForMember(d => d.Distrito, o => o.MapFrom(s => s.Distrito))
        .ForMember(d => d.Pais, o => o.MapFrom(s => s.Pais))
        .ForMember(d => d.EntidadeContactos, o => o.MapFrom(s => s.EntidadeContactos));
      _ = CreateMap<CentroSaude, CentroSaudeDtos.CentroSaudeLightDTO>();
      _ = CreateMap<UnidadesLocaisSaude, UnidadesLocaisSaudeDtos.UnidadesLocaisSaudeLightDTO>();
      _ = CreateMap<CentroSaude, CentroSaudeDtos.CentroSaudeTableDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.HasValue ? (int?)s.Status.Value : null))
        .ForMember(d => d.Rua, o => o.MapFrom(s => s.Rua))
        .ForMember(d => d.CodigoPostal, o => o.MapFrom(s => s.CodigoPostal))
        .ForMember(d => d.Freguesia, o => o.MapFrom(s => s.Freguesia))
        .ForMember(d => d.Concelho, o => o.MapFrom(s => s.Concelho))
        .ForMember(d => d.Distrito, o => o.MapFrom(s => s.Distrito))
        .ForMember(d => d.Pais, o => o.MapFrom(s => s.Pais))
        .ForMember(d => d.ContactoCount, o => o.MapFrom(s => s.EntidadeContactos != null ? s.EntidadeContactos.Count : 0));
      _ = CreateMap<CentroSaudeDtos.CreateCentroSaudeRequest, CentroSaude>()
        .ForMember(d => d.RuaId, o => o.MapFrom(s => ToNullableGuid(s.RuaId)))
        .ForMember(d => d.CodigoPostalId, o => o.MapFrom(s => ToNullableGuid(s.CodigoPostalId)))
        .ForMember(d => d.FreguesiaId, o => o.MapFrom(s => ToNullableGuid(s.FreguesiaId)))
        .ForMember(d => d.ConcelhoId, o => o.MapFrom(s => ToNullableGuid(s.ConcelhoId)))
        .ForMember(d => d.DistritoId, o => o.MapFrom(s => ToNullableGuid(s.DistritoId)))
        .ForMember(d => d.PaisId, o => o.MapFrom(s => ToNullableGuid(s.PaisId)))
        .ForMember(d => d.Rua, o => o.Ignore())
        .ForMember(d => d.CodigoPostal, o => o.Ignore())
        .ForMember(d => d.Freguesia, o => o.Ignore())
        .ForMember(d => d.Concelho, o => o.Ignore())
        .ForMember(d => d.Distrito, o => o.Ignore())
        .ForMember(d => d.Pais, o => o.Ignore())
        .ForMember(d => d.EntidadeContactos, o => o.Ignore());
      _ = CreateMap<CentroSaudeDtos.UpdateCentroSaudeRequest, CentroSaude>()
        .ForMember(d => d.RuaId, o => o.MapFrom(s => ToNullableGuid(s.RuaId)))
        .ForMember(d => d.CodigoPostalId, o => o.MapFrom(s => ToNullableGuid(s.CodigoPostalId)))
        .ForMember(d => d.FreguesiaId, o => o.MapFrom(s => ToNullableGuid(s.FreguesiaId)))
        .ForMember(d => d.ConcelhoId, o => o.MapFrom(s => ToNullableGuid(s.ConcelhoId)))
        .ForMember(d => d.DistritoId, o => o.MapFrom(s => ToNullableGuid(s.DistritoId)))
        .ForMember(d => d.PaisId, o => o.MapFrom(s => ToNullableGuid(s.PaisId)))
        .ForMember(d => d.Rua, o => o.Ignore())
        .ForMember(d => d.CodigoPostal, o => o.Ignore())
        .ForMember(d => d.Freguesia, o => o.Ignore())
        .ForMember(d => d.Concelho, o => o.Ignore())
        .ForMember(d => d.Distrito, o => o.Ignore())
        .ForMember(d => d.Pais, o => o.Ignore())
        .ForMember(d => d.EntidadeContactos, o => o.Ignore());

      // ---- Fornecedor ----
      _ = CreateMap<Fornecedor, FornecedorDtos.FornecedorDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.HasValue ? (int?)s.Status.Value : null))
        .ForMember(d => d.Rua, o => o.MapFrom(s => s.Rua))
        .ForMember(d => d.CodigoPostal, o => o.MapFrom(s => s.CodigoPostal))
        .ForMember(d => d.Freguesia, o => o.MapFrom(s => s.Freguesia))
        .ForMember(d => d.Concelho, o => o.MapFrom(s => s.Concelho))
        .ForMember(d => d.Distrito, o => o.MapFrom(s => s.Distrito))
        .ForMember(d => d.Pais, o => o.MapFrom(s => s.Pais))
        .ForMember(d => d.EntidadeContactos, o => o.MapFrom(s => s.EntidadeContactos));
      _ = CreateMap<Fornecedor, FornecedorDtos.FornecedorLightDTO>();
      _ = CreateMap<Fornecedor, FornecedorDtos.FornecedorTableDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.HasValue ? (int?)s.Status.Value : null))
        .ForMember(d => d.Rua, o => o.MapFrom(s => s.Rua))
        .ForMember(d => d.CodigoPostal, o => o.MapFrom(s => s.CodigoPostal))
        .ForMember(d => d.Freguesia, o => o.MapFrom(s => s.Freguesia))
        .ForMember(d => d.Concelho, o => o.MapFrom(s => s.Concelho))
        .ForMember(d => d.Distrito, o => o.MapFrom(s => s.Distrito))
        .ForMember(d => d.Pais, o => o.MapFrom(s => s.Pais))
        .ForMember(d => d.ContactoCount, o => o.MapFrom(s => s.EntidadeContactos != null ? s.EntidadeContactos.Count : 0))
        .ForMember(d => d.Contacto, o => o.MapFrom(s => s.EntidadeContactos != null ? s.EntidadeContactos.Where(c => c.EntidadeContactoTipoId == 1).Select(c => c.Valor).FirstOrDefault() : null))
        .ForMember(d => d.Fax, o => o.MapFrom(s => s.EntidadeContactos != null ? s.EntidadeContactos.Where(c => c.EntidadeContactoTipoId == 2).Select(c => c.Valor).FirstOrDefault() : null));
      _ = CreateMap<FornecedorDtos.CreateFornecedorRequest, Fornecedor>()
        .ForMember(d => d.RuaId, o => o.MapFrom(s => ToNullableGuid(s.RuaId)))
        .ForMember(d => d.CodigoPostalId, o => o.MapFrom(s => ToNullableGuid(s.CodigoPostalId)))
        .ForMember(d => d.FreguesiaId, o => o.MapFrom(s => ToNullableGuid(s.FreguesiaId)))
        .ForMember(d => d.ConcelhoId, o => o.MapFrom(s => ToNullableGuid(s.ConcelhoId)))
        .ForMember(d => d.DistritoId, o => o.MapFrom(s => ToNullableGuid(s.DistritoId)))
        .ForMember(d => d.PaisId, o => o.MapFrom(s => ToNullableGuid(s.PaisId)))
        .ForMember(d => d.InstituicaoFinanceiraId, o => o.MapFrom(s => ToNullableGuid(s.InstituicaoFinanceiraId)))
        .ForMember(d => d.Rua, o => o.Ignore())
        .ForMember(d => d.CodigoPostal, o => o.Ignore())
        .ForMember(d => d.Freguesia, o => o.Ignore())
        .ForMember(d => d.Concelho, o => o.Ignore())
        .ForMember(d => d.Distrito, o => o.Ignore())
        .ForMember(d => d.Pais, o => o.Ignore())
        .ForMember(d => d.EntidadeContactos, o => o.Ignore());
      _ = CreateMap<FornecedorDtos.UpdateFornecedorRequest, Fornecedor>()
        .ForMember(d => d.RuaId, o => o.MapFrom(s => ToNullableGuid(s.RuaId)))
        .ForMember(d => d.CodigoPostalId, o => o.MapFrom(s => ToNullableGuid(s.CodigoPostalId)))
        .ForMember(d => d.FreguesiaId, o => o.MapFrom(s => ToNullableGuid(s.FreguesiaId)))
        .ForMember(d => d.ConcelhoId, o => o.MapFrom(s => ToNullableGuid(s.ConcelhoId)))
        .ForMember(d => d.DistritoId, o => o.MapFrom(s => ToNullableGuid(s.DistritoId)))
        .ForMember(d => d.PaisId, o => o.MapFrom(s => ToNullableGuid(s.PaisId)))
        .ForMember(d => d.InstituicaoFinanceiraId, o => o.MapFrom(s => ToNullableGuid(s.InstituicaoFinanceiraId)))
        .ForMember(d => d.Rua, o => o.Ignore())
        .ForMember(d => d.CodigoPostal, o => o.Ignore())
        .ForMember(d => d.Freguesia, o => o.Ignore())
        .ForMember(d => d.Concelho, o => o.Ignore())
        .ForMember(d => d.Distrito, o => o.Ignore())
        .ForMember(d => d.Pais, o => o.Ignore())
        .ForMember(d => d.EntidadeContactos, o => o.Ignore());

      // ---- Empresa ----
      _ = CreateMap<Empresa, EmpresaDtos.EmpresaDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.HasValue ? (int?)s.Status.Value : null))
        .ForMember(d => d.Rua, o => o.MapFrom(s => s.Rua))
        .ForMember(d => d.CodigoPostal, o => o.MapFrom(s => s.CodigoPostal))
        .ForMember(d => d.Freguesia, o => o.MapFrom(s => s.Freguesia))
        .ForMember(d => d.Concelho, o => o.MapFrom(s => s.Concelho))
        .ForMember(d => d.Distrito, o => o.MapFrom(s => s.Distrito))
        .ForMember(d => d.Pais, o => o.MapFrom(s => s.Pais))
        .ForMember(d => d.Banco, o => o.MapFrom(s => s.Banco))
        .ForMember(d => d.EntidadeContactos, o => o.MapFrom(s => s.EntidadeContactos));
      _ = CreateMap<Empresa, EmpresaDtos.EmpresaTableDTO>()
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.HasValue ? (int?)s.Status.Value : null))
        .ForMember(d => d.Rua, o => o.MapFrom(s => s.Rua))
        .ForMember(d => d.CodigoPostal, o => o.MapFrom(s => s.CodigoPostal))
        .ForMember(d => d.Freguesia, o => o.MapFrom(s => s.Freguesia))
        .ForMember(d => d.Concelho, o => o.MapFrom(s => s.Concelho))
        .ForMember(d => d.Distrito, o => o.MapFrom(s => s.Distrito))
        .ForMember(d => d.Pais, o => o.MapFrom(s => s.Pais))
        .ForMember(d => d.ContactoCount, o => o.MapFrom(s => s.EntidadeContactos != null ? s.EntidadeContactos.Count : 0))
        .ForMember(d => d.Contacto, o => o.MapFrom(s => s.EntidadeContactos != null ? s.EntidadeContactos.Where(c => c.EntidadeContactoTipoId == 1).Select(c => c.Valor).FirstOrDefault() : s.Contacto))
        .ForMember(d => d.Fax, o => o.MapFrom(s => s.EntidadeContactos != null ? s.EntidadeContactos.Where(c => c.EntidadeContactoTipoId == 2).Select(c => c.Valor).FirstOrDefault() : null));
      _ = CreateMap<Empresa, EmpresaDtos.EmpresaLightDTO>();
      _ = CreateMap<EmpresaDtos.CreateEmpresaRequest, Empresa>()
        .ForMember(d => d.RuaId, o => o.MapFrom(s => ToNullableGuid(s.RuaId)))
        .ForMember(d => d.CodigoPostalId, o => o.MapFrom(s => ToNullableGuid(s.CodigoPostalId)))
        .ForMember(d => d.FreguesiaId, o => o.MapFrom(s => ToNullableGuid(s.FreguesiaId)))
        .ForMember(d => d.ConcelhoId, o => o.MapFrom(s => ToNullableGuid(s.ConcelhoId)))
        .ForMember(d => d.DistritoId, o => o.MapFrom(s => ToNullableGuid(s.DistritoId)))
        .ForMember(d => d.PaisId, o => o.MapFrom(s => ToNullableGuid(s.PaisId)))
        .ForMember(d => d.BancoId, o => o.MapFrom(s => ToNullableGuid(s.BancoId)))
        .ForMember(d => d.Rua, o => o.Ignore())
        .ForMember(d => d.CodigoPostal, o => o.Ignore())
        .ForMember(d => d.Freguesia, o => o.Ignore())
        .ForMember(d => d.Concelho, o => o.Ignore())
        .ForMember(d => d.Distrito, o => o.Ignore())
        .ForMember(d => d.Pais, o => o.Ignore())
        .ForMember(d => d.Banco, o => o.Ignore())
        .ForMember(d => d.EntidadeContactos, o => o.Ignore());
      _ = CreateMap<EmpresaDtos.UpdateEmpresaRequest, Empresa>()
        .ForMember(d => d.RuaId, o => o.MapFrom(s => ToNullableGuid(s.RuaId)))
        .ForMember(d => d.CodigoPostalId, o => o.MapFrom(s => ToNullableGuid(s.CodigoPostalId)))
        .ForMember(d => d.FreguesiaId, o => o.MapFrom(s => ToNullableGuid(s.FreguesiaId)))
        .ForMember(d => d.ConcelhoId, o => o.MapFrom(s => ToNullableGuid(s.ConcelhoId)))
        .ForMember(d => d.DistritoId, o => o.MapFrom(s => ToNullableGuid(s.DistritoId)))
        .ForMember(d => d.PaisId, o => o.MapFrom(s => ToNullableGuid(s.PaisId)))
        .ForMember(d => d.BancoId, o => o.MapFrom(s => ToNullableGuid(s.BancoId)))
        .ForMember(d => d.Rua, o => o.Ignore())
        .ForMember(d => d.CodigoPostal, o => o.Ignore())
        .ForMember(d => d.Freguesia, o => o.Ignore())
        .ForMember(d => d.Concelho, o => o.Ignore())
        .ForMember(d => d.Distrito, o => o.Ignore())
        .ForMember(d => d.Pais, o => o.Ignore())
        .ForMember(d => d.Banco, o => o.Ignore())
        .ForMember(d => d.EntidadeContactos, o => o.Ignore());

      // ---- ConfiguracaoADSE ----
      _ = CreateMap<ConfiguracaoADSE, ConfiguracaoADSEDtos.ConfiguracaoADSEDTO>();
      _ = CreateMap<ConfiguracaoADSEDtos.GuardarConfiguracaoADSERequest, ConfiguracaoADSE>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.EmpresaId, o => o.Ignore())
        .ForMember(d => d.CreatedBy, o => o.Ignore())
        .ForMember(d => d.CreatedOn, o => o.Ignore())
        .ForMember(d => d.LastModifiedBy, o => o.Ignore())
        .ForMember(d => d.LastModifiedOn, o => o.Ignore())
        .ForMember(d => d.DeletedOn, o => o.Ignore())
        .ForMember(d => d.DeletedBy, o => o.Ignore());
      _ = CreateMap<ConfiguracaoADSEDtos.CreateConfiguracaoADSERequest, ConfiguracaoADSE>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.CreatedBy, o => o.Ignore())
        .ForMember(d => d.CreatedOn, o => o.Ignore())
        .ForMember(d => d.LastModifiedBy, o => o.Ignore())
        .ForMember(d => d.LastModifiedOn, o => o.Ignore())
        .ForMember(d => d.DeletedOn, o => o.Ignore())
        .ForMember(d => d.DeletedBy, o => o.Ignore());
      _ = CreateMap<ConfiguracaoADSEDtos.UpdateConfiguracaoADSERequest, ConfiguracaoADSE>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.EmpresaId, o => o.Ignore())
        .ForMember(d => d.CreatedBy, o => o.Ignore())
        .ForMember(d => d.CreatedOn, o => o.Ignore())
        .ForMember(d => d.LastModifiedBy, o => o.Ignore())
        .ForMember(d => d.LastModifiedOn, o => o.Ignore())
        .ForMember(d => d.DeletedOn, o => o.Ignore())
        .ForMember(d => d.DeletedBy, o => o.Ignore());

      // ---- Doenca (ICD-11: ver/editar/eliminar, sem criar) ----
      _ = CreateMap<Doenca, DoencaDtos.DoencaDTO>();
      _ = CreateMap<DoencaDtos.UpdateDoencaRequest, Doenca>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.IcdId, o => o.Ignore())
        .ForMember(d => d.ClassKind, o => o.Ignore())
        .ForMember(d => d.Level, o => o.Ignore())
        .ForMember(d => d.ParentId, o => o.Ignore())
        .ForMember(d => d.Parent, o => o.Ignore())
        .ForMember(d => d.Children, o => o.Ignore())
        .ForMember(d => d.CreatedBy, o => o.Ignore())
        .ForMember(d => d.CreatedOn, o => o.Ignore())
        .ForMember(d => d.LastModifiedBy, o => o.Ignore())
        .ForMember(d => d.LastModifiedOn, o => o.Ignore())
        .ForMember(d => d.DeletedOn, o => o.Ignore())
        .ForMember(d => d.DeletedBy, o => o.Ignore());

      // ---- TipoConsulta ----
      _ = CreateMap<TipoConsultaItem, TipoConsultaDtos.TipoConsultaDTO>();
      _ = CreateMap<TipoConsultaItem, TipoConsultaDtos.TipoConsultaTableDTO>();
      _ = CreateMap<TipoConsultaDtos.CreateTipoConsultaRequest, TipoConsultaItem>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.CreatedBy, o => o.Ignore())
        .ForMember(d => d.CreatedOn, o => o.Ignore())
        .ForMember(d => d.LastModifiedBy, o => o.Ignore())
        .ForMember(d => d.LastModifiedOn, o => o.Ignore())
        .ForMember(d => d.DeletedOn, o => o.Ignore())
        .ForMember(d => d.DeletedBy, o => o.Ignore());
      _ = CreateMap<TipoConsultaDtos.UpdateTipoConsultaRequest, TipoConsultaItem>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.CreatedBy, o => o.Ignore())
        .ForMember(d => d.CreatedOn, o => o.Ignore())
        .ForMember(d => d.LastModifiedBy, o => o.Ignore())
        .ForMember(d => d.LastModifiedOn, o => o.Ignore())
        .ForMember(d => d.DeletedOn, o => o.Ignore())
        .ForMember(d => d.DeletedBy, o => o.Ignore());

      // ---- TipoAdmissao ----
      _ = CreateMap<TipoAdmissao, CliCloud.Application.Services.Consultas.TipoAdmissaoService.DTOs.TipoAdmissaoDTO>();

      // ---- Admissao administrativa ----
      _ = CreateMap<AdmissaoServico, CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs.AdmissaoServicoDTO>();
      _ = CreateMap<CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs.AdmissaoServicoDTO, AdmissaoServico>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.AdmissaoId, o => o.Ignore())
        .ForMember(d => d.Admissao, o => o.Ignore());
      _ = CreateMap<Admissao, CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs.AdmissaoDTO>()
        .ForMember(d => d.UtenteNumero, o => o.MapFrom(s => s.Utente != null ? s.Utente.NumeroUtente : null))
        .ForMember(d => d.UtenteNome, o => o.MapFrom(s => s.Utente != null ? s.Utente.Nome : null))
        .ForMember(d => d.DoencaPrincipalCodigo, o => o.MapFrom(s => s.DoencaPrincipal != null ? s.DoencaPrincipal.Code : null))
        .ForMember(d => d.DoencaPrincipalTitulo, o => o.MapFrom(s => s.DoencaPrincipal != null ? s.DoencaPrincipal.Title : null))
        .ForMember(d => d.DoencaSecundariaCodigo, o => o.MapFrom(s => s.DoencaSecundaria != null ? s.DoencaSecundaria.Code : null))
        .ForMember(d => d.DoencaSecundariaTitulo, o => o.MapFrom(s => s.DoencaSecundaria != null ? s.DoencaSecundaria.Title : null));
      _ = CreateMap<Admissao, CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs.AdmissaoTableDTO>()
        .ForMember(d => d.UtenteNumero, o => o.MapFrom(s => s.Utente != null ? s.Utente.NumeroUtente : null))
        .ForMember(d => d.UtenteNome, o => o.MapFrom(s => s.Utente != null ? s.Utente.Nome : null))
        .ForMember(d => d.MedicoNome, o => o.MapFrom(s => s.Medico != null ? s.Medico.Nome : null))
        .ForMember(d => d.EspecialidadeDesignacao, o => o.MapFrom(s => s.Especialidade != null ? s.Especialidade.Nome : null))
        .ForMember(d => d.OrganismoNome, o => o.MapFrom(s => s.Organismo != null ? s.Organismo.Nome : null))
        .ForMember(d => d.SalaNome, o => o.MapFrom(s => s.Sala != null ? s.Sala.Nome : null))
        .ForMember(d => d.TipoAdmissaoDesignacao, o => o.MapFrom(s => s.TipoAdmissao != null ? s.TipoAdmissao.Designacao : null))
        .ForMember(d => d.TipoConsultaDesignacao,o => o.MapFrom(s => s.TipoConsultaItem != null ? s.TipoConsultaItem.Designacao : null));
      _ = CreateMap<CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs.CreateAdmissaoRequest, Admissao>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.Consulta, o => o.Ignore())
        .ForMember(d => d.Pago, o => o.Ignore())
        .ForMember(d => d.Faturado, o => o.Ignore());
      _ = CreateMap<CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs.UpdateAdmissaoRequest, Admissao>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.Consulta, o => o.Ignore())
        .ForMember(d => d.Servicos, o => o.Ignore())
        .ForMember(d => d.Pago, o => o.Ignore())
        .ForMember(d => d.Faturado, o => o.Ignore());

      // Histórico administrativo: Consulta ↔ mesmo DTO de admissão (edição pós-promoção)
      _ = CreateMap<Consulta, CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs.AdmissaoDTO>()
        .ForMember(d => d.UtenteId, o => o.MapFrom(s => s.UtenteId ?? Guid.Empty))
        .ForMember(d => d.UtenteNumero, o => o.MapFrom(s => s.Utente != null ? s.Utente.NumeroUtente : null))
        .ForMember(d => d.UtenteNome, o => o.MapFrom(s => s.Utente != null ? s.Utente.Nome : null))
        .ForMember(d => d.MedicoNome, o => o.MapFrom(s => s.Medico != null ? s.Medico.Nome : null))
        .ForMember(d => d.OrganismoNome, o => o.MapFrom(s => s.Organismo != null ? s.Organismo.Nome : null))
        .ForMember(d => d.SalaNome, o => o.MapFrom(s => s.Sala != null ? s.Sala.Nome : null))
        .ForMember(d => d.EspecialidadeNome, o => o.MapFrom(s => s.Especialidade != null ? s.Especialidade.Nome : null))
        .ForMember(d => d.MedicoExternoNome, o => o.MapFrom(s => s.MedicoExterno != null ? s.MedicoExterno.Nome : null))
        .ForMember(d => d.Servicos, o => o.Ignore())
        .ForMember(d => d.Origem, o => o.MapFrom(_ => OrigemAdmissao.Manual))
        .ForMember(d => d.Pago, o => o.Ignore())
        .ForMember(d => d.Faturado, o => o.Ignore())
        .ForMember(d => d.DoencaPrincipalCodigo, o => o.MapFrom(s => s.DoencaPrincipal != null ? s.DoencaPrincipal.Code : null))
        .ForMember(d => d.DoencaPrincipalTitulo, o => o.MapFrom(s => s.DoencaPrincipal != null ? s.DoencaPrincipal.Title : null))
        .ForMember(d => d.DoencaSecundariaCodigo, o => o.MapFrom(s => s.DoencaSecundaria != null ? s.DoencaSecundaria.Code : null))
        .ForMember(d => d.DoencaSecundariaTitulo, o => o.MapFrom(s => s.DoencaSecundaria != null ? s.DoencaSecundaria.Title : null));
      _ = CreateMap<ServicoConsulta, CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs.AdmissaoServicoDTO>();
      _ = CreateMap<CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs.AdmissaoServicoDTO, ServicoConsulta>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.ConsultaId, o => o.Ignore())
        .ForMember(d => d.Consulta, o => o.Ignore())
        .ForMember(d => d.Servico, o => o.Ignore())
        .ForMember(d => d.Exame, o => o.Ignore());
      _ = CreateMap<CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs.UpdateAdmissaoRequest, Consulta>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.Servicos, o => o.Ignore())
        .ForMember(d => d.Utente, o => o.Ignore())
        .ForMember(d => d.Medico, o => o.Ignore())
        .ForMember(d => d.Especialidade, o => o.Ignore())
        .ForMember(d => d.Tecnico, o => o.Ignore())
        .ForMember(d => d.Sala, o => o.Ignore())
        .ForMember(d => d.MedicoExterno, o => o.Ignore())
        .ForMember(d => d.Organismo, o => o.Ignore())
        .ForMember(d => d.Seguradora, o => o.Ignore())
        .ForMember(d => d.Tratamento, o => o.Ignore())
        .ForMember(d => d.Funcionario, o => o.Ignore())
        .ForMember(d => d.Documento, o => o.Ignore())
        .ForMember(d => d.TipoDocumento, o => o.Ignore())
        .ForMember(d => d.TipoConsultaItem, o => o.Ignore())
        .ForMember(d => d.ConsultaMarcacao, o => o.Ignore())
        .ForMember(d => d.Admissao, o => o.Ignore())
        .ForMember(d => d.TipoAdmissao, o => o.Ignore())
        .ForMember(d => d.DoencaPrincipal, o => o.Ignore())
        .ForMember(d => d.DoencaSecundaria, o => o.Ignore());
      _ = CreateMap<CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService.DTOs.UpdateConsultaHistoricoRequest, Consulta>()
        .IncludeBase<CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs.UpdateAdmissaoRequest, Consulta>();

      // ---- MotivoConsulta ----
      _ = CreateMap<MotivoConsulta, CliCloud.Application.Services.Consultas.MotivoConsultaService.DTOs.MotivoConsultaDTO>();
      _ = CreateMap<MotivoConsulta, CliCloud.Application.Services.Consultas.MotivoConsultaService.DTOs.MotivoConsultaTableDTO>();
      _ = CreateMap<CliCloud.Application.Services.Consultas.MotivoConsultaService.DTOs.CreateMotivoConsultaRequest, MotivoConsulta>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<CliCloud.Application.Services.Consultas.MotivoConsultaService.DTOs.UpdateMotivoConsultaRequest, MotivoConsulta>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- Sala ----
      _ = CreateMap<Sala, CliCloud.Application.Services.Consultas.SalaService.DTOs.SalaDTO>();
      _ = CreateMap<Sala, CliCloud.Application.Services.Consultas.SalaService.DTOs.SalaTableDTO>()
        .ForMember(d => d.ClinicaNome, o => o.MapFrom(s => s.Clinica != null ? s.Clinica.Nome : null));
      _ = CreateMap<CliCloud.Application.Services.Consultas.SalaService.DTOs.CreateSalaRequest, Sala>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.Clinica, o => o.Ignore());
      _ = CreateMap<CliCloud.Application.Services.Consultas.SalaService.DTOs.UpdateSalaRequest, Sala>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.Clinica, o => o.Ignore());

      // ---- TipoCarta ----
      _ = CreateMap<TipoCarta, CliCloud.Application.Services.Utility.TipoCartaService.DTOs.TipoCartaDTO>();
      _ = CreateMap<TipoCarta, CliCloud.Application.Services.Utility.TipoCartaService.DTOs.TipoCartaTableDTO>();
      _ = CreateMap<CliCloud.Application.Services.Utility.TipoCartaService.DTOs.CreateTipoCartaRequest, TipoCarta>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<CliCloud.Application.Services.Utility.TipoCartaService.DTOs.UpdateTipoCartaRequest, TipoCarta>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- Patologia ----
      _ = CreateMap<Patologia, PatologiaDtos.PatologiaDTO>()
        .ForMember(d => d.OrganismoNome, o => o.MapFrom(s => s.Organismo != null ? s.Organismo.Nome : null))
        .ForMember(d => d.LocalTratamentoDesignacao, o => o.MapFrom(s => s.LocalTratamento != null ? s.LocalTratamento.Designacao : null))
        .ForMember(d => d.DoencaIds, o => o.MapFrom(s => s.PatologiaDoencas != null ? s.PatologiaDoencas.Select(pd => pd.DoencaId) : null));
      _ = CreateMap<Patologia, PatologiaDtos.PatologiaLightDTO>();
      _ = CreateMap<Patologia, PatologiaDtos.PatologiaTableDTO>()
        .ForMember(d => d.OrganismoNome, o => o.MapFrom(s => s.Organismo != null ? s.Organismo.Nome : null));
      _ = CreateMap<PatologiaDtos.CreatePatologiaRequest, Patologia>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.PatologiaServicos, o => o.Ignore())
        .ForMember(d => d.PatologiaDoencas, o => o.Ignore());
      _ = CreateMap<PatologiaDtos.UpdatePatologiaRequest, Patologia>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.PatologiaServicos, o => o.Ignore())
        .ForMember(d => d.PatologiaDoencas, o => o.Ignore());

      // ---- PatologiaServico ----
      _ = CreateMap<PatologiaServico, PatologiaDtos.PatologiaServicoDTO>()
        .ForMember(d => d.CodigoServico, o => o.MapFrom(s => s.SubsistemaServico != null && s.SubsistemaServico.Servico != null ? s.SubsistemaServico.Servico.Id.ToString() : null))
        .ForMember(d => d.DescricaoServico, o => o.MapFrom(s => s.SubsistemaServico != null && s.SubsistemaServico.Servico != null ? s.SubsistemaServico.Servico.Designacao : null))
        .ForMember(d => d.SubSistema, o => o.MapFrom(s => s.SubsistemaServico != null ? s.SubsistemaServico.SubsistemaId.ToString() : null));
      _ = CreateMap<PatologiaDtos.CreatePatologiaServicoRequest, PatologiaServico>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.PatologiaId, o => o.Ignore())
        .ForMember(d => d.Patologia, o => o.Ignore())
        .ForMember(d => d.SubsistemaServico, o => o.Ignore());
      _ = CreateMap<PatologiaDtos.UpdatePatologiaServicoRequest, PatologiaServico>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.PatologiaId, o => o.Ignore())
        .ForMember(d => d.Patologia, o => o.Ignore())
        .ForMember(d => d.SubsistemaServico, o => o.Ignore());

      // ---- RegiaoCorpo ----
      _ = CreateMap<RegiaoCorpo, RegiaoCorpoDtos.RegiaoCorpoDTO>();
      _ = CreateMap<RegiaoCorpo, RegiaoCorpoDtos.RegiaoCorpoLightDTO>();
      _ = CreateMap<RegiaoCorpo, RegiaoCorpoDtos.RegiaoCorpoTableDTO>();
      _ = CreateMap<RegiaoCorpoDtos.CreateRegiaoCorpoRequest, RegiaoCorpo>();
      _ = CreateMap<RegiaoCorpoDtos.UpdateRegiaoCorpoRequest, RegiaoCorpo>();

      // ---- Goniometrias ----
      _ = CreateMap<Goniometrias, GoniometriasDtos.GoniometriasDTO>();
      _ = CreateMap<Goniometrias, GoniometriasDtos.GoniometriasLightDTO>();
      _ = CreateMap<Goniometrias, GoniometriasDtos.GoniometriasTableDTO>();
      _ = CreateMap<GoniometriasDtos.CreateGoniometriasRequest, Goniometrias>();
      _ = CreateMap<GoniometriasDtos.UpdateGoniometriasRequest, Goniometrias>();

      // ---- TipoDeDor ----
      _ = CreateMap<TipoDeDor, CliCloud.Application.Services.Tratamentos.TipoDeDorService.DTOs.TipoDeDorDTO>();
      _ = CreateMap<TipoDeDor, CliCloud.Application.Services.Tratamentos.TipoDeDorService.DTOs.TipoDeDorLightDTO>();
      _ = CreateMap<TipoDeDor, CliCloud.Application.Services.Tratamentos.TipoDeDorService.DTOs.TipoDeDorTableDTO>();
      _ = CreateMap<CliCloud.Application.Services.Tratamentos.TipoDeDorService.DTOs.CreateTipoDeDorRequest, TipoDeDor>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<CliCloud.Application.Services.Tratamentos.TipoDeDorService.DTOs.UpdateTipoDeDorRequest, TipoDeDor>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- FraquezasMusculares ----
      _ = CreateMap<FraquezasMusculares, CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService.DTOs.FraquezasMuscularesDTO>();
      _ = CreateMap<FraquezasMusculares, CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService.DTOs.FraquezasMuscularesLightDTO>();
      _ = CreateMap<FraquezasMusculares, CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService.DTOs.FraquezasMuscularesTableDTO>();
      _ = CreateMap<CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService.DTOs.CreateFraquezasMuscularesRequest, FraquezasMusculares>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService.DTOs.UpdateFraquezasMuscularesRequest, FraquezasMusculares>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- ServicoConsulta (Consulta → Serviços) ----
      _ = CreateMap<ServicoConsulta, CliCloud.Application.Services.Consultas.ServicoConsultaService.DTOs.ServicoConsultaDTO>();
      _ = CreateMap<ServicoConsulta, CliCloud.Application.Services.Consultas.ServicoConsultaService.DTOs.ServicoConsultaTableDTO>();
      _ = CreateMap<CliCloud.Application.Services.Consultas.ServicoConsultaService.DTOs.CreateServicoConsultaRequest, ServicoConsulta>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.Consulta, o => o.Ignore())
        .ForMember(d => d.Servico, o => o.Ignore())
        .ForMember(d => d.Exame, o => o.Ignore());
      _ = CreateMap<CliCloud.Application.Services.Consultas.ServicoConsultaService.DTOs.UpdateServicoConsultaRequest, ServicoConsulta>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.ConsultaId, o => o.Ignore())
        .ForMember(d => d.Consulta, o => o.Ignore())
        .ForMember(d => d.Servico, o => o.Ignore())
        .ForMember(d => d.Exame, o => o.Ignore());

      // ---- MotivoAlta ----
      _ = CreateMap<MotivoAlta, MotivoAltaDtos.MotivoAltaDTO>();
      _ = CreateMap<MotivoAlta, MotivoAltaDtos.MotivoAltaLightDTO>();
      _ = CreateMap<MotivoAlta, MotivoAltaDtos.MotivoAltaTableDTO>();
      _ = CreateMap<MotivoAltaDtos.CreateMotivoAltaRequest, MotivoAlta>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<MotivoAltaDtos.UpdateMotivoAltaRequest, MotivoAlta>()
        .ForMember(d => d.Id, o => o.Ignore());
      
      // ---- MotivosDesmarcacao ----
      _ = CreateMap<MotivosDesmarcacao, MotivosDesmarcacaoDtos.MotivosDesmarcacaoDTO>();
      _ = CreateMap<MotivosDesmarcacao, MotivosDesmarcacaoDtos.MotivosDesmarcacaoLightDTO>();
      _ = CreateMap<MotivosDesmarcacao, MotivosDesmarcacaoDtos.MotivosDesmarcacaoTableDTO>();
      _ = CreateMap<MotivosDesmarcacaoDtos.CreateMotivosDesmarcacaoRequest, MotivosDesmarcacao>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<MotivosDesmarcacaoDtos.UpdateMotivosDesmarcacaoRequest, MotivosDesmarcacao>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<MotivosDesmarcacaoDtos.DeleteMultipleMotivosDesmarcacaoRequest, MotivosDesmarcacao>()
        .ForMember(d => d.Id, o => o.Ignore());
        
        // ---- Antecedentes Pessoais ----
      _ = CreateMap<AntecedentesPessoais, AntecedentesPessoaisDtos.AntecedentesPessoaisDTO>();
      _ = CreateMap<AntecedentesPessoais, AntecedentesPessoaisDtos.AntecedentesPessoaisLightDTO>();
      _ = CreateMap<AntecedentesPessoais, AntecedentesPessoaisDtos.AntecedentesPessoaisTableDTO>();
      _ = CreateMap<AntecedentesPessoaisDtos.CreateAntecedentesPessoaisRequest, AntecedentesPessoais>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<AntecedentesPessoaisDtos.UpdateAntecedentesPessoaisRequest, AntecedentesPessoais>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<AntecedentesPessoaisDtos.DeleteMultipleAntecedentesPessoaisRequest, AntecedentesPessoais>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- Antecedentes Familiares Utente ----
      _ = CreateMap<AntecedentesFamiliaresUtente, AntecedentesFamiliaresUtenteDtos.AntecedentesFamiliaresUtenteDTO>()
        .ForMember(d => d.GrauParentesco, o => o.MapFrom(s => s.GrauParentesco != null ? s.GrauParentesco.Descricao : null))
        .ForMember(d => d.NomeDoenca, o => o.MapFrom(s => s.Doenca != null ? s.Doenca.Title : s.NomeDoenca));
      _ = CreateMap<AntecedentesFamiliaresUtente, AntecedentesFamiliaresUtenteDtos.AntecedentesFamiliaresUtenteLightDTO>()
        .ForMember(d => d.GrauParentesco, o => o.MapFrom(s => s.GrauParentesco != null ? s.GrauParentesco.Descricao : null))
        .ForMember(d => d.NomeDoenca, o => o.MapFrom(s => s.Doenca != null ? s.Doenca.Title : s.NomeDoenca));
      _ = CreateMap<AntecedentesFamiliaresUtente, AntecedentesFamiliaresUtenteDtos.AntecedentesFamiliaresUtenteTableDTO>()
        .ForMember(d => d.GrauParentesco, o => o.MapFrom(s => s.GrauParentesco != null ? s.GrauParentesco.Descricao : null))
        .ForMember(d => d.NomeDoenca, o => o.MapFrom(s => s.Doenca != null ? s.Doenca.Title : s.NomeDoenca));
      _ = CreateMap<AntecedentesFamiliaresUtenteDtos.CreateAntecedentesFamiliaresUtenteRequest, AntecedentesFamiliaresUtente>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<AntecedentesFamiliaresUtenteDtos.UpdateAntecedentesFamiliaresUtenteRequest, AntecedentesFamiliaresUtente>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<AntecedentesFamiliaresUtenteDtos.DeleteMultipleAntecedentesFamiliaresUtenteRequest, AntecedentesFamiliaresUtente>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- Antecedentes Cirurgicos ----
      _ = CreateMap<AntecedentesCirurgicos, AntecedentesCirurgicosDtos.AntecedentesCirurgicosDTO>();
      _ = CreateMap<AntecedentesCirurgicos, AntecedentesCirurgicosDtos.AntecedentesCirurgicosLightDto>();
      _ = CreateMap<AntecedentesCirurgicos, AntecedentesCirurgicosDtos.AntecedentesCirurgicosTableDTO>();
      _ = CreateMap<AntecedentesCirurgicosDtos.CreateAntecedentesCirurgicosRequest, AntecedentesCirurgicos>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<AntecedentesCirurgicosDtos.UpdateAntecedentesCirurgicosRequest, AntecedentesCirurgicos>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<AntecedentesCirurgicosDtos.DeleteMultipleAntecedentesCirurgicosRequest, AntecedentesCirurgicos>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- Questionario Utente ----
      _ = CreateMap<QuestionarioUtente, QuestionarioUtenteDtos.QuestionarioUtenteDTO>();
      _ = CreateMap<QuestionarioUtente, QuestionarioUtenteDtos.QuestionarioUtenteTableDTO>();
      _ = CreateMap<QuestionarioUtenteDtos.CreateQuestionarioUtenteRequest, QuestionarioUtente>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.DataCriacao, o => o.Ignore());
      _ = CreateMap<QuestionarioUtenteDtos.UpdateQuestionarioUtenteRequest, QuestionarioUtente>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.DataCriacao, o => o.Ignore());

      // ---- HabitosEVicios ----
      _ = CreateMap<HabitosEVicios, HabitosEViciosDtos.HabitosEViciosDTO>();
      _ = CreateMap<HabitosEVicios, HabitosEViciosDtos.HabitosEViciosLightDTO>();
      _ = CreateMap<HabitosEVicios, HabitosEViciosDtos.HabitosEViciosTableDTO>();
      _ = CreateMap<HabitosEViciosDtos.CreateHabitosEViciosRequest, HabitosEVicios>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<HabitosEViciosDtos.UpdateHabitosEViciosRequest, HabitosEVicios>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<HabitosEViciosDtos.DeleteMultipleHabitosEViciosRequest, HabitosEVicios>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- Sinais Vitais - Tensão Arterial ----
      _ = CreateMap<TensaoArterial, TensaoArterialDtos.TensaoArterialDTO>();
      _ = CreateMap<TensaoArterial, TensaoArterialDtos.TensaoArterialLightDTO>();
      _ = CreateMap<TensaoArterial, TensaoArterialDtos.TensaoArterialTableDTO>();
      _ = CreateMap<TensaoArterialDtos.CreateTensaoArterialRequest, TensaoArterial>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<TensaoArterialDtos.UpdateTensaoArterialRequest, TensaoArterial>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- Sinais Vitais - Glicemia Capilar ----
      _ = CreateMap<GlicemiaCapilar, GlicemiaCapilarDtos.GlicemiaCapilarDTO>();
      _ = CreateMap<GlicemiaCapilar, GlicemiaCapilarDtos.GlicemiaCapilarTableDTO>();
      _ = CreateMap<GlicemiaCapilarDtos.CreateGlicemiaCapilarRequest, GlicemiaCapilar>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<GlicemiaCapilarDtos.UpdateGlicemiaCapilarRequest, GlicemiaCapilar>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- Sinais Vitais - Temperatura Corporal ----
      _ = CreateMap<TemperaturaCorporal, TemperaturaCorporalDtos.TemperaturaCorporalDTO>();
      _ = CreateMap<TemperaturaCorporal, TemperaturaCorporalDtos.TemperaturaCorporalTableDTO>();
      _ = CreateMap<TemperaturaCorporalDtos.CreateTemperaturaCorporalRequest, TemperaturaCorporal>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<TemperaturaCorporalDtos.UpdateTemperaturaCorporalRequest, TemperaturaCorporal>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- Sinais Vitais - Índice Massa Corporal ----
      _ = CreateMap<IndiceMassaCorporal, IndiceMassaCorporalDtos.IndiceMassaCorporalDTO>();
      _ = CreateMap<IndiceMassaCorporalDtos.CreateIndiceMassaCorporalRequest, IndiceMassaCorporal>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<IndiceMassaCorporalDtos.UpdateIndiceMassaCorporalRequest, IndiceMassaCorporal>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- HistoriaClinica ----
      _ = CreateMap<HistoriaClinica, HistoriaClinicaDtos.HistoriaClinicaDTO>();
      _ = CreateMap<HistoriaClinica, HistoriaClinicaDtos.HistoriaClinicaTableDTO>()
        .ForMember(d => d.UtenteNome, o => o.MapFrom(s => s.Utente != null ? s.Utente.Nome : string.Empty))
        .ForMember(d => d.MedicoNome, o => o.MapFrom(s => s.Medico != null ? s.Medico.Nome : string.Empty))
        .ForMember(d => d.EspecialidadeNome, o => o.MapFrom(s => s.Especialidade != null ? s.Especialidade.Nome : null));
      _ = CreateMap<HistoriaClinicaDtos.CreateHistoriaClinicaRequest, HistoriaClinica>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<HistoriaClinicaDtos.UpdateHistoriaClinicaRequest, HistoriaClinica>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<HistoriaClinicaDtos.DeleteMultipleHistoriaClinicaRequest, HistoriaClinica>()
        .ForMember(d => d.Id, o => o.Ignore());

      _ = CreateMap<GorduraMassaMuscular, GorduraMassaMuscularDtos.GorduraMassaMuscularDTO>();
      _ = CreateMap<GorduraMassaMuscular, GorduraMassaMuscularDtos.GorduraMassaMuscularLightDTO>();
      _ = CreateMap<GorduraMassaMuscular, GorduraMassaMuscularDtos.GorduraMassaMuscularTableDTO>();
      _ = CreateMap<GorduraMassaMuscularDtos.CreateGorduraMassaMuscularRequest, GorduraMassaMuscular>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<GorduraMassaMuscularDtos.UpdateGorduraMassaMuscularRequest, GorduraMassaMuscular>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- Sinais Vitais - Avaliação Antropométrica ----
      _ = CreateMap<AvaliacaoAntropometrica, AvaliacaoAntropometricaDtos.AvaliacaoAntropometricaDTO>();
      _ = CreateMap<AvaliacaoAntropometrica, AvaliacaoAntropometricaDtos.AvaliacaoAntropometricaLightDTO>();
      _ = CreateMap<AvaliacaoAntropometrica, AvaliacaoAntropometricaDtos.AvaliacaoAntropometricaTableDTO>();
      _ = CreateMap<AvaliacaoAntropometricaDtos.CreateAvaliacaoAntropometricaRequest, AvaliacaoAntropometrica>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<AvaliacaoAntropometricaDtos.UpdateAvaliacaoAntropometricaRequest, AvaliacaoAntropometrica>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- Sinais Vitais - Avaliação Postural ----
      _ = CreateMap<AvaliacaoPostural, AvaliacaoPosturalDtos.AvaliacaoPosturalDTO>();
      _ = CreateMap<AvaliacaoPostural, AvaliacaoPosturalDtos.AvaliacaoPosturalLightDTO>();
      _ = CreateMap<AvaliacaoPostural, AvaliacaoPosturalDtos.AvaliacaoPosturalTableDTO>();
      _ = CreateMap<AvaliacaoPosturalDtos.CreateAvaliacaoPosturalRequest, AvaliacaoPostural>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<AvaliacaoPosturalDtos.UpdateAvaliacaoPosturalRequest, AvaliacaoPostural>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- Periocidade Tratamento ----
      _ = CreateMap<PeriocidadeTratamento, PeriocidadeTratamentoDtos.PeriocidadeTratamentoDTO>();
      _ = CreateMap<PeriocidadeTratamento, PeriocidadeTratamentoDtos.PeriocidadeTratamentoLightDTO>();
      _ = CreateMap<PeriocidadeTratamento, PeriocidadeTratamentoDtos.PeriocidadeTratamentoTableDTO>();
      _ = CreateMap<PeriocidadeTratamentoDtos.CreatePeriocidadeTratamentoRequest, PeriocidadeTratamento>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<PeriocidadeTratamentoDtos.UpdatePeriocidadeTratamentoRequest, PeriocidadeTratamento>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- Evolucao Tratamento ----
      _ = CreateMap<EvolucaoTratamento, EvolucaoTratamentoDtos.EvolucaoTratamentoDTO>();
      _ = CreateMap<EvolucaoTratamento, EvolucaoTratamentoDtos.EvolucaoTratamentoLightDTO>();
      _ = CreateMap<EvolucaoTratamento, EvolucaoTratamentoDtos.EvolucaoTratamentoTableDTO>();
      _ = CreateMap<EvolucaoTratamentoDtos.CreateEvolucaoTratamentoRequest, EvolucaoTratamento>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<EvolucaoTratamentoDtos.UpdateEvolucaoTratamentoRequest, EvolucaoTratamento>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<EvolucaoTratamentoDtos.DeleteMultipleEvolucaoTratamentoRequest, EvolucaoTratamento>()
        .ForMember(d => d.Id, o => o.Ignore());

      _ = CreateMap<EvolucaoTratamentoFicheiro, EvolucaoTratamentoFicheiroDtos.EvolucaoTratamentoFicheiroDTO>();
      _ = CreateMap<EvolucaoTratamentoFicheiroDtos.CreateEvolucaoTratamentoFicheiroRequest, EvolucaoTratamentoFicheiro>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- Mapa Body Chart ----
      _ = CreateMap<CliCloud.Domain.Entities.ProcessoClinico.BodyChart.MarcadorBodyChart, MapaBodyChartDtos.MarcadorBodyChartDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.ProcessoClinico.BodyChart.MapaBodyChart, MapaBodyChartDtos.MapaBodyChartDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.ProcessoClinico.BodyChart.MapaBodyChart, MapaBodyChartDtos.MapaBodyChartLightDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.ProcessoClinico.BodyChart.MapaBodyChart, MapaBodyChartDtos.MapaBodyChartTableDTO>();
      _ = CreateMap<MapaBodyChartDtos.CreateMapaBodyChartRequest, CliCloud.Domain.Entities.ProcessoClinico.BodyChart.MapaBodyChart>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<MapaBodyChartDtos.UpdateMapaBodyChartRequest, CliCloud.Domain.Entities.ProcessoClinico.BodyChart.MapaBodyChart>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- Notas Body Chart ----
      _ = CreateMap<CliCloud.Domain.Entities.ProcessoClinico.BodyChart.NotaBodyChart, NotasBodyChartDtos.NotasBodyChartDTO>()
        .ForMember(d => d.Nome, o => o.MapFrom(s => s.Titulo));
      _ = CreateMap<CliCloud.Domain.Entities.ProcessoClinico.BodyChart.NotaBodyChart, NotasBodyChartDtos.NotasBodyChartLightDTO>()
        .ForMember(d => d.Nome, o => o.MapFrom(s => s.Titulo));
      _ = CreateMap<CliCloud.Domain.Entities.ProcessoClinico.BodyChart.NotaBodyChart, NotasBodyChartDtos.NotasBodyChartTableDTO>()
        .ForMember(d => d.Nome, o => o.MapFrom(s => s.Titulo));
      _ = CreateMap<NotasBodyChartDtos.CreateNotasBodyChartRequest, CliCloud.Domain.Entities.ProcessoClinico.BodyChart.NotaBodyChart>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.Titulo, o => o.MapFrom(s => s.Nome));
      _ = CreateMap<NotasBodyChartDtos.UpdateNotasBodyChartRequest, CliCloud.Domain.Entities.ProcessoClinico.BodyChart.NotaBodyChart>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.Titulo, o => o.MapFrom(s => s.Nome));

      // ---- Relatorio Exames ----
      _ = CreateMap<RelatorioExames, RelatorioExamesDtos.RelatorioExamesDTO>();
      _ = CreateMap<RelatorioExamesDtos.CreateRelatorioExamesRequest, RelatorioExames>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<RelatorioExamesDtos.UpdateRelatorioExamesRequest, RelatorioExames>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- DocumentosFichaClinica ----
      _ = CreateMap<DocumentosFichaClinica, DocumentosFichaClinicaDtos.DocumentosFichaClinicaDTO>()
        .ForMember(d => d.Categoria, o => o.MapFrom(s => s.Categoria.ToString()))
        .ForMember(d => d.Tipo, o => o.MapFrom(s => s.Tipo.ToString()));

      // ---- RelatorioAtestado ----
      _ = CreateMap<RelatorioAtestado, RelatorioAtestadoDtos.RelatorioAtestadoDTO>()
        .ForMember(d => d.MedicoNome, o => o.MapFrom(s => s.Medico.Nome))
        .ForMember(d => d.MedicoNumeroProfissional, o => o.MapFrom(s => s.Medico.Carteira));
      _ = CreateMap<RelatorioAtestadoDtos.CreateRelatorioAtestadoRequest, RelatorioAtestado>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<RelatorioAtestadoDtos.UpdateRelatorioAtestadoRequest, RelatorioAtestado>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- ModeloRelatorioAtestado ----
      _ = CreateMap<ModeloRelatorioAtestado, ModeloRelatorioAtestadoDtos.ModeloRelatorioAtestadoDTO>();
      _ = CreateMap<ModeloRelatorioAtestadoDtos.CreateModeloRelatorioAtestadoRequest, ModeloRelatorioAtestado>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.EmpresaId, o => o.Ignore());
      _ = CreateMap<ModeloRelatorioAtestadoDtos.UpdateModeloRelatorioAtestadoRequest, ModeloRelatorioAtestado>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.EmpresaId, o => o.Ignore());
        
      // ---- AnamneseOdontopediatria (Estomatologia) ----
      _ = CreateMap<AnamneseOdontopediatria, AnamneseOdontopediatriaDtos.AnamneseOdontopediatriaDTO>();
      _ = CreateMap<AnamneseOdontopediatriaDtos.CreateAnamneseOdontopediatriaRequest, AnamneseOdontopediatria>();
      _ = CreateMap<AnamneseOdontopediatriaDtos.UpdateAnamneseOdontopediatriaRequest, AnamneseOdontopediatria>();
      _ = CreateMap<ExameDtos.UpdateExameLinhaRequest, ExameLinha>()
        .ForMember(d => d.ExameId, o => o.Ignore())
        .ForMember(d => d.Exame, o => o.Ignore())
        .ForMember(d => d.TipoExame, o => o.Ignore());

      // ---- AnamneseOrtodonticaAnaliseGeral ----
      _ = CreateMap<AnamneseOrtodonticaAnaliseGeral, AnamneseOrtodonticaAnaliseGeralDtos.AnamneseOrtodonticaAnaliseGeralDTO>();
      _ = CreateMap<AnamneseOrtodonticaAnaliseGeralDtos.CreateAnamneseOrtodonticaAnaliseGeralRequest, AnamneseOrtodonticaAnaliseGeral>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<AnamneseOrtodonticaAnaliseGeralDtos.UpdateAnamneseOrtodonticaAnaliseGeralRequest, AnamneseOrtodonticaAnaliseGeral>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- AnamneseOrtodonticaAnaliseDentaria ----
      _ = CreateMap<AnamneseOrtodonticaAnaliseDentaria, AnamneseOrtodonticaAnaliseDentariaDtos.AnamneseOrtodonticaAnaliseDentariaDTO>();
      _ = CreateMap<AnamneseOrtodonticaAnaliseDentariaDtos.CreateAnamneseOrtodonticaAnaliseDentariaRequest, AnamneseOrtodonticaAnaliseDentaria>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<AnamneseOrtodonticaAnaliseDentariaDtos.UpdateAnamneseOrtodonticaAnaliseDentariaRequest, AnamneseOrtodonticaAnaliseDentaria>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- AnamneseOrtodonticaDenticaoDeciduaeMista ----
      _ = CreateMap<AnamneseOrtodonticaDenticaoDeciduaeMista, AnamneseOrtodonticaDenticaoDeciduaeMistaDtos.AnamneseOrtodonticaDenticaoDeciduaeMistaDTO>();
      _ = CreateMap<AnamneseOrtodonticaDenticaoDeciduaeMistaDtos.CreateAnamneseOrtodonticaDenticaoDeciduaeMistaRequest, AnamneseOrtodonticaDenticaoDeciduaeMista>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<AnamneseOrtodonticaDenticaoDeciduaeMistaDtos.UpdateAnamneseOrtodonticaDenticaoDeciduaeMistaRequest, AnamneseOrtodonticaDenticaoDeciduaeMista>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- AnamneseOrtodonticaATM ----
      _ = CreateMap<AnamneseOrtodonticaATM, AnamneseOrtodonticaATMDtos.AnamneseOrtodonticaATMDTO>();
      _ = CreateMap<AnamneseOrtodonticaATMDtos.CreateAnamneseOrtodonticaATMRequest, AnamneseOrtodonticaATM>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<AnamneseOrtodonticaATMDtos.UpdateAnamneseOrtodonticaATMRequest, AnamneseOrtodonticaATM>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- AnamneseOrtodonticaAnaliseFuncional ----
      _ = CreateMap<AnamneseOrtodonticaAnaliseFuncional, AnamneseOrtodonticaAnaliseFuncionalDtos.AnamneseOrtodonticaAnaliseFuncionalDTO>();
      _ = CreateMap<AnamneseOrtodonticaAnaliseFuncionalDtos.CreateAnamneseOrtodonticaAnaliseFuncionalRequest, AnamneseOrtodonticaAnaliseFuncional>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<AnamneseOrtodonticaAnaliseFuncionalDtos.UpdateAnamneseOrtodonticaAnaliseFuncionalRequest, AnamneseOrtodonticaAnaliseFuncional>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- EstadosDentarios ----
      _ = CreateMap<EstadosDentarios, EstadosDentariosDtos.EstadosDentariosDTO>();
      _ = CreateMap<EstadosDentariosDtos.CreateEstadosDentariosRequest, EstadosDentarios>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<EstadosDentariosDtos.UpdateEstadosDentariosRequest, EstadosDentarios>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- OdontogramaDefinitivo ----
      _ = CreateMap<OdontogramaDefinitivo, OdontogramaDefinitivoDtos.OdontogramaDefinitivoDTO>();
      _ = CreateMap<OdontogramaDefinitivoDtos.CreateOdontogramaDefinitivoRequest, OdontogramaDefinitivo>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<OdontogramaDefinitivoDtos.UpdateOdontogramaDefinitivoRequest, OdontogramaDefinitivo>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- TipoTratamentoDentario ----
      _ = CreateMap<TipoTratamentoDentario, TiposTratamentoDentarioDtos.TiposTratamentoDentarioDTO>();
      _ = CreateMap<TiposTratamentoDentarioDtos.CreateTiposTratamentoDentarioRequest, TipoTratamentoDentario>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<TiposTratamentoDentarioDtos.UpdateTiposTratamentoDentarioRequest, TipoTratamentoDentario>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- NotificacaoTipo ----
       _ = CreateMap<CliCloud.Domain.Entities.Notificacoes.NotificacaoTipo, NotificacaoTipoDtos.NotificacaoTipoDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.Notificacoes.NotificacaoTipo, NotificacaoTipoDtos.NotificacaoTipoLightDTO>();
      _ = CreateMap<CliCloud.Domain.Entities.Notificacoes.NotificacaoTipo, NotificacaoTipoDtos.NotificacaoTipoTableDTO>();
      _ = CreateMap<NotificacaoTipoDtos.CreateNotificacaoTipoRequest, CliCloud.Domain.Entities.Notificacoes.NotificacaoTipo>()
        .ForMember(d => d.Id, o => o.Ignore());
      _ = CreateMap<NotificacaoTipoDtos.UpdateNotificacaoTipoRequest, CliCloud.Domain.Entities.Notificacoes.NotificacaoTipo>()
        .ForMember(d => d.Id, o => o.Ignore());

      // ---- Notificacao ----
      _ = CreateMap<Notificacao, NotificacaoDtos.NotificacaoDTO>()
        .ForMember(d => d.TipoDesignacao,o => o.MapFrom(s => s.NotificacaoTipo != null ? s.NotificacaoTipo.DesignacaoTipo : null))
        .ForMember(d => d.EstadoDesignacao, o => o.Ignore())
        .ForMember(d => d.PrioridadeDesignacao, o => o.Ignore())
        .ForMember(d => d.AlcanceResumo, o => o.Ignore());
      _ = CreateMap<Notificacao, NotificacaoDtos.NotificacaoTableDTO>()
        .ForMember(d => d.TipoDesignacao,o => o.MapFrom(s => s.NotificacaoTipo != null ? s.NotificacaoTipo.DesignacaoTipo : null))
        .ForMember(d => d.Lida, o => o.MapFrom(s => s.DataLeitura.HasValue))
        .ForMember(d => d.EstadoDesignacao,o => o.MapFrom(s => CliCloud.Application.Services.Notificacoes.NotificacaoService.NotificacaoLabels.EstadoPt(s.Estado)))
        .ForMember(d => d.PrioridadeDesignacao,o => o.MapFrom(s => CliCloud.Application.Services.Notificacoes.NotificacaoService.NotificacaoLabels.PrioridadePt(s.Prioridade)));
      _ = CreateMap<NotificacaoDtos.CreateNotificacaoRequest, Notificacao>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.NotificacaoTipo, o => o.Ignore())
        .ForMember(d => d.RemetenteId, o => o.Ignore())
        .ForMember(d => d.DataLeitura, o => o.Ignore())
        .ForMember(d => d.LeituraPor, o => o.Ignore());


      // ---- EstadoSinistro ----

      _ = CreateMap<EstadoSinistroItem, CliCloud.Application.Services.Sinistros.EstadoSinistroService.DTOs.EstadoSinistroDTO>();
      _ = CreateMap<CliCloud.Application.Services.Sinistros.EstadoSinistroService.DTOs.CreateEstadoSinistroRequest, EstadoSinistroItem>();
      _ = CreateMap<CliCloud.Application.Services.Sinistros.EstadoSinistroService.DTOs.UpdateEstadoSinistroRequest, EstadoSinistroItem>();

      // ---- Sinistrado ----
      _ = CreateMap<SinistradoLinhaServico, CliCloud.Application.Services.Sinistros.SinistradoService.DTOs.SinistradoLinhaServicoDTO>();
      _ = CreateMap<Sinistrado, CliCloud.Application.Services.Sinistros.SinistradoService.DTOs.SinistradoDTO>()
        .ForMember(d => d.EstadoSinistroDesignacao, o => o.MapFrom(s => s.EstadoSinistro != null ? s.EstadoSinistro.Designacao : null));
      _ = CreateMap<Sinistrado, CliCloud.Application.Services.Sinistros.SinistradoService.DTOs.SinistradoTableDTO>()
        .ForMember(d => d.EstadoSinistroDesignacao, o => o.MapFrom(s => s.EstadoSinistro != null ? s.EstadoSinistro.Designacao : null))
        .ForMember(d => d.UtenteNumero, o => o.MapFrom(s => s.Utente != null ? s.Utente.NumeroUtente : null))
        .ForMember(d => d.UtenteNome, o => o.MapFrom(s => s.Utente != null ? s.Utente.Nome : null));
      _ = CreateMap<CliCloud.Application.Services.Sinistros.SinistradoService.DTOs.CreateSinistradoRequest, Sinistrado>();
      _ = CreateMap<CliCloud.Application.Services.Sinistros.SinistradoService.DTOs.UpdateSinistradoRequest, Sinistrado>();

      // ---- LoteDirect ----
      _ = CreateMap<LoteDirect, CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs.LoteDirectDTO>()
        .ForMember(d => d.UtenteNome, o => o.MapFrom(s => s.Utente != null ? s.Utente.Nome : null))
        .ForMember(d => d.MedicoNome, o => o.MapFrom(s => s.Medico != null ? s.Medico.Nome : null))
        .ForMember(d => d.MedicoExternoNome, o => o.MapFrom(s => s.MedicoExterno != null ? s.MedicoExterno.Nome : null))
        .ForMember(d => d.TipoServicoRegistoDescricao,o => o.MapFrom(s => s.TipoServicoRegisto != null ? s.TipoServicoRegisto.Descricao : null))
        .ForMember(d => d.ServicoConsultaDesignacao,o => o.MapFrom(s => s.ServicoConsultaRegisto != null ? s.ServicoConsultaRegisto.Designacao : null))
        .ForMember(d => d.OrganismoSigla, o => o.Ignore());
      _ = CreateMap<LoteDirect, CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs.LoteDirectTableDTO>()
        .ForMember(d => d.UtenteNumero, o => o.MapFrom(s => s.Utente != null ? s.Utente.NumeroUtente : null))
        .ForMember(d => d.UtenteNome, o => o.MapFrom(s => s.Utente != null ? s.Utente.Nome : null))
        .ForMember(d => d.MesAno, o => o.MapFrom(s => s.Mes.HasValue && s.Ano.HasValue ? $"{s.Mes:00}/{s.Ano}" : null))
        .ForMember(d => d.OrganismoSigla, o => o.Ignore());
      // Linhas / Linhas789 vêm em DTOs de upsert e são gravadas por ILoteDirectLinhasSyncRepository (não mapear para a entidade).
      _ = CreateMap<CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs.CreateLoteDirectRequest, LoteDirect>()
        .ForMember(d => d.Linhas, o => o.Ignore())
        .ForMember(d => d.Linhas789, o => o.Ignore())
        .ForMember(d => d.Utente, o => o.Ignore())
        .ForMember(d => d.Medico, o => o.Ignore())
        .ForMember(d => d.MedicoExterno, o => o.Ignore())
        .ForMember(d => d.TipoServicoRegisto, o => o.Ignore())
        .ForMember(d => d.ServicoConsultaRegisto, o => o.Ignore());

      // Linhas / Linhas789 (leitura no GetById)
      _ = CreateMap<LoteDirectLinha, CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs.LoteDirectLinhaDTO>()
        .ForMember(d => d.ServicoDesignacao,o => o.MapFrom(s => s.Servico != null ? s.Servico.Designacao : null));
      _ = CreateMap<LoteDirectLinha789, CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs.LoteDirectLinhaDTO>()
        .ForMember(d => d.ServicoDesignacao,o => o.MapFrom(s => s.Servico != null ? s.Servico.Designacao : null));

      _ = CreateMap<CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs.UpdateLoteDirectRequest, LoteDirect>()
        .ForMember(d => d.Linhas, o => o.Ignore())
        .ForMember(d => d.Linhas789, o => o.Ignore())
        .ForMember(d => d.Utente, o => o.Ignore())
        .ForMember(d => d.Medico, o => o.Ignore())
        .ForMember(d => d.MedicoExterno, o => o.Ignore())
        .ForMember(d => d.TipoServicoRegisto, o => o.Ignore())
        .ForMember(d => d.ServicoConsultaRegisto, o => o.Ignore());
        
      _ = CreateMap<TipoLote, CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs.TipoLoteLightDTO>()
        .ForMember(d => d.Codigo, o => o.MapFrom(s => s.Id));

      _ = CreateMap<LoteDirectAgregado, CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs.LoteDirectAgregadoTableDTO>()
        .ForMember(d => d.OrganismoSigla, o => o.Ignore())
        .ForMember(d => d.TipoLoteDesignacao, o => o.Ignore());

      _ = CreateMap<LoteDirectAgregado, CliCloud.Application.Services.Faturacao.CredenciaisSnsService.DTOs.CredenciaisSnsLoteTableDTO>()
        .ForMember(d => d.MesNome, o => o.Ignore())
        .ForMember(d => d.OrganismoSigla, o => o.Ignore())
        .ForMember(d => d.OrganismoNome, o => o.Ignore())
        .ForMember(d => d.TipoLoteDesignacao, o => o.Ignore())
        .ForMember(d => d.TipoServicoDesignacao, o => o.Ignore());

      _ = CreateMap<Admissao, CliCloud.Application.Services.Consultas.OrdemEntradaAdministrativoService.DTOs.OrdemEntradaTableDTO>()
        .ForMember(d => d.UtenteNumero, o => o.MapFrom(s => s.Utente != null ? s.Utente.NumeroUtente : null))
        .ForMember(d => d.UtenteNome, o => o.MapFrom(s => s.Utente != null ? s.Utente.Nome : null))
        .ForMember(d => d.MedicoNome, o => o.MapFrom(s => s.Medico != null ? s.Medico.Nome : null))
        .ForMember(d => d.EspecialidadeDesignacao, o => o.MapFrom(s => s.Especialidade != null ? s.Especialidade.Nome : null))
        .ForMember(d => d.TipoConsultaDesignacao, o => o.MapFrom(s => s.TipoConsultaItem != null ? s.TipoConsultaItem.Designacao : null))
        .ForMember(d => d.ConsultaId, o => o.MapFrom(s => s.Consulta != null ? (Guid?)s.Consulta.Id : null))
        .ForMember(d => d.ConsultaPromovida, o => o.MapFrom(s => s.Consulta != null))
        .ForMember(d => d.StatusConsultaLabel, o => o.Ignore());

      _ = CreateMap<ListaEsperaConsulta, CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService.DTOs.ListaEsperaDTO>()
        .ForMember(d => d.UtenteNumero, o => o.MapFrom(s => s.Utente != null ? s.Utente.NumeroUtente : null))
        .ForMember(d => d.UtenteNome, o => o.MapFrom(s => s.Utente != null ? s.Utente.Nome : null))
        .ForMember(d => d.MedicoNome, o => o.MapFrom(s => s.Medico != null ? s.Medico.Nome : null))
        .ForMember(d => d.EspecialidadeDesignacao, o => o.MapFrom(s => s.Especialidade != null ? s.Especialidade.Nome : null))
        .ForMember(d => d.OrganismoNome, o => o.MapFrom(s => s.Organismo != null ? s.Organismo.Nome : null))
        .ForMember(d => d.PrioridadeDesignacao, o => o.MapFrom(s => s.Prioridade != null ? s.Prioridade.Descricao : null))
        .ForMember(d => d.TipoConsultaDesignacao,o => o.MapFrom(s => s.TipoConsultaItem != null ? s.TipoConsultaItem.Designacao : null));

      _ = CreateMap<ListaEsperaConsulta, CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService.DTOs.ListaEsperaTableDTO>()
        .ForMember(d => d.UtenteNumero, o => o.MapFrom(s => s.Utente != null ? s.Utente.NumeroUtente : null))
        .ForMember(d => d.UtenteNome, o => o.MapFrom(s => s.Utente != null ? s.Utente.Nome : null))
        .ForMember(d => d.MedicoNome, o => o.MapFrom(s => s.Medico != null ? s.Medico.Nome : null))
        .ForMember(d => d.EspecialidadeDesignacao, o => o.MapFrom(s => s.Especialidade != null ? s.Especialidade.Nome : null))
        .ForMember(d => d.OrganismoNome, o => o.MapFrom(s => s.Organismo != null ? s.Organismo.Nome : null))
        .ForMember(d => d.PrioridadeDesignacao, o => o.MapFrom(s => s.Prioridade != null ? s.Prioridade.Descricao : null))
        .ForMember(d => d.TipoConsultaDesignacao,o => o.MapFrom(s => s.TipoConsultaItem != null ? s.TipoConsultaItem.Designacao : null))
        .ForMember(d => d.HoraInicio,o => o.MapFrom(s =>s.HoraInicio.HasValue? $"{s.HoraInicio.Value.Hours:D2}:{s.HoraInicio.Value.Minutes:D2}": null))
        .ForMember(d => d.Convertido, o => o.MapFrom(s => s.ConsultaMarcacaoId != null));

      _ = CreateMap<CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService.DTOs.CreateListaEsperaRequest, ListaEsperaConsulta>()
        .ForMember(d => d.Id, o => o.Ignore());

      _ = CreateMap<CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService.DTOs.UpdateListaEsperaRequest, ListaEsperaConsulta>()
        .ForMember(d => d.Id, o => o.Ignore())
        .ForMember(d => d.ConsultaMarcacaoId, o => o.Ignore())
        .ForMember(d => d.ConvertidoEm, o => o.Ignore());

    }

    private static string? ResolveDocumentoOrigemLabel(Documento documento)
    {
      if (documento.ModuloOrigem.HasValue)
      {
        return documento.ModuloOrigem.Value switch
        {
          ModuloOrigemDocumento.Faturacao => "Faturação",
          ModuloOrigemDocumento.Tratamentos => "Tratamentos",
          ModuloOrigemDocumento.Consultas => "Consultas",
          ModuloOrigemDocumento.Exames => "Exames",
          ModuloOrigemDocumento.CredenciaisSns => "Credenciais SNS",
          ModuloOrigemDocumento.HistorialUtente => "Historial utente",
          ModuloOrigemDocumento.Modalidades => "Modalidades",
          _ => documento.ModuloOrigem.Value.ToString(),
        };
      }

      if (documento.Origem.HasValue)
        return documento.Origem.Value.ToString(CultureInfo.InvariantCulture);

      return null;
    }

    private static string? ResolveDocumentoReferenciaListagem(Documento documento)
    {
      if (documento.DocumentoOrigem == null)
        return null;

      if (!string.IsNullOrWhiteSpace(documento.DocumentoOrigem.NumeroExibicao))
        return documento.DocumentoOrigem.NumeroExibicao.Trim();

      if (documento.DocumentoOrigem.TipoDocumento?.Abreviatura != null
          && documento.DocumentoOrigem.NumeroDocumento > 0)
      {
        return $"{documento.DocumentoOrigem.TipoDocumento.Abreviatura.Trim()} {documento.DocumentoOrigem.NumeroDocumento}";
      }

      return null;
    }

    private static string? ResolveDocumentoAdmissoesResumo(Documento documento)
    {
      const int maxLen = 80;
      var tokens = new List<string>();

      void TryAdd(ModuloOrigemDocumento? modulo, Admissao? admissao)
      {
        if (admissao == null || tokens.Count > 20)
          return;

        string prefix = modulo switch
        {
          ModuloOrigemDocumento.Consultas => "C-",
          ModuloOrigemDocumento.Modalidades => "M-",
          ModuloOrigemDocumento.Tratamentos => "T-",
          _ => documento.ModuloOrigem switch
          {
            ModuloOrigemDocumento.Consultas => "C-",
            ModuloOrigemDocumento.Modalidades => "M-",
            ModuloOrigemDocumento.Tratamentos => "T-",
            _ => "C-",
          },
        };

        string codigo = admissao.Ordem?.ToString(CultureInfo.InvariantCulture)
          ?? admissao.Id.ToString("N")[..8];
        string token = prefix + codigo;
        if (!tokens.Contains(token, StringComparer.Ordinal))
          tokens.Add(token);
      }

      if (documento.OrigemClinica?.Admissao != null)
      {
        TryAdd(
          documento.OrigemClinica.ModuloOrigem,
          documento.OrigemClinica.Admissao);
      }

      if (documento.Linhas != null)
      {
        foreach (DocumentoLinha linha in documento.Linhas)
        {
          if (linha.AdmissaoServico?.Admissao != null)
            TryAdd(documento.ModuloOrigem, linha.AdmissaoServico.Admissao);
        }
      }

      if (tokens.Count == 0)
        return null;

      var resumo = string.Join(' ', tokens);
      if (resumo.Length > maxLen)
        return resumo[..maxLen] + "...";

      return resumo;
    }

    /// <summary>Garante que o DTO da linha do subsistema tenha sempre Empresa (Id + Nome) quando existir EmpresaId, mesmo que o Include não tenha carregado a navegação.</summary>
    private static EmpresaDtos.EmpresaLightDTO? ResolveSubsistemaLinhaEmpresa(UtenteSubsistemaLinha s)
    {
      if (s.Empresa != null)
        return new EmpresaDtos.EmpresaLightDTO { Id = s.Empresa.Id, Nome = s.Empresa.Nome };
      if (s.EmpresaId != null)
        return new EmpresaDtos.EmpresaLightDTO { Id = s.EmpresaId.Value, Nome = null };
      return null;
    }

    private static Guid? ToNullableGuid(string? value)
    {
      return string.IsNullOrWhiteSpace(value) ? null : Guid.Parse(value);
    }

    private static Guid ToGuid(string value)
    {
      return Guid.Parse(value);
    }
  }
}
