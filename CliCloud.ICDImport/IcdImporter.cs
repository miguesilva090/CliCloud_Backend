using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CliCloud.Domain.Entities.Doencas;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.ICDImport;

/// <summary>
/// Importa hierarquia ICD-11 MMS da API WHO para a BD.
/// Executa por blocos (subcapítulos) para batches curtos (~500 entidades).
/// </summary>
public class IcdImporter
{
  private readonly HttpClient _httpClient;
  private readonly ApplicationDbContext _context;
  private readonly IcdImportOptions _options;
  private int _totalImported;

  public IcdImporter(HttpClient httpClient, ApplicationDbContext context, IcdImportOptions options)
  {
    _httpClient = httpClient;
    _context = context;
    _options = options;
  }

  /// <summary>Remove todas as entidades ICD-11 existentes (para recomeçar do zero).</summary>
  public async Task<int> ClearAllAsync(CancellationToken ct = default)
  {
    var count = await _context.Doencas.ExecuteDeleteAsync(ct);
    return count;
  }

  /// <summary>Remove apenas o capítulo indicado e todos os seus descendentes.</summary>
  public async Task<int> ClearChapterAsync(int chapterIndex, CancellationToken ct = default)
  {
    var rootJson = await FetchJsonAsync(_options.MmsRootUrl, ct);
    var children = GetChildren(rootJson);
    if (chapterIndex < 0 || chapterIndex >= children.Count)
    {
      Console.WriteLine($"Índice de capítulo inválido: {chapterIndex}");
      return 0;
    }
    var chapterUrl = children[chapterIndex];
    var chapterIcdId = ExtractIcdId(chapterUrl);
    var chapter = await _context.Doencas.FirstOrDefaultAsync(e => e.IcdId == chapterIcdId, ct);
    if (chapter == null)
    {
      Console.WriteLine($"Capítulo {chapterIndex} não encontrado na BD.");
      return 0;
    }
    var count = await DeleteEntityAndDescendantsAsync(chapter.Id, ct);
    return count;
  }

  private async Task<int> DeleteEntityAndDescendantsAsync(Guid entityId, CancellationToken ct)
  {
    var children = await _context.Doencas.Where(e => e.ParentId == entityId).ToListAsync(ct);
    var count = 0;
    foreach (var child in children)
      count += await DeleteEntityAndDescendantsAsync(child.Id, ct);
    var entity = await _context.Doencas.FindAsync([entityId], ct);
    if (entity != null)
    {
      _context.Doencas.Remove(entity);
      await _context.SaveChangesAsync(ct);
      count++;
    }
    return count;
  }

  public async Task<string> GetTokenAsync(CancellationToken ct = default)
  {
    using var request = new HttpRequestMessage(HttpMethod.Post, _options.TokenEndpoint);
    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
    {
      ["grant_type"] = "client_credentials",
      ["client_id"] = _options.ClientId,
      ["client_secret"] = _options.ClientSecret
    });

    var response = await _httpClient.SendAsync(request, ct);
    response.EnsureSuccessStatusCode();
    var json = await response.Content.ReadAsStringAsync(ct);
    var doc = JsonDocument.Parse(json);
    return doc.RootElement.GetProperty("access_token").GetString()
      ?? throw new InvalidOperationException("Token não retornado.");
  }

  public async Task<int> ImportByBlocksAsync(
    int? chapterIndex = null,
    int? blockIndex = null,
    CancellationToken ct = default)
  {
    _totalImported = 0;

    // 1. GET root para obter capítulos
    var rootJson = await FetchJsonAsync(_options.MmsRootUrl, ct);
    var children = GetChildren(rootJson);
    if (children.Count == 0)
    {
      Console.WriteLine("Nenhum capítulo na raiz.");
      return 0;
    }

    var chaptersToProcess = chapterIndex.HasValue
      ? [children[chapterIndex.Value]]
      : children;

    foreach (var chapterUrl in chaptersToProcess)
    {
      // 2. GET capítulo para obter blocos (filhos)
      var chapterJson = await FetchJsonAsync(chapterUrl, ct);
      var chapterEntity = MapEntity(chapterJson, null, 1);
      await SaveEntityAsync(chapterEntity, ct);
      var chapterId = chapterEntity.Id;

      var blockUrls = GetChildren(chapterJson)
        .Where(u => !u.EndsWith("/unspecified", StringComparison.OrdinalIgnoreCase))
        .Where(u => !u.EndsWith("/other", StringComparison.OrdinalIgnoreCase))
        .ToList();

      var blocksToProcess = blockIndex.HasValue && blockIndex.Value < blockUrls.Count
        ? blockUrls.Skip(blockIndex.Value).Take(1).ToList()
        : blockUrls;

      foreach (var blockUrl in blocksToProcess)
      {
        await TraverseAndSaveAsync(blockUrl, chapterId, 2, ct);
      }
    }

    return _totalImported;
  }

  private async Task TraverseAndSaveAsync(
    string url,
    Guid parentId,
    int level,
    CancellationToken ct)
  {
    var json = await FetchJsonAsync(url, ct);
    var entity = MapEntity(json, parentId, level);
    await SaveEntityAsync(entity, ct);

    var children = GetChildren(json)
      .Where(u => !u.EndsWith("/unspecified", StringComparison.OrdinalIgnoreCase))
      .Where(u => !u.EndsWith("/other", StringComparison.OrdinalIgnoreCase))
      .ToList();

    foreach (var childUrl in children)
    {
      await TraverseAndSaveAsync(childUrl, entity.Id, level + 1, ct);
    }
  }

  private async Task<JsonDocument> FetchJsonAsync(string url, CancellationToken ct)
  {
    // A API devolve URIs com http://; usar https para evitar redirect que descarta Authorization
    var httpsUrl = url.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
      ? "https" + url[4..] : url;
    await Task.Delay(_options.DelayMs, ct);
    var response = await _httpClient.GetAsync(httpsUrl, ct);
    if (!response.IsSuccessStatusCode)
    {
      var body = await response.Content.ReadAsStringAsync(ct);
      throw new HttpRequestException(
        $"WHO API erro {response.StatusCode}: {response.ReasonPhrase}. URL: {url}. Body: {body[..Math.Min(500, body.Length)]}");
    }
    var content = await response.Content.ReadAsStringAsync(ct);
    return JsonDocument.Parse(content);
  }

  private static List<string> GetChildren(JsonDocument json)
  {
    var root = json.RootElement;
    if (!root.TryGetProperty("child", out var childProp))
      return [];
    var list = new List<string>();
    foreach (var item in childProp.EnumerateArray())
    {
      var s = item.GetString();
      if (!string.IsNullOrEmpty(s))
        list.Add(s);
    }
    return list;
  }

  private static Doenca MapEntity(JsonDocument json, Guid? parentId, int level)
  {
    var root = json.RootElement;
    var idStr = root.TryGetProperty("@id", out var idProp) ? idProp.GetString() ?? "" : "";
    var icdId = ExtractIcdId(idStr);
    var code = root.TryGetProperty("code", out var codeProp) ? codeProp.GetString() : null;
    var title = "Sem título";
    if (root.TryGetProperty("title", out var titleProp) &&
        titleProp.TryGetProperty("@value", out var valueProp))
      title = valueProp.GetString() ?? title;
    var classKind = root.TryGetProperty("classKind", out var kindProp) ? kindProp.GetString() ?? "" : "";

    return new Doenca
    {
      IcdId = icdId,
      Code = string.IsNullOrWhiteSpace(code) ? null : code,
      Title = title,
      ClassKind = classKind,
      Level = level,
      ParentId = parentId
    };
  }

  private static string ExtractIcdId(string uri)
  {
    if (string.IsNullOrEmpty(uri)) return "";
    var last = uri.TrimEnd('/').Split('/').LastOrDefault();
    return last ?? uri;
  }

  private async Task SaveEntityAsync(Doenca entity, CancellationToken ct)
  {
    // Evitar violar o índice único em IcdId quando a mesma entidade
    // aparece mais do que uma vez na hierarquia (janelas, refs, etc.)
    var existing = await _context.Doencas
      .FirstOrDefaultAsync(e => e.IcdId == entity.IcdId, ct);

    if (existing is null)
    {
      _context.Doencas.Add(entity);
      await _context.SaveChangesAsync(ct);
      _totalImported++;
      Console.WriteLine($"  [{_totalImported}] {entity.ClassKind} {entity.Code}: {entity.Title[..Math.Min(50, entity.Title.Length)]}...");
    }
    else
    {
      // Atualizar metadados básicos, mantendo a árvore original (ParentId/Level)
      existing.Code = entity.Code;
      existing.Title = entity.Title;
      existing.ClassKind = entity.ClassKind;
      await _context.SaveChangesAsync(ct);
      Console.WriteLine($"  [=] {existing.ClassKind} {existing.Code}: {existing.Title[..Math.Min(50, existing.Title.Length)]}... (já existia)");
    }
  }

}

public class IcdImportOptions
{
  public string ClientId { get; set; } = "";
  public string ClientSecret { get; set; } = "";
  public string TokenEndpoint { get; set; } = "https://icdaccessmanagement.who.int/connect/token";
  public string MmsRootUrl { get; set; } = "https://id.who.int/icd/release/11/2025-01/mms";
  public int DelayMs { get; set; } = 500;
  public int BatchSize { get; set; } = 500;
}
