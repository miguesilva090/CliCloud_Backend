using System.Text;
using System.Text.RegularExpressions;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.ReportService.DTOs;
using Microsoft.AspNetCore.Hosting;

namespace CliCloud.Application.Services.Utility.ReportService
{
  public partial class ReportService (IWebHostEnvironment environment) : IReportService
  {
    [GeneratedRegex(@"[<>:""/\\|?*\x00- \x1f]", RegexOptions.None)]
    private static partial Regex DangerousFileNameCharactersRegex();

    private readonly IWebHostEnvironment _environment = environment;
    private const long MaxFileSize = 10 * 1024 * 1024;
    private const string ReportsDirectory = "assets/reports";
    private const string OriginalReportsDirectory = "assets/reports-originais";

    public async Task<Response<string>> SaveReportAsync(SaveReportRequest request)
    {
      try
      {
        if(string.IsNullOrWhiteSpace(request.Filename))
        {
          return ResponseFactory.Fail<string>("O nome do ficheiro é obrigatório");
        }
        if(string.IsNullOrWhiteSpace(request.Content))
        {
          return ResponseFactory.Fail<string>("O conteúdo do ficheiro é obrigatório");
        }

        string decodedContent;
        try
        {
          byte[] decodedBytes = Convert.FromBase64String(request.Content);
          decodedContent = Encoding.UTF8.GetString(decodedBytes);
        }
        catch(Exception ex)
        {
          return ResponseFactory.Fail<string>("Formato de conteúdo inválido: " + ex.Message);
        }

        try
        {
          _ = System.Text.Json.JsonSerializer.Deserialize<object>(decodedContent);
        }
        catch(Exception ex)
        {
          return ResponseFactory.Fail<string>("Formato de JSON inválido: "+ ex.Message);
        }

        if(Encoding.UTF8.GetByteCount(decodedContent) > MaxFileSize)
        {
          return ResponseFactory.Fail<string>($" O tamanho do ficheiro não pode exceder {MaxFileSize / 1024 / 1024}MB");
        }

        string sanitazedFilename = SanitazeFileName(request.Filename);

        if(!sanitazedFilename.EndsWith(".mrt", StringComparison.OrdinalIgnoreCase))
        {
          sanitazedFilename += ".mrt";
        }

        string reportsPath = Path.Combine(_environment.WebRootPath, ReportsDirectory);

        if(!Directory.Exists(reportsPath))
        {
          Directory.CreateDirectory(reportsPath);
        }

        string filePath = Path.Combine(reportsPath, sanitazedFilename);

        await File.WriteAllTextAsync(filePath, decodedContent, Encoding.UTF8);

        return ResponseFactory.Success<string>("Ficheiro salvo com sucesso");
      
      }
      catch(Exception ex)
      {
        return ResponseFactory.Fail<string>($"Falha ao salvar o ficheiro: {ex.Message}");
      }
    }

    public async Task<Response<string>> GetReportAsync(string reportName)
    {
      try
      {
        if(string.IsNullOrWhiteSpace(reportName))
        {
          return ResponseFactory.Fail<string>("Nome do relatório é obrigatório");
        }

        string sanitazedFilename = SanitazeFileName(reportName);

        if(!sanitazedFilename.EndsWith(".mrt", StringComparison.OrdinalIgnoreCase))
        {
          sanitazedFilename += ".mrt";
        }

        string filePath = Path.Combine(
          _environment.WebRootPath,
          ReportsDirectory,
          sanitazedFilename
        );

        if(!File.Exists(filePath))
        {
          return ResponseFactory.Fail<string>("Relatório não encontrado");
        }

        string reportContent = await File.ReadAllTextAsync(filePath, Encoding.UTF8);

        try
        {
          _ = System.Text.Json.JsonSerializer.Deserialize<object>(reportContent);
        }
        catch(Exception ex)
        {
          return ResponseFactory.Fail<string>($"Formato do relatório inválido:  {ex.Message}");
        }

        return ResponseFactory.Success<string>(reportContent);
      }
      catch(Exception ex)
      {
        return ResponseFactory.Fail<string>($"Falha ao restaurar o relatório : {ex.Message}");
      }
    }

    public Task<Response<string[]>> GetAllReportsAsync()
    {
      try
      {
        string reportsPath = Path.Combine(_environment.WebRootPath, ReportsDirectory);

        if(!Directory.Exists(reportsPath))
        {
          return Task.FromResult(ResponseFactory.Success<string[]>([]));
        }

        string[] reportFiles = Directory
          .GetFiles(reportsPath, "*.mrt", SearchOption.TopDirectoryOnly)
          .Select(Path.GetFileName)
          .Where(name => !string.IsNullOrWhiteSpace(name))
          .Cast<string>()
          .ToArray();

        return Task.FromResult(ResponseFactory.Success<string[]>(reportFiles));
      }
      catch(Exception ex)
      {
        return Task.FromResult(ResponseFactory.Fail<string[]>($"Falha ao obter os relatórios: {ex.Message}"));
      }
    }

    public async Task<Response<string>> RevertReportAsync(RevertReportRequest request)
    {
      try
      {
        if(string.IsNullOrWhiteSpace(request.ReportName))
        {
          return ResponseFactory.Fail<string>("Nome do relatório é obrigatório");
        }

        string sanitazedFilename = SanitazeFileName(request.ReportName);
        if(!sanitazedFilename.EndsWith(".mrt", StringComparison.OrdinalIgnoreCase))
        {
          sanitazedFilename += ".mrt";
        }

        string originalPath = Path.Combine(
          _environment.WebRootPath,
          OriginalReportsDirectory,
          sanitazedFilename
        );

        string destinationPath = Path.Combine(
          _environment.WebRootPath,
          ReportsDirectory,
          sanitazedFilename
        );

        if(!File.Exists(originalPath))
        {
          return ResponseFactory.Fail<string>($"O relatório original '{sanitazedFilename}' não foi encontrado na pasta de relatórios originais");
        }

        string reportsDir = Path.GetDirectoryName(destinationPath)!;
        if(!Directory.Exists(reportsDir))
        {
          Directory.CreateDirectory(reportsDir);
        }


        await Task.Run(() => File.Copy(originalPath, destinationPath, overwrite: true));

        return ResponseFactory.Success<string>("Relatório restaurado com sucesso");
      }
      catch(UnauthorizedAccessException ex)
      {
        return ResponseFactory.Fail<string>($"Acesso não autorizado : {ex.Message}");
      }
      catch(DirectoryNotFoundException ex)
      {
        return ResponseFactory.Fail<string>($"Pasta de relatórios não encontrada: {ex.Message}");
      }
      catch(IOException ex)
      {
        return ResponseFactory.Fail<string>($"Erro de sistema de ficheiros: {ex.Message}");
      }
      catch(Exception ex)
      {
        return ResponseFactory.Fail<string>($"Falha ao restaurar o relatório: {ex.Message}");
      }
    }

    public async Task<Response<string[]>> GetOriginalReportsAsync()
    {
      try
      {
        string originalReportsPath = Path.Combine(
          _environment.WebRootPath,
          OriginalReportsDirectory
        );

        string reportsPath = Path.Combine(_environment.WebRootPath, ReportsDirectory);


        if(!Directory.Exists(reportsPath))
        {
          Directory.CreateDirectory(reportsPath);
        }

        if(!Directory.Exists(originalReportsPath))
        {
          return ResponseFactory.Success<string[]>([]);
        }

        string[] originalFiles = Directory.GetFiles(
          originalReportsPath,
          "*.mrt",
          SearchOption.TopDirectoryOnly
        );

        List<string> copiedReports = new();
        List<string> errors = new();

        foreach(string originalFilePath in originalFiles)
        {
          string fileName = Path.GetFileName(originalFilePath);
          if(string.IsNullOrEmpty(fileName))
          {
            continue;
          }

          string destinationPath = Path.Combine(reportsPath, fileName);

          if(!File.Exists(destinationPath))
          {
            try
            {
              await Task.Run(() => File.Copy(originalFilePath, destinationPath, overwrite: false));
              copiedReports.Add(fileName);
            }
            catch(Exception ex)
            {
              errors.Add($"Erro ao copiar o relatório '{fileName}': {ex.Message}");
            }
          }
        }


        if(errors.Count > 0 && copiedReports.Count > 0)
        {
          return ResponseFactory.PartialSuccess<string[]>(
            copiedReports.ToArray(),
            string.Join("; ",errors)
          );
        }

        if(errors.Count > 0 && copiedReports.Count == 0)
        {
          return ResponseFactory.Fail<string[]>(string.Join("; ", errors));
        }

        return ResponseFactory.Success<string[]>(copiedReports.ToArray());
      }
      catch(UnauthorizedAccessException ex)
      {
        return ResponseFactory.Fail<string[]>($"Permissões negadas: {ex.Message}");
      }
      catch(DirectoryNotFoundException ex)
      {
        return ResponseFactory.Fail<string[]>($"Pasta de relatórios originais não encontrada: {ex.Message}");
      }
      catch(IOException ex)
      {
        return ResponseFactory.Fail<string[]>($"Erro de sistema de ficheiros: {ex.Message}");
      }
      catch(Exception ex)
      {
        return ResponseFactory.Fail<string[]>($"Falha ao obter os relatórios originais: {ex.Message}");
      }
    }

    public async Task<Response<string>> DeleteReportAsync(string reportName)
    {
      try
      {
        if(string.IsNullOrWhiteSpace(reportName))
        {
          return ResponseFactory.Fail<string>("Nome do relatório é obrigatório");
        }

        string sanitazedFilename = SanitazeFileName(reportName);

        if(!sanitazedFilename.EndsWith(".mrt", StringComparison.OrdinalIgnoreCase))
        {
          sanitazedFilename += ".mrt";
        }

        string filePath = Path.Combine(
          _environment.WebRootPath,
          ReportsDirectory,
          sanitazedFilename
        );

        if(!File.Exists(filePath))
        {
          return ResponseFactory.Fail<string>("O relatório nao foi encontrado");
        }

        await Task.Run(() => File.Delete(filePath));

        return ResponseFactory.Success<string>("Relatório eliminado com sucesso");
      }
      catch(UnauthorizedAccessException ex)
      {
        return ResponseFactory.Fail<string>($"Permissões negadas: {ex.Message}");
      }
      catch(DirectoryNotFoundException ex)
      {
        return ResponseFactory.Fail<string>($"Pasta de relatórios não encontrada: {ex.Message}");
      }
      catch(IOException ex)
      {
        return ResponseFactory.Fail<string>($"Erro de sistema de ficheiros: {ex.Message}");
      }
      catch(Exception ex)
      {
        return ResponseFactory.Fail<string>($"Falha ao eliminar o relatório: {ex.Message}");
      }
    }

    private static string SanitazeFileName(string fileName)
    {
      if(string.IsNullOrWhiteSpace(fileName))
      {
        throw new ArgumentException("O nome do ficheiro é obrigatório", nameof(fileName));
      }

      string sanitazed = DangerousFileNameCharactersRegex().Replace(fileName, string.Empty);

      sanitazed = sanitazed.Trim('.', ' ');

      sanitazed = Path.GetFileName(sanitazed);

      if(string.IsNullOrWhiteSpace(sanitazed))
      {
        throw new ArgumentException("O nome do ficheiro inválido após a sanitização", nameof(fileName));
      }

      return sanitazed;
    }

    [GeneratedRegex(@"[<>:""/\\|?*\x00- \x1f]", RegexOptions.Compiled)]
    private static partial Regex DangerousCharsRegex();

  }
}