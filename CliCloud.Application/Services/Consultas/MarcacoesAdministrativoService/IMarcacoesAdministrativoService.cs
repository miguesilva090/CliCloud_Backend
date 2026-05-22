using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.Filters;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService;

public interface IMarcacoesAdministrativoService : ITransientService
{
    Task<PaginatedResponse<MarcacaoAdministrativoTableDTO>> GetPaginatedAsync(MarcacaoAdministrativoTableFilter filter);
    Task<Response<MarcacaoAdministrativoDTO>> GetByIdAsync(Guid id);
    Task<Response<Guid>> CreateAsync(CreateMarcacaoAdministrativoRequest request);
    Task<Response<Guid>> UpdateAsync(Guid id, UpdateMarcacaoAdministrativoRequest request);
    Task<Response<Guid>> DesmarcarAsync(Guid id, DesmarcarMarcacaoAdministrativoRequest request);
    Task<Response<Guid>> MudarHorarioAsync(Guid id, MudarHorarioMarcacaoAdministrativoRequest request);

    Task<Response<IEnumerable<SalaDisponivelDTO>>> GetSalasDisponiveisAsync(SalasDisponiveisRequest request);
    Task<Response<Guid>> AssociarSalaAsync(Guid marcacaoId, AssociarSalaMarcacaoRequest request);
    Task<Response<Guid>> RemoverSalaAsync(Guid marcacaoId);

    Task<Response<TrocaMarcacoesMedicosPreviewDTO>> PreviewTrocaMedicosAsync(TrocaMarcacoesMedicosRequest request);

    Task<Response<TrocaMarcacoesMedicosResultDTO>> ExecutarTrocaMedicosAsync(TrocaMarcacoesMedicosRequest request);

    Task<Response<MarcacaoCalendarioDTO>> GetCalendarioAsync(MarcacaoCalendarioRequest request);

    Task<Response<List<DisponibilidadeMedicoDiaEventoDTO>>> GetDisponibilidadeMedicosMesAsync(
        DisponibilidadeMedicosMesRequest request);

    Task<Response<MedicoLegadoResolveDTO>> ResolveMedicoLegadoAsync(string key);

    Task<Response<Guid>> SincronizarAdmissaoAsync(Guid marcacaoId);
}