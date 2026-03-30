using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.MarcaAparelhoService.DTOs;
using CliCloud.Application.Services.Tratamentos.MarcaAparelhoService.Filters;

namespace CliCloud.Application.Services.Tratamentos.MarcaAparelhoService
{
    public interface IMarcaAparelhoService : ITransientService
    {
        Task<Response<IEnumerable<MarcaAparelhoDTO>>> GetMarcaAparelhoAsync(string keyword = "");
        Task<Response<IEnumerable<MarcaAparelhoLightDTO>>> GetMarcaAparelhoLightAsync(string keyword = "");
        Task<PaginatedResponse<MarcaAparelhoTableDTO>> GetMarcaAparelhoPaginatedAsync(MarcaAparelhoTableFilter filter);
        Task<Response<IEnumerable<MarcaAparelhoTableDTO>>> GetAllMarcaAparelhoAsync(MarcaAparelhoAllFilter filter);
        Task<Response<MarcaAparelhoDTO>> GetMarcaAparelhoAsync(Guid id);
        Task<Response<MarcaAparelhoDTO>> GetMarcaAparelhoByDesignacaoAsync(string designacao);
        Task<Response<Guid>> CreateMarcaAparelhoAsync(CreateMarcaAparelhoRequest request);
        Task<Response<Guid>> UpdateMarcaAparelhoAsync(UpdateMarcaAparelhoRequest request, Guid id);
        Task<Response<Guid>> DeleteMarcaAparelhoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleMarcaAparelhoAsync(IEnumerable<Guid> ids);
    }
}
