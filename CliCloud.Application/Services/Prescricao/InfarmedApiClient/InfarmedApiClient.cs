using System.Net.Http.Json;
using System.Text.Json;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Prescricao.InfarmedApiClient.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CliCloud.Application.Services.Prescricao.InfarmedApiClient;

public class InfarmedApiClient(
    IHttpClientFactory httpClientFactory,
    IOptions<InfarmedApiOptions> options,
    ILogger<InfarmedApiClient> logger) : IInfarmedApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly HashSet<int> TiposPermitidos = [10, 20, 30 ,40, 50];

    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly InfarmedApiOptions _options = options.Value;
    private readonly ILogger<InfarmedApiClient> _logger = logger;

    public async Task<Response<IReadOnlyList<MedicamentoAutocompleteItemDto>>> AutocompleteAsync(
        string query,
        int? tipoReceita = null,
        bool? prescritivel = null,
        CancellationToken cancellationToken = default)
    {
        var termo = query?.Trim() ?? string.Empty;
        if (termo.Length < 3)
            return ResponseFactory.Fail<IReadOnlyList<MedicamentoAutocompleteItemDto>>(
                "Indique pelo menos 3 caracteres para pesquisar.");

        if (string.IsNullOrWhiteSpace(_options.BaseUrl))
            return ResponseFactory.Fail<IReadOnlyList<MedicamentoAutocompleteItemDto>>(
                "InfarmedApi:BaseUrl não está configurado.");

        try
        {
            var client = _httpClientFactory.CreateClient(InfarmedApiOptions.HttpClientName);

            var qs = new List<string>
            {
                $"nome={Uri.EscapeDataString(termo)}",
                "modo=autocomplete"
            };

            if (tipoReceita is not null)
                qs.Add($"tipoReceita={tipoReceita.Value}");

            if (prescritivel is not null)
                qs.Add($"prescritivel={(prescritivel.Value ? "true" : "false")}");

            var url = "medicamentos?" + string.Join("&", qs);

            using var response = await client.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogWarning(
                    "ApiInfarmed autocomplete falhou. Status={StatusCode} Body={Body}",
                    (int)response.StatusCode,
                    body);

                return ResponseFactory.Fail<IReadOnlyList<MedicamentoAutocompleteItemDto>>(
                    $"Erro ao contactar ApiInfarmed ({(int)response.StatusCode}).");
            }

            var payload = await response.Content.ReadFromJsonAsync<InfarmedAutocompleteApiResponse>(
                JsonOptions,
                cancellationToken);

            IReadOnlyList<MedicamentoAutocompleteItemDto> items =
                payload?.Items ?? [];

            return ResponseFactory.Success(items);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado no autocomplete Infarmed. Query={Query}", termo);
            return ResponseFactory.Fail<IReadOnlyList<MedicamentoAutocompleteItemDto>>(
                "Erro inesperado ao pesquisar medicamentos no Infarmed.");
        }
    }

    public async Task<Response<MedicamentoListagemResumoResultDto>> ListagemResumoAsync(
        string? nome = null,
        int tipo = 10,
        int page = 1,
        bool contar = false,
        int? tipoReceita = null,
        bool? prescritivel = null,
        CancellationToken cancellationToken = default
    )
    {
        if (!TiposPermitidos.Contains(tipo))
            return ResponseFactory.Fail<MedicamentoListagemResumoResultDto>(
                "Parâmetro 'tipo' deve ser 10, 20, 30, 40 ou 50.");
        
        if (page < 1)
            return ResponseFactory.Fail<MedicamentoListagemResumoResultDto>(
                "Parâmetro 'page' deve ser maior ou igual a 1.");
        
        if (string.IsNullOrWhiteSpace(_options.BaseUrl))
            return ResponseFactory.Fail<MedicamentoListagemResumoResultDto>(
                "InfarmedApi: BaseUrl não está configurado.");
        
        try
        {
            var client = _httpClientFactory.CreateClient(InfarmedApiOptions.HttpClientName);

            var qs = new List<string>
            {
                "modo=resumo",
                $"tipo={tipo}",
                $"page={page}",
                $"contar={(contar ? "true" : "false")}"
            };

            if (!string.IsNullOrWhiteSpace(nome))
                qs.Add($"nome={Uri.EscapeDataString(nome.Trim())}");

            if (tipoReceita is not null)
                qs.Add($"tipoReceita={tipoReceita.Value}");
            
            if (prescritivel is not null)
                qs.Add($"prescritivel={(prescritivel.Value ? "true" : "false")}");
            
            var url = "medicamentos?" + string.Join("&", qs);

            using var response = await client.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogWarning(
                    "ApiInfarmed listagem resumo falhou. Status={StatusCode} Body={Body}",
                    (int)response.StatusCode,
                    body);
                
                return ResponseFactory.Fail<MedicamentoListagemResumoResultDto>(
                    $"Erro ao contactar ApiInfarmed ({(int)response.StatusCode})");
            }

            var payload = await response.Content.ReadFromJsonAsync<InfarmedListagemResumoApiResponse>(
                JsonOptions,
                cancellationToken);
            
            var result = new MedicamentoListagemResumoResultDto
            {
                Items = payload?.Items ?? [],
                Page = payload?.Page ?? page,
                Tipo = payload?.Tipo ?? tipo,
                TotalCount = payload?.TotalCount,
                TotalPages = payload?.TotalPages,
            };

            return ResponseFactory.Success(result);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado na listagem resumo Infarmed. Nome={Nome}", nome);
            return ResponseFactory.Fail<MedicamentoListagemResumoResultDto>(
                "Erro inesperado ao listar medicamentos no Infarmed.");
        }
    }

    public async Task<Response<MedicamentoPrescricaoLinhaDto>> GetPrescricaoByEmbIdAsync(
        string embId,
        IReadOnlyList<int>? patologias = null,
        CancellationToken cancellationToken = default
    )
    {
        if (string.IsNullOrWhiteSpace(embId))
            return ResponseFactory.Fail<MedicamentoPrescricaoLinhaDto>("embId é obrigatório.");
        
        var qs = BuildPatologiasQuery(patologias);
        var url = $"medicamentos/embalagem/{Uri.EscapeDataString(embId.Trim())}/prescricao{qs}";

        var (ok, linha, error) = await GetPrescricaoInternalAsync(url, embId.Trim(), cancellationToken);
        if (!ok)
            return ResponseFactory.Fail<MedicamentoPrescricaoLinhaDto>(error);
        

        return ResponseFactory.Success(linha!);
    }

    public async Task<Response<MedicamentoPrescricaoLinhaDto>> GetPrescricaoByCnpemAsync(
    string cnpem,
    string? nrRegisto = null,
    IReadOnlyList<int>? patologias = null,
    CancellationToken cancellationToken = default)
{
    if (string.IsNullOrWhiteSpace(cnpem))
        return ResponseFactory.Fail<MedicamentoPrescricaoLinhaDto>("cnpem é obrigatório.");
    var parts = new List<string>();
    if (!string.IsNullOrWhiteSpace(nrRegisto))
        parts.Add($"nrRegisto={Uri.EscapeDataString(nrRegisto.Trim())}");
    AppendPatologias(parts, patologias);
    var qs = parts.Count == 0 ? string.Empty : "?" + string.Join("&", parts);
    var url = $"medicamentos/cnpem/{Uri.EscapeDataString(cnpem.Trim())}/prescricao{qs}";
    var (ok, linha, error) = await GetPrescricaoInternalAsync(url, embIdKnown: null, cancellationToken);
    if (!ok)
        return ResponseFactory.Fail<MedicamentoPrescricaoLinhaDto>(error!);
    return ResponseFactory.Success(linha!);
}
public async Task<Response<IReadOnlyList<MedicamentoPrescricaoOpcaoDto>>> GetEquivalentesByCnpemAsync(
    string cnpem,
    IReadOnlyList<int>? patologias = null,
    CancellationToken cancellationToken = default)
{
    if (string.IsNullOrWhiteSpace(cnpem))
        return ResponseFactory.Fail<IReadOnlyList<MedicamentoPrescricaoOpcaoDto>>("cnpem é obrigatório.");
    if (string.IsNullOrWhiteSpace(_options.BaseUrl))
        return ResponseFactory.Fail<IReadOnlyList<MedicamentoPrescricaoOpcaoDto>>(
            "InfarmedApi:BaseUrl não está configurado.");
    try
    {
        var client = _httpClientFactory.CreateClient(InfarmedApiOptions.HttpClientName);
        var qs = BuildPatologiasQuery(patologias);
        var url = $"medicamentos/cnpem/{Uri.EscapeDataString(cnpem.Trim())}/equivalentes{qs}";
        using var response = await client.GetAsync(url, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogWarning(
                "ApiInfarmed equivalentes falhou. Status={StatusCode} Body={Body}",
                (int)response.StatusCode,
                body);
            return ResponseFactory.Fail<IReadOnlyList<MedicamentoPrescricaoOpcaoDto>>(
                $"Erro ao contactar ApiInfarmed ({(int)response.StatusCode}).");
        }
        var items = await response.Content.ReadFromJsonAsync<List<MedicamentoPrescricaoOpcaoDto>>(
            JsonOptions,
            cancellationToken);
        IReadOnlyList<MedicamentoPrescricaoOpcaoDto> result = items ?? [];
        return ResponseFactory.Success(result);
    }
    catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
    {
        throw;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Erro inesperado nos equivalentes Infarmed. Cnpem={Cnpem}", cnpem);
        return ResponseFactory.Fail<IReadOnlyList<MedicamentoPrescricaoOpcaoDto>>(
            "Erro inesperado ao obter equivalentes no Infarmed.");
    }
}
private async Task<(bool Ok, MedicamentoPrescricaoLinhaDto? Linha, string? Error)> GetPrescricaoInternalAsync(
    string relativeUrl,
    string? embIdKnown,
    CancellationToken cancellationToken)
{
    if (string.IsNullOrWhiteSpace(_options.BaseUrl))
        return (false, null, "InfarmedApi:BaseUrl não está configurado.");
    try
    {
        var client = _httpClientFactory.CreateClient(InfarmedApiOptions.HttpClientName);
        using var response = await client.GetAsync(relativeUrl, cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return (false, null, "Medicamento/embalagem não encontrado no Infarmed.");
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogWarning(
                "ApiInfarmed prescrição falhou. Url={Url} Status={StatusCode} Body={Body}",
                relativeUrl,
                (int)response.StatusCode,
                body);
            return (false, null, $"Erro ao contactar ApiInfarmed ({(int)response.StatusCode}).");
        }
        var payload = await response.Content.ReadFromJsonAsync<InfarmedPrescricaoApiResponse>(
            JsonOptions,
            cancellationToken);
        if (payload is null)
            return (false, null, "Resposta inválida da ApiInfarmed.");
        var linha = MapPrescricao(payload, embIdKnown);
        return (true, linha, null);
    }
    catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
    {
        throw;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Erro inesperado na prescrição Infarmed. Url={Url}", relativeUrl);
        return (false, null, "Erro inesperado ao obter dados de prescrição no Infarmed.");
    }
}
private static MedicamentoPrescricaoLinhaDto MapPrescricao(
    InfarmedPrescricaoApiResponse payload,
    string? embIdKnown)
{
    var r = payload.Resumo;
    return new MedicamentoPrescricaoLinhaDto
    {
        EmbId = embIdKnown,
        Cnpem = r?.Cnpem ?? string.Empty,
        Nome = r?.Nome ?? string.Empty,
        Dosagem = r?.Dosagem,
        Embalagem = r?.Embalagem,
        NrRegisto = r?.NrRegisto,
        PrincipioActivo = r?.PrincipioActivo,
        FormaFarmaceutica = r?.FormaFarmaceutica,
        Generico = r?.Generico ?? false,
        PrecoPvp = payload.PrecoPvp,
        PrecoReferencia = payload.PrecoReferencia,
        PrecoUnitario = payload.PrecoUnitario,
        PvpNotificado = payload.PvpNotificado,
        PvpMax100Re = payload.PvpMax100Re,
        ComparticipacaoGeral = payload.ComparticipacaoGeral,
        ComparticipacoesEspeciais = payload.ComparticipacoesEspeciais,
        ComparticipacaoEfectiva = payload.ComparticipacaoEfectiva,
        TaxaComparticipacaoEfectiva = payload.TaxaComparticipacaoEfectiva,
        PatologiasConsideradas = payload.PatologiasConsideradas,
        BaseCalculo = payload.BaseCalculo,
        PrescritivelAmbulatorio = payload.PrescritivelAmbulatorio,
        PrescritivelMesSeguinte = payload.PrescritivelMesSeguinte,
        GrupoHomogeneo = payload.GrupoHomogeneo,
        OpcoesEquivalentes = payload.OpcoesEquivalentes
    };
}
private static string BuildPatologiasQuery(IReadOnlyList<int>? patologias)
{
    var parts = new List<string>();
    AppendPatologias(parts, patologias);
    return parts.Count == 0 ? string.Empty : "?" + string.Join("&", parts);
}
private static void AppendPatologias(List<string> parts, IReadOnlyList<int>? patologias)
{
    if (patologias is null || patologias.Count == 0)
        return;
    parts.Add($"patologias={Uri.EscapeDataString(string.Join(",", patologias))}");
}


}