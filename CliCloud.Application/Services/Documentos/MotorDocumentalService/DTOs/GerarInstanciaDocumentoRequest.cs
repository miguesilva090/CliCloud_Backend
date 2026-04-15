using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.MotorDocumentalService.DTOs;

public class GerarInstanciaDocumentoRequest: IDto
{
    public Guid ModeloDocumentoId { get; set; }
    public Guid? UtenteId { get; set; }
    public string? Titulo { get; set; } = string.Empty;
    public Dictionary<string, string>? Marcadores { get; set; } 
}

public class GerarInstanciaDocumentoValidator: AbstractValidator<GerarInstanciaDocumentoRequest>
{
    public GerarInstanciaDocumentoValidator()
    {
        RuleFor(x => x.ModeloDocumentoId).NotEmpty();
    }
}