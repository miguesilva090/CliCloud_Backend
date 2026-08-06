using CliCloud.Application.Services.Consultas.FechoDiarioAdministrativoService;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Infrastructure.Persistence.Consultas;

public sealed class RequisicaoEspFechoUpdater(ApplicationDbContext dbContext) : IRequisicaoEspFechoUpdater
{
  private const int EstadoEfetivadoFallback = 5;

  public async Task MarcarRealizadoSeAplicavelAsync(
    string numeroRequisicao,
    DateTime dataRealizacao,
    CancellationToken cancellationToken = default
  )
  {
    RequisicaoEsp? req = await dbContext
      .Set<RequisicaoEsp>()
      .FirstOrDefaultAsync(x => x.NumeroRequisicao == numeroRequisicao, cancellationToken)
      .ConfigureAwait(false);

    if (req is null)
      return;

    int estadoEfet = await ObterCodigoEstadoAsync("EFET", cancellationToken).ConfigureAwait(false)
      ?? EstadoEfetivadoFallback;

    if (req.Estado == estadoEfet)
    {
      req.DataRealizacao = dataRealizacao;
      req.UltimaData = dataRealizacao;
    }
    else
    {
      int? estadoReal = await ObterCodigoEstadoAsync("REAL", cancellationToken).ConfigureAwait(false);
      if (estadoReal.HasValue)
        req.Estado = estadoReal.Value;

      req.DataRealizacao = dataRealizacao;
      req.UltimaData = dataRealizacao;
    }

    _ = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
  }

  public async Task<bool> ReverterAgendamentoSePossivelAsync(
    string numeroRequisicao,
    CancellationToken cancellationToken = default
  )
  {
    RequisicaoEsp? req = await dbContext
      .Set<RequisicaoEsp>()
      .FirstOrDefaultAsync(x => x.NumeroRequisicao == numeroRequisicao, cancellationToken)
      .ConfigureAwait(false);

    if (req is null)
      return true;

    int estadoEfet = await ObterCodigoEstadoAsync("EFET", cancellationToken).ConfigureAwait(false)
      ?? EstadoEfetivadoFallback;
    if (req.Estado == estadoEfet)
      return false;

    int? estadoCat = await ObterCodigoEstadoAsync("CAT", cancellationToken).ConfigureAwait(false);
    if (estadoCat.HasValue)
      req.Estado = estadoCat.Value;

    req.DataAgendamento = null;
    req.DataServico = null;
    req.CodigoMedico = null;
    req.UltimaData = req.DataCativacao;

    _ = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    return true;
  }

  private async Task<int?> ObterCodigoEstadoAsync(string abreviatura, CancellationToken cancellationToken)
    => await dbContext
      .Set<EstadoExameEsp>()
      .Where(x => x.Abreviatura == abreviatura)
      .Select(x => (int?)x.Codigo)
      .FirstOrDefaultAsync(cancellationToken)
      .ConfigureAwait(false);
}
