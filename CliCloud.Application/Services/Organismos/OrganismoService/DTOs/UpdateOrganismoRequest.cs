using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;

namespace CliCloud.Application.Services.Organismos.OrganismoService.DTOs
{
    public class UpdateOrganismoRequest : IDto
    {
        // Campos da entidade base Entidade
        public required string Nome { get; set; }
        public required int TipoEntidadeId { get; set; }
        public required string Email { get; set; }
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

        // Campos específicos de Organismo
        public string? NomeComercial { get; set; }
        public string? Abreviatura { get; set; }
        public int? PrazoPagamento { get; set; }
        public decimal? Desconto { get; set; }
        public decimal? DescontoUtente { get; set; }
        public Guid? CondicaoPagamentoId { get; set; }
        public Guid? ModoPagamentoId { get; set; }
        public string? BancoId { get; set; }
        public string? NumeroIdentificacaoBancaria { get; set; }
        public string? Apolice { get; set; }
        public decimal? Avenca { get; set; }
        public DateOnly? DataInicioContrato { get; set; }
        public DateOnly? DataFimContrato { get; set; }
        public int? NumeroPagamentos { get; set; }
        public string? CodigoClinica { get; set; }
        public int? Faltas { get; set; }
        public string? Contacto { get; set; }
        public string? Categoria { get; set; }
        public string? Ars { get; set; }
        public string? Subregiao { get; set; }
        public string? Regiao { get; set; }
        public string? FraseADM { get; set; }
        public int? Bloqueio { get; set; }
        public bool LimitarConsultas { get; set; }
        public int? NumeroConsultas { get; set; }
        public bool ContabilizarFaltas { get; set; }
        public int? AssinarPagaDocumento { get; set; }
        public int? AdmissaoCC { get; set; }
        public int? FaturaCredencial { get; set; }
        public bool DiscriminaServicos { get; set; }
        public string? DesignaTratamentos { get; set; }
        public bool ApresentarCredenciaisPrimeiraSessaoTratamento { get; set; }
        public bool ApresentarCredenciaisPrimeiraConsulta { get; set; }
        public string? CodigoFaturacao { get; set; }
        public int? FiltroFaturacao { get; set; }
        public string? CServicoFaturaResumo { get; set; }
        public int FaturarPorDatas { get; set; }
        public bool TRUST { get; set; }
        public bool ADM { get; set; }
        public bool SADGNR { get; set; }
        public bool SADPSP { get; set; }
        public bool Globalbooking { get; set; }
        public bool AlterarPrecoTratamento { get; set; }
        public string? ContabContaFA { get; set; }
        public string? ContabContaFR { get; set; }
        public string? ContabTipoContaFA { get; set; }
        public string? ContabTipoContaFR { get; set; }
        public int? CodigoULSNova { get; set; }
        public int? TratamentoCred { get; set; }
        public int Nacional { get; set; }
        public int? CodigoRegiaoAtestadoCC { get; set; }
    }

    public class UpdateOrganismoValidator : AbstractValidator<UpdateOrganismoRequest>
    {
        public UpdateOrganismoValidator()
        {
            _ = RuleFor(x => x.Nome).NotEmpty();
            _ = RuleFor(x => x.TipoEntidadeId)
              .NotEmpty()
              .Equal(6) // Organismo = 6
              .WithMessage("TipoEntidadeId deve ser 6 (Organismo).");
            _ = RuleFor(x => x.Email).NotEmpty().EmailAddress();
            _ = RuleFor(x => x.NumeroContribuinte).NotEmpty();
            _ = RuleFor(x => x.RuaId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("RuaId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.CodigoPostalId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("CodigoPostalId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.FreguesiaId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("FreguesiaId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.ConcelhoId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("ConcelhoId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.DistritoId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("DistritoId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.PaisId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("PaisId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.NumeroPorta).NotEmpty();
            _ = RuleFor(x => x.AndarRua).NotEmpty();
            _ = RuleFor(x => x.Status).NotEmpty().InclusiveBetween(1, 3).WithMessage("Status deve ser um valor válido e não estar vazio.");
            _ = RuleFor(x => x.UrlFoto).Must(url => string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _)).WithMessage("UrlFoto deve ser uma URL válida.");
            _ = RuleFor(x => x.EntidadeContactos).NotEmpty().WithMessage("EntidadeContactos deve ser um array não vazio.");
            _ = RuleForEach(x => x.EntidadeContactos).SetValidator(new UpsertEntidadeContactoItemValidator()).When(x => x.EntidadeContactos != null);
            
            // Validações específicas de Organismo
            _ = RuleFor(x => x.BancoId)
                .Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id))
                .WithMessage("BancoId deve ser um GUID válido ou vazio.");
        }
    }
}
