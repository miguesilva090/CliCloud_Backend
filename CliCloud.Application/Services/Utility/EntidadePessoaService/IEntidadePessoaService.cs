using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.EntidadePessoaService.DTOs;
using CliCloud.Application.Services.Utility.EntidadePessoaService.Filters;

namespace CliCloud.Application.Services.Utility.EntidadePessoaService
{
    public interface IEntidadePessoaService : ITransientService
    {
        Task<Response<IEnumerable<EntidadePessoaDTO>>> GetEntidadePessoaAsync(string keyword = "");
        Task<Response<IEnumerable<EntidadePessoaLightDTO>>> GetEntidadePessoaLightAsync(string keyword = "");
        Task<PaginatedResponse<EntidadePessoaTableDTO>> GetEntidadePessoaPaginatedAsync(EntidadePessoaTableFilter filter);
        Task<Response<IEnumerable<EntidadePessoaTableDTO>>> GetAllEntidadePessoaAsync(EntidadePessoaAllFilter filter);
        Task<Response<EntidadePessoaDTO>> GetEntidadePessoaAsync(Guid id);
        Task<Response<EntidadePessoaDTO>> GetEntidadePessoaByNContribAsync(string ncontrib);
        Task<Response<IEnumerable<EntidadePessoaDTO>>> GetEntidadePessoaByNameAsync(string nome);
        Task<Response<Guid>> CreateEntidadePessoaAsync(CreateEntidadePessoaRequest request);
        Task<Response<Guid>> UpdateEntidadePessoaAsync(UpdateEntidadePessoaRequest request, Guid id);
        Task<Response<Guid>> DeleteEntidadePessoaAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleEntidadePessoaAsync(IEnumerable<Guid> ids);
    }
}
