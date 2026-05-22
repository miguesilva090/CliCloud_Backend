using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.FechoDiarioAdministrativoService;

public interface IRequisicaoEspFechoUpdater : IScopedService
{
  Task MarcarRealizadoSeAplicavelAsync(
    string numeroRequisicao,
    DateTime dataRealizacao,
    CancellationToken cancellationToken = default
  );
}
