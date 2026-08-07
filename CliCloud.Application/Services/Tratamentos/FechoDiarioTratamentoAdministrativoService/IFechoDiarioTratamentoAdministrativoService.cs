using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.FechoDiarioTratamentoAdministrativoService.DTOs;

namespace CliCloud.Application.Services.Tratamentos.FechoDiarioTratamentoAdministrativoService;

public interface IFechoDiarioTratamentoAdministrativoService : ITransientService
{
    Task<Response<FechoDiarioTratamentoResultDTO>> ExecutarFechoAsync(
        FechoDiarioTratamentoRequest request
    );

    Task<Response<int>> ContarElegiveisAsync(DateTime data);
}