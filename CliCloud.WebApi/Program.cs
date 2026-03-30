using CliCloud.Application.Common.Wrapper;
using CliCloud.WebApi.Extensions;
using CliCloud.WebApi.Logging;
using CliCloud.WebApi.Middleware;
using CliCloud.WebApi.Seeding;

try
{
  StartupLogger.LogInfo("Starting application initialization...");
  StartupLogger.LogInfo(
    $"Environment: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Not set"}"
  );

  WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

  // Customize configuration loading:
  // - Development: appsettings.Development.json
  // - Production: appsettings.Production.json (client-specific, excluded from build)
  StartupLogger.LogInfo("Loading configuration files...");
  builder.Configuration.Sources.Clear();
  
  builder.Services.AddHttpContextAccessor();

  _ = builder.Environment.IsDevelopment()
    ? builder.Configuration.AddJsonFile(
      "appsettings.Development.json",
      optional: false,
      reloadOnChange: true
    )
    : builder.Configuration.AddJsonFile(
      "appsettings.Production.json",
      optional: false,
      reloadOnChange: true
    );
  _ = builder.Configuration.AddEnvironmentVariables().AddCommandLine(args);
  StartupLogger.LogInfo("Configuration loaded successfully");

  StartupLogger.LogInfo("Configuring application services...");
  builder.Services.ConfigureApplicationServices(builder.Configuration); // Register Services / CORS / Configure Identity Requirements / JWT Settings / Register DB Contexts / Image Handling, Mailer, Fluent Validation, Automapper
  StartupLogger.LogInfo("Application services configured successfully");

  StartupLogger.LogInfo("Building application...");
  WebApplication app = builder.Build(); // Create the App
  StartupLogger.LogInfo("Application built successfully");

  // DEV seed: clinica + api key + user(role=client) para login
  if (builder.Environment.IsDevelopment())
  {
    StartupLogger.LogInfo(
      $"DEV AUTH seed: user={DevAuthSeed.SeedUserEmail}, role={DevAuthSeed.SeedRole}, apiKey={DevAuthSeed.SeedApiKey}"
    );
    await DevAuthSeed.SeedAsync(app.Services);
  }

  StartupLogger.LogInfo("Configuring middleware pipeline...");
  _ = app.UseCors("defaultPolicy"); // CORS policy (default - allow any orgin)

  // UseHttpsRedirection() removed to support both HTTP and HTTPS
  // If you want to redirect HTTP to HTTPS, configure it at IIS level instead

  _ = app.UseStaticFiles(); // Serve static files from wwwroot folder

  _ = app.UseRouting();

  _ = app.UseAuthentication();
  _ = app.UseAuthorization();

  _ = app.UseSwagger();
  _ = app.UseSwaggerUI();

  _ = app.UseMiddleware<RateLimitingMiddleware>();
  _ = app.UseMiddleware<APIKeyMiddleware>();
  _ = app.UseMiddleware<UserResolver>();
  _ = app.UseMiddleware<ExceptionHandlingMiddleware>();
  _ = app.MapControllers();


  app.MapFallback(async context =>
  {
    context.Response.StatusCode = StatusCodes.Status404NotFound;
    context.Response.ContentType = "application/json";
    Response response = Response.Fail("Não encontrado");
    await context.Response.WriteAsJsonAsync(response);
  });

  StartupLogger.LogInfo("Middleware pipeline configured successfully");
  StartupLogger.LogInfo("Application startup completed successfully. Starting web server...");

  app.Run();
}
catch (Exception ex)
{
  // Log startup errors to console (captured by IIS stdout logging)
  StartupLogger.LogError("CRITICAL: Application failed to start", ex);

  // Also write to a file for additional debugging
  StartupLogger.WriteErrorToFile(ex, "Application startup failure");

  throw; // Re-throw to ensure IIS sees the error
}
