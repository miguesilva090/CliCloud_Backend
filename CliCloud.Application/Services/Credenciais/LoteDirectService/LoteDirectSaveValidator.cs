using CliCloud.Application.Common;
using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;
using CliCloud.Application.Services.Credenciais.LoteDirectService.Specifications;
using CliCloud.Domain.Entities.Credenciais;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService;

public sealed class LoteDirectSaveValidator(
    IRepositoryAsync repository,
    ILoteDirectEspHistoricoGateway espHistoricoGateway) : ILoteDirectSaveValidator
{
    private const int TipoLoteValorExamesSemPapel = 97;

    private readonly IRepositoryAsync _repository = repository;
    private readonly ILoteDirectEspHistoricoGateway _espHistoricoGateway = espHistoricoGateway;

    public async Task<string?> ValidateAsync(CreateLoteDirectRequest request, Guid? excludeId = null)
    {
        if (request.CodigoOrganismo is null or <= 0)
            return "Organismo em falta.";

        if (string.IsNullOrWhiteSpace(request.Credencial))
            return "Nº credencial em falta.";

        bool credencialDuplicada = await _repository
            .ExistsAsync<LoteDirect, Guid>(new LoteDirectByCredencialSpec(request.Credencial.Trim(), excludeId))
            .ConfigureAwait(false);

        if (credencialDuplicada)
            return "Já existe uma credencial com este número.";

        if (request is { Mes: >= 1 and <= 12, Ano: >= 1900, CodigoOrganismo: int org })
        {
            bool mesEmHistorico = await _repository
                .ExistsAsync<LoteDirect, Guid>(new LoteDirectOrganismoMesAnoHistoricoSpec(org, request.Mes.Value, request.Ano.Value))
                .ConfigureAwait(false);

            if (mesEmHistorico)
                return "O mês/ano deste organismo já foi transferido para histórico.";
        }

        if (excludeId.HasValue && await IsTipoExamesSemPapelAsync(request.TipoLote).ConfigureAwait(false))
        {
            string? erroEsp = await ValidarRegrasEspUpdateAsync(request).ConfigureAwait(false);
            if (erroEsp is not null)
                return erroEsp;
        }

        return null;
    }

    private async Task<bool> IsTipoExamesSemPapelAsync(int? tipoLoteId)
    {
        if (tipoLoteId is null or <= 0)
            return false;

        List<TipoLote> tipos = (await _repository
            .GetListAsync<TipoLote, int>(new TipoLoteByIdsSpec([tipoLoteId.Value]))
            .ConfigureAwait(false))
            .ToList();

        return tipos.FirstOrDefault()?.Valor == TipoLoteValorExamesSemPapel;
    }

    private async Task<string?> ValidarRegrasEspUpdateAsync(CreateLoteDirectRequest request)
    {
        string credencial = request.Credencial!.Trim();
        LoteDirectEspRequisicaoData? req = await _espHistoricoGateway
            .ObterPorCredencialAsync(credencial)
            .ConfigureAwait(false);

        if (req is null)
            return null;

        int codigoOrganismo = request.CodigoOrganismo!.Value;

        if (!string.IsNullOrWhiteSpace(request.CodigoServicoConsulta))
        {
            string? cservinstConsulta = await _espHistoricoGateway
                .ResolverCservinstAsync(codigoOrganismo, request.CodigoServicoConsulta.Trim())
                .ConfigureAwait(false);

            if (cservinstConsulta is null)
                return "Acor_ins da consulta inicial não encontrado.";

            if (!ContemMcdt(req.CodigosMcdt, cservinstConsulta))
                return $"No exame sem papel não existe requisição com o código de MCDT {cservinstConsulta}";
        }

        List<LoteDirectLinhaUpsertRequest> linhasServicos = (request.Linhas ?? [])
            .Where(linha => request.ServicoConsultaId is null || linha.ServicoId != request.ServicoConsultaId)
            .ToList();

        foreach (LoteDirectLinhaUpsertRequest linha in linhasServicos)
        {
            string? codigoMcdt = await ResolverCodigoMcdtLinhaAsync(codigoOrganismo, linha).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(codigoMcdt))
                continue;

            if (!ContemMcdt(req.CodigosMcdt, codigoMcdt))
                return $"No exame sem papel não existe requisição com o código de MCDT {codigoMcdt}";
        }

        if (!req.EnpAssinado)
            return null;

        List<LoteDirectLinhaUpsertRequest> linhasPnp = request.Linhas789 ?? [];
        if (linhasPnp.Count == 0)
            return "Os PNP's não coincidem com serviços do lançamento de credenciais. Por favor, verifique.";

        List<string> lcList = [];
        foreach (LoteDirectLinhaUpsertRequest linha in linhasPnp)
        {
            string? codigoMcdt = await ResolverCodigoMcdtLinhaAsync(codigoOrganismo, linha).ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(codigoMcdt))
                lcList.Add(codigoMcdt);
        }

        lcList.RemoveAll(item => req.CodigosMcdt.Any(mcdt => string.Equals(mcdt, item, StringComparison.OrdinalIgnoreCase)));

        List<string> espList = req.EfetuadosNaoPrescritos
            .Select(x => x.CodigoMcdt.Trim())
            .Where(x => x.Length > 0)
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
            .ToList();

        List<string> lcOrdenada = lcList
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (lcOrdenada.Count != espList.Count
            || !lcOrdenada.SequenceEqual(espList, StringComparer.OrdinalIgnoreCase))
        {
            return "Os PNP's não coincidem com serviços do lançamento de credenciais. Por favor, verifique.";
        }

        foreach (LoteDirectEspEfetuadoNaoPrescritoData item in req.EfetuadosNaoPrescritos)
        {
            List<LoteDirectLinhaUpsertRequest> exames = [];
            foreach (LoteDirectLinhaUpsertRequest linha in linhasPnp)
            {
                string? codigoMcdt = await ResolverCodigoMcdtLinhaAsync(codigoOrganismo, linha).ConfigureAwait(false);
                if (string.Equals(codigoMcdt, item.CodigoMcdt, StringComparison.OrdinalIgnoreCase))
                    exames.Add(linha);
            }

            if (exames.Count != 1)
                continue;

            if (exames[0].Quantidade != item.NAmostras)
            {
                return $"A quantidade do serviço com o código {item.CodigoMcdt} não coincide com os exames sem papel assinados";
            }
        }

        return null;
    }

    private async Task<string?> ResolverCodigoMcdtLinhaAsync(int codigoOrganismo, LoteDirectLinhaUpsertRequest linha)
    {
        if (!string.IsNullOrWhiteSpace(linha.CodigoMcdt))
            return linha.CodigoMcdt.Trim();

        Servico? servico = await _repository
            .GetByIdAsync<Servico, Guid>(linha.ServicoId)
            .ConfigureAwait(false);

        string? codigoServico = servico?.EAN?.Trim();
        if (string.IsNullOrWhiteSpace(codigoServico))
            return null;

        return await _espHistoricoGateway
            .ResolverCservinstAsync(codigoOrganismo, codigoServico)
            .ConfigureAwait(false);
    }

    private static bool ContemMcdt(IReadOnlyList<string> codigos, string codigo)
        => codigos.Any(x => string.Equals(x, codigo, StringComparison.OrdinalIgnoreCase));
}
