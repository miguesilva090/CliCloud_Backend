using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Infrastructure.Auth.JWT.DTOs;
using CliCloud.Infrastructure.Encryption;
using CliCloud.Infrastructure.Identity;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CliCloud.Infrastructure.Auth.JWT
{
  /// <summary>
  /// Implementação do Auth Server (login + refresh) dentro do próprio backend.
  /// Multi-client: requer X-API-Key válido (ClinicaApiKey) e emite JWT com claim "code".
  /// </summary>
  public class TokenService : ITokenService
  {
    private readonly JWTSettings _jwtSettings;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEncryptionService _encryptionService;
    private readonly ApplicationDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenService(
      IOptions<JWTSettings> jwtSettings,
      UserManager<ApplicationUser> userManager,
      SignInManager<ApplicationUser> signInManager,
      IEncryptionService encryptionService,
      ApplicationDbContext dbContext,
      IHttpContextAccessor httpContextAccessor
    )
    {
      _jwtSettings = jwtSettings.Value;
      _userManager = userManager;
      _signInManager = signInManager;
      _encryptionService = encryptionService;
      _dbContext = dbContext;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Response<TokenResponse>> GetTokenAsync(TokenRequest request)
    {
      string? apiKey = _httpContextAccessor.HttpContext?.Items["APIKey"] as string
        ?? _httpContextAccessor.HttpContext?.Request.Headers["X-API-Key"].FirstOrDefault();

      if (string.IsNullOrWhiteSpace(apiKey))
      {
        return ResponseFactory.Fail<TokenResponse>("API Key em falta");
      }

      var clinicaApiKey = await _dbContext.ClinicasApiKeys.AsNoTracking()
        .FirstOrDefaultAsync(k => k.ApiKey == apiKey && k.Ativo);

      if (clinicaApiKey == null)
      {
        return ResponseFactory.Fail<TokenResponse>("Chave de API inválida");
      }

      ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);
      if (user == null || !user.IsActive)
      {
        return ResponseFactory.Fail<TokenResponse>("Credenciais inválidas");
      }

      SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(
        user,
        request.Password,
        lockoutOnFailure: false
      );
      if (!signInResult.Succeeded)
      {
        return ResponseFactory.Fail<TokenResponse>("Credenciais inválidas");
      }

      // Gerar refresh token e guardar no utilizador
      string refreshToken = GenerateRefreshToken();
      DateTime refreshTokenExpiryTime = DateTime.Now.AddDays(_jwtSettings.RefreshTokenDurationInDays);
      user.RefreshToken = refreshToken;
      user.RefreshTokenExpiryTime = refreshTokenExpiryTime;
      _ = await _userManager.UpdateAsync(user);

      JwtSecurityToken jwt = await GenerateJwtAsync(user, apiKey, clinicaApiKey.ClinicaId);
      DateTime expiryTime = jwt.ValidTo.ToLocalTime();

      return ResponseFactory.Success(
        new TokenResponse
        {
          Token = new JwtSecurityTokenHandler().WriteToken(jwt),
          RefreshToken = refreshToken,
          RefreshTokenExpiryTime = refreshTokenExpiryTime,
          ExpiryTime = expiryTime,
        }
      );
    }

    public async Task<Response<TokenResponse>> RefreshTokenAsync(string refreshToken)
    {
      if (string.IsNullOrWhiteSpace(refreshToken))
      {
        return ResponseFactory.Fail<TokenResponse>("Refresh token em falta");
      }

      string? apiKey = _httpContextAccessor.HttpContext?.Items["APIKey"] as string
        ?? _httpContextAccessor.HttpContext?.Request.Headers["X-API-Key"].FirstOrDefault();

      if (string.IsNullOrWhiteSpace(apiKey))
      {
        return ResponseFactory.Fail<TokenResponse>("API Key em falta");
      }

      var clinicaApiKey = await _dbContext.ClinicasApiKeys.AsNoTracking()
        .FirstOrDefaultAsync(k => k.ApiKey == apiKey && k.Ativo);

      if (clinicaApiKey == null)
      {
        return ResponseFactory.Fail<TokenResponse>("Chave de API inválida");
      }

      ApplicationUser? user = await _userManager.Users.FirstOrDefaultAsync(u =>
        u.RefreshToken == refreshToken
      );

      if (user == null)
      {
        return ResponseFactory.Fail<TokenResponse>("Token inválido");
      }

      if (user.RefreshTokenExpiryTime == null || user.RefreshTokenExpiryTime < DateTime.Now)
      {
        return ResponseFactory.Fail<TokenResponse>("Refresh token expirado");
      }

      JwtSecurityToken jwt = await GenerateJwtAsync(user, apiKey, clinicaApiKey.ClinicaId);
      DateTime expiryTime = jwt.ValidTo.ToLocalTime();

      return ResponseFactory.Success(
        new TokenResponse
        {
          Token = new JwtSecurityTokenHandler().WriteToken(jwt),
          RefreshToken = user.RefreshToken ?? refreshToken,
          RefreshTokenExpiryTime = user.RefreshTokenExpiryTime.Value,
          ExpiryTime = expiryTime,
        }
      );
    }

    private async Task<JwtSecurityToken> GenerateJwtAsync(
      ApplicationUser user,
      string apiKey,
      Guid clinicaId
    )
    {
      IList<string> roles = await _userManager.GetRolesAsync(user);

      // code claim (bind token <-> api key)
      string specificChars = GSHelpers.GetSpecificChars(apiKey, [19, 11, 12, 25]);
      string encryptedCode = _encryptionService.EncryptString(specificChars);

      List<Claim> claims =
      [
        new(JwtRegisteredClaimNames.Sub, user.UserName ?? user.Email ?? user.Id),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
        new("uid", user.Id),
        new("code", encryptedCode),
        new("clinica_id", clinicaId.ToString()),
      ];

      foreach (string role in roles)
      {
        claims.Add(new Claim("roles", role));
        // também adiciona Role standard para [Authorize(Roles=...)]
        claims.Add(new Claim(ClaimTypes.Role, role));
      }

      SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_jwtSettings.Key));
      SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

      return new JwtSecurityToken(
        issuer: _jwtSettings.Issuer,
        audience: _jwtSettings.Audience,
        claims: claims,
        expires: DateTime.Now.AddMinutes(_jwtSettings.AuthTokenDurationInMinutes),
        signingCredentials: creds
      );
    }

    private static string GenerateRefreshToken()
    {
      byte[] randomNumber = new byte[32];
      using RandomNumberGenerator generator = RandomNumberGenerator.Create();
      generator.GetBytes(randomNumber);
      return Convert.ToBase64String(randomNumber).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }
  }
}

