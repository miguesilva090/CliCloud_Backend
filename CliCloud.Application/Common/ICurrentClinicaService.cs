using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Common
{
  public interface ICurrentClinicaService : IScopedService
  {
    Task SetClinicaAsync();
    string? ClinicaId { get; set; }
  }
}

