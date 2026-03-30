using CliCloud.Application.Common.Marker;

namespace CliCloud.Infrastructure.Auth.JWT.DTOs
{
  public class TokenResponse : IDto
  {
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public DateTime ExpiryTime { get; set; }
  }
}
