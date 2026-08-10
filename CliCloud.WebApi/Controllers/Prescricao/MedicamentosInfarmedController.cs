using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Prescricao.InfarmedApiClient;
using CliCloud.Application.Services.Prescricao.InfarmedApiClient.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Prescricao;

[Route("client/prescricao/medicamentos")]
[ApiController]
public class MedicamentosInfarmedController(IInfarmedApiClient infarmedApiClient) : ControllerBase
{
    private readonly IInfarmedApiClient _infarmedApiClient = infarmedApiClient;

    [Authorize(Roles = "client")]
    [HttpGet("autocomplete")]
    public async Task<IActionResult> AutocompleteAsync(
        [FromQuery] string q = "",
        [FromQuery] int? tipoReceita = null,
        [FromQuery] bool? prescritivel = null,
        CancellationToken cancellationToken = default)
    {
        Response<IReadOnlyList<MedicamentoAutocompleteItemDto>> result =
            await _infarmedApiClient.AutocompleteAsync(q, tipoReceita, prescritivel, cancellationToken);
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpGet]
    public async Task<IActionResult> ListagemResumoAsync(
        [FromQuery] string? nome = null,
        [FromQuery] string? dci = null,
        [FromQuery] int tipo = 30,
        [FromQuery] int page = 1,
        [FromQuery] bool contar = false,
        [FromQuery] int? tipoReceita = null,
        [FromQuery] bool? prescritivel = null,
        CancellationToken cancellationToken = default)
    {
        Response<MedicamentoListagemResumoResultDto> result =
            await _infarmedApiClient.ListagemResumoAsync(
                nome, tipo, page, contar, tipoReceita, prescritivel, dci, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Load linha por embalagem exacta (preferido — evita CNPEM ambíguo).
    /// Ex.: GET .../embalagem/{embId}/prescricao?patologias=1,2
    /// </summary>
    [Authorize(Roles = "client")]
    [HttpGet("embalagem/{embId}/prescricao")]
    public async Task<IActionResult> GetPrescricaoByEmbIdAsync(
        string embId,
        [FromQuery] string? patologias = null,
        CancellationToken cancellationToken = default)
    {
        if (!TryParsePatologias(patologias, out var ids, out var error))
            return BadRequest(ResponseFactory.Fail<MedicamentoPrescricaoLinhaDto>(error!));

        Response<MedicamentoPrescricaoLinhaDto> result =
            await _infarmedApiClient.GetPrescricaoByEmbIdAsync(embId, ids, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Load linha por CNPEM (+ nrRegisto recomendado se houver ambiguidades).
    /// </summary>
    [Authorize(Roles = "client")]
    [HttpGet("cnpem/{cnpem}/prescricao")]
    public async Task<IActionResult> GetPrescricaoByCnpemAsync(
        string cnpem,
        [FromQuery] string? nrRegisto = null,
        [FromQuery] string? patologias = null,
        CancellationToken cancellationToken = default)
    {
        if (!TryParsePatologias(patologias, out var ids, out var error))
            return BadRequest(ResponseFactory.Fail<MedicamentoPrescricaoLinhaDto>(error!));

        Response<MedicamentoPrescricaoLinhaDto> result =
            await _infarmedApiClient.GetPrescricaoByCnpemAsync(cnpem, nrRegisto, ids, cancellationToken);
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpGet("cnpem/{cnpem}/equivalentes")]
    public async Task<IActionResult> GetEquivalentesByCnpemAsync(
        string cnpem,
        [FromQuery] string? patologias = null,
        CancellationToken cancellationToken = default)
    {
        if (!TryParsePatologias(patologias, out var ids, out var error))
            return BadRequest(ResponseFactory.Fail<IReadOnlyList<MedicamentoPrescricaoOpcaoDto>>(error!));

        Response<IReadOnlyList<MedicamentoPrescricaoOpcaoDto>> result =
            await _infarmedApiClient.GetEquivalentesByCnpemAsync(cnpem, ids, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Catálogo SNS de regimes excepcionais (patologias/comparticipação) via ApiInfarmed.
    /// </summary>
    [Authorize(Roles = "client")]
    [HttpGet("regimes-excecionais")]
    public async Task<IActionResult> GetRegimesExcepcionaisAsync(
        CancellationToken cancellationToken = default)
    {
        Response<IReadOnlyList<RegimeExcepcionalDto>> result =
            await _infarmedApiClient.GetRegimesExcepcionaisAtivosAsync(cancellationToken);
        return Ok(result);
    }

    private static bool TryParsePatologias(
        string? raw,
        out IReadOnlyList<int> ids,
        out string? error)
    {
        ids = [];
        error = null;

        if (string.IsNullOrWhiteSpace(raw))
            return true;

        var list = new List<int>();
        foreach (var part in raw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (!int.TryParse(part, out var id) || id <= 0)
            {
                error = "Parâmetro 'patologias' inválido. Use IDs numéricos separados por vírgula (ex.: 1,2,3).";
                return false;
            }
            list.Add(id);
        }

        ids = list;
        return true;
    }
}