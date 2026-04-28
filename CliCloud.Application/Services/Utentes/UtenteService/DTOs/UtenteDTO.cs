using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Services.Utility.RuaService.DTOs;
using CliCloud.Application.Services.Utility.CodigoPostalService.DTOs;
using CliCloud.Application.Services.Utility.FreguesiaService.DTOs;
using CliCloud.Application.Services.Utility.ConcelhoService.DTOs;
using CliCloud.Application.Services.Utility.DistritoService.DTOs;
using CliCloud.Application.Services.Utility.PaisService.DTOs;
using CliCloud.Application.Services.EstadosCivis.EstadoCivilService.DTOs;
using CliCloud.Application.Services.Habilitacoes.HabilitacaoService.DTOs;
using CliCloud.Application.Services.Utility.GrupoSanguineoService.DTOs;
using CliCloud.Application.Services.CentroSaude.CentroSaudeService.DTOs;
using CliCloud.Application.Services.Medicos.MedicoExternoService.DTOs;
using CliCloud.Application.Services.Medicos.MedicoService.DTOs;
using CliCloud.Application.Services.Empresas.EmpresaService.DTOs;
using CliCloud.Application.Services.Organismos.OrganismoService.DTOs;
using CliCloud.Application.Services.ProvenienciasUtente.ProvenienciaUtenteService.DTOs;
using CliCloud.Application.Services.Profissoes.ProfissaoService.DTOs;
using CliCloud.Application.Services.Seguradoras.SeguradoraService.DTOs;
using CliCloud.Application.Services.Sexos.SexoService.DTOs;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Utentes.UtenteService.DTOs
{
    public class UtenteDTO : IDto
    {
        // Campos da entidade base Entidade
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int TipoEntidadeId { get; set; }
        public string? Email { get; set; }
        public string? NumeroContribuinte { get; set; }
        public Guid? RuaId { get; set; }
        public RuaDTO? Rua { get; set; }
        public Guid? CodigoPostalId { get; set; }
        public CodigoPostalDTO? CodigoPostal { get; set; }
        public Guid? FreguesiaId { get; set; }
        public FreguesiaDTO? Freguesia { get; set; }
        public Guid? ConcelhoId { get; set; }
        public ConcelhoDTO? Concelho { get; set; }
        public Guid? DistritoId { get; set; }
        public DistritoDTO? Distrito { get; set; }
        public Guid? PaisId { get; set; }
        public PaisDTO? Pais { get; set; }
        public string? NumeroPorta { get; set; }
        public string? AndarRua { get; set; }
        public string? Observacoes { get; set; }
        public Status? Status { get; set; }
        public string? UrlFoto { get; set; }
        public DateTime CreatedOn { get; set; }
        public IEnumerable<EntidadeContactoDTO>? EntidadeContactos { get; set; }

        // Campos específicos de EntidadePessoa
        public DateOnly? DataNascimento { get; set; }
        public Guid? SexoId { get; set; }
        public SexoLightDTO? Sexo { get; set; }
        public Guid? EstadoCivilId { get; set; }
        public EstadoCivilLightDTO? EstadoCivil { get; set; }
        public Guid? HabilitacaoId { get; set; }
        public HabilitacaoLightDTO? Habilitacao { get; set; }
        public string? Nacionalidade { get; set; }
        public string? Naturalidade { get; set; }
        public string? NumeroCartaoIdentificacao { get; set; }
        public DateOnly? DataEmissaoCartaoIdentificacao { get; set; }
        public DateOnly? DataValidadeCartaoIdentificacao { get; set; }
        public string? Arquivo { get; set; }
        public string? Carteira { get; set; }
        public string? NomeUtilizador { get; set; }
        public string? UrlFotoAssinatura { get; set; }
        public string? NumeroIdentificacaoBancaria { get; set; }

        // Campos específicos de Utente
        public string? NomePai { get; set; }
        public string? NomeMae { get; set; }
        public string? NumeroSegurancaSocial { get; set; }
        public Guid? ProfissaoId { get; set; }
        public ProfissaoLightDTO? Profissao { get; set; }
        public Guid? GrupoSanguineoId { get; set; }
        public GrupoSanguineoLightDTO? GrupoSanguineo { get; set; }
        public Guid? ProvenienciaUtenteId { get; set; }
        public ProvenienciaUtenteLightDTO? ProvenienciaUtente { get; set; }
        public Guid? OrganismoId { get; set; }
        public OrganismoLightDTO? Organismo { get; set; }
        public Guid? SeguradoraId { get; set; }
        public SeguradoraLightDTO? Seguradora { get; set; }
        public Guid? EmpresaId { get; set; }
        public EmpresaLightDTO? Empresa { get; set; }
        public Guid? CentroSaudeId { get; set; }
        public CentroSaudeLightDTO? CentroSaude { get; set; }
        public Guid? MedicoExternoId { get; set; }
        public MedicoExternoLightDTO? MedicoExterno { get; set; }
        public Guid? MedicoId { get; set; }
        public MedicoLightDTO? Medico { get; set; }
        public string? NumeroUtente { get; set; }
        public string? Aviso { get; set; }
        public bool Desistencia { get; set; }
        public bool Cronico { get; set; }
        public TipoConsulta TipoConsulta { get; set; }
        public bool Migrante { get; set; }
        public int? CondicaoSns { get; set; }
        public Guid? EntidadeFinanceiraResponsavelId { get; set; }
        public string? NumeroBeneficiarioEfr { get; set; }
        public DateOnly? DataValidadeEfr { get; set; }
        public string? MigranteTipoCartao { get; set; }
        public int MarkConsentimento { get; set; }
        public int RgpdConsentimento { get; set; }
        public DateTime? DataConsentimentoRgpd { get; set; }
        public DateTime? DataRevogacaoRgpd { get; set; }
        public DateTime? DataConsentimentoMark { get; set; }
        public DateTime? DataRevogacaoMark { get; set; }
        public bool MarkTratamentoDados { get; set; }
        public StatusValidacao? CCValidado { get; set; }
        public DateTime? CCDataValidacao { get; set; }
        public DateOnly? DataValidadeCU { get; set; }
        public string? NDocMigrante { get; set; }
        public DateTime? DataRegisto { get; set; }
        public TipoTaxaModeradora? TipoTaxaModeradora { get; set; }
        /// <summary>Conta de utilizador na plataforma (portal / notificações).</summary>
        public Guid? IdUtilizador { get; set; }
        public IEnumerable<UtenteSubsistemaLinhaDTO>? SubsistemaLinhas { get; set; }
    }
}
