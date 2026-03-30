using System.Globalization;
using System.Net;
using CliCloud.Application.Common.Wrapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace CliCloud.WebApi.Middleware
{
  public class RateLimitingOptions
  {
    public const string SectionName = "RateLimiting";
    public int MaxRequests { get; set; } = 100;
    public int WindowSeconds { get; set; } = 60;
    public List<string>? ExcludedPaths { get; set; }
  }

  public class RateLimitInfo
  {
    public int Count { get; set; }
    public DateTime StartTime { get; set; }
  }

  public class RateLimitingMiddleware(
    RequestDelegate next,
    IMemoryCache cache,
    IOptions<RateLimitingOptions> options
  )
  {
    private readonly RequestDelegate _next = next;
    private readonly IMemoryCache _cache = cache;
    private readonly RateLimitingOptions _options = options.Value;

    public async Task InvokeAsync(HttpContext context)
    {
      // 1. Check if path should be excluded
      if (IsPathExcluded(context.Request.Path))
      {
        await _next(context);
        return;
      }

      // 2. Get client IP
      string clientIp = GetClientIpAddress(context);

      // 3. Create cache key (per IP and endpoint)
      string endpointPath = context.Request.Path.Value ?? "/";
      string cacheKey = $"rate_limit_{clientIp}_{endpointPath}";

      // 4. Get/update request count using sliding window
      RateLimitInfo? rateLimitInfo = _cache.Get<RateLimitInfo>(cacheKey);
      DateTime now = DateTime.UtcNow;

      if (rateLimitInfo == null)
      {
        // First request for this IP+endpoint combination
        rateLimitInfo = new RateLimitInfo { Count = 1, StartTime = now };
        _ = _cache.Set(
          cacheKey,
          rateLimitInfo,
          new MemoryCacheEntryOptions
          {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_options.WindowSeconds),
          }
        );
      }
      else
      {
        // Check if window expired
        TimeSpan elapsed = now - rateLimitInfo.StartTime;
        if (elapsed.TotalSeconds >= _options.WindowSeconds)
        {
          // Window expired, reset
          rateLimitInfo = new RateLimitInfo { Count = 1, StartTime = now };
          _ = _cache.Set(
            cacheKey,
            rateLimitInfo,
            new MemoryCacheEntryOptions
            {
              AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_options.WindowSeconds),
            }
          );
        }
        else
        {
          // Window still active, increment count
          rateLimitInfo.Count++;
          // Calculate remaining time until window expires
          TimeSpan remainingTime = TimeSpan.FromSeconds(_options.WindowSeconds) - elapsed;
          _ = _cache.Set(
            cacheKey,
            rateLimitInfo,
            new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = remainingTime }
          );
        }
      }

      // 5. Check if limit exceeded → return 429
      if (rateLimitInfo.Count > _options.MaxRequests)
      {
        DateTime resetTime = rateLimitInfo.StartTime.AddSeconds(_options.WindowSeconds);
        int retryAfter = (int)Math.Ceiling((resetTime - now).TotalSeconds);

        Response response = Response.Fail(
          "Muitas requisições. Por favor, tente novamente mais tarde."
        );

        context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests; // 429
        context.Response.ContentType = "application/json";

        // Set rate limit headers
        context.Response.Headers.Append(
          "X-RateLimit-Limit",
          _options.MaxRequests.ToString(CultureInfo.InvariantCulture)
        );
        context.Response.Headers.Append(
          "X-RateLimit-Remaining",
          Math.Max(0, _options.MaxRequests - rateLimitInfo.Count)
            .ToString(CultureInfo.InvariantCulture)
        );
        context.Response.Headers.Append(
          "X-RateLimit-Reset",
          resetTime.ToString("R", CultureInfo.InvariantCulture) // RFC 1123 format
        );
        context.Response.Headers.Append(
          "Retry-After",
          retryAfter.ToString(CultureInfo.InvariantCulture)
        );

        await context.Response.WriteAsJsonAsync(response);
        return;
      }

      // 6. Set rate limit headers for successful requests
      DateTime nextResetTime = rateLimitInfo.StartTime.AddSeconds(_options.WindowSeconds);
      int remaining = Math.Max(0, _options.MaxRequests - rateLimitInfo.Count);

      context.Response.Headers.Append(
        "X-RateLimit-Limit",
        _options.MaxRequests.ToString(CultureInfo.InvariantCulture)
      );
      context.Response.Headers.Append(
        "X-RateLimit-Remaining",
        remaining.ToString(CultureInfo.InvariantCulture)
      );
      context.Response.Headers.Append(
        "X-RateLimit-Reset",
        nextResetTime.ToString("R", CultureInfo.InvariantCulture)
      );

      // 7. Continue to next middleware
      await _next(context);
    }

    private bool IsPathExcluded(PathString path)
    {
      if (_options.ExcludedPaths == null || _options.ExcludedPaths.Count == 0)
      {
        return false;
      }

      string pathValue = path.Value?.ToLowerInvariant() ?? string.Empty;

      return _options.ExcludedPaths.Any(excludedPath =>
        pathValue.StartsWith(excludedPath.ToLowerInvariant(), StringComparison.Ordinal)
      );
    }

    private static string GetClientIpAddress(HttpContext context)
    {
      // Priority 1: X-Forwarded-For header (first IP if multiple)
      if (
        context.Request.Headers.TryGetValue(
          "X-Forwarded-For",
          out Microsoft.Extensions.Primitives.StringValues forwardedFor
        ) && !string.IsNullOrEmpty(forwardedFor)
      )
      {
        string firstIp = forwardedFor.ToString().Split(',')[0].Trim();
        if (!string.IsNullOrEmpty(firstIp))
        {
          return firstIp;
        }
      }

      // Priority 2: X-Real-IP header
      if (
        context.Request.Headers.TryGetValue(
          "X-Real-IP",
          out Microsoft.Extensions.Primitives.StringValues realIp
        ) && !string.IsNullOrEmpty(realIp)
      )
      {
        return realIp.ToString().Trim();
      }

      // Priority 3: Connection.RemoteIpAddress
      if (context.Connection.RemoteIpAddress != null)
      {
        return context.Connection.RemoteIpAddress.ToString();
      }

      // Fallback
      return "unknown";
    }
  }
}
