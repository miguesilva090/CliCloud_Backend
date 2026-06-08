using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas;

/// <summary>
/// Totais por linha de admissão alinhados ao legado (SERV_AD / HISERVAD).
/// <c>valor_ut</c>, <c>rec_inst</c> e <c>valor_desc</c> são totais de linha, não unitários.
/// </summary>
internal static class AdmissaoServicoValoresHelper
{
    public static (decimal ValorServicoTotal, decimal ValorUtenteTotal, decimal ValorOrganismoTotal) ResolverTotaisLinha(
        AdmissaoServico srv,
        decimal quantidade)
    {
        decimal qtd = quantidade > 0 ? quantidade : 1;
        decimal unit = srv.ValorServico ?? srv.ValorArtigo ?? 0m;
        decimal valorServicoTotal = unit * qtd;
        decimal valorUtente = srv.ValorUt ?? 0m;
        decimal valorOrganismo = ResolverOrganismoTotal(srv, valorServicoTotal, valorUtente);
        return (valorServicoTotal, valorUtente, valorOrganismo);
    }

    /// <summary>
    /// Valor organismo da linha (legado: <c>rec_inst</c> / <c>desc_inst</c> / <c>valor_desc</c>).
    /// </summary>
    public static decimal ResolverOrganismoTotal(
        AdmissaoServico srv,
        decimal valorServicoTotal,
        decimal valorUtenteTotal)
    {
        if (srv.RecInst is > 0)
            return srv.RecInst.Value;
        if (srv.DescInst is > 0)
            return srv.DescInst.Value;
        if (srv.ValorDesc is > 0)
            return srv.ValorDesc.Value;
        return Math.Max(0m, valorServicoTotal - valorUtenteTotal);
    }
}
