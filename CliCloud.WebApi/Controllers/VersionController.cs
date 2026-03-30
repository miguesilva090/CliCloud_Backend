using System.Text.Json;
using System.Text.Json.Serialization;
using CliCloud.Application.Common.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class VersionController(IWebHostEnvironment environment) : ControllerBase
  {
    private readonly IWebHostEnvironment _environment = environment;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
      PropertyNameCaseInsensitive = true,
    };

    [Authorize(Roles = "client")]
    [HttpGet]
    public IActionResult GetVersion()
    {
      try
      {
        // Get the path to version.json (at the root of the solution)
        // Try multiple possible locations
        string? versionFilePath = null;
        string contentRoot = _environment.ContentRootPath;

        // Try going up from ContentRootPath to find version.json
        DirectoryInfo? currentDir = new DirectoryInfo(contentRoot);
        while (currentDir != null && versionFilePath == null)
        {
          string candidatePath = Path.Combine(currentDir.FullName, "version.json");
          if (System.IO.File.Exists(candidatePath))
          {
            versionFilePath = candidatePath;
            break;
          }
          currentDir = currentDir.Parent;
        }

        // If still not found, try the original approach
        if (versionFilePath == null)
        {
          versionFilePath = Path.Combine(contentRoot, "..", "..", "..", "version.json");
          versionFilePath = Path.GetFullPath(versionFilePath);
        }

        // Check if file exists
        if (!System.IO.File.Exists(versionFilePath))
        {
          Response<VersionDTO> errorResponse = ResponseFactory.Fail<VersionDTO>(
            "Version file not found"
          );
          return NotFound(errorResponse);
        }

        // Read and parse the JSON file
        string jsonContent = System.IO.File.ReadAllText(versionFilePath).Trim();

        if (string.IsNullOrWhiteSpace(jsonContent))
        {
          Response<VersionDTO> errorResponse = ResponseFactory.Fail<VersionDTO>(
            "Version file is empty"
          );
          return BadRequest(errorResponse);
        }

        // Validate and parse JSON
        VersionDTO? versionData;
        try
        {
          // First validate JSON structure
          using (JsonDocument doc = JsonDocument.Parse(jsonContent))
          {
            if (!doc.RootElement.TryGetProperty("apiVersion", out JsonElement versionElement))
            {
              Response<VersionDTO> errorResponse = ResponseFactory.Fail<VersionDTO>(
                $"Version file is missing 'apiVersion' property. File content: {jsonContent}"
              );
              return BadRequest(errorResponse);
            }

            // Try to get the version value directly from JsonElement
            if (versionElement.ValueKind == JsonValueKind.String)
            {
              string versionValue = versionElement.GetString() ?? string.Empty;
              if (string.IsNullOrWhiteSpace(versionValue))
              {
                Response<VersionDTO> errorResponse = ResponseFactory.Fail<VersionDTO>(
                  "Version property is empty or null"
                );
                return BadRequest(errorResponse);
              }
              versionData = new VersionDTO { Version = versionValue };
            }
            else
            {
              // If not a string, try deserializing the whole document
              versionData = JsonSerializer.Deserialize<VersionDTO>(jsonContent, JsonOptions);
            }
          }
        }
        catch (JsonException jsonEx)
        {
          Response<VersionDTO> errorResponse = ResponseFactory.Fail<VersionDTO>(
            $"Invalid JSON format: {jsonEx.Message}. File content: {jsonContent}"
          );
          return BadRequest(errorResponse);
        }

        if (versionData == null)
        {
          Response<VersionDTO> errorResponse = ResponseFactory.Fail<VersionDTO>(
            $"Deserialization returned null. File content: {jsonContent}"
          );
          return BadRequest(errorResponse);
        }

        if (string.IsNullOrWhiteSpace(versionData.Version))
        {
          Response<VersionDTO> errorResponse = ResponseFactory.Fail<VersionDTO>(
            $"Version property is missing or empty. File content: {jsonContent}"
          );
          return BadRequest(errorResponse);
        }

        Response<VersionDTO> successResponse = ResponseFactory.Success(versionData);
        return Ok(successResponse);
      }
      catch (JsonException ex)
      {
        Response<VersionDTO> errorResponse = ResponseFactory.Fail<VersionDTO>(
          $"Failed to parse version file: {ex.Message}"
        );
        return BadRequest(errorResponse);
      }
      catch (Exception ex)
      {
        Response<VersionDTO> errorResponse = ResponseFactory.Fail<VersionDTO>(
          $"Failed to retrieve version: {ex.Message}"
        );
        return StatusCode(500, errorResponse);
      }
    }
  }

  public class VersionDTO
  {
    [JsonPropertyName("apiVersion")]
    public string Version { get; set; } = string.Empty;
  }
}
