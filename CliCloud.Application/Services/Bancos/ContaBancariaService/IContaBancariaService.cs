using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Bancos.ContaBancariaService.DTOs;
using CliCloud.Application.Services.Bancos.ContaBancariaService.Filters;

namespace CliCloud.Application.Services.Bancos.ContaBancariaService
{
    public interface IContaBancariaService : ITransientService
    {
        Task<Response<IEnumerable<ContaBancariaDTO>>> GetContaBancariaAsync(string keyword = "");
        Task<Response<IEnumerable<ContaBancariaLightDTO>>> GetContaBancariaLightAsync(string keyword = "");
        Task<PaginatedResponse<ContaBancariaTableDTO>> GetContaBancariaPaginatedAsync(ContaBancariaTableFilter filter);
        Task<Response<IEnumerable<ContaBancariaTableDTO>>> GetAllContaBancariaAsync(ContaBancariaAllFilter filter);
        Task<Response<ContaBancariaDTO>> GetContaBancariaAsync(Guid id);
        Task<Response<Guid>> CreateContaBancariaAsync(CreateContaBancariaRequest request);
        Task<Response<Guid>> UpdateContaBancariaAsync(UpdateContaBancariaRequest request, Guid id);
        Task<Response<Guid>> DeleteContaBancariaAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleContaBancariaAsync(IEnumerable<Guid> ids);
    }
}