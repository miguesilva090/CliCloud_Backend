using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Common
{
  public interface ICurrentTenantUserService : IScopedService
  {
    public void SetUser();
    string? UserId { get; set; }
  }
}
