using CliCloud.Application.Services.Credenciais.LoteDirectService;
using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;
using CliCloud.Domain.Entities.Credenciais;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Infrastructure.Persistence.Credenciais;

public sealed class LoteDirectLinhasSyncRepository(ApplicationDbContext dbContext) : ILoteDirectLinhasSyncRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task SincronizarAsync(
        Guid loteDirectId,
        IEnumerable<LoteDirectLinhaUpsertRequest>? linhas,
        IEnumerable<LoteDirectLinhaUpsertRequest>? linhas789,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<LoteDirectLinha>()
            .Where(x => x.LoteDirectId == loteDirectId)
            .ExecuteDeleteAsync(cancellationToken);

        await _dbContext.Set<LoteDirectLinha789>()
            .Where(x => x.LoteDirectId == loteDirectId)
            .ExecuteDeleteAsync(cancellationToken);

        List<LoteDirectLinha> novasLinhas = (linhas ?? [])
            .Select(linha => new LoteDirectLinha
            {
                Id = Guid.NewGuid(),
                LoteDirectId = loteDirectId,
                ServicoId = linha.ServicoId,
                Quantidade = linha.Quantidade,
                ValorUnitario = linha.ValorUnitario,
                ValorUtenteOriginal = linha.ValorUtenteOriginal,
                ValorInstituicaoOriginal = linha.ValorInstituicaoOriginal,
                ValorUtente = linha.ValorUtente,
                ValorInstituicao = linha.ValorInstituicao
            })
            .ToList();

        if (novasLinhas.Count > 0)
        {
            await _dbContext.Set<LoteDirectLinha>().AddRangeAsync(novasLinhas, cancellationToken);
        }

        List<LoteDirectLinha789> novasLinhas789 = (linhas789 ?? [])
            .Select(linha => new LoteDirectLinha789
            {
                Id = Guid.NewGuid(),
                LoteDirectId = loteDirectId,
                ServicoId = linha.ServicoId,
                Quantidade = linha.Quantidade,
                ValorUnitario = linha.ValorUnitario,
                ValorUtenteOriginal = linha.ValorUtenteOriginal,
                ValorInstituicaoOriginal = linha.ValorInstituicaoOriginal,
                ValorUtente = linha.ValorUtente,
                ValorInstituicao = linha.ValorInstituicao
            })
            .ToList();

        if (novasLinhas789.Count > 0)
        {
            await _dbContext.Set<LoteDirectLinha789>().AddRangeAsync(novasLinhas789, cancellationToken);
        }
    }
}
