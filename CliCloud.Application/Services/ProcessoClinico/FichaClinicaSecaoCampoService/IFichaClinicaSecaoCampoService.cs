using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoCampoService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoCampoService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoCampoService
{
    public interface IFichaClinicaSecaoCampoService : ITransientService
    {
        Task<Response<IEnumerable<FichaClinicaSecaoCampoDTO>>> GetFichaClinicaSecaoCampoAsync(
            Guid separadorId,
            string keyword = ""
        );

        Task<
          PaginatedResponse<FichaClinicaSecaoCampoDTO>
        > GetFichaClinicaSecaoCampoPaginatedAsync(FichaClinicaSecaoCampoTableFilter filter);

        Task<Response<FichaClinicaSecaoCampoDTO>> GetFichaClinicaSecaoCampoAsync(Guid id);

        Task<Response<Guid>> CreateFichaClinicaSecaoCampoAsync(
            CreateFichaClinicaSecaoCampoRequest request
        );

        Task<Response<Guid>> UpdateFichaClinicaSecaoCampoAsync(
            UpdateFichaClinicaSecaoCampoRequest request,
            Guid id
        );

        Task<Response<Guid>> DeleteFichaClinicaSecaoCampoAsync(Guid id);

        Task<Response<IEnumerable<Guid>>> DeleteMultipleFichaClinicaSecaoCampoAsync(
            IEnumerable<Guid> ids
        );
    }
}

