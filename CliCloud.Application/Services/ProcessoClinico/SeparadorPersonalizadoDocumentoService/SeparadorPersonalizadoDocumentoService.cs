using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoDocumentoService.DTOs;
using CliCloud.Domain.Entities.ProcessoClinico.RelatorioAtestado;
using System.Globalization;
using System.Text;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoDocumentoService;

public class SeparadorPersonalizadoDocumentoService(
    IRepositoryAsync repository,
    ICurrentTenantUserService currentTenantUserService
) : ISeparadorPersonalizadoDocumentoService
{
    private readonly IRepositoryAsync _repository = repository;
    private readonly ICurrentTenantUserService _currentTenantUserService = currentTenantUserService;
    private static readonly Guid EmpresaDefault = Guid.Empty;

    private Guid GetCurrentUserId()
    {
        _currentTenantUserService.SetUser();
        if (Guid.TryParse(_currentTenantUserService.UserId, out Guid userId))
        {
            return userId;
        }
        throw new InvalidOperationException("Utilizador atual inválido.");
    }

    private static string GetModeloKeyPrefix(Guid separadorId) => $"[SEP:{separadorId}]";

    public async Task<Response<SeparadorPersonalizadoModeloDTO>> GetModeloAsync(Guid separadorId)
    {
        try
        {
            Guid userId = GetCurrentUserId();
            string prefix = GetModeloKeyPrefix(separadorId);
            IEnumerable<ModeloRelatorioAtestado> modelos =
                await _repository.GetListAsync<ModeloRelatorioAtestado, Guid>();

            ModeloRelatorioAtestado? modelo = modelos
                .Where(m => m.EmpresaId == EmpresaDefault)
                .Where(m => m.MedicoId == null || m.MedicoId == userId)
                .Where(m => m.Titulo.StartsWith(prefix))
                .OrderByDescending(m => m.CreatedOn)
                .FirstOrDefault();

            SeparadorPersonalizadoModeloDTO dto = new()
            {
                Id = modelo?.Id,
                Existe = modelo != null,
                TextoHtml = modelo?.TextoHtml ?? string.Empty
            };

            return ResponseFactory.Success(dto);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<SeparadorPersonalizadoModeloDTO>(ex.Message);
        }
    }

    public async Task<Response<Guid>> UpsertModeloAsync(UpsertSeparadorPersonalizadoModeloRequest request)
    {
        try
        {
            Guid userId = GetCurrentUserId();
            string prefix = GetModeloKeyPrefix(request.SeparadorId);
            string titulo = $"{prefix} {request.TituloSeparador}".Trim();

            IEnumerable<ModeloRelatorioAtestado> modelos =
                await _repository.GetListAsync<ModeloRelatorioAtestado, Guid>();

            ModeloRelatorioAtestado? modelo = modelos
                .Where(m => m.EmpresaId == EmpresaDefault)
                .Where(m => m.MedicoId == null || m.MedicoId == userId)
                .Where(m => m.Titulo.StartsWith(prefix))
                .OrderByDescending(m => m.CreatedOn)
                .FirstOrDefault();

            if (modelo == null)
            {
                ModeloRelatorioAtestado novo = new()
                {
                    EmpresaId = EmpresaDefault,
                    MedicoId = userId,
                    Titulo = titulo,
                    TextoHtml = request.TextoHtml
                };

                ModeloRelatorioAtestado created =
                    await _repository.CreateAsync<ModeloRelatorioAtestado, Guid>(novo);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(created.Id);
            }

            modelo.Titulo = titulo;
            modelo.TextoHtml = request.TextoHtml;
            ModeloRelatorioAtestado updated =
                await _repository.UpdateAsync<ModeloRelatorioAtestado, Guid>(modelo);
            _ = await _repository.SaveChangesAsync();
            return ResponseFactory.Success(updated.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<GerarImpressaoSeparadorPersonalizadoResponse>> GerarImpressaoAsync(
        GerarImpressaoSeparadorPersonalizadoRequest request
    )
    {
        try
        {
            Response<SeparadorPersonalizadoModeloDTO> modeloRes = await GetModeloAsync(request.SeparadorId);
            string modeloHtml = modeloRes.Data?.TextoHtml?.Trim() ?? string.Empty;

            string dataHojePt = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("pt-PT"));
            StringBuilder sb = new();
            foreach (GerarImpressaoSeparadorCampoRequest campo in request.Campos)
            {
                string texto = request.ApenasHoje
                    ? FilterHistoricoHoje(campo.HistoricoTexto, dataHojePt)
                    : campo.HistoricoTexto;
                if (string.IsNullOrWhiteSpace(texto))
                {
                    continue;
                }

                string safeNome = System.Net.WebUtility.HtmlEncode(campo.NomeCampo);
                string safeTexto = System.Net.WebUtility.HtmlEncode(texto);
                sb.Append(
                    $"""
                    <div style="margin-bottom:16px;">
                        <div style="font-weight:700; margin-bottom:6px;">{safeNome}</div>
                        <pre style="white-space:pre-wrap; font-family:Arial, sans-serif; font-size:12px; margin:0;">{safeTexto}</pre>
                    </div>
                    """
                );
            }

            string conteudo = sb.ToString();
            string htmlBody;
            if (string.IsNullOrWhiteSpace(modeloHtml))
            {
                htmlBody = conteudo;
            }
            else if (modeloHtml.Contains("{{CONTEUDO}}", StringComparison.OrdinalIgnoreCase))
            {
                htmlBody = modeloHtml.Replace(
                    "{{CONTEUDO}}",
                    conteudo,
                    StringComparison.OrdinalIgnoreCase
                );
            }
            else
            {
                // Mantem compatibilidade: se o modelo nao tiver placeholder, acrescenta no fim.
                htmlBody = modeloHtml + Environment.NewLine + conteudo;
            }

            string html =
                $"""
                <!doctype html>
                <html>
                  <head><meta charset="utf-8" /><title>{System.Net.WebUtility.HtmlEncode(request.TituloSeparador)}</title></head>
                  <body style="font-family:Arial,sans-serif; margin:24px;">
                    <h2 style="margin:0 0 12px 0;">{System.Net.WebUtility.HtmlEncode(request.TituloSeparador)}</h2>
                    <div style="font-size:12px; color:#555; margin-bottom:16px;">
                      Impresso em {DateTime.Now.ToString("dd/MM/yyyy HH:mm", CultureInfo.GetCultureInfo("pt-PT"))}
                    </div>
                    {htmlBody}
                  </body>
                </html>
                """;

            return ResponseFactory.Success(new GerarImpressaoSeparadorPersonalizadoResponse
            {
                ModeloVazio = string.IsNullOrWhiteSpace(modeloHtml),
                Html = html
            });
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<GerarImpressaoSeparadorPersonalizadoResponse>(ex.Message);
        }
    }

    private static string FilterHistoricoHoje(string historico, string dataHojePt)
    {
        if (string.IsNullOrWhiteSpace(historico))
        {
            return string.Empty;
        }

        string[] blocos = historico
            .Split(["\r\n\r\n", "\n\n"], StringSplitOptions.RemoveEmptyEntries)
            .Select(b => b.Trim())
            .Where(b => !string.IsNullOrWhiteSpace(b))
            .ToArray();

        IEnumerable<string> hoje = blocos.Where(b => b.Contains(dataHojePt, StringComparison.Ordinal));
        return string.Join(Environment.NewLine + Environment.NewLine, hoje);
    }
}
