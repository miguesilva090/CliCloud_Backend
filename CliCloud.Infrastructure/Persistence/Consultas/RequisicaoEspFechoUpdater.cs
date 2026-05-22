using CliCloud.Application.Services.Consultas.FechoDiarioAdministrativoService;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Infrastructure.Persistence.Consultas;

public sealed class RequisicaoEspFechoUpdater(ApplicationDbContext dbContext) : IRequisicaoEspFechoUpdater
{
  private const int EstadoEfetivadoLegado = 5;

  public async Task MarcarRealizadoSeAplicavelAsync(
    string numeroRequisicao,
    DateTime dataRealizacao,
    CancellationToken cancellationToken = default
  )
  {
    int? estado = await dbContext.Database
      .SqlQuery<int?>(
        $"""
        SELECT TOP 1 Estado AS Value
        FROM dbo.RequisicoesEsp
        WHERE NumeroRequisicao = {numeroRequisicao} AND ISNULL(Apagado, 0) = 0
        """
      )
      .FirstOrDefaultAsync(cancellationToken);

    if (estado is null)
    {
      return;
    }

    if (estado == EstadoEfetivadoLegado)
    {
      _ = await dbContext.Database.ExecuteSqlRawAsync(
        """
        UPDATE dbo.RequisicoesEsp
        SET DataRealizacao = {0}, UltimaData = {0}
        WHERE NumeroRequisicao = {1} AND ISNULL(Apagado, 0) = 0
        """,
        [dataRealizacao, numeroRequisicao],
        cancellationToken
      );
      return;
    }

    _ = await dbContext.Database.ExecuteSqlRawAsync(
      """
      UPDATE dbo.RequisicoesEsp
      SET DataRealizacao = {0},
          Estado = (SELECT Codigo FROM dbo.EstadoExameESP WHERE Abreviatura = 'REAL'),
          UltimaData = {0}
      WHERE NumeroRequisicao = {1} AND ISNULL(Apagado, 0) = 0
      """,
      [dataRealizacao, numeroRequisicao],
      cancellationToken
    );
  }
}
