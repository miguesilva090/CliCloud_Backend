using CliCloud.Application.Common.Wrapper;
using CliCloud.Infrastructure.Auth.JWT;
using CliCloud.Infrastructure.Auth.JWT.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers
{
  [AllowAnonymous]
  [Route("api/client-token")]
  [ApiController]
  public class ClientTokenController : ControllerBase
  {
    private readonly ITokenService _tokenService;

    public ClientTokenController(ITokenService tokenService)
    {
      _tokenService = tokenService;
    }

    [HttpPost]
    public async Task<IActionResult> GetClientTokenAsync([FromBody] TokenRequest request)
    {
      Response<TokenResponse> response = await _tokenService.GetTokenAsync(request);
      return Ok(response);
    }

    [HttpGet("refresh/{refreshToken}")]
    public async Task<IActionResult> RefreshClientTokenAsync(string refreshToken)
    {
      Response<TokenResponse> response = await _tokenService.RefreshTokenAsync(refreshToken);
      return Ok(response);
    }
  }
}

