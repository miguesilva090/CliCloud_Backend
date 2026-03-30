using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.RelatorioAtestadoService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.RelatorioAtestadoService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.RelatorioAtestadoService
{
    public interface IRelatorioAtestadoService : ITransientService
    {
        Task<Response<IEnumerable<RelatorioAtestadoDTO>>> GetRelatorioAtestadoAsync(string keyword = "");
        Task<PaginatedResponse<RelatorioAtestadoDTO>> GetRelatorioAtestadoPaginatedAsync(RelatorioAtestadoTableFilter filter);
        Task<Response<RelatorioAtestadoDTO>> GetRelatorioAtestadoAsync(Guid id);
        Task<Response<Guid>> CreateRelatorioAtestadoAsync(CreateRelatorioAtestadoRequest request);
        Task<Response<Guid>> UpdateRelatorioAtestadoAsync(UpdateRelatorioAtestadoRequest request, Guid id);
        Task<Response<Guid>> DeleteRelatorioAtestadoAsync(Guid id);

        /// <summary>Obter todos os relatórios/atestados de um utente.</summary>
        Task<Response<IEnumerable<RelatorioAtestadoDTO>>> GetByUtenteAsync(Guid utenteId);

        /// <summary>Assinar um relatório/atestado (preenche a data de assinatura se ainda não existir).</summary>
        Task<Response<DateTime>> AssinarRelatorioAtestadoAsync(Guid id);
    }
}
