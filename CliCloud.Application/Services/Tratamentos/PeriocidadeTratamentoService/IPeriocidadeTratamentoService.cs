using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.PeriocidadeTratamentoService.DTOs;
using CliCloud.Application.Services.Tratamentos.PeriocidadeTratamentoService.Filters;

namespace CliCloud.Application.Services.Tratamentos.PeriocidadeTratamentoService
{
    public interface IPeriocidadeTratamentoService : ITransientService
    {
        Task<Response<IEnumerable<PeriocidadeTratamentoDTO>>> GetPeriocidadeTratamentoAsync(string keyword = "");
        Task<Response<IEnumerable<PeriocidadeTratamentoLightDTO>>> GetPeriocidadeTratamentoLightAsync(string keyword = "");
        Task<PaginatedResponse<PeriocidadeTratamentoTableDTO>> GetPeriocidadeTratamentoPaginatedAsync(PeriocidadeTratamentoTableFilter filter);
        Task<Response<IEnumerable<PeriocidadeTratamentoTableDTO>>> GetAllPeriocidadeTratamentoAsync(PeriocidadeTratamentoAllFilter filter);
        Task<Response<PeriocidadeTratamentoDTO>> GetPeriocidadeTratamentoAsync(Guid id);
        Task<Response<PeriocidadeTratamentoDTO>> GetPeriocidadeTratamentoByDescricaoAsync(string descricao);
        Task<Response<Guid>> CreatePeriocidadeTratamentoAsync(CreatePeriocidadeTratamentoRequest request);
        Task<Response<Guid>> UpdatePeriocidadeTratamentoAsync(UpdatePeriocidadeTratamentoRequest request, Guid id);
        Task<Response<Guid>> DeletePeriocidadeTratamentoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultiplePeriocidadeTratamentoAsync(IEnumerable<Guid> ids);
    }
}
