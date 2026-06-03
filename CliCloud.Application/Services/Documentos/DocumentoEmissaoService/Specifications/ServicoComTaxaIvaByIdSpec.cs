using Ardalis.Specification;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Specifications;

public sealed class ServicoComTaxaIvaByIdSpec : Specification<Servico>
{
    public ServicoComTaxaIvaByIdSpec(Guid servicoId)
    {
        Query.Where(x => x.Id == servicoId)
            .Include(x => x.TaxaIva);
    }
}
