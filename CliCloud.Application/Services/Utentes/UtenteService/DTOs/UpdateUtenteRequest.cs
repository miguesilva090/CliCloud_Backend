using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Utility;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Utentes.UtenteService.DTOs
{
    public class UpdateUtenteRequest : IDto
    {
        // Campos da entidade base Entidade
        public required string Nome { get; set; }
        public required int TipoEntidadeId { get; set; }
        /// <summary>Opcional: nem todos os utentes têm email.</summary>
        public string? Email { get; set; }
        public required string NumeroContribuinte { get; set; }
        public required string RuaId { get; set; }
        public required string CodigoPostalId { get; set; }
        public required string FreguesiaId { get; set; }
        public required string ConcelhoId { get; set; }
        public required string DistritoId { get; set; }
        public required string PaisId { get; set; }
        public required string NumeroPorta { get; set; }
        public required string AndarRua { get; set; }
        public string? Observacoes { get; set; }
        public required int Status { get; set; }
        public string? UrlFoto { get; set; }
        public IEnumerable<UpsertEntidadeContactoItemRequest>? EntidadeContactos { get; set; }

        // Campos específicos de EntidadePessoa
        public DateOnly? DataNascimento { get; set; }
        public string? SexoId { get; set; }
        public string? EstadoCivilId { get; set; }
        public string? HabilitacaoId { get; set; }
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
        public string? ProfissaoId { get; set; }
        public string? GrupoSanguineoId { get; set; }
        public string? ProvenienciaUtenteId { get; set; }
        public string? OrganismoId { get; set; }
        public string? SeguradoraId { get; set; }
        public string? EmpresaId { get; set; }
        public string? CentroSaudeId { get; set; }
        public string? MedicoExternoId { get; set; }
        /// <summary>Médico (interno) associado ao utente.</summary>
        public string? MedicoId { get; set; }
        public string? NumeroUtente { get; set; }
        public string? Aviso { get; set; }
        public bool Desistencia { get; set; }
        public bool Cronico { get; set; }
        public TipoConsulta TipoConsulta { get; set; }
        public bool Migrante { get; set; }
        /// <summary>Condição SNS / Terceiro Pagador / Não especificado.</summary>
        public int? CondicaoSns { get; set; }
        /// <summary>FK para a Entidade Financeira Responsável selecionada.</summary>
        public string? EntidadeFinanceiraResponsavelId { get; set; }
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
        public DateTime? DataTratamentoDados { get; set; }
        public StatusValidacao? CCValidado { get; set; }
        public DateTime? CCDataValidacao { get; set; }
        public DateOnly? DataValidadeCU { get; set; }
        public string? NDocMigrante { get; set; }
        public DateTime? DataRegisto { get; set; }
        public TipoTaxaModeradora? TipoTaxaModeradora { get; set; }
        public IEnumerable<UpsertUtenteSubsistemaLinhaItemRequest>? SubsistemaLinhas { get; set; }
        /// <summary>GUID da conta na plataforma (opcional).</summary>
        public string? IdUtilizador { get; set; }
    }

    public class UpdateUtenteValidator : AbstractValidator<UpdateUtenteRequest>
    {
        public UpdateUtenteValidator()
        {
            _ = RuleFor(x => x.Nome).NotEmpty();
            _ = RuleFor(x => x.TipoEntidadeId)
              .NotEmpty()
              .InclusiveBetween(1, 9)
              .WithMessage("TipoEntidadeId deve ser um valor válido e não estar vazio.");
            // Email opcional: validar formato apenas quando preenchido
            _ = RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email));
            _ = RuleFor(x => x.NumeroContribuinte).NotEmpty();
            _ = RuleFor(x => x.RuaId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("RuaId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.CodigoPostalId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("CodigoPostalId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.FreguesiaId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("FreguesiaId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.ConcelhoId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("ConcelhoId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.DistritoId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("DistritoId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.PaisId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("PaisId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.NumeroPorta).NotEmpty();
            _ = RuleFor(x => x.AndarRua).NotEmpty();
            _ = RuleFor(x => x.Status)
              .InclusiveBetween(0, 3)
              .WithMessage("Status deve ser um valor válido (0 a 3).");
            _ = RuleFor(x => x.UrlFoto).Must(url => string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _)).WithMessage("UrlFoto deve ser uma URL válida.");
            _ = RuleForEach(x => x.EntidadeContactos)
                .SetValidator(new UpsertEntidadeContactoItemValidator())
                .When(x => x.EntidadeContactos != null && x.EntidadeContactos.Any());
            _ = RuleFor(x => x.IdUtilizador).Must(id => string.IsNullOrWhiteSpace(id) || GSHelpers.BeValidGuid(id)).WithMessage("IdUtilizador deve ser um GUID válido.");
        }
    }
}
