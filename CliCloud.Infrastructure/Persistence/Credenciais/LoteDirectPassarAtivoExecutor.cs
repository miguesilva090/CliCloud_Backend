using CliCloud.Application.Services.Credenciais.LoteDirectService;
using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;
using CliCloud.Domain.Entities.Credenciais;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CliCloud.Infrastructure.Persistence.Credenciais;

public sealed class LoteDirectPassarAtivoExecutor(
    ApplicationDbContext dbContext,
    ILoteDirectNovoLoteResolver novoLoteResolver,
    ILoteDirectEspHistoricoGateway espHistoricoGateway)
    : ILoteDirectPassarAtivoExecutor
{
    public async Task<PassarParaAtivoResultDTO> ExecutarAsync(
        Guid loteDirectId,
        int mesNovo,
        int anoNovo,
        CancellationToken cancellationToken = default)
    {
        await using IDbContextTransaction tx = await dbContext.Database
            .BeginTransactionAsync(cancellationToken)
            .ConfigureAwait(false);

        LoteDirect lote = await dbContext
            .Set<LoteDirect>()
            .FirstOrDefaultAsync(x => x.Id == loteDirectId, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new InvalidOperationException("Registo não encontrado.");

        if (!lote.Historico)
            throw new InvalidOperationException("O registo já está ativo.");

        if (lote is not { CodigoOrganismo: int org, Mes: int mesOrigem, Ano: int anoOrigem, TipoLote: int tipoLote, TipoServico: int tipoServico })
            throw new InvalidOperationException("Organismo, mês, ano, tipo de lote ou tipo de serviço em falta.");

        (_, int novoLote) = await novoLoteResolver
            .ResolverAsync(org, tipoLote, tipoServico, mesNovo, anoNovo, cancellationToken)
            .ConfigureAwait(false);

        List<LoteDirectLinha> linhas = await dbContext
            .Set<LoteDirectLinha>()
            .Where(x => x.LoteDirectId == loteDirectId)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        List<LoteDirectLinha789> linhas789 = await dbContext
            .Set<LoteDirectLinha789>()
            .Where(x => x.LoteDirectId == loteDirectId)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        int quantidade = linhas.Sum(x => x.Quantidade)
            + linhas789.Sum(x => x.Quantidade)
            + (lote.QuantidadeConsulta ?? 0);

        decimal valor = linhas.Sum(x => x.ValorUtente + x.ValorInstituicao)
            + linhas789.Sum(x => x.ValorUtente + x.ValorInstituicao)
            + (lote.ValorConsulta ?? 0);

        decimal valorTaxa = linhas.Sum(x => x.ValorUtente)
            + linhas789.Sum(x => x.ValorUtente)
            + (lote.TaxaConsulta ?? 0);

        await dbContext
            .Set<LoteDirectDetalhe>()
            .Where(x => x.LoteDirectId == loteDirectId)
            .ExecuteDeleteAsync(cancellationToken)
            .ConfigureAwait(false);

        LoteDirectAgregado? agregado = await dbContext
            .Set<LoteDirectAgregado>()
            .FirstOrDefaultAsync(
                x => x.Ano == anoNovo
                    && x.Mes == mesNovo
                    && x.CodigoOrganismo == org
                    && x.TipoLote == tipoLote
                    && x.TipoServico == tipoServico
                    && x.NumeroLote == novoLote,
                cancellationToken)
            .ConfigureAwait(false);

        if (agregado is null)
        {
            agregado = new LoteDirectAgregado
            {
                Id = Guid.NewGuid(),
                NumeroLote = novoLote,
                Ano = anoNovo,
                Mes = mesNovo,
                CodigoOrganismo = org,
                TipoLote = tipoLote,
                TipoServico = tipoServico,
                DataLote = DateTime.UtcNow,
                Quantidade = quantidade,
                Valor = valor,
                ValorTaxa = valorTaxa,
                Isencao = lote.Isencao,
                NumeroRequisicoes = 1,
            };

            dbContext.Set<LoteDirectAgregado>().Add(agregado);
            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        else
        {
            agregado.Quantidade += quantidade;
            agregado.Valor += valor;
            agregado.ValorTaxa += valorTaxa;
            agregado.NumeroRequisicoes += 1;
        }

        dbContext.Set<LoteDirectDetalhe>().Add(new LoteDirectDetalhe
        {
            Id = Guid.NewGuid(),
            LoteDirectAgregadoId = agregado.Id,
            LoteDirectId = loteDirectId,
            Indice = agregado.Indice,
            NumeroLote = novoLote,
            Ano = anoNovo,
            Mes = mesNovo,
            CodigoOrganismo = org,
            TipoServico = tipoServico,
            TipoLote = tipoLote,
            Credencial = lote.Credencial,
            Quantidade = quantidade,
            Valor = valor,
            ValorTaxa = valorTaxa,
            Isencao = lote.Isencao,
            Data = lote.DataFim,
        });

        lote.Historico = false;
        lote.Mes = mesNovo;
        lote.Ano = anoNovo;
        lote.NumeroLote = novoLote;
        lote.IndiceLote = agregado.Indice;

        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        if (!string.IsNullOrWhiteSpace(lote.Credencial))
        {
            await espHistoricoGateway
                .UpdateParaAtivoAsync(lote.Credencial, cancellationToken)
                .ConfigureAwait(false);
        }

        await tx.CommitAsync(cancellationToken).ConfigureAwait(false);

        return new PassarParaAtivoResultDTO
        {
            CredenciaisActualizadas = 1,
            CodigoOrganismo = org,
            MesOrigem = mesOrigem,
            AnoOrigem = anoOrigem,
            MesNovo = mesNovo,
            AnoNovo = anoNovo,
        };
    }
}
