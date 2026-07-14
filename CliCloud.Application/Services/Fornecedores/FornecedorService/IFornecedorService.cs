using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Fornecedores.FornecedorService.DTOs;
using CliCloud.Application.Services.Fornecedores.FornecedorService.Filters;

namespace CliCloud.Application.Services.Fornecedores.FornecedorService
{
    public interface IFornecedorService : ITransientService
    {
        Task<Response<IEnumerable<FornecedorDTO>>> GetFornecedorAsync(string keyword = "");
        Task<Response<IEnumerable<FornecedorLightDTO>>> GetFornecedorLightAsync(string keyword = "");
        Task<PaginatedResponse<FornecedorTableDTO>> GetFornecedorPaginatedAsync(FornecedorTableFilter filter);
        Task<Response<IEnumerable<FornecedorTableDTO>>> GetAllFornecedorAsync(FornecedorAllFilter filter);
        Task<Response<FornecedorDTO>> GetFornecedorAsync(Guid id);
        Task<Response<FornecedorDTO>> GetFornecedorByNContribAsync(string ncontrib);
        Task<Response<IEnumerable<FornecedorDTO>>> GetFornecedorByNameAsync(string nome);
        Task<Response<Guid>> CreateFornecedorAsync(CreateFornecedorRequest request);
        Task<Response<Guid>> UpdateFornecedorAsync(UpdateFornecedorRequest request, Guid id);
        Task<Response<Guid>> DeleteFornecedorAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleFornecedorAsync(IEnumerable<Guid> ids);
    }
}
