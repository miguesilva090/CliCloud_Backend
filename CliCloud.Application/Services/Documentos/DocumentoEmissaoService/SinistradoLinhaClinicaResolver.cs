#nullable enable

using CliCloud.Application.Common;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;
using CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Specifications;
using CliCloud.Application.Services.Sinistros.SinistradoService.Specifications;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Servicos;
using CliCloud.Domain.Entities.Sinistros;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService;

/// <summary>
/// Resolve serviço/preços para linhas sinistro com códigos CONS-/TRAT- (importadas de consultas/tratamentos).
/// </summary>
internal static class SinistradoLinhaClinicaResolver
{
    internal sealed record ContextoServico(
        Servico? Servico,
        decimal? ValorServico,
        decimal? ValorOrganismo,
        int Quantidade);

    internal static async Task<ContextoServico?> ResolverAsync(
        IRepositoryAsync repository,
        SinistradoLinhaServico linha)
    {
        if (linha.TratamentoId.HasValue)
        {
            var ctx = await ResolverDesdeTratamentoAsync(repository, linha, linha.TratamentoId.Value);
            if (ctx is not null)
                return ctx;
        }

        if (linha.AdmissaoId.HasValue)
        {
            var consultas = (
                await repository.GetListAsync<Consulta, Guid>(
                    new ConsultaPorAdmissaoSpec(linha.AdmissaoId.Value))
            ).ToList();
            foreach (var consulta in consultas)
            {
                var ctx = await ResolverDesdeConsultaAsync(repository, linha, consulta.Id);
                if (ctx is not null)
                    return ctx;
            }
        }

        var codigo = linha.CodigoServico.Trim();
        if (codigo.StartsWith("CONS-", StringComparison.OrdinalIgnoreCase))
        {
            var hex = codigo[5..];
            if (Guid.TryParseExact(hex, "N", out Guid consultaId)
                || Guid.TryParse(hex, out consultaId))
            {
                return await ResolverDesdeConsultaAsync(repository, linha, consultaId);
            }

            var consultas = (
                await repository.GetListAsync<Consulta, Guid>(new ConsultaByIdHexPrefixSpec(hex))
            ).ToList();
            if (consultas.Count == 1)
                return await ResolverDesdeConsultaAsync(repository, linha, consultas[0].Id);
        }

        if (codigo.StartsWith("TRAT-", StringComparison.OrdinalIgnoreCase))
        {
            var hex = codigo[5..];
            if (Guid.TryParseExact(hex, "N", out Guid tratamentoId)
                || Guid.TryParse(hex, out tratamentoId))
            {
                return await ResolverDesdeTratamentoAsync(repository, linha, tratamentoId);
            }
        }

        return null;
    }

    private static async Task<ContextoServico?> ResolverDesdeConsultaAsync(
        IRepositoryAsync repository,
        SinistradoLinhaServico linha,
        Guid consultaId)
    {
        var rows = (
            await repository.GetListAsync<ServicoConsulta, Guid>(
                new ServicosConsultaByConsultaIdSpec(consultaId))
        ).ToList();

        if (rows.Count > 0)
        {
            ServicoConsulta row = EscolherServicoConsulta(rows, linha) ?? rows[0];
            if (row.Servico is not null)
            {
                var qty = QuantidadeEfectiva(linha, row.Quantidade);
                return new ContextoServico(
                    row.Servico,
                    row.ValorServico ?? row.ValorArtigo ?? linha.ValorServico,
                    row.ValorUt ?? linha.ValorContratado,
                    qty);
            }

            if (row.ServicoId.HasValue)
            {
                var servico = (
                    await repository.GetListAsync<Servico, Guid>(
                        new ServicoComTaxaIvaByCodigoSpec(row.ServicoId.Value.ToString()))
                ).FirstOrDefault();
                if (servico is not null)
                {
                    var qty = QuantidadeEfectiva(linha, row.Quantidade);
                    return new ContextoServico(
                        servico,
                        row.ValorServico ?? row.ValorArtigo ?? linha.ValorServico,
                        row.ValorUt ?? linha.ValorContratado,
                        qty);
                }
            }
        }

        if (linha.ValorServico.HasValue || !string.IsNullOrWhiteSpace(linha.DesignacaoServico))
        {
            return new ContextoServico(
                null,
                linha.ValorServico,
                linha.ValorContratado,
                Math.Max(1, linha.Quantidade));
        }

        return null;
    }

    private static async Task<ContextoServico?> ResolverDesdeTratamentoAsync(
        IRepositoryAsync repository,
        SinistradoLinhaServico linha,
        Guid tratamentoId)
    {
        var rows = (
            await repository.GetListAsync<ServicoTratamento, Guid>(
                new ServicosTratamentoByTratamentoIdSpec(tratamentoId))
        ).ToList();

        if (rows.Count > 0)
        {
            ServicoTratamento row = rows[0];
            if (row.Servico is not null)
            {
                return new ContextoServico(
                    row.Servico,
                    row.Preco ?? linha.ValorServico,
                    row.ValorUt ?? linha.ValorContratado,
                    Math.Max(1, linha.Quantidade));
            }

            if (row.ServicoId.HasValue)
            {
                var servico = (
                    await repository.GetListAsync<Servico, Guid>(
                        new ServicoComTaxaIvaByCodigoSpec(row.ServicoId.Value.ToString()))
                ).FirstOrDefault();
                if (servico is not null)
                {
                    return new ContextoServico(
                        servico,
                        row.Preco ?? linha.ValorServico,
                        row.ValorUt ?? linha.ValorContratado,
                        Math.Max(1, linha.Quantidade));
                }
            }
        }

        if (linha.ValorServico.HasValue || !string.IsNullOrWhiteSpace(linha.DesignacaoServico))
        {
            return new ContextoServico(
                null,
                linha.ValorServico,
                linha.ValorContratado,
                Math.Max(1, linha.Quantidade));
        }

        return null;
    }

    private static ServicoConsulta? EscolherServicoConsulta(
        List<ServicoConsulta> rows,
        SinistradoLinhaServico linha)
    {
        if (string.IsNullOrWhiteSpace(linha.DesignacaoServico))
            return rows.FirstOrDefault(r => r.ServicoId.HasValue);

        return rows.FirstOrDefault(r =>
                string.Equals(
                    r.Servico?.Designacao,
                    linha.DesignacaoServico,
                    StringComparison.OrdinalIgnoreCase)
                || string.Equals(
                    r.NomeArtigo,
                    linha.DesignacaoServico,
                    StringComparison.OrdinalIgnoreCase))
            ?? rows.FirstOrDefault(r => r.ServicoId.HasValue);
    }

    private static int QuantidadeEfectiva(SinistradoLinhaServico linha, decimal? quantidadeConsulta)
    {
        if (linha.Quantidade > 0)
            return linha.Quantidade;
        if (quantidadeConsulta.HasValue && quantidadeConsulta.Value > 0)
            return Math.Max(1, Convert.ToInt32(quantidadeConsulta.Value));
        return 1;
    }
}
