using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Faturacao.FicheirosEletronicosService.DTOs;

namespace CliCloud.Application.Services.Faturacao.FicheirosEletronicosService;

public interface IFicheirosEletronicosService : ITransientService
{
    Task<Response<IEnumerable<FIcheiroEletronicoRegistoTableDTO>>> ListarAsync(Guid clinicaId, string sigla, CancellationToken ct = default);
    Task<Response<GerarFicheiroEletronicoResponse>> GerarAsync(Guid clinicaId, GerarFicheiroEletronicoRequest request, CancellationToken ct = default);
    Task<Response<GerarFicheiroEletronicoResponse>> JuntarAsync(Guid clinicaId, JuntarFicheirosEletronicosRequest request, CancellationToken ct = default);
}