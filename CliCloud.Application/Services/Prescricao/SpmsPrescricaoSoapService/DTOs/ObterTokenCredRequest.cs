using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Prescricao.SpmsPrescricaoSoapService.DTOs;

public class ObterTokenCredRequest : IDto
{
    public Guid MedicoId { get; set; }
    public string PasswordPrvr { get; set; } = string.Empty;
}