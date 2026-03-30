using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoService.DTOs;
using CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoService.Filters;

namespace CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoService
{
    public interface IEvolucaoTratamentoService : ITransientService
    {
        Task<Response<IEnumerable<EvolucaoTratamentoDTO>>> GetEvolucaoTratamentoAsync(string keyword = "");
        Task<Response<IEnumerable<EvolucaoTratamentoLightDTO>>> GetEvolucaoTratamentoLightAsync(string keyword = "");
        Task<PaginatedResponse<EvolucaoTratamentoTableDTO>> GetEvolucaoTratamentoPaginatedAsync(EvolucaoTratamentoTableFilter filter);
        Task<Response<IEnumerable<EvolucaoTratamentoTableDTO>>> GetAllEvolucaoTratamentoAsync(EvolucaoTratamentoAllFilter filter);
        Task<Response<EvolucaoTratamentoDTO>> GetEvolucaoTratamentoAsync(Guid id);
        Task<Response<EvolucaoTratamentoDTO>> GetEvolucaoTratamentoByDescricaoAsync(string descricao);
        Task<Response<Guid>> CreateEvolucaoTratamentoAsync(CreateEvolucaoTratamentoRequest request);
        Task<Response<Guid>> UpdateEvolucaoTratamentoAsync(UpdateEvolucaoTratamentoRequest request, Guid id);
        Task<Response<Guid>> DeleteEvolucaoTratamentoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleEvolucaoTratamentoAsync(IEnumerable<Guid> ids);
        Task<Response<EvolucaoTratamentoReportDTO>> GetEvolucaoTratamentoReportAsync(Guid id);
    }
}
