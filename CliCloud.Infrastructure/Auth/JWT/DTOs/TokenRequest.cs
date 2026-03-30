using CliCloud.Application.Common.Marker;

namespace CliCloud.Infrastructure.Auth.JWT.DTOs
{
  public class TokenRequest : IDto
  {
    // tenant key in header
    public string Email { get; set; }
    public string Password { get; set; }
  }
}
