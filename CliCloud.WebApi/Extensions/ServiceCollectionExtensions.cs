using System.Text;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using CliCloud.Application.Common.Logging;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.VozService;
using CliCloud.Application.Services.Core.ChamadaUtentesService;
using CliCloud.Application.Services.Core.SmsService;
using CliCloud.Application.Utility;
using CliCloud.Infrastructure.Auth.JWT;
using CliCloud.Infrastructure.Encryption;
using CliCloud.Infrastructure.Images;
using CliCloud.Infrastructure.Mailer;
using CliCloud.Infrastructure.Mapper;
using CliCloud.Infrastructure.Persistence.Contexts;
using CliCloud.Infrastructure.Persistence.Extensions;
using CliCloud.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CliCloud.WebApi.HostedServices;
using CliCloud.Application.Services.Core.ConfigCartaConducaoService;
using CliCloud.Application.Services.Core.ConfigExamesSemPapelService;
using CliCloud.Application.Services.Core.ConfigWebServiceService;
using CliCloud.Application.Services.Atestados.SpmsCartaConducaoService;
using CliCloud.Application.Services.Core.EmailService;
using CliCloud.Application.Services.Prescricao.SpmsPrescricaoSoapService;
using CliCloud.Application.Services.ProcessoClinico.SeparadorVinculoService;
using CliCloud.Application.Services.Consultas.TeleconsultaService;
using CliCloud.Application.Services.Core.TeleconsultaService;

namespace CliCloud.WebApi.Extensions
{
  public static class ServiceCollectionExtensions // configure application services
  {
    public static void ConfigureApplicationServices(
      this IServiceCollection services,
      IConfiguration configuration
    )
    {
      #region [-- CORS --]
      _ = services.AddCors(p =>
        p.AddPolicy(
          "defaultPolicy",
          builder =>
          {
            _ = builder
              .WithOrigins("*")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("Content-Disposition");
          }
        )
      );
      #endregion

      #region [-- ADD CONTROLLERS AND SERVICES --]

      _ = services
        .AddControllers(opt =>
        {
          var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
          opt.Filters.Add(new AuthorizeFilter(policy)); // makes so that all the controllers require authorization by default
        })
        .AddJsonOptions(options =>
        {
          options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
          options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
          options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        })
        .ConfigureApiBehaviorOptions(options =>
        {
          options.InvalidModelStateResponseFactory = context =>
          {
            var errorMessages = new Dictionary<string, List<string>>();
            foreach (var (key, value) in context.ModelState)
            {
              errorMessages[key] = value.Errors.Select(e => e.ErrorMessage).ToList();
            }

            var response = new Response
            {
              Status = ResponseStatus.Failure,
              Messages = errorMessages,
            };
            return new BadRequestObjectResult(response);
          };
        });

      _ = services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

      _ = services
        .AddValidatorsFromAssemblyContaining<IRequestValidator>()
        .AddValidatorsFromAssemblyContaining<Infrastructure.Utility.IRequestValidator>();

      _ = services.AddEndpointsApiExplorer();
      _ = services.AddAutoMapper(_ => { }, typeof(MappingProfiles).Assembly);
      _ = services.AddSwaggerGen(c =>
        {
          c.OperationFilter<CliCloud.WebApi.Swagger.ApiKeyHeaderOperationFilter>();
        });
      _ = services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
      _ = services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));

      _ = services.Configure<SmsAutomaticoOptions>(configuration.GetSection("SmsAutomatico"));
      _ = services.AddTransient<IServicoSmsAutomaticoDados, ServicoSmsAutomaticoDados>();
      _ = services.AddTransient<IServicoSmsAutomatico, ServicoSmsAutomatico>();
      _ = services.AddHostedService<SmsAutomaticoHostedService>();
      _ = services.Configure<CliCloud.WebApi.HostedServices.EmailAutomaticoOptions>(configuration.GetSection("EmailAutomatico"));
      _ = services.AddTransient<IServicoEmailAutomaticoDados, ServicoEmailAutomaticoDados>();
      _ = services.AddTransient<IConfiguracaoEmailAutomaticoService, ConfiguracaoEmailAutomaticoService>();
      _ = services.AddHostedService<CliCloud.WebApi.HostedServices.EmailAutomaticoHostedService>();

      _ = services.AddTransient<IConfigCartaConducaoService, ConfigCartaConducaoService>();
      _ = services.AddTransient<IConfigExamesSemPapelService, ConfigExamesSemPapelService>();
      _ = services.AddTransient<IConfigWebServiceService, ConfigWebServiceService>();
      _ = services.AddTransient<ISpmsPrescricaoSoapService, SpmsPrescricaoSoapService>();
      _ = services.AddTransient<ISeparadorVinculoService, SeparadorVinculoService>();

      _ = services.AddTransient<IConfiguracaoEmailService, ConfiguracaoEmailService>();

      _ = services.AddServices(); // dynamic services registration

      //----------- Add Services (Dependency Injection) -------------------------------------------
      _ = services.AddSingleton<AppLogger>();

      // From DynamicServiceRegistrationExtensions
      // Auto registers scoped/transient marked services

      // ICurrentTenantUserService -- registered as Scoped (resolve the tenant/user from token/header)
      // IIdentityService, ITokenService, IRepositoryAsync, ITenantManagementService -- registered as Transient

      // Any additional app services should be registered as Transient

      //---------------------------------------------------------------------------
      _ = services.AddTransient<IServicoSms, ServicoSms>();
      _ = services.AddTransient<IServicoWebhookSms, ServicoWebhookSms>();
      _ = services.AddTransient<IServicoVoz, ServicoVoz>();
      _ = services.AddTransient<ITokenService, TokenService>();
      _ = services.AddTransient<IConfiguracaoTeleconsultaService, ConfiguracaoTeleconsultaService>();
      _ = services.AddTransient<IServicoTeleconsulta, ServicoTeleconsulta>();
      _ = services.AddTransient<IChamadaUtentesService, ChamadaUtentesService>();
      _ = services.AddTransient<ISpmsCartaConducaoService, SpmsCartaConducaoService>();

      #endregion

      #region [-- REGISTERING DB CONTEXT SERVICE --]
      _ = services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(
          configuration.GetConnectionString("DefaultConnection"),
          sqlOptions => sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
        )
      );

      _ = services.AddHttpContextAccessor();

      _ = services.AddAndMigrateDatabase<ApplicationDbContext>(configuration);
      #endregion

      #region [-- JWT SETTINGS --]

      _ = services.Configure<JWTSettings>(configuration.GetSection("JWTSettings")); // get settings from configuration (appsettings.Development.json or appsettings.Production.json)
      _ = services
        .AddAuthentication(options =>
        {
          options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o =>
        {
          // Preserva nomes das claims tal como no JWT (aspnet_user_id, clinica_id, uid, roles, …).
          // Com o default true, o JwtSecurityTokenHandler pode mapear tipos e a leitura por FindFirstValue("aspnet_user_id") falha.
          o.MapInboundClaims = false;
          o.RequireHttpsMetadata = false;
          o.SaveToken = false;
          o.TokenValidationParameters = new TokenValidationParameters
          {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = configuration["JWTSettings:Issuer"],
            ValidAudience = configuration["JWTSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
              Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"] ?? string.Empty)
            ),
            RoleClaimType = "roles",
          };
          o.Events = new JwtBearerEvents()
          {
            OnChallenge = context =>
            {
              context.HandleResponse();

              // Create a custom response for unauthorized access (401)
              Response response = Response.Fail("N�o autorizado");

              context.Response.ContentType = "application/json";
              context.Response.StatusCode = 401;

              return context.Response.WriteAsJsonAsync(response);
            },
            OnForbidden = context =>
            {
              context.Response.StatusCode = 403;
              context.Response.ContentType = "application/json";

              // Create a custom response for forbidden access (403)
              Response response = Response.Fail("Acesso proibido");

              return context.Response.WriteAsJsonAsync(response);
            },
          };
        });

      #endregion

      #region [-- ENCRYPTION --]

      _ = services.Configure<EncryptionSettings>(configuration.GetSection("EncryptionSettings"));

      #endregion

      #region [-- RATE LIMITING --]

      _ = services.AddMemoryCache();
      _ = services.Configure<RateLimitingOptions>(
        configuration.GetSection(RateLimitingOptions.SectionName)
      );

      #endregion
    }
  }
}
