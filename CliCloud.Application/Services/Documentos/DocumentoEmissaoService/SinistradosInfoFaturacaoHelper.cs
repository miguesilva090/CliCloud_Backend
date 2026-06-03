#nullable enable

using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Documentos.DocumentoEmissaoService.DTOs;
using CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Specifications;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Entities.Servicos;
using CliCloud.Domain.Entities.Sinistros;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService;

internal static class SinistradosInfoFaturacaoHelper
{
    private static readonly Guid LinhaObservacaoId = Guid.Empty;

    public static async Task<Response<SinistradosInfoFaturacaoResponse>> ObterAsync(
        IRepositoryAsync repository,
        SinistradosInfoFaturacaoRequest request)
    {
        var sinistrado = (
            await repository.GetListAsync<Sinistrado, Guid>(
                new SinistradoParaFaturacaoByIdSpec(request.SinistradoId))
        ).FirstOrDefault();

        if (sinistrado is null)
            return ResponseFactory.Fail<SinistradosInfoFaturacaoResponse>("Sinistrado não encontrado.");

        var utente = sinistrado.Utente;
        if (utente?.SeguradoraId is null || utente.SeguradoraOrganismo is null)
            return ResponseFactory.Fail<SinistradosInfoFaturacaoResponse>(
                "O utente do sinistrado não tem seguradora configurada.");

        if (!TemApolice(utente))
            return ResponseFactory.Fail<SinistradosInfoFaturacaoResponse>(
                "O utente do sinistrado não tem número de apólice configurado.");

        var organismo = utente.SeguradoraOrganismo;
        var desde = request.DataDesde?.Date;
        var ate = request.DataAte?.Date;

        var linhasNaoFaturadas = sinistrado.LinhasServico
            .Where(l => string.IsNullOrWhiteSpace(l.NumeroTFatura))
            .Where(l => !l.DataFatura.HasValue)
            .Where(l => DataNoIntervalo(l.DataServico, desde, ate))
            .ToList();

        if (linhasNaoFaturadas.Count == 0)
            return ResponseFactory.Fail<SinistradosInfoFaturacaoResponse>(
                "O sinistrado não tem linhas por faturar no intervalo indicado.");

        var linhasDto = new List<SinistradosInfoFaturacaoLinhaDTO>();
        Guid? primeiroSubsistemaId = null;

        foreach (var linha in linhasNaoFaturadas)
        {
            var map = await MapearLinhaAsync(repository, organismo.Id, linha);
            if (!map.Success)
                return ResponseFactory.Fail<SinistradosInfoFaturacaoResponse>(map.Error!);
            linhasDto.Add(map.Value!);
            primeiroSubsistemaId ??= map.SubsistemaId;
        }

        var obs = organismo.TRUST
            ? $"Serviços prestados ao utente {utente.Nome}"
            : $"Serviços prestados ao utente {utente.Nome}, apólice {ObterApoliceTexto(utente)}, processo {sinistrado.NumeroProcesso ?? "—"}";

        var response = new SinistradosInfoFaturacaoResponse
        {
            SinistradoId = sinistrado.Id,
            CodigoSinistro = sinistrado.CodigoSinistro,
            OrganismoId = organismo.Id,
            UtenteId = utente.Id,
            Cliente = MapClienteOrganismo(organismo),
            LinhaObservacao = obs,
            NumeroProcesso = organismo.TRUST ? sinistrado.NumeroProcesso : null,
            Linhas = linhasDto,
        };

        if (organismo.TRUST && !string.IsNullOrWhiteSpace(sinistrado.NumeroProcesso) && primeiroSubsistemaId.HasValue)
        {
            response.Linhas.Add(new SinistradosInfoFaturacaoLinhaDTO
            {
                SinistradoLinhaServicoId = LinhaObservacaoId,
                Descricao = $"##TRUSTREQ A- {primeiroSubsistemaId}",
                Quantidade = 1,
                PrecoUnitario = 0,
                PrecoUtente = 0,
                PrecoOrganismo = 0,
                TaxaIvaPercentagem = 0,
            });
        }

        response.Linhas.Add(new SinistradosInfoFaturacaoLinhaDTO
        {
            SinistradoLinhaServicoId = LinhaObservacaoId,
            Descricao = obs,
            Quantidade = 1,
            PrecoUnitario = 0,
            PrecoUtente = 0,
            PrecoOrganismo = 0,
            TaxaIvaPercentagem = 0,
        });

        return ResponseFactory.Success(response);
    }

    private static SinistradosInfoFaturacaoClienteDTO MapClienteOrganismo(Organismo organismo) =>
        new()
        {
            Nome = organismo.NomeComercial ?? organismo.Nome,
            Morada = FormatarMoradaEntidade(organismo),
            Localidade = organismo.CodigoPostal?.Localidade
                ?? organismo.Rua?.CodigoPostal?.Localidade,
            NumeroContribuinte = organismo.NumeroContribuinte,
            CodigoPostalId = organismo.CodigoPostalId ?? organismo.Rua?.CodigoPostalId,
        };

    private static string FormatarMoradaEntidade(Entidade entidade)
    {
        List<string> partes = [];
        if (!string.IsNullOrWhiteSpace(entidade.Rua?.Nome))
            partes.Add(entidade.Rua.Nome.Trim());
        if (!string.IsNullOrWhiteSpace(entidade.NumeroPorta))
            partes.Add(entidade.NumeroPorta.Trim());
        if (!string.IsNullOrWhiteSpace(entidade.AndarRua))
            partes.Add(entidade.AndarRua.Trim());
        return partes.Count > 0 ? string.Join(", ", partes) : "—";
    }

    private static bool DataNoIntervalo(DateTime? data, DateTime? desde, DateTime? ate)
    {
        if (!data.HasValue) return true;
        var d = data.Value.Date;
        if (desde.HasValue && d < desde.Value) return false;
        if (ate.HasValue && d > ate.Value) return false;
        return true;
    }

    private static bool TemApolice(Utente utente)
    {
        if (!string.IsNullOrWhiteSpace(utente.SeguradoraOrganismo?.Apolice)) return true;
        return utente.SubsistemaLinhas?.Any(x => !string.IsNullOrWhiteSpace(x.NumeroApolice)) == true;
    }

    private static string ObterApoliceTexto(Utente utente) =>
        utente.SeguradoraOrganismo?.Apolice
        ?? utente.SubsistemaLinhas?.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.NumeroApolice))?.NumeroApolice
        ?? "—";

    private sealed record MapResult(
        bool Success,
        SinistradosInfoFaturacaoLinhaDTO? Value,
        Guid? SubsistemaId,
        string? Error)
    {
        public static MapResult Ok(SinistradosInfoFaturacaoLinhaDTO v, Guid subsistemaId) =>
            new(true, v, subsistemaId, null);

        public static MapResult Fail(string e) => new(false, null, null, e);
    }

    private static async Task<Servico?> ResolverServicoAsync(
        IRepositoryAsync repository,
        SinistradoLinhaServico linha)
    {
        var codigo = linha.CodigoServico.Trim();
        if (string.IsNullOrEmpty(codigo))
            return null;

        if (codigo.StartsWith("CONS-", StringComparison.OrdinalIgnoreCase)
            || codigo.StartsWith("TRAT-", StringComparison.OrdinalIgnoreCase))
            return null;

        var porCodigo = (
            await repository.GetListAsync<Servico, Guid>(new ServicoComTaxaIvaByCodigoSpec(codigo))
        ).ToList();

        if (porCodigo.Count == 1)
            return porCodigo[0];

        if (porCodigo.Count > 1)
        {
            Servico? exact = porCodigo.FirstOrDefault(s =>
                string.Equals(s.Id.ToString(), codigo, StringComparison.OrdinalIgnoreCase));
            if (exact is not null)
                return exact;

            Servico? prefix = porCodigo.FirstOrDefault(s =>
                s.Id.ToString().StartsWith(codigo, StringComparison.OrdinalIgnoreCase));
            if (prefix is not null)
                return prefix;
        }

        if (!string.IsNullOrWhiteSpace(linha.DesignacaoServico))
        {
            return (
                await repository.GetListAsync<Servico, Guid>(
                    new ServicoComTaxaIvaByDesignacaoSpec(linha.DesignacaoServico))
            ).FirstOrDefault();
        }

        return null;
    }

    private static async Task<Servico?> ObterServicoPorIdAsync(
        IRepositoryAsync repository,
        Guid servicoId) =>
        (
            await repository.GetListAsync<Servico, Guid>(new ServicoComTaxaIvaByIdSpec(servicoId))
        ).FirstOrDefault();

    /// <summary>Legado: preço = Valorserv; parte organismo = Valor (ValorContratado).</summary>
    private static (decimal PrecoTotal, decimal PrecoOrganismoTotal) ResolverPrecosLinha(
        SinistradoLinhaServico linha,
        SinistradoLinhaClinicaResolver.ContextoServico? clinica,
        SubsistemaServico? subsistema)
    {
        decimal? valorServico = linha.ValorServico ?? clinica?.ValorServico;
        decimal? valorOrganismo = linha.ValorContratado ?? clinica?.ValorOrganismo;

        decimal precoTotal = valorServico is > 0
            ? valorServico.Value
            : valorOrganismo is > 0
                ? valorOrganismo.Value
                : subsistema?.ValorServico ?? 0m;

        decimal precoOrgTotal = valorOrganismo is > 0
            ? valorOrganismo.Value
            : subsistema?.ValorOrganismo ?? precoTotal;

        return (precoTotal, precoOrgTotal);
    }

    private static bool EhCodigoClinica(string codigo) =>
        codigo.StartsWith("CONS-", StringComparison.OrdinalIgnoreCase)
        || codigo.StartsWith("TRAT-", StringComparison.OrdinalIgnoreCase);

    private static async Task<MapResult> MapearLinhaAsync(
        IRepositoryAsync repository,
        Guid organismoSeguradoraId,
        SinistradoLinhaServico linha)
    {
        var codigo = linha.CodigoServico.Trim();
        SinistradoLinhaClinicaResolver.ContextoServico? clinica = EhCodigoClinica(codigo)
            ? await SinistradoLinhaClinicaResolver.ResolverAsync(repository, linha)
            : null;

        Servico? servico = linha.ServicoId.HasValue
            ? await ObterServicoPorIdAsync(repository, linha.ServicoId.Value)
            : clinica?.Servico;

        if (servico is null)
        {
            clinica ??= await SinistradoLinhaClinicaResolver.ResolverAsync(repository, linha);
            servico = clinica?.Servico ?? await ResolverServicoAsync(repository, linha);
        }

        if (servico is null && clinica is null)
            return MapResult.Fail(
                $"Serviço não encontrado para o código «{linha.CodigoServico}». "
                + "Abra o sinistrado, confirme o serviço na linha, ou configure o acordo (subsistema) com a seguradora.");

        var qty = clinica?.Quantidade ?? Math.Max(1, linha.Quantidade);

        if (servico is null && clinica is not null)
        {
            var precoTotal = clinica.ValorServico ?? 0m;
            var precoOrgTotal = clinica.ValorOrganismo ?? 0m;
            var precoUnit = precoTotal / qty;
            var precoOrgUnit = precoOrgTotal / qty;
            return MapResult.Ok(
                new SinistradosInfoFaturacaoLinhaDTO
                {
                    SinistradoLinhaServicoId = linha.Id,
                    ServicoId = null,
                    Descricao = linha.DesignacaoServico ?? linha.CodigoServico,
                    CodigoServico = linha.CodigoServico,
                    Quantidade = qty,
                    PrecoUnitario = precoUnit,
                    PrecoUtente = Math.Max(0, precoUnit - precoOrgUnit),
                    PrecoOrganismo = precoOrgUnit,
                    TaxaIvaPercentagem = 0,
                },
                Guid.Empty);
        }

        servico ??= clinica!.Servico!;

        var subsistema = (
            await repository.GetListAsync<SubsistemaServico, Guid>(
                new SubsistemaServicoByServicoOrganismoSpec(servico.Id, organismoSeguradoraId))
        ).FirstOrDefault();

        var (precoTotalLinha, precoOrgTotalLinha) = ResolverPrecosLinha(linha, clinica, subsistema);

        if (subsistema is null && precoTotalLinha <= 0 && precoOrgTotalLinha <= 0)
            return MapResult.Fail(
                $"Serviço {linha.DesignacaoServico ?? linha.CodigoServico} sem preço na linha nem acordo (subsistema) com a seguradora.");

        if (precoTotalLinha <= 0 && subsistema is not null)
            precoTotalLinha = subsistema.ValorServico;
        if (precoOrgTotalLinha <= 0 && subsistema is not null)
            precoOrgTotalLinha = subsistema.ValorOrganismo;

        var precoUnitFinal = precoTotalLinha / qty;
        var precoOrgUnitFinal = precoOrgTotalLinha / qty;
        var taxaPct = servico.TaxaIva?.Taxa ?? 0m;

        return MapResult.Ok(
            new SinistradosInfoFaturacaoLinhaDTO
            {
                SinistradoLinhaServicoId = linha.Id,
                ServicoId = servico.Id,
                Descricao = linha.DesignacaoServico ?? servico.Designacao,
                CodigoServico = linha.CodigoServico,
                Quantidade = qty,
                PrecoUnitario = precoUnitFinal,
                PrecoUtente = Math.Max(0, precoUnitFinal - precoOrgUnitFinal),
                PrecoOrganismo = precoOrgUnitFinal,
                TaxaIvaId = servico.TaxaIvaId,
                TaxaIvaPercentagem = taxaPct,
                MotivoIsencaoId = servico.MotivoIsencaoId,
            },
            subsistema?.SubsistemaId ?? Guid.Empty);
    }

    public static bool IsLinhaObservacao(Guid sinistradoLinhaServicoId) =>
        sinistradoLinhaServicoId == LinhaObservacaoId;
}
