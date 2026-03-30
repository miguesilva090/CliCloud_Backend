using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Infrastructure.Auth.JWT.DTOs;

namespace CliCloud.Infrastructure.Auth.JWT
{
  public interface ITokenService : ITransientService
  {
    Task<Response<TokenResponse>> GetTokenAsync(TokenRequest request);
    Task<Response<TokenResponse>> RefreshTokenAsync(string refreshToken);
  }
}
