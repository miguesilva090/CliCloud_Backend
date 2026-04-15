using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoDocumentoService.DTOs;

public class SeparadorPersonalizadoModeloDTO : IDto
{
    public Guid? Id { get; set; }
    public bool Existe { get; set; }
    public string TextoHtml { get; set; } = string.Empty;
}

public class UpsertSeparadorPersonalizadoModeloRequest : IDto
{
    public Guid SeparadorId { get; set; }
    public string TituloSeparador { get; set; } = string.Empty;
    public string TextoHtml { get; set; } = string.Empty;
}

public class UpsertSeparadorPersonalizadoModeloValidator
    : AbstractValidator<UpsertSeparadorPersonalizadoModeloRequest>
{
    public UpsertSeparadorPersonalizadoModeloValidator()
    {
        _ = RuleFor(x => x.SeparadorId).NotEmpty();
        _ = RuleFor(x => x.TituloSeparador).NotEmpty().MaximumLength(200);
        _ = RuleFor(x => x.TextoHtml).NotEmpty();
    }
}

public class GerarImpressaoSeparadorPersonalizadoRequest : IDto
{
    public Guid SeparadorId { get; set; }
    public string TituloSeparador { get; set; } = string.Empty;
    public bool ApenasHoje { get; set; }
    public List<GerarImpressaoSeparadorCampoRequest> Campos { get; set; } = [];
}

public class GerarImpressaoSeparadorCampoRequest : IDto
{
    public string NomeCampo { get; set; } = string.Empty;
    public string HistoricoTexto { get; set; } = string.Empty;
}

public class GerarImpressaoSeparadorPersonalizadoResponse : IDto
{
    public bool ModeloVazio { get; set; }
    public string Html { get; set; } = string.Empty;
}
