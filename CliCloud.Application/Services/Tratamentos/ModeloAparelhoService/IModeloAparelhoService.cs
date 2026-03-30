using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.ModeloAparelhoService.DTOs;
using CliCloud.Application.Services.Tratamentos.ModeloAparelhoService.Filters;

namespace CliCloud.Application.Services.Tratamentos.ModeloAparelhoService
{
    public interface IModeloAparelhoService : ITransientService
    {
        Task<Response<IEnumerable<ModeloAparelhoDTO>>> GetModeloAparelhoAsync(string keyword = "");
        Task<Response<IEnumerable<ModeloAparelhoLightDTO>>> GetModeloAparelhoLightAsync(string keyword = "");
        Task<PaginatedResponse<ModeloAparelhoTableDTO>> GetModeloAparelhoPaginatedAsync(ModeloAparelhoTableFilter filter);
        Task<Response<IEnumerable<ModeloAparelhoTableDTO>>> GetAllModeloAparelhoAsync(ModeloAparelhoAllFilter filter);
        Task<Response<ModeloAparelhoDTO>> GetModeloAparelhoAsync(Guid id);
        Task<Response<ModeloAparelhoDTO>> GetModeloAparelhoByDesignacaoAsync(string designacao);
        Task<Response<Guid>> CreateModeloAparelhoAsync(CreateModeloAparelhoRequest request);
        Task<Response<Guid>> UpdateModeloAparelhoAsync(UpdateModeloAparelhoRequest request, Guid id);
        Task<Response<Guid>> DeleteModeloAparelhoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleModeloAparelhoAsync(IEnumerable<Guid> ids);
    }
}
