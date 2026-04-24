using CliCloud.Application.Common.Wrapper;
using CliCloud.Infrastructure.Auth.JWT;
using CliCloud.Infrastructure.Auth.JWT.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers
{
  [ApiController]
  [Route("api/client-token")]
  [AllowAnonymous]
  public class ClientTokenController(ITokenService tokenService) : ControllerBase
  {
    private readonly ITokenService _tokenService = tokenService;

    [HttpPost]
    [ProducesResponseType(typeof(Response<TokenResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<TokenResponse>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Response<TokenResponse>>> Login([FromBody] TokenRequest request)
    {
      Response<TokenResponse> response = await _tokenService.GetTokenAsync(request);
      return Ok(response);
    }

    [HttpGet("refresh/{refreshToken}")]
    [ProducesResponseType(typeof(Response<TokenResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<TokenResponse>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Response<TokenResponse>>> Refresh(string refreshToken)
    {
      Response<TokenResponse> response = await _tokenService.RefreshTokenAsync(refreshToken);
      return Ok(response);
    }
  }
}
