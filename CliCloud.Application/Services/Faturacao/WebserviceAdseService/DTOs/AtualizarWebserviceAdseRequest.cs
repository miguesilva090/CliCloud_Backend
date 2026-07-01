using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Faturacao.WebserviceAdseService.DTOs;

public class AtualizarWebserviceAdseRequest : IDto 
{
    public Guid OrganismoId { get; set; }
    public Guid ClinicaFisioterapiaId { get; set; }
    public string UrlAdse { get; set; } = string.Empty;
    public string DominioUserAdse { get; set; } = string.Empty;
    public string UserAdse { get; set; } = string.Empty;
    public string PasswordAdse { get; set; } = string.Empty;
    public string PasslocalAdse { get; set; } = string.Empty;
    public int NumlocalAdse { get; set; }
    public string PastaPdfAdse { get; set; } = string.Empty;
}

public class AtualizarWebserviceAdseRequestValidator : AbstractValidator<AtualizarWebserviceAdseRequest>
{
    public AtualizarWebserviceAdseRequestValidator()
    {
        RuleFor(x => x.OrganismoId).NotEmpty();
        RuleFor(x => x.ClinicaFisioterapiaId).NotEmpty();
        RuleFor(x => x.UrlAdse).NotEmpty().MaximumLength(254);
        RuleFor(x => x.DominioUserAdse).NotEmpty().MaximumLength(20);
        RuleFor(x => x.UserAdse).NotEmpty().MaximumLength(20);
        RuleFor(x => x.PasswordAdse).NotEmpty().MaximumLength(20);
        RuleFor(x => x.PasslocalAdse).NotEmpty().MaximumLength(50);
        RuleFor(x => x.NumlocalAdse).GreaterThan(0);
        RuleFor(x => x.PastaPdfAdse).NotEmpty().MaximumLength(200);
    }
}