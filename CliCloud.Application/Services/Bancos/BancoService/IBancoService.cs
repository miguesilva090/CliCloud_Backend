using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Bancos.BancoService.DTOs;
using CliCloud.Application.Services.Bancos.BancoService.Filters;

namespace CliCloud.Application.Services.Bancos.BancoService
{
    public interface IBancoService : ITransientService
    {
        Task<Response<IEnumerable<BancoDTO>>> GetBancoAsync(string keyword = "");
        Task<Response<IEnumerable<BancoLightDTO>>> GetBancoLightAsync(string keyword = "");
        Task<PaginatedResponse<BancoTableDTO>> GetBancoPaginatedAsync(BancoTableFilter filter);
        Task<Response<IEnumerable<BancoTableDTO>>> GetAllBancoAsync(BancoAllFilter filter);
        Task<Response<BancoDTO>> GetBancoAsync(Guid id);
        Task<Response<BancoDTO>> GetBancoByNContribAsync(string ncontrib);
        Task<Response<IEnumerable<BancoDTO>>> GetBancoByNameAsync(string nome);
        Task<Response<Guid>> CreateBancoAsync(CreateBancoRequest request);
        Task<Response<Guid>> UpdateBancoAsync(UpdateBancoRequest request, Guid id);
        Task<Response<Guid>> DeleteBancoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleBancoAsync(IEnumerable<Guid> ids);
    }
}
