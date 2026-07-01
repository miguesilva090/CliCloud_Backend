using Ardalis.Specification;
using CliCloud.Domain.Entities.Faturacao;

namespace CliCloud.Application.Services.Faturacao.AdseComunicacaoService.Specifications;

public class AdseCoPagamentoPorDocumentoSpec : Specification<AdseCoPagamento>
{
    public AdseCoPagamentoPorDocumentoSpec(Guid documentoId) =>
        Query.Where(x => x.DocumentoId == documentoId && x.DeletedOn == null);
}
