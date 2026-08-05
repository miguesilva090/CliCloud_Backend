using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.DisponibilidadeTecnicoTratamentoService.DTOs;

namespace CliCloud.Application.Services.Tratamentos.DisponibilidadeTecnicoTratamentoService;

public interface IDisponibilidadeTecnicoTratamentoService : ITransientService 
{
    Task<Response<UnidadesTempoTecnicoResponse>> GetUnidadesTempoAsync(Guid tecnicoId);
    Task<Response<HorasPossiveisTecnicoResponse>> GetHorasPossiveisAsync(HorasPossiveisTecnicoRequest request);
}