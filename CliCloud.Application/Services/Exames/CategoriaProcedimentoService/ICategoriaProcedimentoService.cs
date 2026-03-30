using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Exames.CategoriaProcedimentoService.DTOs;
using CliCloud.Application.Services.Exames.CategoriaProcedimentoService.Filters;

namespace CliCloud.Application.Services.Exames.CategoriaProcedimentoService
{
    public interface ICategoriaProcedimentoService : ITransientService
    {
        Task<Response<IEnumerable<CategoriaProcedimentoDTO>>> GetCategoriaProcedimentoAsync(string keyword = "");
        Task<Response<IEnumerable<CategoriaProcedimentoLightDTO>>> GetCategoriaProcedimentoLightAsync(string keyword = "");
        Task<PaginatedResponse<CategoriaProcedimentoTableDTO>> GetCategoriaProcedimentoPaginatedAsync(CategoriaProcedimentoTableFilter filter);
        Task<Response<IEnumerable<CategoriaProcedimentoTableDTO>>> GetAllCategoriaProcedimentoAsync(CategoriaProcedimentoAllFilter? filter);
        Task<Response<CategoriaProcedimentoDTO>> GetCategoriaProcedimentoAsync(Guid id);
        Task<Response<Guid>> CreateCategoriaProcedimentoAsync(CreateCategoriaProcedimentoRequest request);
        Task<Response<Guid>> UpdateCategoriaProcedimentoAsync(UpdateCategoriaProcedimentoRequest request, Guid id);
        Task<Response<Guid>> DeleteCategoriaProcedimentoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleCategoriaProcedimentoAsync(IEnumerable<Guid> ids);
    }
}
