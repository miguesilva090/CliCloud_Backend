#nullable enable

using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;
using CliCloud.Application.Services.Documentos.DocumentoEmissaoService.DTOs;
using CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Specifications;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Entities.Servicos;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService;

internal static class FaturaGlobalObterHelper
{
    private const string CodigoServicoResumoDefault = "FactG_SR";

    private static readonly HashSet<string> TiposFase1 =
    [
        "Consultas",
        "Consultas e Tratamentos",
    ];

    public static async Task<Response<FaturaGlobalObterResponse>> ObterAsync(
        IRepositoryAsync repository,
        FaturaGlobalObterRequest request)
    {
        if (request.OrganismoId == Guid.Empty)
            return ResponseFactory.Fail<FaturaGlobalObterResponse>("Organismo é obrigatório.");

        var dataDe = request.DataDe.Date;
        var dataAte = request.DataAte.Date;
        if (dataAte < dataDe)
            return ResponseFactory.Fail<FaturaGlobalObterResponse>("Data final anterior à inicial.");

        var tipo = (request.TipoFatura ?? string.Empty).Trim();
        if (string.IsNullOrEmpty(tipo))
            return ResponseFactory.Fail<FaturaGlobalObterResponse>("Indique o tipo de fatura global.");

        if (!TiposFase1.Contains(tipo))
        {
            return ResponseFactory.Fail<FaturaGlobalObterResponse>(
                "Tipo de fatura global ainda não migrado (Fase 2): Tratamentos, Utente, Especialidade ou Acordo de Cooperacao da ARS.");
        }

        var organismo = (
            await repository.GetListAsync<Organismo, Guid>(
                new OrganismoComMoradaByIdSpec(request.OrganismoId))
        ).FirstOrDefault();

        if (organismo is null)
            return ResponseFactory.Fail<FaturaGlobalObterResponse>("Organismo não encontrado.");

        var exigirReciboPago = organismo.SADGNR;
        var admissoes = (
            await repository.GetListAsync<Admissao, Guid>(
                new AdmissoesFaturaGlobalByOrganismoSpec(
                    request.OrganismoId,
                    dataDe,
                    dataAte,
                    request.UtenteId,
                    exigirReciboPago))
        )
            .Where(a => a.Faturado != true)
            .ToList();

        if (admissoes.Count == 0)
            return ResponseFactory.Fail<FaturaGlobalObterResponse>(
                "Não existem admissões por faturar no intervalo indicado.");

        var todosServicoIds = admissoes
            .SelectMany(a => a.Servicos)
            .Select(s => s.Id)
            .Distinct()
            .ToList();

        var jaFaturados = (
            await repository.GetListAsync<Domain.Entities.Documentos.DocumentoLinha, Guid>(
                new DocumentoLinhaByAdmissaoServicoIdsSpec(todosServicoIds))
        )
            .Where(l => l.AdmissaoServicoId.HasValue)
            .Select(l => l.AdmissaoServicoId!.Value)
            .ToHashSet();

        var opcaoTipo = request.OpcaoTipo is 1 or 2 ? request.OpcaoTipo.Value : 1;

        var codigoResumo = string.IsNullOrWhiteSpace(organismo.CServicoFaturaResumo)
            ? CodigoServicoResumoDefault
            : organismo.CServicoFaturaResumo.Trim();

        var response = new FaturaGlobalObterResponse
        {
            OrganismoId = organismo.Id,
            UtenteId = request.UtenteId,
            Cliente = MapClienteOrganismo(organismo),
            DataDe = dataDe,
            DataAte = dataAte,
        };

        if (opcaoTipo == 1)
        {
            var mapResumo = await MontarResumoAsync(repository, codigoResumo, admissoes, jaFaturados, response);
            if (!mapResumo.Success)
                return ResponseFactory.Fail<FaturaGlobalObterResponse>(mapResumo.Error!);
        }
        else
        {
            MontarPorAdmissao(admissoes, jaFaturados, response);
        }

        if (response.Linhas.Count == 0)
            return ResponseFactory.Fail<FaturaGlobalObterResponse>(
                "Não existem linhas por faturar no intervalo indicado.");

        return ResponseFactory.Success(response);
    }

    private static async Task<(bool Success, string? Error)> MontarResumoAsync(
        IRepositoryAsync repository,
        string codigoResumo,
        List<Admissao> admissoes,
        HashSet<Guid> jaFaturados,
        FaturaGlobalObterResponse response)
    {
        var servicoResumo = (
            await repository.GetListAsync<Servico, Guid>(
                new ServicoComTaxaIvaByCodigoLegadoSpec(codigoResumo))
        ).FirstOrDefault();

        if (servicoResumo is null)
            return (false, $"O serviço com o código {codigoResumo} não existe.");

        if (servicoResumo.TaxaIva is null)
            return (false, $"O serviço com o código {codigoResumo} não tem taxa de IVA associada.");

        decimal sumUnit = 0;
        decimal sumUtente = 0;
        decimal sumOrganismo = 0;
        var admissaoServicosIds = new List<Guid>();

        foreach (var adm in admissoes)
        {
            response.Admissoes.Add(MapAdmissao(adm));
            foreach (var srv in adm.Servicos.Where(s => !jaFaturados.Contains(s.Id)))
            {
                var (unit, utente, organismo) = ResolverValores(srv);
                sumUnit += unit;
                sumUtente += utente;
                sumOrganismo += organismo;
                admissaoServicosIds.Add(srv.Id);
            }
        }

        if (admissaoServicosIds.Count == 0)
            return (true, null);

        response.Linhas.Add(new FaturaGlobalLinhaDTO
        {
            ServicoId = servicoResumo.Id,
            CodigoArtigo = codigoResumo,
            Descricao = servicoResumo.Designacao,
            Quantidade = 1,
            PrecoUnitario = sumUnit,
            PrecoUtente = sumUtente,
            PrecoOrganismo = sumOrganismo,
            TaxaIvaId = servicoResumo.TaxaIvaId,
            TaxaIvaPercentagem = servicoResumo.TaxaIva.Taxa,
            MotivoIsencaoId = servicoResumo.MotivoIsencaoId,
            AdmissaoServicosIds = admissaoServicosIds,
        });

        return (true, null);
    }

    private static void MontarPorAdmissao(
        List<Admissao> admissoes,
        HashSet<Guid> jaFaturados,
        FaturaGlobalObterResponse response)
    {
        foreach (var adm in admissoes)
        {
            var servicos = adm.Servicos.Where(s => !jaFaturados.Contains(s.Id)).ToList();
            if (servicos.Count == 0)
                continue;

            response.Admissoes.Add(MapAdmissao(adm));

            response.Linhas.Add(new FaturaGlobalLinhaDTO
            {
                Descricao = $"Beneficiário: {adm.Utente?.Nome ?? "—"}",
                Quantidade = 0,
                PrecoUnitario = 0,
                PrecoUtente = 0,
                PrecoOrganismo = 0,
                AdmissaoServicosIds = [],
            });

            foreach (var srv in servicos)
            {
                var (unit, utente, organismo) = ResolverValores(srv);
                var qty = srv.Quantidade.GetValueOrDefault(1);
                if (qty <= 0) qty = 1;

                var descricao = srv.Servico?.Designacao
                    ?? srv.NomeArtigo
                    ?? "Serviço";
                if (!string.IsNullOrWhiteSpace(srv.Dente))
                    descricao += $" {srv.Dente}";
                if (adm.Data.HasValue)
                    descricao += $" - Data {adm.Data.Value:dd-MM-yyyy}";

                response.Linhas.Add(new FaturaGlobalLinhaDTO
                {
                    ServicoId = srv.ServicoId,
                    CodigoArtigo = srv.Servico?.Designacao ?? srv.CodigoArtigo,
                    Descricao = descricao,
                    Quantidade = qty,
                    PrecoUnitario = unit,
                    PrecoUtente = utente / qty,
                    PrecoOrganismo = organismo / qty,
                    TaxaIvaId = srv.Servico?.TaxaIvaId,
                    TaxaIvaPercentagem = srv.Servico?.TaxaIva?.Taxa ?? 0,
                    MotivoIsencaoId = srv.Servico?.MotivoIsencaoId,
                    AdmissaoServicosIds = [srv.Id],
                });
            }
        }
    }

    private static (decimal Unit, decimal Utente, decimal Organismo) ResolverValores(AdmissaoServico srv)
    {
        var qty = srv.Quantidade.GetValueOrDefault(1);
        if (qty <= 0) qty = 1;

        if (srv.ServicoId.HasValue)
        {
            var unit = srv.ValorServico.GetValueOrDefault();
            var utente = srv.ValorUt.GetValueOrDefault();
            var organismo = srv.DescInst.GetValueOrDefault();
            if (organismo == 0)
                organismo = srv.ValorDesc.GetValueOrDefault();
            return (unit, utente, organismo);
        }

        var artigo = srv.ValorArtigo.GetValueOrDefault() * qty;
        var desc = srv.ValorDesc.GetValueOrDefault();
        return (artigo, 0, desc);
    }

    private static FaturaGlobalAdmissaoDTO MapAdmissao(Admissao adm) =>
        new()
        {
            AdmissaoId = adm.Id,
            ModuloOrigem = (int)ModuloOrigemDocumento.Consultas,
            CodigoExibicao = adm.Ordem?.ToString() ?? adm.Id.ToString()[..8],
            Ordem = adm.Ordem,
        };

    private static FaturaGlobalClienteDTO MapClienteOrganismo(Organismo organismo) =>
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
}
