using System.Net.Http.Headers;
using CliCloud.ICDImport;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
  .SetBasePath(AppContext.BaseDirectory)
  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
  .AddEnvironmentVariables()
  .Build();

var connStr = config.GetConnectionString("DefaultConnection")
  ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection em falta.");

var whoSection = config.GetSection("WhoIcd");
var options = new IcdImportOptions
{
  ClientId = whoSection["ClientId"] ?? "",
  ClientSecret = whoSection["ClientSecret"] ?? "",
  TokenEndpoint = whoSection["TokenEndpoint"] ?? "https://icdaccessmanagement.who.int/connect/token",
  MmsRootUrl = whoSection["MmsRootUrl"] ?? "https://id.who.int/icd/release/11/2025-01/mms",
  DelayMs = int.TryParse(whoSection["DelayMs"], out var d) ? d : 500,
  BatchSize = int.TryParse(whoSection["BatchSize"], out var b) ? b : 500
};

if (string.IsNullOrEmpty(options.ClientId) || string.IsNullOrEmpty(options.ClientSecret))
{
  Console.WriteLine("Configure WhoIcd:ClientId e WhoIcd:ClientSecret em appsettings.json");
  Console.WriteLine("Regista-te em https://icd.who.int/icdapi para obter as credenciais.");
  return 1;
}

int? chapterIndex = null;
int? blockIndex = null;
var reset = args.Contains("--reset");
int? resetChapter = null;
for (var i = 0; i < args.Length; i++)
{
  if (args[i] == "--chapter" && i + 1 < args.Length)
    chapterIndex = int.Parse(args[i + 1], System.Globalization.CultureInfo.InvariantCulture);
  if (args[i] == "--block" && i + 1 < args.Length)
    blockIndex = int.Parse(args[i + 1], System.Globalization.CultureInfo.InvariantCulture);
  if (args[i] == "--reset-chapter" && i + 1 < args.Length)
    resetChapter = int.Parse(args[i + 1], System.Globalization.CultureInfo.InvariantCulture);
}

Console.WriteLine("Obtendo token OAuth2...");
using var httpClient = new HttpClient();
var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
  .UseSqlServer(connStr)
  .Options;
using var context = new ApplicationDbContext(dbOptions);

var importer = new IcdImporter(httpClient, context, options);
var token = await importer.GetTokenAsync();
httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
httpClient.DefaultRequestHeaders.Add("API-Version", "v2");
httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("pt"));

if (reset)
{
  var deleted = await importer.ClearAllAsync();
  Console.WriteLine($"Apagadas {deleted} entidades existentes.");
}
else if (resetChapter.HasValue)
{
  var deleted = await importer.ClearChapterAsync(resetChapter.Value);
  Console.WriteLine($"Apagadas {deleted} entidades do capítulo {resetChapter.Value}.");
}

Console.WriteLine($"Importando ICD-11 MMS (pt)... Chapter={chapterIndex?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "todos"}, Block={blockIndex?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "todos"}");
var count = await importer.ImportByBlocksAsync(chapterIndex, blockIndex);
Console.WriteLine($"Concluído. Importadas {count} entidades.");

return 0;
