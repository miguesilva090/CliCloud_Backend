using Ardalis.Specification;
using CliCloud.Domain.Entities.Faturacao;

namespace CliCloud.Application.Services.Faturacao.AdseComunicacaoService.Specifications;

public class AdseCoPagamentosPorDocumentosSpec : Specification<AdseCoPagamento>
{
    public AdseCoPagamentosPorDocumentosSpec(Guid clinicaId, IEnumerable<Guid> documentoIds)
    {
        HashSet<Guid> ids = documentoIds.ToHashSet();
        Query.Where(x =>
            x.ClinicaId == clinicaId
            && x.DeletedOn == null
            && ids.Contains(x.DocumentoId));
    }
}
