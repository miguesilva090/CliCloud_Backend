using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoConteudoService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoConteudoService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoConteudoService
{
    public interface IFichaClinicaSecaoConteudoService : ITransientService
    {
        Task<Response<IEnumerable<FichaClinicaSecaoConteudoDTO>>> GetFichaClinicaSecaoConteudoAsync(string keyword = "");

        Task<PaginatedResponse<FichaClinicaSecaoConteudoDTO>> GetFichaClinicaSecaoConteudoPaginatedAsync(
            FichaClinicaSecaoConteudoTableFilter filter
        );

        Task<Response<FichaClinicaSecaoConteudoDTO>> GetFichaClinicaSecaoConteudoAsync(Guid id);

        Task<Response<Guid>> CreateFichaClinicaSecaoConteudoAsync(CreateFichaClinicaSecaoConteudoRequest request);

        Task<Response<Guid>> UpdateFichaClinicaSecaoConteudoAsync(UpdateFichaClinicaSecaoConteudoRequest request, Guid id);

        Task<Response<Guid>> DeleteFichaClinicaSecaoConteudoAsync(Guid id);

        Task<Response<IEnumerable<Guid>>> DeleteMultipleFichaClinicaSecaoConteudoAsync(IEnumerable<Guid> ids);
    }
}