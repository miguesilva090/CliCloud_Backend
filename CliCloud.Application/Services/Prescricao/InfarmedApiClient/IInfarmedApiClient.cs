using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Prescricao.InfarmedApiClient.DTOs;

namespace CliCloud.Application.Services.Prescricao.InfarmedApiClient;

public interface IInfarmedApiClient : ITransientService
{
    Task<Response<IReadOnlyList<MedicamentoAutocompleteItemDto>>> AutocompleteAsync(
        string query,
        int? tipoReceita = null,
        bool? prescritivel = null,
        CancellationToken cancellationToken = default
    );

    Task<Response<MedicamentoListagemResumoResultDto>> ListagemResumoAsync(
        string? nome = null,
        int tipo = 10,
        int page = 1,
        bool contar = false,
        int? tipoReceita = null,
        bool? prescritivel = null,
        CancellationToken cancellationToken = default
    );

    Task<Response<MedicamentoPrescricaoLinhaDto>> GetPrescricaoByEmbIdAsync(
        string embId,
        IReadOnlyList<int>? patologias = null,
        CancellationToken cancellationToken = default
    );

    Task<Response<MedicamentoPrescricaoLinhaDto>> GetPrescricaoByCnpemAsync(
        string cnpem,
        string? nrRegisto = null,
        IReadOnlyList<int>? patologias = null,
        CancellationToken cancellationToken = default
    );

    Task<Response<IReadOnlyList<MedicamentoPrescricaoOpcaoDto>>> GetEquivalentesByCnpemAsync(
        string cnpem,
        IReadOnlyList<int>? patologias = null,
        CancellationToken cancellationToken = default
    );
}