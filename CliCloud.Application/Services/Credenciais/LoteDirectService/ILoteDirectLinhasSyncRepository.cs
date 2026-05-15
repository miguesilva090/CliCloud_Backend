using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService;

public interface ILoteDirectLinhasSyncRepository : ITransientService
{
    Task SincronizarAsync(
        Guid loteDirectId,
        IEnumerable<LoteDirectLinhaUpsertRequest>? linhas,
        IEnumerable<LoteDirectLinhaUpsertRequest>? linhas789,
        CancellationToken cancellationToken = default);
}
