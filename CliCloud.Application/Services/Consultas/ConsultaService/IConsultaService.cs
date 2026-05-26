using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.ConsultaService.DTOs;
using CliCloud.Application.Services.Consultas.ConsultaService.Filters;

namespace CliCloud.Application.Services.Consultas.ConsultaService
{
    public interface IConsultaService : ITransientService
    {
        Task<Response<IEnumerable<ConsultaDTO>>> GetConsultaAsync(string keyword = "");
        Task<Response<IEnumerable<ConsultaLightDTO>>> GetConsultaLightAsync(string keyword = "");
        Task<PaginatedResponse<ConsultaTableDTO>> GetConsultaPaginatedAsync(ConsultaTableFilter filter);
        Task<Response<IEnumerable<ConsultaTableDTO>>> GetAllConsultaAsync(ConsultaAllFilter? filter);
        Task<Response<IEnumerable<ConsultaDoDiaDTO>>> GetConsultasDoDiaAsync(DateTime data, bool desmarcadas = false);
        Task<Response<ConsultaDTO>> GetConsultaAsync(Guid id);
        Task<Response<Guid>> CreateConsultaAsync(CreateConsultaRequest request);
        Task<Response<IniciarAtendimentoConsultaDTO>> IniciarAtendimentoAsync(IniciarAtendimentoConsultaRequest request);
        Task<Response<Guid>> CreateConsultaFromMarcacaoAsync(Guid marcacaoId);
        Task<Response<Guid>> UpdateConsultaAsync(UpdateConsultaRequest request, Guid id);
        Task<Response<Guid>> DeleteConsultaAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleConsultaAsync(IEnumerable<Guid> ids);
        Task<Response<Guid>> FinalizarConsultaAsync(Guid id);
    }
}
