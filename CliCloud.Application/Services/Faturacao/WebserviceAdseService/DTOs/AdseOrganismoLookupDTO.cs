using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Faturacao.WebserviceAdseService.DTOs;

public class AdseOrganismoLookupDTO : IDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
}
