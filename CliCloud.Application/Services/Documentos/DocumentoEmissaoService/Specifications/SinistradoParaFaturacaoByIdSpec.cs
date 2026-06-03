using Ardalis.Specification;
using CliCloud.Domain.Entities.Sinistros;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Specifications;

public sealed class SinistradoParaFaturacaoByIdSpec : Specification<Sinistrado>
{
    public SinistradoParaFaturacaoByIdSpec(Guid sinistradoId)
    {
        Query.Where(x => x.Id == sinistradoId)
            .Include(x => x.Utente!)
                .ThenInclude(u => u.SeguradoraOrganismo!)
                    .ThenInclude(o => o.Rua!)
                        .ThenInclude(r => r.CodigoPostal)
            .Include(x => x.Utente!)
                .ThenInclude(u => u.SeguradoraOrganismo!)
                    .ThenInclude(o => o.CodigoPostal)
            .Include(x => x.Utente!)
                .ThenInclude(u => u.SubsistemaLinhas)
            .Include(x => x.LinhasServico);
    }
}
