using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.RelatorioExamesService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.RelatorioExamesService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.RelatorioExamesService
{
    public interface IRelatorioExamesService : ITransientService
    {
        Task<Response<RelatorioExamesDTO?>> GetByUtenteAndMedicoAsync(Guid utenteId, Guid medicoId);
        Task<Response<Guid>> CreateRelatorioExamesAsync(CreateRelatorioExamesRequest request, Guid medicoId);
        Task<Response<Guid>> UpdateRelatorioExamesAsync(UpdateRelatorioExamesRequest request, Guid medicoId);
    }
}
