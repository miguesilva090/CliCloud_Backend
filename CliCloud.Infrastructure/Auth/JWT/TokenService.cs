using System.Collections.Concurrent;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Core;
using CliCloud.Infrastructure.Auth.JWT.DTOs;
using CliCloud.Infrastructure.Encryption;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CliCloud.Infrastructure.Auth.JWT
{
  public class TokenService(
    ApplicationDbContext dbContext,
    IOptions<JWTSettings> jwtSettings,
    IEncryptionService encryptionService
  ) : ITokenService
  {
    private static readonly ConcurrentDictionary<string, RefreshTokenEntry> RefreshTokens = new();
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly JWTSettings _jwtSettings = jwtSettings.Value;
    private readonly IEncryptionService _encryptionService = encryptionService;

    public async Task<Response<TokenResponse>> GetTokenAsync(TokenRequest request)
    {
      if (string.IsNullOrWhiteSpace(request?.Email) || string.IsNullOrWhiteSpace(request.Password))
      {
        return ResponseFactory.Fail<TokenResponse>("Email e password são obrigatórios.");
      }

      // AspNetUsers primeiro: se o email existir também em Clinica.AtUser, a autenticação
      // por clínica ganhava e o token ficava sem aspnet_user_id (UserId caía no uid = clínica).
      string? aspNetUserId = null;
      (bool legacyOk, string? legacyUserId) = await TryAuthenticateByLegacyUserAsync(request);
      bool authenticated = legacyOk;
      aspNetUserId = legacyUserId;

      Clinica? clinica = null;
      if (!authenticated)
      {
        clinica = await TryAuthenticateByClinicaCredentialsAsync(request);
        authenticated = clinica != null;
      }

      if (!authenticated)
      {
        return ResponseFactory.Fail<TokenResponse>("Credenciais inválidas.");
      }

      clinica ??= await ResolveClinicaForTokenAsync(request.Email);
      if (clinica == null)
      {
        return ResponseFactory.Fail<TokenResponse>("Não foi possível determinar a clínica do utilizador.");
      }

      ClinicaApiKey? activeApiKey = await _dbContext.ClinicasApiKeys
        .FirstOrDefaultAsync(k => k.ClinicaId == clinica.Id && k.Ativo);

      if (activeApiKey == null || string.IsNullOrWhiteSpace(activeApiKey.ApiKey))
      {
        return ResponseFactory.Fail<TokenResponse>(
          "Não foi encontrada uma API key ativa para a clínica."
        );
      }

      TokenResponse tokenResponse = BuildTokenResponse(clinica, request.Email, activeApiKey.ApiKey, aspNetUserId);
      return ResponseFactory.Success(tokenResponse);
    }

    private Task<Clinica?> TryAuthenticateByClinicaCredentialsAsync(TokenRequest request)
    {
      return _dbContext.Clinicas.FirstOrDefaultAsync(c =>
        c.AtUser != null
        && c.AtPass != null
        && c.AtUser == request.Email
        && c.AtPass == request.Password
      );
    }

    private async Task<(bool Ok, string? AspNetUserId)> TryAuthenticateByLegacyUserAsync(TokenRequest request)
    {
      const string sql = """
SELECT TOP(1)
  [Id],
  [Email],
  [PasswordHash]
FROM [AspNetUsers]
WHERE [IsActive] = 1
  AND (
    [NormalizedUserName] = @normalized
    OR [NormalizedEmail] = @normalized
    OR [UserName] = @email
    OR [Email] = @email
  );
""";

      string normalized = request.Email.Trim().ToUpperInvariant();

      var connection = _dbContext.Database.GetDbConnection();
      bool shouldClose = connection.State != ConnectionState.Open;
      if (shouldClose)
      {
        await connection.OpenAsync();
      }

      try
      {
        await using var command = connection.CreateCommand();
        command.CommandText = sql;

        var normalizedParam = command.CreateParameter();
        normalizedParam.ParameterName = "@normalized";
        normalizedParam.Value = normalized;
        command.Parameters.Add(normalizedParam);

        var emailParam = command.CreateParameter();
        emailParam.ParameterName = "@email";
        emailParam.Value = request.Email.Trim();
        command.Parameters.Add(emailParam);

        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
          return (false, null);
        }

        string? passwordHash = reader["PasswordHash"]?.ToString();
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
          return (false, null);
        }

        var user = new LegacyUserAuthRecord
        {
          Id = reader["Id"]?.ToString(),
          Email = reader["Email"]?.ToString(),
          PasswordHash = passwordHash,
        };

        var passwordHasher = new PasswordHasher<LegacyUserAuthRecord>();
        PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(
          user,
          user.PasswordHash,
          request.Password
        );

        if (result is not PasswordVerificationResult.Success and not PasswordVerificationResult.SuccessRehashNeeded)
        {
          return (false, null);
        }

        string? id = user.Id?.Trim();
        return string.IsNullOrWhiteSpace(id) ? (true, null) : (true, id);
      }
      finally
      {
        if (shouldClose)
        {
          await connection.CloseAsync();
        }
      }
    }

    private async Task<Clinica?> ResolveClinicaForTokenAsync(string email)
    {
      Clinica? byEmail = await _dbContext.Clinicas.FirstOrDefaultAsync(c => c.AtUser == email);
      if (byEmail != null)
      {
        return byEmail;
      }

      Clinica? defaultClinica = await _dbContext.Clinicas.FirstOrDefaultAsync(c => c.PorDefeito);
      if (defaultClinica != null)
      {
        return defaultClinica;
      }

      return await _dbContext.Clinicas.FirstOrDefaultAsync();
    }

    public async Task<Response<TokenResponse>> RefreshTokenAsync(string refreshToken)
    {
      if (string.IsNullOrWhiteSpace(refreshToken))
      {
        return ResponseFactory.Fail<TokenResponse>("Refresh token inválido.");
      }

      if (!RefreshTokens.TryGetValue(refreshToken, out RefreshTokenEntry? entry))
      {
        return ResponseFactory.Fail<TokenResponse>("Refresh token inválido.");
      }

      if (entry.ExpiresAtUtc <= DateTime.UtcNow)
      {
        _ = RefreshTokens.TryRemove(refreshToken, out _);
        return ResponseFactory.Fail<TokenResponse>("Refresh token expirado.");
      }

      Clinica? clinica = await _dbContext.Clinicas.FirstOrDefaultAsync(c => c.Id == entry.ClinicaId);
      if (clinica == null)
      {
        _ = RefreshTokens.TryRemove(refreshToken, out _);
        return ResponseFactory.Fail<TokenResponse>("Clínica não encontrada.");
      }

      TokenResponse tokenResponse = BuildTokenResponse(clinica, entry.Email, entry.ApiKey, entry.AspNetUserId);
      _ = RefreshTokens.TryRemove(refreshToken, out _);

      return ResponseFactory.Success(tokenResponse);
    }

    private TokenResponse BuildTokenResponse(Clinica clinica, string email, string apiKey, string? aspNetUserId)
    {
      DateTime issuedAtUtc = DateTime.UtcNow;
      DateTime accessTokenExpiry = issuedAtUtc.AddMinutes(_jwtSettings.AuthTokenDurationInMinutes);
      DateTime refreshTokenExpiry = issuedAtUtc.AddDays(_jwtSettings.RefreshTokenDurationInDays);

      string codePlainText = GSHelpers.GetSpecificChars(apiKey, [19, 11, 12, 25]);
      string codeEncrypted = _encryptionService.EncryptString(codePlainText);
      string refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

      List<Claim> claims =
      [
        new Claim(JwtRegisteredClaimNames.Sub, clinica.Id.ToString()),
        new Claim("uid", clinica.Id.ToString()),
        new Claim("email", email),
        new Claim("clinica_id", clinica.Id.ToString()),
        new Claim("roles", "client"),
        new Claim("code", codeEncrypted),
      ];

      if (!string.IsNullOrWhiteSpace(aspNetUserId))
      {
        claims.Add(new Claim("aspnet_user_id", aspNetUserId.Trim()));
      }

      SigningCredentials signingCredentials = new(
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
        SecurityAlgorithms.HmacSha256
      );

      JwtSecurityToken jwtToken = new(
        issuer: _jwtSettings.Issuer,
        audience: _jwtSettings.Audience,
        claims: claims,
        notBefore: issuedAtUtc,
        expires: accessTokenExpiry,
        signingCredentials: signingCredentials
      );

      _ = RefreshTokens.TryAdd(
        refreshToken,
        new RefreshTokenEntry(clinica.Id, email, apiKey, refreshTokenExpiry, aspNetUserId)
      );

      return new TokenResponse
      {
        Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
        RefreshToken = refreshToken,
        ExpiryTime = accessTokenExpiry,
        RefreshTokenExpiryTime = refreshTokenExpiry,
      };
    }

    private sealed record RefreshTokenEntry(
      Guid ClinicaId,
      string Email,
      string ApiKey,
      DateTime ExpiresAtUtc,
      string? AspNetUserId
    );

    private sealed class LegacyUserAuthRecord
    {
      public string? Id { get; init; }
      public string? Email { get; init; }
      public string PasswordHash { get; init; } = string.Empty;
    }
  }
}
