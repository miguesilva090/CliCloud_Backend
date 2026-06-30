namespace CliCloud.Application.Services.Faturacao.CredenciaisSnsService;

public interface ICredenciaisSnsLegadoLookup
{
    /// <summary>Nomes de tipo de serviço (legado dbo.TIPO_SRV.nome) por c_tipo_srv.</summary>
    Task<IReadOnlyDictionary<int, string>> ObterNomesTipoServicoAsync(
        IReadOnlyCollection<int> codigosTipoServico,
        int? filtroLegado = null,
        CancellationToken cancellationToken = default);
}
